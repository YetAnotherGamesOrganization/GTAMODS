using System.Collections.Generic;

namespace GTAMods.Core.Models
{
    /// <summary>
    /// Main configuration model for GTAMods
    /// </summary>
    public class Config
    {
        public string Version { get; set; } = "0.1.0";
        public string GTAModsRootPath { get; set; } = "C:\\GTAMods";
        public bool UseAutoDetection { get; set; } = true;
        public Dictionary<string, GameInfo> Games { get; set; } = new Dictionary<string, GameInfo>();
        public Dictionary<string, PresetInfo> Presets { get; set; } = new Dictionary<string, PresetInfo>();
        public int MaxBackupVersions { get; set; } = 5;
        public bool AutoBackupOnModChange { get; set; } = true;
        public bool MonitorMultiplayer { get; set; } = true;
        public bool AutoDisableModsForOnline { get; set; } = true;
    }

    /// <summary>
    /// Represents a saved preset (collection of mod configurations)
    /// </summary>
    public class PresetInfo
    {
        public string PresetName { get; set; }
        public string GameCode { get; set; }
        public Dictionary<string, bool> ModStates { get; set; } = new Dictionary<string, bool>(); // ModName -> IsEnabled
        public Dictionary<string, Dictionary<string, object>> ModSettings { get; set; } = new Dictionary<string, Dictionary<string, object>>(); // ModName -> Settings
    }
}