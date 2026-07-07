; Inno Setup Script - MaliyeHesaplama
; Derlemek için: iscc setup.iss

#define MyAppName "MaliyeHesaplama"
#define MyAppVersion "1.2"
#define MyAppPublisher "MaliyeHesaplama"
#define MyAppURL ""
#define MyAppExeName "MaliyeHesaplama.exe"

[Setup]
AppId={{B8A3C8D1-2E4F-4A7B-9C5D-1E2F3A4B5C6D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=output
OutputBaseFilename=MaliyeHesaplama_Setup_v{#MyAppVersion}
SetupIconFile=assets\icons\app.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin

[Languages]
Name: "turkish"; MessagesFile: "compiler:Languages\Turkish.isl"

[Tasks]
Name: "desktopicon"; Description: "Masaüstü kısayolu oluştur"; GroupDescription: "Kısayollar:"
Name: "desktopicon\common"; Description: "Tüm kullanıcılar için"; GroupDescription: "Kısayollar:"; Flags: exclusive

[Files]
; Ana uygulama
Source: "bin\Release\net6.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net6.0-windows\MaliyeHesaplama.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net6.0-windows\MaliyeHesaplama.dll.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net6.0-windows\MaliyeHesaplama.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net6.0-windows\MaliyeHesaplama.deps.json"; DestDir: "{app}"; Flags: ignoreversion

; Bağımlılıklar
Source: "bin\Release\net6.0-windows\*.dll"; DestDir: "{app}"; Flags: ignoreversion

; Konfigürasyon (varsa koru)
Source: "bin\Release\net6.0-windows\dbconfig.json"; DestDir: "{app}"; Flags: ignoreversion onlyifdoesntexist

; Alt dizinler
Source: "bin\Release\net6.0-windows\runtimes\*"; DestDir: "{app}\runtimes"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\cs-CZ\*"; DestDir: "{app}\cs-CZ"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\de\*"; DestDir: "{app}\de"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\es\*"; DestDir: "{app}\es"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\fr\*"; DestDir: "{app}\fr"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\hu\*"; DestDir: "{app}\hu"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\it\*"; DestDir: "{app}\it"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\ja-JP\*"; DestDir: "{app}\ja-JP"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\pt-BR\*"; DestDir: "{app}\pt-BR"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\ro\*"; DestDir: "{app}\ro"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\ru\*"; DestDir: "{app}\ru"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\sv\*"; DestDir: "{app}\sv"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "bin\Release\net6.0-windows\zh-Hans\*"; DestDir: "{app}\zh-Hans"; Flags: ignoreversion createallsubdirs recursesubdirs

; Uygulama ikonu (exe yanında)
Source: "assets\icons\app.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "{app}\{#MyAppExeName}"; Parameters: "/uninstall"; Flags: runhidden

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    if not FileExists(ExpandConstant('{app}\dbconfig.json')) then
    begin
      if MsgBox('dbconfig.json bulunamadı. Uygulama çalışmadan önce bir veritabanı konfigürasyon dosyası gereklidir.' + #13#10 +
                'Devam etmek istiyor musunuz?', mbConfirmation, MB_YESNO) = IDNO then
      begin
        Abort;
      end;
    end;
  end;
end;
