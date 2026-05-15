using System.IO;
using System.Xml.Serialization;

namespace EasyLog
{
    /// <summary>
    /// Formats log entries as XML
    /// </summary>
    public class XmlFormatter : ILogFormatter
    {
        public string GetExtension() => "xml";

        public string Format(LogEntry entry)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LogEntry));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, entry);
                return writer.ToString();
            }
        }
    }
}