using System.Collections.Generic;
using EasySave.Models;

namespace EasySave.Services
{
    /// <summary>
    /// Manages bilingual support (FR / EN)
    /// </summary>
    public class LanguageManager
    {
        private Language _locale;

        private readonly Dictionary<string, string> _translationsEN = new Dictionary<string, string>
	{
            { "menu_title",       "===== EasySave v1.0 =====" },
            { "menu_add",         "1. Add a backup job" },
            { "menu_list",        "2. List backup jobs" },
            { "menu_execute",     "3. Execute a backup job" },
            { "menu_execute_all", "4. Execute all backup jobs" },
            { "menu_remove",      "5. Remove a backup job" },
            { "menu_quit",        "6. Quit" },
            { "menu_choice",      "Your choice: " },
            { "job_name",         "Job name: " },
            { "job_source",       "Source path: " },
            { "job_target",       "Target path: " },
            { "job_type",         "Type (1=Complete, 2=Differential): " },
            { "job_added",        "Job added successfully." },
            { "job_removed",      "Job removed." },
            { "job_max",          "Maximum of 5 jobs reached." },
            { "job_number",       "Job number: " },
            { "job_none",         "No backup jobs configured." },
            { "backup_done",      "Backup completed." },
            { "all_done",         "All backups completed." },
            { "invalid_choice",   "Invalid choice." },
            { "invalid_number",   "Invalid number." }
        };

        private readonly Dictionary<string, string> _translationsFR = new Dictionary<string, string>
	{
            { "menu_title",       "===== EasySave v1.0 =====" },
            { "menu_add",         "1. Ajouter un travail de sauvegarde" },
            { "menu_list",        "2. Lister les travaux de sauvegarde" },
            { "menu_execute",     "3. Ex\u00e9cuter un travail de sauvegarde" },
            { "menu_execute_all", "4. Ex\u00e9cuter tous les travaux" },
            { "menu_remove",      "5. Supprimer un travail de sauvegarde" },
            { "menu_quit",        "6. Quitter" },
            { "menu_choice",      "Votre choix : " },
            { "job_name",         "Nom du travail : " },
            { "job_source",       "Chemin source : " },
            { "job_target",       "Chemin cible : " },
            { "job_type",         "Type (1=Compl\u00e8te, 2=Diff\u00e9rentielle) : " },
            { "job_added",        "Travail ajout\u00e9 avec succ\u00e8s." },
            { "job_removed",      "Travail supprim\u00e9." },
            { "job_max",          "Maximum de 5 travaux atteint." },
            { "job_number",       "Num\u00e9ro du travail : " },
            { "job_none",         "Aucun travail de sauvegarde configur\u00e9." },
            { "backup_done",      "Sauvegarde termin\u00e9e." },
            { "all_done",         "Toutes les sauvegardes sont termin\u00e9es." },
            { "invalid_choice",   "Choix invalide." },
            { "invalid_number",   "Num\u00e9ro invalide." }
        };

        public LanguageManager(Language locale = Language.English)
        {
            _locale = locale;
        }

        public void SetLocale(Language lang)
        {
            _locale = lang;
        }

        /// <summary>
        /// Returns the translated string for the given key
        /// </summary>
        public string Get(string key)
        {
            Dictionary<string, string> translations = _locale == Language.French ? _translationsFR : _translationsEN;
            return translations.TryGetValue(key, out string? value) ? value : key;
        }
    }
}
