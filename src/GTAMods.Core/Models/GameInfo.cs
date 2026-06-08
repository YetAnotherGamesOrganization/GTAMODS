using System;
using System.Collections.Generic;

namespace GTAMods.Core.Models
{
    /// <summary>
    /// Represents information about a GTA game installation
    /// </summary>
    public class GameInfo
    {
        public string GameName { get; set; }  // "GTA V", "GTA IV", etc.
        public string GameCode { get; set; } // "V", "IV", "SA", "VC", "III"
        public string GamePath { get; set; } // Full path to game installation
        public string GTAModsPath { get; set; } // Path to GTAMods folder for this game
        public string ModsPath { get; set; } // C:\GTAMods\V\Mods\
        public string BackupsPath { get; set; } // C:\GTAMods\V\Backups\
        public string PresetsPath { get; set; } // C:\GTAMods\V\Presets\
        public bool IsInstalled { get; set; }
        public DateTime LastDetected { get; set; }
        public List<string> InstalledMods { get; set; } = new List<string>();
    }
}