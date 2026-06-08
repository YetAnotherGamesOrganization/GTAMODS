using System;
using System.Collections.Generic;
using System.IO;
using GTAMods.Core.Models;

namespace GTAMods.Core
{
    /// <summary>
    /// Detects and manages GTA game installations
    /// Supports: GTA III, VC, SA, IV, V
    /// </summary>
    public class GameDetector
    {
        private readonly Dictionary<string, GameInfo> _detectedGames = new Dictionary<string, GameInfo>();

        private readonly Dictionary<string, string[]> _gameExecutables = new Dictionary<string, string[]>
        {
            { "III", new[] { "gta3.exe" } },
            { "VC", new[] { "gta-vc.exe" } },
            { "SA", new[] { "gta_sa.exe" } },
            { "IV", new[] { "GTAIV.exe" } },
            { "V", new[] { "GTA5.exe", "gta5.exe" } }
        };

        private readonly string[] _defaultRockstarPaths = new[]
        {
            "C:\\Program Files\\Rockstar Games",
            "C:\\Program Files (x86)\\Rockstar Games",
            "C:\\Games\\Rockstar Games",
            "C:\\Games"
        };

        /// <summary>
        /// Auto-detect all GTA games in default Rockstar locations
        /// </summary>
        public Dictionary<string, GameInfo> AutoDetectGames()
        {
            Console.WriteLine("[GameDetector] Starting auto-detection...");
            _detectedGames.Clear();

            foreach (var path in _defaultRockstarPaths)
            {
                if (!Directory.Exists(path)) continue;

                foreach (var game in _gameExecutables)
                {
                    string gameCode = game.Key;
                    string[] executables = game.Value;
                    string gamePath = FindGameInPath(path, executables, gameCode);

                    if (gamePath != null)
                    {
                        _detectedGames[gameCode] = new GameInfo
                        {
                            GameCode = gameCode,
                            GameName = GetGameNameFromCode(gameCode),
                            GamePath = gamePath,
                            IsInstalled = true,
                            LastDetected = DateTime.Now
                        };
                        Console.WriteLine($"[GameDetector] Found {GetGameNameFromCode(gameCode)} at {gamePath}");
                    }
                }
            }

            return _detectedGames;
        }

        /// <summary>
        /// Manually add a game path
        /// </summary>
        public GameInfo ManuallyAddGame(string gameCode, string gamePath)
        {
            if (!Directory.Exists(gamePath))
            {
                Console.WriteLine($"[GameDetector] Path does not exist: {gamePath}");
                return null;
            }

            // Verify game executable exists
            if (!_gameExecutables.ContainsKey(gameCode))
            {
                Console.WriteLine($"[GameDetector] Unknown game code: {gameCode}");
                return null;
            }

            var gameInfo = new GameInfo
            {
                GameCode = gameCode,
                GameName = GetGameNameFromCode(gameCode),
                GamePath = gamePath,
                IsInstalled = VerifyGamePath(gamePath, _gameExecutables[gameCode]),
                LastDetected = DateTime.Now
            };

            if (gameInfo.IsInstalled)
            {
                _detectedGames[gameCode] = gameInfo;
                Console.WriteLine($"[GameDetector] Manually added {gameInfo.GameName} at {gamePath}");
            }
            else
            {
                Console.WriteLine($"[GameDetector] Could not verify game at {gamePath}");
            }

            return gameInfo;
        }

        /// <summary>
        /// Search for game executable in given path
        /// </summary>
        private string FindGameInPath(string basePath, string[] executables, string gameCode)
        {
            try
            {
                foreach (var exe in executables)
                {
                    var fullPath = Path.Combine(basePath, exe);
                    if (File.Exists(fullPath))
                        return basePath;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameDetector] Error searching {basePath}: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Verify game executable exists at path
        /// </summary>
        private bool VerifyGamePath(string gamePath, string[] executables)
        {
            foreach (var exe in executables)
            {
                if (File.Exists(Path.Combine(gamePath, exe)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Convert game code to readable name
        /// </summary>
        private string GetGameNameFromCode(string code)
        {
            return code switch
            {
                "III" => "GTA III",
                "VC" => "GTA: Vice City",
                "SA" => "GTA: San Andreas",
                "IV" => "GTA IV",
                "V" => "GTA V",
                _ => "Unknown"
            };
        }

        public Dictionary<string, GameInfo> GetDetectedGames() => _detectedGames;
    }
}