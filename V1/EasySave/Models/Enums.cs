namespace EasySave.Models
{
    /// <summary>Determines how files are selected during a backup run.</summary>
    public enum BackupType
    {
        /// <summary>Copy every file from source to target unconditionally.</summary>
        Complete,
        /// <summary>Copy only files that are newer in the source than in the target.</summary>
        Differential
    }

    /// <summary>Supported UI languages.</summary>
    public enum Language
    {
        French,
        English
    }

    /// <summary>Lifecycle states of a backup job.</summary>
    public enum JobStatus
    {
        Inactive,
        Active,
        End,
        Error
    }
}