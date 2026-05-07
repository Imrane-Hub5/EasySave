using System;
using System.IO;
using System.Text.Json;
using EasySave.Models;

namespace EasySave.Services
{
    /// <summary>
    /// Writes the real-time state of the running backup job to a JSON file.
    /// Only one job runs at a time, so the file holds a single <see cref="StateEntry"/>.
    /// </summary>
    public class StateManager
    {
        // Absolute path to the state file shared across the application lifetime
        private readonly string _stateFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EasySave", "state.json"
        );

        /// <summary>
        /// Persists the current state of a backup job to <c>state.json</c>.
        /// Overwrites the file on each call to reflect the latest progress.
        /// </summary>
        public void UpdateState(StateEntry state)
        {
            try
            {
                state.Timestamp = DateTime.Now;
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(state, options);
                Directory.CreateDirectory(Path.GetDirectoryName(_stateFilePath)!);
                File.WriteAllText(_stateFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"State update error: {ex.Message}");
            }
        }

        /// <summary>
        /// Marks a job as inactive once it has finished, clearing progress counters.
        /// </summary>
        public void ResetState(string jobName)
        {
            UpdateState(new StateEntry { JobName = jobName, Status = "Inactive" });
        }
    }
}