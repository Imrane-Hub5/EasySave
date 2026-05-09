using System;
using System.IO;
using System.Xml.Serialization;

namespace EasyLog
{
    public class XmlFormatter : ILogFormatter
    {
        // Définit l'extension du fichier pour le Logger
        public string Extension => ".xml";

        public string Serialize(LogEntry entry)
        {
            // On utilise le sérialiseur natif .NET pour la classe LogEntry
            XmlSerializer serializer = new XmlSerializer(typeof(LogEntry));
            
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, entry);
                return writer.ToString();
            }
        }
    }
}
