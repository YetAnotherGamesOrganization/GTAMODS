using System;
using System.Collections.Generic;

namespace GTAMods.Core.Models
{
    /// <summary>
    /// Represents information about a single mod
    /// </summary>
    public class ModInfo
    {
        public string ModName { get; set; }
        public string ModPath { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDefault { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();
        public List<string> RequiredFiles { get; set; } = new List<string>(); // DLLs, INIs, etc.
        public int LoadOrder { get; set; } // 0 = default, 1+ = 3rd party
        public DateTime DateInstalled { get; set; }
        public bool HasMissingDependencies { get; set; }
        public List<string> MissingFiles { get; set; } = new List<string>();
    }
}