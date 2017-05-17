using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Simple_Serialization_XML_Binary_JSON
{
    /// <summary>
    /// Utiltiy class for serialize deserialize data.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Simple XML serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void SerializeDataXML<T>(T data, string path)
        {
            #region SerializeDataXML      
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                Encoding encoding = Encoding.GetEncoding("UTF-16");
                XmlSerializer w = new XmlSerializer(data.GetType());
                StreamWriter sw = new StreamWriter(fs, encoding);
                w.Serialize(sw, data);
            }
            #endregion
        }

        /// <summary>
        /// Simple XML Deserialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeDataXML<T>(string path)
        {
            #region DeserializeDataXML
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    Encoding encoding = Encoding.GetEncoding("UTF-16");
                    XmlSerializer w = new XmlSerializer(typeof(T));
                    StreamReader sw = new StreamReader(fs, encoding);
                    T data = (T)w.Deserialize(sw);

                    return data;
                }
            }
            catch
            {
                return default(T);
            }
            #endregion
        }

        /// <summary>
        /// Simple Binary serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void SerializeDataBinary<T>(T data, string path)
        {
            #region SerializeDataBinary
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter w = new BinaryFormatter();
                w.Serialize(fs, data);
            }
            #endregion
        }

        /// <summary>
        /// Simple Binary Deserialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeDataBinary<T>(string path)
        {
            #region DeserializeDataBinary
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter w = new BinaryFormatter();
                    T data = (T)w.Deserialize(fs);

                    return data;
                }
            }
            catch
            {
                return default(T);
            }
            #endregion
        }

        /// <summary>
        /// Simple JSON serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void SerializeDataJSON<T>(T data, string path)
        {
            #region SerializeDataJSON
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                MemoryStream stream1 = new MemoryStream();
                DataContractJsonSerializer w = new DataContractJsonSerializer(typeof(T));
                w.WriteObject(fs, data);
            }

            #endregion
        }

        /// <summary>
        /// Simple JSON Deserialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeDataJSON<T>(string path)
        {
            #region DeserializeDataBinary
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    DataContractJsonSerializer w = new DataContractJsonSerializer(typeof(T));
                    T data = (T)w.ReadObject(fs);

                    return data;
                }
            }
            catch
            {
                return default(T);
            }
            #endregion
        }
    }
}
