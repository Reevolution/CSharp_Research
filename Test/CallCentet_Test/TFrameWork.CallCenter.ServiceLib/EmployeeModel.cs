using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.ServiceLib
{
    public class EmployeeModel
    {
        public string Login { get; set; }

        public EmployeeCategory Category { get; set; }

        public Status Status { get; set; }

        public int Priority { get; set; }

    }
}
