using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Simple_ListBox_Filter_MVVM
{
    public class MainWindowViewModel
    {
        public ObservableCollection<BaseData> Items { get; set; }

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<BaseData>()
            {
                new BaseData(){Id = 5, Data = "Simple data"},
                new BaseData(){Id = 32, Data = "Another data"},
                new BaseData(){Id = 7, Data = "What ?"},
                new BaseData(){Id = 5554, Data = "Heyyyy"},
                new BaseData(){Id = 3348, Data = "VEGA"},
            };
        }
    }
}
