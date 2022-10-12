; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Modelling Wizard +"
#define MyAppVersion "2.0.0.1"
#define MyAppPublisher "Murr Elektronik"
#define MyAppURL "https://www.murrelektronik.com/de/"
#define MyAppExeName "Modelling Wizard +.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{AEDE8E67-443A-4C1C-8348-80B170DCF30B}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\Murr Elektronik\Modelling Wizard
DefaultGroupName=Modelling Wizard
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
PrivilegesRequired=admin
;PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\xsensors\Modelling Wizard\INSTALLER
DisableWelcomePage=no
OutputBaseFilename=Modelling Wizard Installer
Compression=lzma
SolidCompression=yes
WizardStyle=modern
;WizardImageFile=WizMURR.bmp
;WizardSmallImageFile=WizMURRSmal.bmp

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\xsensors\Modelling Wizard\SOURCE\Application\bin\Debug\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\xsensors\Modelling Wizard\SOURCE\Application\bin\Debug\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\xsensors\Modelling Wizard\INSTALLER\changelog\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "C:\xsensors\Modelling Wizard\SOURCE\Application\packages\SevenZipSharp.Interop.19.0.2\build\x64\*"; DestDir: "{userappdata}\MURR\x64"; Flags: createallsubdirs recursesubdirs
Source: "C:\xsensors\Modelling Wizard\SOURCE\Application\packages\SevenZipSharp.Interop.19.0.2\build\x86\*"; DestDir: "{userappdata}\MURR\x86"; Flags: createallsubdirs recursesubdirs
Source: "C:\xsensors\Modelling Wizard\SOURCE\Application\Templates\*"; DestDir: "{userappdata}\MURR\Templates"; Flags: createallsubdirs recursesubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"; ValueType: expandsz; ValueName: "Path"; ValueData: "{olddata};{app}" 

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

