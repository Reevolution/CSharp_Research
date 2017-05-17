using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Serialization_XML_Binary_JSON
{
    // GitHub repo.
    // https://github.com/Reevolution/CSharp_Research

    /// <summary>
    /// Simple example serialization deserialization data xml binary json format.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Serialize XML data

            SimpleDataXML dataXml = new SimpleDataXML()
            {
                Id = 1,
                Data = "Simple XML Data"
            };

            Utility.SerializeDataXML(dataXml, @"D:\SimpleXMLData.xml");

            // Serialize Binary data

            SimpleDataBinary dataBinary = new SimpleDataBinary()
            {
                Id = 1,
                Data = "Simple Binary Data"
            };

            Utility.SerializeDataBinary(dataBinary, @"D:\SimpleBinaryData.bin");

            // Serialize JSON data

            SimpleDataJSON dataJson = new SimpleDataJSON()
            {
                Id = 1,
                Data = "Simple JSON Data"
            };

            Utility.SerializeDataJSON(dataJson, @"D:\SimpleJSONData.json");

            // Load data and deserialize data from XML Binary JSON.
            var loadXMLData = Utility.DeserializeDataXML<SimpleDataXML>(@"D:\SimpleXmlData.xml");
            Console.WriteLine($"Load data from XML id = {loadXMLData.Id} | Data = {loadXMLData.Data}");

            var loadBinaryData = Utility.DeserializeDataBinary<SimpleDataBinary>(@"D:\SimpleBinaryData.bin");
            Console.WriteLine($"Load data from Binary id = {loadBinaryData.Id} | Data = {loadBinaryData.Data}");

            var loadJSONData = Utility.DeserializeDataJSON<SimpleDataJSON>(@"D:\SimpleJSONData.json");
            Console.WriteLine($"Load data from JSON id = {loadJSONData.Id} | Data = {loadJSONData.Data}");
        }
    }
}
