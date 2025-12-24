# Script para criar setup do Genuss Automação
# Executa a compilação e criação do instalador

Write-Host "=== CRIANDO SETUP GENUSS AUTOMAÇÃO ===" -ForegroundColor Green
Write-Host ""

# Definir caminhos
$projectPath = "F:\pdv\pdv\Controle de Vendas\Controle de Vendas"
$solutionPath = "F:\pdv\pdv\Controle de Vendas\Controle de Vendas.sln"
$outputPath = "F:\pdv\pdv\Controle de Vendas\Setup_Output"
$setupProjectPath = "F:\pdv\pdv\Controle de Vendas\Setup_Genuss_Automacao"

# Criar diretório de saída se não existir
if (!(Test-Path $outputPath)) {
    New-Item -ItemType Directory -Path $outputPath -Force
    Write-Host "Diretório de saída criado: $outputPath" -ForegroundColor Yellow
}

# Verificar se MSBuild está disponível
$msbuildPath = ""
$possiblePaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\MSBuild\12.0\Bin\MSBuild.exe"
)

foreach ($path in $possiblePaths) {
    if (Test-Path $path) {
        $msbuildPath = $path
        break
    }
}

if ($msbuildPath -eq "") {
    Write-Host "ERRO: MSBuild não encontrado. Instale o Visual Studio ou Build Tools." -ForegroundColor Red
    exit 1
}

Write-Host "MSBuild encontrado em: $msbuildPath" -ForegroundColor Green

# Compilar o projeto principal
Write-Host "Compilando projeto principal..." -ForegroundColor Yellow
try {
    & "$msbuildPath" "$solutionPath" /p:Configuration=Release /p:Platform="Any CPU" /p:OutputPath="$outputPath\\"
    if ($LASTEXITCODE -ne 0) {
        throw "Erro na compilação"
    }
    Write-Host "Compilação concluída com sucesso!" -ForegroundColor Green
} catch {
    Write-Host "ERRO na compilação: $_" -ForegroundColor Red
    exit 1
}

# Copiar dependências necessárias
Write-Host "Copiando dependências..." -ForegroundColor Yellow

$sourceBinPath = "$projectPath\bin\Release"
$targetPath = "$outputPath\Release"

# Criar estrutura de diretórios
if (!(Test-Path $targetPath)) {
    New-Item -ItemType Directory -Path $targetPath -Force
}

# Copiar arquivos principais
$filesToCopy = @(
    "Genuss automacao.exe",
    "Genuss automacao.exe.config",
    "*.dll",
    "*.xml"
)

foreach ($pattern in $filesToCopy) {
    $files = Get-ChildItem -Path $sourceBinPath -Filter $pattern -ErrorAction SilentlyContinue
    foreach ($file in $files) {
        Copy-Item $file.FullName -Destination $targetPath -Force
        Write-Host "Copiado: $($file.Name)" -ForegroundColor Cyan
    }
}

# Copiar diretórios importantes
$dirsToCreate = @("DB", "Xml", "Certificados", "Logs")
foreach ($dir in $dirsToCreate) {
    $sourceDir = "$projectPath\$dir"
    $targetDir = "$targetPath\$dir"
    
    if (Test-Path $sourceDir) {
        if (!(Test-Path $targetDir)) {
            New-Item -ItemType Directory -Path $targetDir -Force
        }
        Copy-Item "$sourceDir\*" -Destination $targetDir -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "Diretório copiado: $dir" -ForegroundColor Cyan
    }
}

# Criar arquivo de informações do sistema
$infoContent = @"
Genuss Automação - Sistema de Vendas
=====================================

Versão: 1.0.0
Data de Compilação: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')
Framework: .NET Framework 4.8

Dependências Principais:
- Microsoft Access Database Engine (ACE.OLEDB.12.0)
- .NET Framework 4.8
- Visual C++ Redistributable

Bibliotecas Incluídas:
- Aspose.PDF (25.8.0)
- Newtonsoft.Json (13.0.3)
- QRCoder (1.6.0)
- iTextSharp (5.5.13.4)
- ZeusNfe (1.5)
- MetroFramework (1.2.0.3)
- System.Data.SQLite (2.0.1)
- ACBrLib.NFe (1.0.14)

Instalação:
1. Execute o Setup_Genuss_Automacao.msi
2. Siga as instruções do assistente
3. Configure o banco de dados na primeira execução
4. Configure os certificados para NFCe se necessário

Suporte:
Para suporte técnico, entre em contato com a equipe de desenvolvimento.
"@

$infoContent | Out-File -FilePath "$targetPath\LEIA-ME.txt" -Encoding UTF8

Write-Host "" 
Write-Host "=== SETUP CRIADO COM SUCESSO ===" -ForegroundColor Green
Write-Host "Localização: $targetPath" -ForegroundColor Yellow
Write-Host "" 
Write-Host "Próximos passos:" -ForegroundColor Cyan
Write-Host "1. Abra o Visual Studio" -ForegroundColor White
Write-Host "2. Crie um novo projeto 'Setup Project' ou 'Visual Studio Installer'" -ForegroundColor White
Write-Host "3. Adicione os arquivos de $targetPath" -ForegroundColor White
Write-Host "4. Configure as dependências (.NET Framework 4.8, Access Database Engine)" -ForegroundColor White
Write-Host "5. Compile o projeto de setup" -ForegroundColor White
Write-Host ""
Write-Host "Arquivos preparados para o instalador:" -ForegroundColor Green
Get-ChildItem $targetPath | ForEach-Object { Write-Host "  - $($_.Name)" -ForegroundColor Gray }

# Abrir pasta de destino
Start-Process explorer.exe $targetPath

Write-Host "" 
Write-Host "Setup preparation completed!" -ForegroundColor Green