using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTAMods.Core.Models;

namespace GTAMods.Core
{
    /// <summary>
    /// Checks mod dependencies and validates file structures
    /// Detects missing DLLs, INI files, and incomplete mods
    /// </summary>
    public class DependencyChecker
    {
        /// <summary>
        /// Validate mod has all required files
        /// </summary>
        public static ValidationResult ValidateMod(ModInfo mod, string modPath)
        {
            var result = new ValidationResult { ModName = mod.ModName, IsValid = true };

            if (!Directory.Exists(modPath))
            {
                result.IsValid = false;
                result.Errors.Add($"Mod directory does not exist: {modPath}");
                return result;
            }

            try
            {
                var files = Directory.GetFiles(modPath, "*.*", SearchOption.AllDirectories);

                if (files.Length == 0)
                {
                    result.IsValid = false;
                    result.Errors.Add("Mod folder is empty");
                    return result;
                }

                // Check for required file types
                var extensions = files.Select(f => Path.GetExtension(f).ToLower()).Distinct().ToList();
                var fileNames = files.Select(f => Path.GetFileName(f).ToLower()).ToList();

                // Analyze mod structure
                bool hasDLL = extensions.Contains(".dll");
                bool hasASI = extensions.Contains(".asi");
                bool hasINI = extensions.Contains(".ini");
                bool hasLUA = extensions.Contains(".lua");
                bool hasCS = extensions.Contains(".cs");
                bool hasConfig = hasINI || hasLUA || hasCS;

                // DLL mods need config
                if ((hasDLL || hasASI) && !hasConfig)
                {
                    result.Warnings.Add("DLL mod detected but no config files (.ini, .lua, .cs) found");
                    result.MissingFiles.Add("Configuration file");
                }

                // Check for common incomplete patterns
                if (hasDLL && fileNames.Count() == 1)
                {
                    result.Warnings.Add("Only DLL file found - likely incomplete mod");
                    result.MissingFiles.Add("Supporting files/configuration");
                }

                // Flag as invalid if too many issues
                if (result.MissingFiles.Count > 0)
                {
                    result.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add($"Error validating mod: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Check if all mod dependencies are installed
        /// </summary>
        public static bool CheckDependencies(List<ModInfo> allMods, ModInfo targetMod)
        {
            foreach (var dependency in targetMod.Dependencies)
            {
                if (!allMods.Any(m => m.ModName == dependency && m.IsEnabled))
                {
                    Console.WriteLine($"[DependencyChecker] Missing dependency: {dependency} for {targetMod.ModName}");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get validation report for mod
        /// </summary>
        public static string GetValidationReport(ValidationResult result)
        {
            var report = $"\n=== Mod Validation Report: {result.ModName} ===\n";
            report += $"Status: {(result.IsValid ? "✓ VALID" : "✗ INVALID")}\n";

            if (result.Errors.Count > 0)
            {
                report += "\nErrors:\n";
                foreach (var error in result.Errors)
                    report += $"  - {error}\n";
            }

            if (result.Warnings.Count > 0)
            {
                report += "\nWarnings:\n";
                foreach (var warning in result.Warnings)
                    report += $"  - {warning}\n";
            }

            if (result.MissingFiles.Count > 0)
            {
                report += "\nMissing Files/Components:\n";
                foreach (var missing in result.MissingFiles)
                    report += $"  - {missing}\n";
            }

            return report;
        }
    }

    /// <summary>
    /// Result of mod validation
    /// </summary>
    public class ValidationResult
    {
        public string ModName { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> MissingFiles { get; set; } = new List<string>();
    }
}