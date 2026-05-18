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

## 📁 V3 — WPF Graphical Application + Parallel Backup

### What's new in v3.0?
- ✅ **Parallel backup** — all jobs run simultaneously via `Task.WhenAll()`
- ✅ **Priority files** — priority extensions processed before others
- ✅ **Large file limit** — only one file above n Ko transferred at a time (`SemaphoreSlim`)
- ✅ **Pause / Play / Stop** — real-time control per job (`CancellationToken` + `ManualResetEventSlim`)
- ✅ **Business software auto-pause** — all jobs pause automatically, resume when closed
- ✅ **CryptoSoft mono-instance** — global `Mutex` prevents multiple simultaneous instances
- ✅ **Centralized logs via Docker** — HTTP POST to a remote log server (Local / Remote / Both)

### Run
```bash
cd EasySave/V3
dotnet build EasySave.sln
dotnet run --project EasySave
```

### Docker (log server)
```bash
cd V3/LogServer
docker build -t easysave-logserver .
docker run -d -p 5000:5000 --name easysave-logs easysave-logserver
docker logs easysave-logs
```

### Command line mode
```bash
EasySave.exe 1        # Execute job 1
EasySave.exe 1-3      # Execute jobs 1 to 3
EasySave.exe 1;3      # Execute jobs 1 and 3
```

### Project Structure
```
V3/
├── EasySave/
│   ├── Models/
│   │   ├── BackupJob.cs               # Updated - ExecuteAsync, parallel, priority
│   │   ├── JobState.cs
│   │   └── Enums.cs
│   ├── Services/
│   │   ├── BackupManager.cs           # Updated - Task.WhenAll()
│   │   ├── StateManager.cs
│   │   ├── LanguageManager.cs
│   │   ├── Settings.cs                # Updated - PriorityExtensions, MaxParallelFileSizeKo, LogDestination, DockerServerUrl
│   │   ├── CryptoSoftService.cs       # Updated - global Mutex mono-instance
│   │   ├── BusinessSoftwareService.cs # Updated - auto-pause via IsBlocked
│   │   ├── BackupJobController.cs     # NEW - Pause/Play/Stop per job
│   │   ├── BackupSemaphore.cs         # NEW - SemaphoreSlim for large files
│   │   └── PriorityQueue.cs           # NEW - priority file extensions management
│   ├── Strategies/
│   │   ├── IBackupStrategy.cs
│   │   ├── FullBackupStrategy.cs
│   │   └── DiffBackupStrategy.cs
│   └── EasySave.csproj
├── EasyLog/
│   ├── Logger.cs                      # Updated - Local/Remote/Both destination
│   ├── LogEntry.cs
│   ├── ILogFormatter.cs
│   ├── JsonFormatter.cs
│   ├── XmlFormatter.cs
│   └── EasyLog.csproj
├── EasySaveUI/
│   ├── MainWindow.xaml                # Updated - Pause/Play/Stop buttons, progress bar
│   ├── MainWindow.xaml.cs
│   ├── AddJobWindow.xaml
│   ├── AddJobWindow.xaml.cs
│   ├── SettingsWindow.xaml            # Updated - Docker URL, log destination, priority extensions
│   ├── SettingsWindow.xaml.cs
│   ├── MainViewModel.cs               # Updated - monitoring thread
│   ├── BackupJobViewModel.cs          # Updated - PauseCommand, PlayCommand, StopCommand
│   ├── SettingsViewModel.cs
│   └── RelayCommand.cs
├── CryptoSoft/
│   ├── Program.cs                     # Updated - Mutex mono-instance
│   └── CryptoSoft.csproj
└── LogServer/
    ├── Program.cs                     # NEW - ASP.NET Core minimal API
    ├── Dockerfile                     # NEW - Docker image
    └── LogServer.csproj
```

### Design Patterns
| Pattern | Where | Why |
|---------|-------|-----|
| **Singleton** | `BackupManager`, `Logger` | Single instance throughout the application |
| **Strategy** | `IBackupStrategy`, `FullBackupStrategy`, `DiffBackupStrategy` | Interchangeable backup algorithms |
| **Strategy** | `ILogFormatter`, `JsonFormatter`, `XmlFormatter` | Interchangeable log formats |
| **MVVM** | `MainViewModel`, `BackupJobViewModel`, `SettingsViewModel` | Separation of UI and business logic |
| **Observer** | `INotifyPropertyChanged` | Real-time UI updates |
| **Mutex** | `CryptoSoftService`, `CryptoSoft/Program.cs` | CryptoSoft mono-instance |
| **SemaphoreSlim** | `BackupSemaphore` | Limit simultaneous large file transfers |

### Priority Files
- Configure priority extensions in Settings (e.g. `.pdf`, `.docx`)
- Non-priority files wait until all priority files are transferred
- Managed by `PriorityQueue` static class with thread-safe `lock`

### Large File Limit
- Configure `MaxParallelFileSizeKo` in Settings (default: 15360 Ko)
- Only one file above the threshold can be transferred at a time
- Smaller files transfer freely in parallel

### Pause / Play / Stop
- Each job has its own `BackupJobController`
- **Pause**: pauses after current file completes (`ManualResetEventSlim`)
- **Play**: resumes from where it stopped
- **Stop**: stops immediately at next file iteration (`CancellationToken`)

### Business Software (v3.0 behaviour)
- Background thread checks every 1000ms if software is running
- If detected: all jobs **pause automatically**
- When closed: all jobs **resume automatically**

### Docker — Centralized Logs
- Log destination configurable in Settings: `Local` / `Remote` / `Both`
- Configure Docker server URL (e.g. `http://localhost:5000`)
- EasySave sends each log entry via `HTTP POST /api/logs`
- One centralized daily log file regardless of number of machines

---

## 📦 Versions

| Version | Date | Description |
|---------|------|-------------|
| **1.0** | April 2026 | Initial release — console application, 5 jobs max, JSON logs |
| **1.1** | May 2026 | JSON/XML log format selection |
| **2.0** | May 2026 | WPF interface, unlimited jobs, CryptoSoft encryption, business software detection |
| **3.0** | May 2026 | Parallel backup, priority files, Pause/Play/Stop, Mutex, Docker centralized logs |

---

*ProSoft — EasySave — 2026*

