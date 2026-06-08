using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTAMods.Core.Models;

namespace GTAMods.Core
{
    /// <summary>
    /// Manages mods for a specific game
    /// Handles enable/disable, installation, and mod detection
    /// </summary>
    public class ModManager
    {
        private readonly string _modsPath;
        private readonly GameInfo _gameInfo;
        private List<ModInfo> _installedMods = new List<ModInfo>();

        public ModManager(string modsPath, GameInfo gameInfo)
        {
            _modsPath = modsPath;
            _gameInfo = gameInfo;
            ScanInstalledMods();
        }

        /// <summary>
        /// Scan mod folders and detect installed mods
        /// </summary>
        public void ScanInstalledMods()
        {
            _installedMods.Clear();

            if (!Directory.Exists(_modsPath))
            {
                Console.WriteLine($"[ModManager] Mods path does not exist: {_modsPath}");
                return;
            }

            try
            {
                var modDirectories = Directory.GetDirectories(_modsPath);

                foreach (var modDir in modDirectories)
                {
                    var modName = Path.GetFileName(modDir);
                    var modInfo = new ModInfo
                    {
                        ModName = modName,
                        ModPath = modDir,
                        IsEnabled = true, // Default to enabled
                        DateInstalled = Directory.GetCreationTime(modDir)
                    };

                    // Check for missing dependencies
                    CheckModDependencies(modInfo, modDir);

                    _installedMods.Add(modInfo);
                    Console.WriteLine($"[ModManager] Detected mod: {modName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ModManager] Error scanning mods: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if mod has all required files
        /// Auto-detects: DLLs, INI files, required structure
        /// </summary>
        private void CheckModDependencies(ModInfo mod, string modDir)
        {
            var files = Directory.GetFiles(modDir, "*.*", SearchOption.AllDirectories);
            var extensions = files.Select(f => Path.GetExtension(f).ToLower()).Distinct().ToList();

            // Check for required file types
            bool hasDLL = extensions.Contains(".dll");
            bool hasINI = extensions.Contains(".ini");
            bool hasLUA = extensions.Contains(".lua");
            bool hasASI = extensions.Contains(".asi");

            mod.RequiredFiles = files.Select(f => Path.GetFileName(f)).ToList();

            // Basic validation: if it has DLL but no config files, that might be incomplete
            if (hasDLL && !hasINI && !hasLUA && !hasASI)
            {
                // This might be incomplete, but we'll flag it as a warning
                Console.WriteLine($"[ModManager] Warning: {mod.ModName} has DLL but no config files");
            }

            // Check for common missing structures
            if (files.Length == 0)
            {
                mod.HasMissingDependencies = true;
                mod.MissingFiles.Add("No files found in mod directory");
            }
        }

        /// <summary>
        /// Enable a mod
        /// </summary>
        public void EnableMod(string modName)
        {
            var mod = _installedMods.FirstOrDefault(m => m.ModName == modName);
            if (mod != null)
            {
                mod.IsEnabled = true;
                Console.WriteLine($"[ModManager] Enabled mod: {modName}");
            }
        }

        /// <summary>
        /// Disable a mod
        /// </summary>
        public void DisableMod(string modName)
        {
            var mod = _installedMods.FirstOrDefault(m => m.ModName == modName);
            if (mod != null)
            {
                mod.IsEnabled = false;
                Console.WriteLine($"[ModManager] Disabled mod: {modName}");
            }
        }

        /// <summary>
        /// Get all installed mods
        /// </summary>
        public List<ModInfo> GetInstalledMods() => _installedMods;

        /// <summary>
        /// Get enabled mods sorted by load order
        /// </summary>
        public List<ModInfo> GetEnabledMods()
        {
            return _installedMods
                .Where(m => m.IsEnabled)
                .OrderBy(m => m.LoadOrder)
                .ThenBy(m => m.ModName)
                .ToList();
        }

        /// <summary>
        /// Get mod by name
        /// </summary>
        public ModInfo GetMod(string modName)
        {
            return _installedMods.FirstOrDefault(m => m.ModName == modName);
        }
    }
}