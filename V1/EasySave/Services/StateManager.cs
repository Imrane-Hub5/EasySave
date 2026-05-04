using System;
using System.IO;
using System.Text.Json;
using EasySave.Models;

namespace EasySave.Services
{
    public class StateManager
    {
        // Le fichier est stocké dans le dossier d'exécution (bin/Debug/...)
        private readonly string _stateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "state.json");

        /// Écrit l'état actuel d'un travail dans le fichier JSON.
      
        public void UpdateState(JobState state)
        {
            try
            {
                // Mise à jour de l'horodatage système
                state.Timestamp = DateTime.Now;

                // On configure le JSON pour qu'il soit bien présenté (indentation)
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonContent = JsonSerializer.Serialize(state, options);

                // On utilise WriteAllText pour écraser le fichier à chaque fois (Temps Réel)
                File.WriteAllText(_stateFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                // En cas d'erreur (fichier utilisé par un autre processus par exemple)
                Console.WriteLine($"Erreur lors de la mise à jour du fichier d'état : {ex.Message}");
            }
        }
    }
}
