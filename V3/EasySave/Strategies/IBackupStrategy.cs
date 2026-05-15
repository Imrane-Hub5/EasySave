namespace EasySave
{
    /// <summary>
    /// Strategy interface for backup algorithms
    /// </summary>
    public interface IBackupStrategy
    {
        /// <summary>
        /// Executes the file copy from source to target
        /// </summary>
        /// <returns>Transfer time in ms, negative if error</returns>
        long Execute(string src, string dst);

        /// <summary>
        /// Returns the name of the strategy
        /// </summary>
        string GetTypeName();
    }
}
