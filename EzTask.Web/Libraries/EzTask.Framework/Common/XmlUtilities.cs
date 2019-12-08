using System;
using System.IO;
using System.Xml.Serialization;

namespace EzTask.Framework.Common
{
    public class XmlUtilities
    {
        public static T Deserialize<T>(string XmlFilename)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(XmlFilename)) return default(T);

            try
            {
                StreamReader xmlStream = new StreamReader(XmlFilename);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObject = (T)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                //ignored   
            }

            return returnObject;
        }
    }
}
