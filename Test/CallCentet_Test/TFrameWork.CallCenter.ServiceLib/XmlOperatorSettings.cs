using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.ServiceLib
{
    public class XmlEmployeeSettings : IEmployeeSettings
    {
        public int TimeOut { get; set; }

        public XmlEmployeeSettings()
        {
            TimeOut = int.Parse(ConfigurationManager.AppSettings["employeetimeout"]);
        }
    }
}
