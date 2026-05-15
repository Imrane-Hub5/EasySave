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

        // Business software to detect
        public string BusinessSoftware { get; set; } = string.Empty;

        // File extensions to encrypt
        public List<string> EncryptedExtensions { get; set; } = new();

        // Path to the CryptoSoft executable
        public string CryptoSoftPath { get; set; } = string.Empty;

        // NEW v3.0 — Priority file extensions (processed before others)
        public List<string> PriorityExtensions { get; set; } = new();

        // NEW v3.0 — Max file size (in Ko) allowed for parallel transfer
        public int MaxParallelFileSizeKo { get; set; } = 1024;

        public static Settings Load()
        {
            if (!File.Exists(_configPath))
                return new Settings();
            string json = File.ReadAllText(_configPath);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }

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
