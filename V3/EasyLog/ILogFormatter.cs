namespace EasyLog
{
    /// <summary>
    /// Strategy interface for log formatting
    /// </summary>
    public interface ILogFormatter
    {
        /// <summary>
        /// Formats a log entry into a string
        /// </summary>
        string Format(LogEntry entry);

        /// <summary>
        /// Returns the file extension (json or xml)
        /// </summary>
        string GetExtension();
    }
}
