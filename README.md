# EasySave v1.0

> File backup application developed by **ProSoft**

---

## 📋 Description

EasySave is a console-based file backup application. It allows users to create, manage and execute up to **5 backup jobs**, copying files from a source directory to a target directory (local, external or network drives).

---

## 🚀 Getting Started

### Prerequisites

- Windows 10 or later
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

```bash
git clone https://github.com/Imrane-Hub5/EasySave.git
cd EasySave/V1
```

### Build

```bash
dotnet build EasySave.sln
```

### Run

**Interactive mode:**
```bash
dotnet run --project EasySave
```

**Command line mode:**
```bash
# Execute job 1
EasySave.exe 1

# Execute jobs 1 to 3
EasySave.exe 1-3

# Execute jobs 1 and 3
EasySave.exe 1;3
```

---

## 📁 Project Structure

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

---

## 🎨 Design Patterns

| Pattern | Where | Why |
|---------|-------|-----|
| **Singleton** | `BackupManager`, `Logger` | Single instance throughout the application |
| **Strategy** | `IBackupStrategy`, `FullBackupStrategy`, `DiffBackupStrategy` | Interchangeable backup algorithms |

---

## ⚙️ Features

- ✅ Create up to **5 backup jobs**
- ✅ **Complete backup** — copies all files
- ✅ **Differential backup** — copies only new or modified files
- ✅ **Bilingual** interface (French / English)
- ✅ **Command line** execution (`EasySave.exe 1-3`)
- ✅ **Daily log** file in JSON format (`EasyLog.dll`)
- ✅ **Real-time state** file (`state.json`)
- ✅ Supports local, external and **network drives**

---

## 📄 Generated Files

| File | Location | Description |
|------|----------|-------------|
| Daily log | `%AppData%\EasySave\Logs\YYYY-MM-DD.json` | Transfer history |
| Real-time state | `%AppData%\EasySave\state.json` | Current backup progress |
| Configuration | `%AppData%\EasySave\config.json` | Saved backup jobs |

---

## 👥 Team

| Developer | Files |
|-----------|-------|
| **Quentin** | `EasySave.sln`, `EasySave.csproj`, `EasyLog.csproj`, `LogEntry.cs`, `Enums.cs` |
| **Hager** | `IBackupStrategy.cs`, `FullBackupStrategy.cs`, `JobState.cs`, `Logger.cs` |
| **Imrane** | `DiffBackupStrategy.cs`, `StateManager.cs`, `BackupJob.cs` |
| **Ghada** | `LanguageManager.cs`, `BackupManager.cs`, `Program.cs` |

---

## 📦 Version

| Version | Date | Description |
|---------|------|-------------|
| 1.0 | 2026 | Initial release — console application |

---

*ProSoft — EasySave v1.0 — 2026*
