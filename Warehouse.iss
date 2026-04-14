; Скрипт создан для приложения Warehouse (C# + React)
#define ToolName "Warehouse"
#define ToolVersion "1.0"
#define ToolPublisher "Student Project"
#define MainExeName "BackendApp.exe"

[Setup]
; Уникальный идентификатор приложения (можно сгенерировать новый в Tools -> Generate GUID)
AppId={{8B3A2C1D-4E5F-4A9B-B0C1-D2E3F4A5B6C7}
AppName={#ToolName}
AppVersion={#ToolVersion}
AppPublisher={#ToolPublisher}
DefaultDirName={autopf}\{#ToolName}
DisableProgramGroupPage=yes
; Куда сохранить готовый инсталлер
OutputDir=D:\Study\TO\Project\Installer
OutputBaseFilename=Warehouse_Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; 1. Копируем все файлы бэкенда из папки win-x64
Source: "D:\Study\TO\Project\C#\BackendApp\BackendApp\bin\Release\net8.0\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; Исключаем лишние файлы, если они есть (опционально)
; Excludes: "*.pdb,*.obj"

; 2. Копируем файлы фронтенда React в папку wwwroot приложения
Source: "D:\Study\TO\Project\React\FrontendApp\frontend\build\*"; DestDir: "{app}\wwwroot"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#ToolName}"; Filename: "{app}\{#MainExeName}"
Name: "{autodesktop}\{#ToolName}"; Filename: "{app}\{#MainExeName}"; Tasks: desktopicon

[Run]
Description: "{cm:LaunchProgram,{#StringChange(ToolName, '&', '&&')}}"; Filename: "{app}\{#MainExeName}"; Flags: nowait postinstall skipifsilent