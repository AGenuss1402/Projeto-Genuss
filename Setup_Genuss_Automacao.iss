; Script do Inno Setup para Genuss Automação
; Compilar com Inno Setup 6.0 ou superior

#define MyAppName "Genuss Automação - Sistema de Vendas"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Genuss Automação"
#define MyAppURL "https://www.genussautomacao.com.br"
#define MyAppExeName "Genuss automacao.exe"
#define MyAppAssocName MyAppName + " File"
#define MyAppAssocExt ".gns"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; NOTE: O valor de AppId identifica unicamente esta aplicação. Não use o mesmo AppId para outros instaladores.
; (Para gerar um novo GUID, clique em Tools | Generate GUID dentro do IDE.)
AppId={{12345678-ABCD-EFGH-1234-567890ABCDEF}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppPublisher}\{#MyAppName}
ChangesAssociations=yes
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=F:\pdv\pdv\Controle de Vendas\LICENSE.txt
InfoBeforeFile=F:\pdv\pdv\Controle de Vendas\README_INSTALACAO.txt
OutputDir=F:\pdv\pdv\Controle de Vendas\Setup_Output
OutputBaseFilename=Setup_Genuss_Automacao_v{#MyAppVersion}
SetupIconFile=F:\pdv\pdv\Controle de Vendas\Controle de Vendas\Favicon 24.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
; Ajuste para instalação em Windows 32 bits
ArchitecturesAllowed=x86
ArchitecturesInstallIn64BitMode=

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1

[Files]
; Arquivo principal
Source: "F:\pdv\pdv\Controle de Vendas\Controle de Vendas\bin\Release\Genuss automacao.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\pdv\pdv\Controle de Vendas\Controle de Vendas\bin\Release\Genuss automacao.exe.config"; DestDir: "{app}"; Flags: ignoreversion

; Dependências .NET
Source: "F:\pdv\pdv\Controle de Vendas\Controle de Vendas\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\pdv\pdv\Controle de Vendas\Controle de Vendas\bin\Release\*.xml"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist


; Diretórios de configuração
Source: "F:\pdv\pdv\Controle de Vendas\Controle de Vendas\Xml\*"; DestDir: "{app}\Xml"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "F:\pdv\pdv\Controle de Vendas\Controle de Vendas\Certificados\*"; DestDir: "{app}\Certificados"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "F:\pdv\pdv\Controle de Vendas\Controle de Vendas\icons\*"; DestDir: "{app}\icons"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist

; Documentação
Source: "F:\pdv\pdv\Informações\*"; DestDir: "{app}\Documentacao"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist

; NOTA: Não use "Flags: ignoreversion" em arquivos compartilhados do sistema

[Registry]
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
// Função para verificar se .NET Framework 4.8 está instalado
function IsDotNet48Installed: Boolean;
var
  Success: Boolean;
  InstallSuccess: Boolean;
  Version: Cardinal;
begin
  Success := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', Version);
  Result := Success and (Version >= 528040); // .NET Framework 4.8
end;

// Função para verificar se Access Database Engine está instalado
function IsAccessEngineInstalled: Boolean;
var
  Success: Boolean;
begin
  Success := RegKeyExists(HKLM, 'SOFTWARE\Classes\Microsoft.ACE.OLEDB.12.0');
  if not Success then
    Success := RegKeyExists(HKLM, 'SOFTWARE\WOW6432Node\Classes\Microsoft.ACE.OLEDB.12.0');
  Result := Success;
end;

// Função executada antes da instalação
function InitializeSetup(): Boolean;
var
  ErrorCode: Integer;
  DotNetMissing: Boolean;
  AccessEngineMissing: Boolean;
  Message: String;
begin
  Result := True;
  DotNetMissing := not IsDotNet48Installed;
  AccessEngineMissing := not IsAccessEngineInstalled;
  
  if DotNetMissing or AccessEngineMissing then
  begin
    Message := 'Para instalar o Genuss Automação, você precisa dos seguintes pré-requisitos:' + #13#10#13#10;
    
    if DotNetMissing then
      Message := Message + '• Microsoft .NET Framework 4.8 ou superior' + #13#10;
      
    if AccessEngineMissing then
      Message := Message + '• Microsoft Access Database Engine 2016 Redistributable' + #13#10;
      
    Message := Message + #13#10 + 'Deseja baixar e instalar os pré-requisitos agora?';
    
    if MsgBox(Message, mbConfirmation, MB_YESNO) = IDYES then
    begin
      if DotNetMissing then
      begin
        ShellExec('open', 'https://dotnet.microsoft.com/download/dotnet-framework/net48', '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
      end;
      
      if AccessEngineMissing then
      begin
        ShellExec('open', 'https://www.microsoft.com/download/details.aspx?id=54920', '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
      end;
      
      MsgBox('Por favor, instale os pré-requisitos e execute este instalador novamente.', mbInformation, MB_OK);
      Result := False;
    end
    else
    begin
      Result := False;
    end;
  end;
end;

// Função executada após a instalação
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // Criar diretórios necessários se não existirem
    if not DirExists(ExpandConstant('{app}\Logs')) then
      CreateDir(ExpandConstant('{app}\Logs'));
      
    if not DirExists(ExpandConstant('{app}\Backup')) then
      CreateDir(ExpandConstant('{app}\Backup'));
      
    if not DirExists(ExpandConstant('{app}\Temp')) then
      CreateDir(ExpandConstant('{app}\Temp'));
  end;
end;

[UninstallDelete]
Type: filesandordirs; Name: "{app}\Logs"
Type: filesandordirs; Name: "{app}\Temp"

[Messages]
brazilianportuguese.WelcomeLabel2=Este assistente irá instalar o [name/ver] em seu computador.%n%nO Genuss Automação é um sistema completo de ponto de venda com suporte a NFCe, controle de estoque e relatórios gerenciais.%n%nÉ recomendado que você feche todos os outros aplicativos antes de continuar.
brazilianportuguese.FinishedHeadingLabel=Concluindo o Assistente de Instalação do [name]
brazilianportuguese.FinishedLabelNoIcons=A instalação do [name] foi concluída com sucesso.
brazilianportuguese.FinishedLabel=A instalação do [name] foi concluída com sucesso. O aplicativo pode ser executado clicando nos ícones instalados.
brazilianportuguese.ClickFinish=Clique em Concluir para sair do Assistente de Instalação.
brazilianportuguese.FinishedRestartLabel=Para concluir a instalação do [name], o Assistente de Instalação deve reiniciar o computador. Deseja reiniciar agora?
brazilianportuguese.FinishedRestartMessage=Para concluir a instalação do [name], o Assistente de Instalação deve reiniciar o computador.%n%nDeseja reiniciar agora?