using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyLog
{
    /// <summary>
    /// Sends log entries to a remote Docker server via HTTP POST.
    /// Implements ILogFormatter for seamless integration with Logger.
    /// </summary>
    public class RemoteLogFormatter : ILogFormatter
    {
        private readonly string _serverUrl;
        private static readonly HttpClient _httpClient = new HttpClient();

        public RemoteLogFormatter(string serverUrl)
        {
            _serverUrl = serverUrl;
        }

        /// <summary>
        /// Serializes the log entry to JSON string.
        /// </summary>
        public string Format(LogEntry entry)
        {
            return JsonSerializer.Serialize(entry, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        /// <summary>
        /// Returns the file extension for local fallback.
        /// </summary>
        public string GetExtension() => "json";

        /// <summary>
        /// Sends the log entry to the Docker server via HTTP POST.
        /// </summary>
        public async Task SendAsync(LogEntry entry)
        {
            try
            {
                string json = Format(entry);
                StringContent content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );
                await _httpClient.PostAsync(_serverUrl, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RemoteLogFormatter] Failed to send log: {ex.Message}");
            }
        }
    }
}