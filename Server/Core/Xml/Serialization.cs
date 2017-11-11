using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Server.Core.Xml
{
    public static class Serialization
    {
        public static TClass Deserialize<TClass>(string xml) where TClass : new()
        {
            TClass tClass = new TClass();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
            using (TextReader textReader = new StringReader(xml))
            {
                tClass = (TClass)xmlSerializer.Deserialize(textReader);
            }
            return tClass;
        }

        public static TClass Deserialize<TClass>(Stream stream) where TClass : new()
        {
            TClass tClass = new TClass();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
                tClass = (TClass)xmlSerializer.Deserialize(stream);
            }
            finally
            {
                stream.Dispose();
            }
            return tClass;
        }


        public static TClass Deserialize<TClass>(string filePath, Encoding encoding) where TClass : new()
        {
            TClass tClass = new TClass();

            using (TextReader textReader = new StreamReader(filePath, encoding))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
                tClass = (TClass)xmlSerializer.Deserialize(textReader);
            }

            return tClass;
        }

        public static string Serialize<TClass>(TClass tClass) where TClass : new()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
            xmlSerializer.Serialize(xmlWriter, tClass);
            return stringBuilder.ToString();
        }

        public static void Serialize<TClass>(TClass tClass, string filePath, Encoding encoding)
            where TClass : new()
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, false, encoding))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
                StringBuilder stringBuilder = new StringBuilder();
                XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
                xmlSerializer.Serialize(xmlWriter, tClass);
                streamWriter.Write(stringBuilder.ToString());
            }
        }

        public static string Serialize<TClass>(TClass tClass, XmlWriterSettings xmlWriterSettings)
            where TClass : new()
        {
            string xml = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
                XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
                xmlSerializer.Serialize(xmlWriter, tClass);
                xml = xmlWriterSettings.Encoding.GetString(memoryStream.ToArray());
            }
            return xml;
        }
    }
}
