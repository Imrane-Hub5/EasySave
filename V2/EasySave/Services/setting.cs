using System.Text.Json;

namespace EasySave.Services
{
    /// <summary>
    /// Stores user preferences
    /// Saved in AppData\Roaming\EasySave\settings.json
    /// </summary>
    public class Settings
    {
        private static readonly string _configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EasySave", "settings.json"
        );

        // "JSON" or "XML"
        public string LogFormat { get; set; } = "JSON";

        /// <summary>
        /// Loads settings from settings.json
        /// If file does not exist, returns default settings (JSON)
        /// </summary>
        public static Settings Load()
        {
            if (!File.Exists(_configPath))
                return new Settings();

            string json = File.ReadAllText(_configPath);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }

        /// <summary>
        /// Saves current settings to settings.json
        /// </summary>
        public void Save()
        {
            string dir = Path.GetDirectoryName(_configPath)!;
            Directory.CreateDirectory(dir);

            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_configPath, json);
        }
    }
}