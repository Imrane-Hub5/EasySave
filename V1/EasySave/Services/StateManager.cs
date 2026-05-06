using System;
using System.IO;
using System.Text.Json;
using EasySave.Models;

namespace EasySave.Services
{
    public class StateManager
    {
        private readonly string _stateFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EasySave", "state.json"
        );

        public void UpdateState(StateEntry state)  // ← JobState → StateEntry
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

        public void ResetState(string jobName)  // ← AJOUTER
        {
            UpdateState(new StateEntry { JobName = jobName, Status = "Inactive" });
        }
    }
}