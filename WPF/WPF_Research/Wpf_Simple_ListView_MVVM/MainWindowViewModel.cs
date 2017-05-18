using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Simple_ListView_MVVM
{
    /// <summary>
    /// Simple MainWindow View Model
    /// </summary>
    public class MainWindowViewModel
    {
        public ObservableCollection<SimpleData> Items { get; set; }

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<SimpleData>()
            {
                new SimpleData(){Id= 23, FirstName = "Evgeny", SecondName = "Petrov"  },
                new SimpleData(){Id= 1, FirstName = "Mixail", SecondName = "Volkov"  },
                new SimpleData(){Id= 55, FirstName = "Dmitry", SecondName = "Serov"  },
                new SimpleData(){Id= 255, FirstName = "Sergey", SecondName = "Smolov"  },
                new SimpleData(){Id= 34, FirstName = "Alex", SecondName = "Ivanov" }
            };
        }
    }
}
