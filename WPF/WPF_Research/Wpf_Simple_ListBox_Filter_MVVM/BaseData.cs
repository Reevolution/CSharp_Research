using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Simple_ListBox_Filter_MVVM
{
    public class BaseData
    {
        public int Id { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return $"BaseData : {Id} {Data}";
        }
    }
}
