namespace EasyLog
{
    /// <summary>
    /// Interface for log formatters (JSON, XML...)
    /// </summary>
    public interface ILogFormatter
    {
        string Format(LogEntry entry);
        string FileExtension { get; }
    }
}