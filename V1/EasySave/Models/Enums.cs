namespace EasySave.Models
{
    public enum BackupType
    {
        Complete,
        Differential
    }

    public enum Language
    {
        French,
        English
    }

    public enum JobStatus
    {
        Inactive,
        Active,
        End,
        Error
    }
}