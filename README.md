# EasySave
> File backup application developed by **ProSoft**

---

## 📋 Description

EasySave is a file backup application that allows users to create, manage and execute backup jobs, copying files from a source directory to a target directory (local, external or network drives).

The application has been developed in 3 versions:
- **v1.0** — Console application, up to 5 backup jobs, JSON logs
- **v1.1** — Console application, adds JSON/XML log format selection
- **v2.0** — WPF graphical application, unlimited jobs, encryption, business software detection

---

## 🚀 Getting Started

### Prerequisites
- Windows 10 or later
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation
```bash
git clone https://github.com/Imrane-Hub5/EasySave.git
```

---

## 📁 V1 — Console Application

### Run
```bash
cd EasySave/V1
dotnet build EasySave.sln
dotnet run --project EasySave
```

### Command line mode
```bash
EasySave.exe 1        # Execute job 1
EasySave.exe 1-3      # Execute jobs 1 to 3
EasySave.exe 1;3      # Execute jobs 1 and 3
```

### Project Structure
```
V1/
├── EasySave/
│   ├── Models/
│   │   ├── BackupJob.cs          # Backup job model
│   │   ├── JobState.cs           # Real-time state model
│   │   └── Enums.cs              # BackupType, Language enums
│   ├── Services/
│   │   ├── BackupManager.cs      # Singleton - manages all jobs
│   │   ├── StateManager.cs       # Writes state.json in real time
│   │   └── LanguageManager.cs    # Bilingual support (FR/EN)
│   ├── Strategies/
│   │   ├── IBackupStrategy.cs    # Strategy interface
│   │   ├── FullBackupStrategy.cs # Complete backup
│   │   └── DiffBackupStrategy.cs # Differential backup
│   ├── Program.cs                # Entry point - interactive menu
│   └── EasySave.csproj
├── EasyLog/
│   ├── Logger.cs                 # Singleton - daily JSON log
│   ├── LogEntry.cs               # Log entry model
│   └── EasyLog.csproj
└── EasySave.sln
```

### Features
- ✅ Create up to **5 backup jobs**
- ✅ **Complete backup** — copies all files
- ✅ **Differential backup** — copies only new or modified files
- ✅ **Bilingual** interface (French / English)
- ✅ **Command line** execution (`EasySave.exe 1-3`)
- ✅ **Daily log** file in JSON format (`EasyLog.dll`)
- ✅ **Real-time state** file (`state.json`)
- ✅ Supports local, external and **network drives**

### Design Patterns
| Pattern | Where | Why |
|---------|-------|-----|
| **Singleton** | `BackupManager`, `Logger` | Single instance throughout the application |
| **Strategy** | `IBackupStrategy`, `FullBackupStrategy`, `DiffBackupStrategy` | Interchangeable backup algorithms |

---

## 📁 V1.1 — Console Application + XML Log Format

### What's new in v1.1?
- ✅ User can choose log format: **JSON or XML**
- ✅ Log format selection displayed in the user's chosen language
- ✅ New `ILogFormatter` interface (Strategy pattern)
- ✅ `JsonFormatter` and `XmlFormatter` implementations
- ✅ `Settings.cs` — persists user preferences

### Run
```bash
cd EasySave/V1.1
dotnet build EasySave.sln
dotnet run --project EasySave
```

### Project Structure
```
V1.1/
├── EasySave/
│   ├── Models/
│   │   ├── BackupJob.cs
│   │   ├── JobState.cs
│   │   └── Enums.cs
│   ├── Services/
│   │   ├── BackupManager.cs      # Loads log format from Settings
│   │   ├── StateManager.cs
│   │   ├── LanguageManager.cs    # Added log format translation keys
│   │   └── Settings.cs           # NEW - user preferences (LogFormat)
│   ├── Strategies/
│   │   ├── IBackupStrategy.cs
│   │   ├── FullBackupStrategy.cs
│   │   └── DiffBackupStrategy.cs
│   ├── Program.cs                # Added log format selection
│   └── EasySave.csproj
├── EasyLog/
│   ├── Logger.cs                 # Updated - uses ILogFormatter
│   ├── LogEntry.cs
│   ├── ILogFormatter.cs          # NEW - Strategy interface
│   ├── JsonFormatter.cs          # NEW - JSON formatting
│   ├── XmlFormatter.cs           # NEW - XML formatting
│   └── EasyLog.csproj
└── EasySave.sln
```

### Design Patterns
| Pattern | Where | Why |
|---------|-------|-----|
| **Singleton** | `BackupManager`, `Logger` | Single instance throughout the application |
| **Strategy** | `IBackupStrategy`, `FullBackupStrategy`, `DiffBackupStrategy` | Interchangeable backup algorithms |
| **Strategy** | `ILogFormatter`, `JsonFormatter`, `XmlFormatter` | Interchangeable log formats |

---

## 📁 V2 — WPF Graphical Application

### What's new in v2.0?
- ✅ **WPF graphical interface** — replaces console
- ✅ **MVVM architecture** — `MainViewModel`, `BackupJobViewModel`, `SettingsViewModel`
- ✅ **Observer pattern** — real-time UI updates via `INotifyPropertyChanged`
- ✅ **Unlimited backup jobs** — no more limit of 5
- ✅ **File encryption** via CryptoSoft.exe
- ✅ **Business software detection** — blocks backup if detected
- ✅ **JSON or XML** log format (inherited from v1.1)
- ✅ `EncryptionTime` added to log entries

### Run
```bash
cd EasySave/V2
dotnet build EasySave.sln
dotnet run --project EasySave
```

### Command line mode
```bash
EasySave.exe 1        # Execute job 1
EasySave.exe 1-3      # Execute jobs 1 to 3
EasySave.exe 1;3      # Execute jobs 1 and 3
```

### Project Structure
```
V2/
├── EasySave/
│   ├── Models/
│   │   ├── BackupJob.cs          # Updated - CryptoSoft + BusinessSoftware
│   │   ├── JobState.cs
│   │   └── Enums.cs
│   ├── Services/
│   │   ├── BackupManager.cs      # Updated - unlimited jobs
│   │   ├── StateManager.cs
│   │   ├── LanguageManager.cs
│   │   ├── Settings.cs           # Updated - BusinessSoftware + EncryptedExtensions
│   │   ├── CryptoSoftService.cs  # NEW - calls CryptoSoft.exe
│   │   └── BusinessSoftwareService.cs # NEW - detects running processes
│   ├── Strategies/
│   │   ├── IBackupStrategy.cs
│   │   ├── FullBackupStrategy.cs
│   │   └── DiffBackupStrategy.cs
│   ├── Program.cs                # Updated - WPF startup
│   └── EasySave.csproj
├── EasyLog/
│   ├── Logger.cs
│   ├── LogEntry.cs               # Updated - EncryptionTime added
│   ├── ILogFormatter.cs
│   ├── JsonFormatter.cs
│   ├── XmlFormatter.cs
│   └── EasyLog.csproj
├── EasySaveUI/
│   ├── MainWindow.xaml           # NEW - main WPF window
│   ├── MainWindow.xaml.cs
│   ├── AddJobWindow.xaml         # NEW - add job dialog
│   ├── AddJobWindow.xaml.cs
│   ├── SettingsWindow.xaml       # NEW - settings dialog
│   ├── SettingsWindow.xaml.cs
│   ├── MainViewModel.cs          # NEW - MVVM main viewmodel
│   ├── BackupJobViewModel.cs     # NEW - per-job viewmodel
│   ├── SettingsViewModel.cs      # NEW - settings viewmodel
│   └── RelayCommand.cs           # NEW - ICommand implementation
└── EasySave.sln
```

### Design Patterns
| Pattern | Where | Why |
|---------|-------|-----|
| **Singleton** | `BackupManager`, `Logger` | Single instance throughout the application |
| **Strategy** | `IBackupStrategy`, `FullBackupStrategy`, `DiffBackupStrategy` | Interchangeable backup algorithms |
| **Strategy** | `ILogFormatter`, `JsonFormatter`, `XmlFormatter` | Interchangeable log formats |
| **MVVM** | `MainViewModel`, `BackupJobViewModel`, `SettingsViewModel` | Separation of UI and business logic |
| **Observer** | `INotifyPropertyChanged` | Real-time UI updates |

### Encryption (CryptoSoft)
CryptoSoft.exe is an external tool that encrypts files using XOR algorithm.
- Configure the path to `CryptoSoft.exe` in Settings
- Define file extensions to encrypt (e.g. `.txt`, `.docx`)
- Encryption time is logged in ms (`0` = no encryption, `>0` = ms, `<0` = error)

### Business Software Detection
- Configure a business software name in Settings (e.g. `calc`)
- If detected **before** backup: backup is blocked
- If detected **during** backup: current file completes then backup stops
- Stop event is written to the log file

---

## 📄 Generated Files

| File | Location | Description |
|------|----------|-------------|
| Daily log | `%AppData%\EasySave\Logs\YYYY-MM-DD.json` (or `.xml`) | Transfer history |
| Real-time state | `%AppData%\EasySave\state.json` | Current backup progress |
| Configuration | `%AppData%\EasySave\config.json` | Saved backup jobs |
| Settings | `%AppData%\EasySave\settings.json` | User preferences |

---

## 👥 Team

| Developer | Files |
|-----------|-------|
| **Quentin** | `EasySave.sln`, `EasySave.csproj`, `EasyLog.csproj`, `LogEntry.cs`, `Enums.cs` |
| **Hager** | `IBackupStrategy.cs`, `FullBackupStrategy.cs`, `JobState.cs`, `Logger.cs` |
| **Imrane** | `DiffBackupStrategy.cs`, `StateManager.cs`, `BackupJob.cs` |
| **Ghada** | `LanguageManager.cs`, `BackupManager.cs`, `Program.cs` |

---

## 📦 Versions

| Version | Date | Description |
|---------|------|-------------|
| **1.0** | April 2026 | Initial release — console application, 5 jobs max, JSON logs |
| **1.1** | May 2026 | JSON/XML log format selection |
| **2.0** | May 2026 | WPF interface, unlimited jobs, CryptoSoft encryption, business software detection |

---

*ProSoft — EasySave — 2026*
