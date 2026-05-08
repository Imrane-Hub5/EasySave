using System.Text.Json;

namespace EasyLog
{
    public class JsonFormatter : ILogFormatter
    {
        public string Format(LogEntry entry)
        {
            return JsonSerializer.Serialize(entry, new JsonSerializerOptions { WriteIndented = true });
        }

        public string FileExtension => ".json";
    }
}