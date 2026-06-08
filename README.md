# GTAMods - Universal GTA Mod Manager

**GTAMods** is a unified mod manager for Grand Theft Auto games (GTA III, Vice City, San Andreas, GTA IV, GTA V) that simplifies mod installation, management, and configuration.

## Features

- 🎮 **Multi-Game Support**: Manages mods for GTA III, VC, SA, IV, and V
- 🔧 **Auto-Detection**: Automatically finds game installations or manual selection
- 📦 **Mod Manager**: Enable/disable mods without reinstalling
- 🎯 **Preset System**: Save and load mod configurations
- 🛡️ **Recovery System**: Backup and restore modified game files
- ⚠️ **Multiplayer Safety**: Automatically detects and disables mods for online play
- 🚗 **Vehicle Manager**: Specialized management for vehicle mods with in-game spawn menu
- 🔄 **Smart Dependency**: Auto-detects missing files and mod conflicts
- 🎨 **Pre-installed Mods**: Traffic Overhaul, Police AI, Driving Physics, Economy, Graphics

## Supported Games

| Game | Year | Status |
|------|------|--------|
| GTA III | 2001 | ✅ Supported |
| GTA: Vice City | 2002 | ✅ Supported |
| GTA: San Andreas | 2004 | ✅ Supported |
| GTA IV | 2008 | ✅ Supported |
| GTA V | 2013 | ✅ Supported |

## Installation

### Requirements
- Windows 7 or later
- .NET Framework 4.8 or later
- At least one GTA game installed

### Build from Source

1. **Clone the repository**
   ```bash
   git clone https://github.com/Stryk3rr/GTAMods.git
   cd GTAMods
   ```

2. **Open in Visual Studio**
   - Open `GTAMods.sln`

3. **Build the project**
   - Press `Ctrl+Shift+B` or Build → Build Solution

4. **Run GTAMods**
   - Press `F5` or Debug → Start Debugging

## First Launch

On first launch, GTAMods will:
1. Ask if your games are in default Rockstar locations
2. If YES → Auto-detect all games
3. If NO → Let you manually select each game folder (user's responsibility)
4. Create folder structure automatically
5. Install and load 5 default pre-configured mods

## Folder Structure

```
C:\GTAMods\
├── GTA3\
│   ├── Mods\
│   ├── Backups\
│   └── Presets\
├── VC\
│   ├── Mods\
│   ├── Backups\
│   └── Presets\
├── SA\
│   ├── Mods\
│   ├── Backups\
│   └── Presets\
├── IV\
│   ├── Mods\
│   ├── Backups\
│   └── Presets\
└── V\
    ├── Mods\
    ├── Backups\
    └── Presets\
```

## Default Pre-installed Mods

GTAMods comes with 5 pre-configured mods for all games:

1. **Traffic Overhaul** - Control traffic density and aggression
2. **Police AI** - Adjust wanted level response and pursuit vehicles
3. **Driving Physics** - Customize vehicle handling, grip, and suspension
4. **Economy** - Modify mission rewards, hospital costs, weapon prices
5. **Graphics** - Enhance visual settings (saturation, bloom, timecycle presets)

## Key Features Explained

### Game Detection
- **Auto-Detect**: Checks default Rockstar installation paths
- **Manual Selection**: User can browse and select custom game folders
- **Auto-Load Mods**: Once game is found, default mods load automatically

### Mod Manager
- Enable/Disable mods without deleting files
- Auto-detects missing mod dependencies (DLLs, INI files, etc.)
- Validates mod file structure
- Warns if mod is incomplete

### Load Order
- Default mods load FIRST
- 3rd party mods load AFTER
- Auto-calculated based on dependencies

### Backup System
- Backs up MODIFIED FILES ONLY (not entire game folder)
- Versioning: `backup_gta5_v1.0.zip`, `backup_gta5_v1.1.zip`, etc.
- User can specify number of versions to keep

### Multiplayer Safety
- GTAMods monitors GTA process automatically
- Detects when GTA connects to online servers
- Detects multiplayer menu selection
- Automatically disables all mods for online play
- Re-enables mods when returning to single-player

### Vehicle Manager
- Keybind-accessible spawn menu in single-player
- Vehicle preview with website images
- Replace vs Add vehicle configuration

### Mod Configuration
- Settings stored in GTAMods JSON config
- Optional: Sync to in-game config
- Real-time sync as you play

## Project Structure

```
GTAMods/
├── GTAMods.sln
├── src/
│   ├── GTAMods.Core/
│   │   ├── GameDetector.cs          # Auto/manual game detection
│   │   ├── ModManager.cs             # Mod enable/disable/install
│   │   ├── DependencyChecker.cs      # Auto-detect missing files
│   │   ├── BackupManager.cs          # Versioned backups
│   │   ├── PresetManager.cs          # Save/load mod profiles
│   │   ├── ProcessMonitor.cs         # Multiplayer detection
│   │   ├── ConfigManager.cs          # JSON config handling
│   │   └── Models/
│   │       ├── GameInfo.cs
│   │       ├── ModInfo.cs
│   │       └── Config.cs
│   ├── GTAMods.UI/
│   │   ├── MainWindow.xaml
│   │   ├── MainWindow.xaml.cs
│   │   └── Views/
│   └── GTAMods.Launcher/
│       └── Program.cs
├── config/
│   └── DefaultMods.json             # Pre-installed mods config
├── docs/
│   ├── ARCHITECTURE.md
│   ├── DEPENDENCIES.md
│   └── API_REFERENCE.md
└── README.md
```

## Development Phases

### Phase 1: Foundation ✅ IN PROGRESS
- [x] Launcher window
- [x] Mod Manager UI (basic)
- [x] Game detection (auto + manual)
- [x] File structure creation
- [x] Settings/configuration
- [x] Default mods pre-loading
- [x] Dependency file checking
- [x] Process monitoring setup

### Phase 2: Smart Features (Upcoming)
- [ ] Full dependency resolution
- [ ] Load order management
- [ ] Backup/recovery system
- [ ] Game launcher integration
- [ ] Multiplayer auto-disable

### Phase 3: Advanced Features (Upcoming)
- [ ] Preset system (save/load)
- [ ] Vehicle Manager (spawn menu, preview)
- [ ] Mod browser integration
- [ ] Graphics conflict detection
- [ ] Fallback recovery

### Phase 4: Roleplay Systems (Upcoming)
- [ ] Police system
- [ ] Gang system
- [ ] Career system
- [ ] Economy system
- [ ] Persistence/save system

### Phase 5: Creator Tools (Upcoming)
- [ ] Mod packaging assistant
- [ ] No-code mod creation
- [ ] Testing environment
- [ ] Distribution tools

## Safety & Legal

⚠️ **Multiplayer Warning**: Using mods in GTA Online will result in a ban. GTAMods automatically disables mods for online play.

⚠️ **Backup Important**: Always backup your original game files before modding.

✅ **Auto-Backup**: GTAMods automatically creates backups before major changes.

✅ **Respect Mods**: Respect individual mod licenses and creators.

## Contributing

Contributions welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## License

This project is provided as-is for personal use. Respect individual mod licenses and creators' rights.

## Support & Feedback

For issues, feature requests, or questions:
- Create an issue on GitHub
- Check existing documentation

## Credits

**Built by**: Stryk3rr
**Framework**: .NET 4.8+, WPF
**Language**: C#

---

**Version**: 0.1.0 (Alpha - Phase 1)
**Status**: Active Development
**Last Updated**: 2026-06-08