using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Simple_ListBox_Scroll_MVVM
{
    public class SimpleData
    {
        public int Id { get; set; }
        public string Data { get; set; }

        /// <summary>
        /// In list box data show from ToString method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"SimpleData : {Id} {Data}";
        }
    }
}
