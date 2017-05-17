using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Serialization_XML_Binary_JSON
{
    [DataContract]
    public class SimpleDataJSON
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Data { get; set; }
    }
}
