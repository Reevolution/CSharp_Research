using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFrameWork.CallCenter.UI.Model
{
    public class CustomerModel
    {
        public string Guid { get; set; }

        public EmployeeModel Employee { get; set; }

        public override string ToString()
        {
            return $"{Guid}";
        }
    }
}
