using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Serialization_XML_Binary_JSON
{
    [Serializable]
    public class SimpleDataBinary
    {
        public int Id { get; set; }
        public string Data { get; set; }
    }
}
