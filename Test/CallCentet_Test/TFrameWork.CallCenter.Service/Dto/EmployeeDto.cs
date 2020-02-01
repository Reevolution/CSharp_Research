using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using TFrameWork.CallCenter.ServiceLib;

namespace TFrameWork.CallCenter.WCF.Service.Dto
{
    [DataContract]
    public class EmployeeDto
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public EmployeeCategory Category { get; set; }

        [DataMember]
        public Status Status { get; set; }

        [DataMember]
        public int Priority { get; set; }
    }
}