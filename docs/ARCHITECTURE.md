# GTAMods Architecture

## Overview

GTAMods is built as a modular system with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│         GTAMods.Launcher                │  (Entry point, .exe)
└────────────────────┬────────────────────┘
                     │
┌────────────────────▼────────────────────┐
│         GTAMods.UI (WPF)                │  (User Interface)
│  ├── MainWindow                          │
│  ├── ModListView                         │
│  ├── SettingsView                        │
│  └── PresetView                          │
└────────────────────┬────────────────────┘
                     │
┌────────────────────▼────────────────────┐
│       GTAMods.Core (Business Logic)     │
│  ├── GameDetector                        │  Auto/Manual game detection
│  ├── ModManager                          │  Mod enable/disable/list
│  ├── DependencyChecker                   │  Validate mod files
│  ├── ConfigManager                       │  JSON config handling
│  ├── BackupManager                       │  Versioned backups
│  ├── PresetManager                       │  Save/load profiles
│  └── ProcessMonitor                      │  Multiplayer detection
└────────────────────┬────────────────────┘
                     │
┌────────────────────▼────────────────────┐
│         File System & Models             │
│  ├── C:\GTAMods\                         │
│  ├── config.json                         │
│  ├── Game folders (III, VC, SA, IV, V)   │
│  ├── Mods\                               │
│  ├── Backups\                            │
│  └── Presets\                            │
└─────────────────────────────────────────┘
```

## Core Components

### GameDetector
- Auto-detects GTA installations in default Rockstar paths
- Supports manual game folder selection
- Validates game executables exist
- Returns detected game information

### ModManager
- Scans mod directories
- Lists installed mods with status (enabled/disabled)
- Manages mod enable/disable state
- Tracks load order

### DependencyChecker
- Validates mod file structure
- Detects missing DLLs, INI files, config
- Checks inter-mod dependencies
- Generates validation reports

### ConfigManager
- Loads/saves JSON config files
- Manages GTAMods settings
- Stores game and mod information
- Persists user preferences

### BackupManager (Phase 2)
- Creates versioned backups of modified files
- Implements restore functionality
- Manages backup storage and cleanup

### PresetManager (Phase 3)
- Saves mod configurations as presets
- Loads presets to restore mod states
- Stores preset metadata

### ProcessMonitor (Phase 2)
- Monitors GTA game process
- Detects multiplayer mode
- Triggers auto-disable of mods
- Reports network activity

## Data Models

### GameInfo
```csharp
public class GameInfo
{
    public string GameName;          // "GTA V"
    public string GameCode;          // "V"
    public string GamePath;          // Path to game installation
    public string GTAModsPath;       // Path to GTAMods folder for this game
    public string ModsPath;          // Mods directory
    public string BackupsPath;       // Backups directory
    public string PresetsPath;       // Presets directory
    public bool IsInstalled;         // Is game detected?
    public List<string> InstalledMods; // List of mod names
}
```

### ModInfo
```csharp
public class ModInfo
{
    public string ModName;           // Mod folder name
    public string ModPath;           // Full path to mod
    public bool IsEnabled;           // Is mod active?
    public bool IsDefault;           // Is it a default mod?
    public List<string> Dependencies; // Required mods
    public List<string> RequiredFiles; // DLLs, INIs, etc.
    public int LoadOrder;            // Load priority
    public bool HasMissingDependencies; // Validation flag
    public List<string> MissingFiles; // Missing file list
}
```

### Config
```csharp
public class Config
{
    public string GTAModsRootPath;   // C:\GTAMods
    public bool UseAutoDetection;    // Auto-detect games?
    public Dictionary<string, GameInfo> Games; // Detected games
    public int MaxBackupVersions;    // How many backups to keep
    public bool AutoBackupOnModChange; // Auto-backup?
    public bool MonitorMultiplayer;  // Monitor for online mode?
    public bool AutoDisableModsForOnline; // Disable mods for MP?
}
```

## File Structure

```
C:\GTAMods\
├── config.json                   # Main configuration
├── GTA3\
│   ├── Mods\
│   │   ├── ModName1\
│   │   │   ├── mod.dll
│   │   │   ├── mod.ini
│   │   │   └── ...
│   │   └── ModName2\
│   ├── Backups\
│   │   ├── backup_gta3_v1.0.zip
│   │   ├── backup_gta3_v1.1.zip
│   │   └── ...
│   └── Presets\
│       ├── Preset_Graphics.json
│       ├── Preset_Roleplay.json
│       └── ...
├── VC\
├── SA\
├── IV\
└── V\
```

## Execution Flow

### First Launch
1. GTAMods.exe starts
2. ConfigManager loads config.json (or creates default)
3. User sees: "Auto-detect games in default location?"
   - YES: GameDetector auto-detects all games
   - NO: User manually selects each game folder
4. For each detected game:
   - Create folder structure (Mods, Backups, Presets)
   - Load default mods config
   - Scan mod directories
   - Enable default mods (Traffic Overhaul, Police AI, etc.)
5. UI displays detected games and mod status

### Playing with Mods
1. User clicks "Play GTA V"
2. ProcessMonitor starts monitoring game process
3. Game launches with enabled mods
4. If multiplayer detected: mods auto-disable
5. If single-player: mods remain active
6. User can pause and access Pause Menu → Options → Mods

## Phase-Based Development

### Phase 1: Foundation (Current)
- Game detection
- Mod manager
- Config system
- Basic UI
- Default mods loading

### Phase 2: Smart Features
- Dependency resolution
- Load order management
- Backup system
- Game launcher
- Process monitoring
- Multiplayer detection

### Phase 3: Advanced Features
- Preset system
- Vehicle manager
- Mod browser
- Graphics conflict detection
- Fallback recovery

### Phase 4: Roleplay Systems
- Police system
- Gang system
- Career tracking
- Economy system

### Phase 5: Creator Tools
- Mod packaging
- No-code mod creation
- Testing environment

## Dependencies

- **.NET Framework 4.8+**: Core runtime
- **WPF**: Windows Presentation Foundation for UI
- **Newtonsoft.Json (NuGet)**: JSON serialization/deserialization
- **System.Diagnostics**: Process monitoring
- **System.Net.NetworkInformation**: Network monitoring (future)

## Error Handling Strategy

- Try-catch in all file I/O operations
- Validation before file operations
- User-friendly error messages
- Logging to console for debugging
- Graceful fallback to defaults

## Security Considerations

- No automatic downloads (future browser will ask confirmation)
- Backup before modifications
- Multiplayer mode auto-disable prevents bans
- No execution of unsigned code
- Config validation before applying

## Performance Targets

- Startup: < 2 seconds
- Mod scanning: < 1 second
- Game detection: < 3 seconds
- Mod load: < 1 second per mod

## Testing Strategy

- Unit tests for core components
- Manual testing on each supported game
- Edge case testing (missing files, corrupted mods)
- Performance profiling
