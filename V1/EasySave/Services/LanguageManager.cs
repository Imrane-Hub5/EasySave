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

        private readonly Dictionary<string, Dictionary<Language, string>> _translations = new()
        {
            { "menu_title",       { { Language.English, "===== EasySave v1.0 =====" },          { Language.French, "===== EasySave v1.0 =====" } } },
            { "menu_add",         { { Language.English, "1. Add a backup job" },                 { Language.French, "1. Ajouter un travail de sauvegarde" } } },
            { "menu_list",        { { Language.English, "2. List backup jobs" },                 { Language.French, "2. Lister les travaux de sauvegarde" } } },
            { "menu_execute",     { { Language.English, "3. Execute a backup job" },             { Language.French, "3. Exécuter un travail de sauvegarde" } } },
            { "menu_execute_all", { { Language.English, "4. Execute all backup jobs" },          { Language.French, "4. Exécuter tous les travaux" } } },
            { "menu_remove",      { { Language.English, "5. Remove a backup job" },              { Language.French, "5. Supprimer un travail de sauvegarde" } } },
            { "menu_quit",        { { Language.English, "6. Quit" },                             { Language.French, "6. Quitter" } } },
            { "menu_choice",      { { Language.English, "Your choice: " },                       { Language.French, "Votre choix : " } } },
            { "job_name",         { { Language.English, "Job name: " },                          { Language.French, "Nom du travail : " } } },
            { "job_source",       { { Language.English, "Source path: " },                       { Language.French, "Chemin source : " } } },
            { "job_target",       { { Language.English, "Target path: " },                       { Language.French, "Chemin cible : " } } },
            { "job_type",         { { Language.English, "Type (1=Complete, 2=Differential): " }, { Language.French, "Type (1=Complète, 2=Différentielle) : " } } },
            { "job_added",        { { Language.English, "Job added successfully." },             { Language.French, "Travail ajouté avec succès." } } },
            { "job_removed",      { { Language.English, "Job removed." },                        { Language.French, "Travail supprimé." } } },
            { "job_max",          { { Language.English, "Maximum of 5 jobs reached." },          { Language.French, "Maximum de 5 travaux atteint." } } },
            { "job_number",       { { Language.English, "Job number: " },                        { Language.French, "Numéro du travail : " } } },
            { "job_none",         { { Language.English, "No backup jobs configured." },          { Language.French, "Aucun travail de sauvegarde configuré." } } },
            { "backup_done",      { { Language.English, "Backup completed." },                   { Language.French, "Sauvegarde terminée." } } },
            { "all_done",         { { Language.English, "All backups completed." },              { Language.French, "Toutes les sauvegardes sont terminées." } } },
            { "invalid_choice",   { { Language.English, "Invalid choice." },                    { Language.French, "Choix invalide." } } },
            { "invalid_number",   { { Language.English, "Invalid number." },                    { Language.French, "Numéro invalide." } } },
        };

        public LanguageManager(Language locale = Language.English)
        {
            _locale = locale;
        }

        public void SetLocale(Language lang) => _locale = lang;

        /// <summary>
        /// Returns the translated string for the given key
        /// </summary>
        public string Get(string key)
        {
            if (_translations.TryGetValue(key, out var langs) && langs.TryGetValue(_locale, out var text))
                return text;
            return key;
        }
    }
}

