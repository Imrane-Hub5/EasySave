using System.Text.Json;

namespace EasySave.Services
{
    /// <summary>
    /// Log destination mode — Local only, Remote only, or Both.
    /// </summary>
    public enum LogDestination
    {
        Local,
        Remote,
        Both
    }

    /// <summary>
    /// Stores user preferences (log format, business software, encrypted extensions)
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

        // Business software to detect
        public string BusinessSoftware { get; set; } = string.Empty;

        // File extensions to encrypt
        public List<string> EncryptedExtensions { get; set; } = new();

        // Path to the CryptoSoft executable
        public string CryptoSoftPath { get; set; } = string.Empty;

        // Log destination mode (Local, Remote, or Both)
        public LogDestination LogDestination { get; set; } = LogDestination.Local;

        // Docker server URL for remote log centralization
        public string DockerServerUrl { get; set; } = string.Empty;

        /// <summary>
        /// Loads settings from settings.json.
        /// If file does not exist, returns default settings.
        /// </summary>
        public static Settings Load()
        {
            if (!File.Exists(_configPath))
                return new Settings();

            string json = File.ReadAllText(_configPath);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }

        /// <summary>
        /// Saves current settings to settings.json.
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