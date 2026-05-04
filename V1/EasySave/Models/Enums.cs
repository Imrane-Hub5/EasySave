namespace EasySave.Models
{
    // On définit les types de travaux pour que Ghada puisse les proposer dans le menu
    public enum JobType
    {
        Full,          // Sauvegarde complète
        Differential   // Sauvegarde différentielle
    }

    // Ca sert à StateManager pour dire ce que fait le logiciel à l'instant té
    public enum JobStatus
    {
        Inactive, // Le travail est en attente
        Active,   // Sauvegarde en cours (on écrit dans le JSON)
        End,      // Travail fini
        Error     // Problème technique
    }
}
