using System;
using System.IO;
using Newtonsoft.Json;
using GTAMods.Core.Models;

namespace GTAMods.Core
{
    /// <summary>
    /// Manages GTAMods configuration (JSON-based)
    /// </summary>
    public class ConfigManager
    {
        private readonly string _configPath;
        private Config _config;

        public ConfigManager(string configPath)
        {
            _configPath = configPath;
            LoadConfig();
        }

        public void LoadConfig()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    string json = File.ReadAllText(_configPath);
                    _config = JsonConvert.DeserializeObject<Config>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading config: {ex.Message}");
                    _config = new Config();
                }
            }
            else
            {
                _config = new Config();
                SaveConfig();
            }
        }

        public void SaveConfig()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_config, Formatting.Indented);
                File.WriteAllText(_configPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving config: {ex.Message}");
            }
        }

        public Config GetConfig() => _config;

        public void SetGTAModsPath(string path)
        {
            _config.GTAModsRootPath = path;
            SaveConfig();
        }

        public void AddGame(string gameCode, GameInfo gameInfo)
        {
            _config.Games[gameCode] = gameInfo;
            SaveConfig();
        }

        public GameInfo GetGame(string gameCode)
        {
            return _config.Games.ContainsKey(gameCode) ? _config.Games[gameCode] : null;
        }
    }
}