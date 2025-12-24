# Script PowerShell para preparar o setup do Genuss Automacao
# Execute como Administrador

Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host "           GENUSS AUTOMACAO - PREPARACAO DO SETUP" -ForegroundColor Cyan
Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host ""

# Definir caminhos
$ProjectPath = "F:\pdv\pdv\Controle de Vendas"
$ProjectFile = "$ProjectPath\Controle de Vendas.sln"
$OutputPath = "$ProjectPath\Controle de Vendas\bin\Release"
$SetupOutputPath = "$ProjectPath\Setup_Output"
$MSBuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"

# Verificar se MSBuild existe
if (-not (Test-Path $MSBuildPath)) {
    $MSBuildPath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
    if (-not (Test-Path $MSBuildPath)) {
        Write-Host "MSBuild nao encontrado. Tentando usar dotnet build..." -ForegroundColor Red
        $UseDotNet = $true
    }
}

Write-Host "Verificando estrutura do projeto..." -ForegroundColor Yellow

# Verificar se o arquivo do projeto existe
if (-not (Test-Path $ProjectFile)) {
    Write-Host "Arquivo do projeto nao encontrado: $ProjectFile" -ForegroundColor Red
    exit 1
}

Write-Host "Projeto encontrado: $ProjectFile" -ForegroundColor Green

# Criar diretorio de saida do setup se nao existir
if (-not (Test-Path $SetupOutputPath)) {
    New-Item -ItemType Directory -Path $SetupOutputPath -Force | Out-Null
    Write-Host "Diretorio de setup criado: $SetupOutputPath" -ForegroundColor Green
}

Write-Host ""
Write-Host "Compilando projeto em modo Release..." -ForegroundColor Yellow

try {
    # Limpar build anterior
    if ($UseDotNet) {
        & dotnet clean "$ProjectFile" --configuration Release
    } else {
        & "$MSBuildPath" "$ProjectFile" /t:Clean /p:Configuration=Release
    }
    
    Write-Host "Build anterior limpo" -ForegroundColor Green
    
    # Compilar projeto
    if ($UseDotNet) {
        & dotnet build "$ProjectFile" --configuration Release --no-restore
    } else {
        & "$MSBuildPath" "$ProjectFile" /t:Build /p:Configuration=Release /p:Platform="Any CPU"
    }
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Compilacao concluida com sucesso!" -ForegroundColor Green
    } else {
        Write-Host "Erro na compilacao. Codigo de saida: $LASTEXITCODE" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "Erro durante a compilacao: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Verificando arquivos compilados..." -ForegroundColor Yellow

# Verificar se os arquivos foram gerados
$MainExe = "$OutputPath\Genuss automacao.exe"
if (-not (Test-Path $MainExe)) {
    Write-Host "Executavel principal nao encontrado: $MainExe" -ForegroundColor Red
    exit 1
}

Write-Host "Executavel principal encontrado" -ForegroundColor Green

# Listar DLLs importantes
$ImportantDLLs = @(
    "ACBrLib.Core.dll",
    "Aspose.Pdf.dll",
    "Bunifu.UI.WinForms.dll",
    "Newtonsoft.Json.dll",
    "QRCoder.dll",
    "System.Data.SQLite.dll",
    "ZeusNFe.dll"
)

Write-Host "Verificando dependencias importantes..." -ForegroundColor Yellow
foreach ($dll in $ImportantDLLs) {
    $dllPath = "$OutputPath\$dll"
    if (Test-Path $dllPath) {
        Write-Host "  OK: $dll" -ForegroundColor Green
    } else {
        Write-Host "  AVISO: $dll (nao encontrado)" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "Estatisticas do build:" -ForegroundColor Cyan

# Contar arquivos
$ExeFiles = Get-ChildItem -Path $OutputPath -Filter "*.exe" | Measure-Object
$DllFiles = Get-ChildItem -Path $OutputPath -Filter "*.dll" | Measure-Object
$ConfigFiles = Get-ChildItem -Path $OutputPath -Filter "*.config" | Measure-Object
$XmlFiles = Get-ChildItem -Path $OutputPath -Filter "*.xml" | Measure-Object

Write-Host "  Executaveis: $($ExeFiles.Count)" -ForegroundColor White
Write-Host "  Bibliotecas (DLL): $($DllFiles.Count)" -ForegroundColor White
Write-Host "  Configuracoes: $($ConfigFiles.Count)" -ForegroundColor White
Write-Host "  Arquivos XML: $($XmlFiles.Count)" -ForegroundColor White

# Calcular tamanho total
$TotalSize = (Get-ChildItem -Path $OutputPath -Recurse | Measure-Object -Property Length -Sum).Sum
$TotalSizeMB = [math]::Round($TotalSize / 1MB, 2)
Write-Host "  Tamanho total: $TotalSizeMB MB" -ForegroundColor White

Write-Host ""
Write-Host "Proximos passos:" -ForegroundColor Cyan
Write-Host "  1. Instale o Inno Setup 6.0 ou superior" -ForegroundColor White
Write-Host "  2. Abra o arquivo Setup_Genuss_Automacao.iss no Inno Setup" -ForegroundColor White
Write-Host "  3. Clique em Build > Compile para gerar o instalador" -ForegroundColor White
Write-Host "  4. O instalador sera criado em: $SetupOutputPath" -ForegroundColor White

Write-Host ""
Write-Host "Abrindo pasta de saida..." -ForegroundColor Yellow
Start-Process explorer.exe -ArgumentList $OutputPath

Write-Host ""
Write-Host "=================================================================" -ForegroundColor Cyan
Write-Host "           PREPARACAO CONCLUIDA COM SUCESSO!" -ForegroundColor Green
Write-Host "=================================================================" -ForegroundColor Cyan

# Perguntar se deseja abrir o Inno Setup
Write-Host ""
$response = Read-Host "Deseja tentar abrir o Inno Setup automaticamente? (S/N)"
if ($response -eq "S" -or $response -eq "s") {
    $InnoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    if (Test-Path $InnoSetupPath) {
        Write-Host "Abrindo Inno Setup..." -ForegroundColor Green
        Start-Process $InnoSetupPath -ArgumentList "\"$ProjectPath\Setup_Genuss_Automacao.iss\""
    } else {
        Write-Host "Inno Setup nao encontrado no caminho padrao" -ForegroundColor Yellow
        Write-Host "   Baixe em: https://jrsoftware.org/isinfo.php" -ForegroundColor White
    }
}

Write-Host ""
Write-Host "Pressione qualquer tecla para sair..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")