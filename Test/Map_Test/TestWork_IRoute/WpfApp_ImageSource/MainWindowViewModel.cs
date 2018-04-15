using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Map.Framework;
using Map.Prototype;
using WpfApp_ImageSource.Annotations;

namespace WpfApp_ImageSource
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public List<IRoute> Routes { get; set; }

        private IRoute _routeSelectedItem;

        public IRoute RouteSelectedItem
        {
            get { return _routeSelectedItem; }
            set
            {
                _routeSelectedItem = value;
                OnPropertyChanged(nameof(RouteSelectedItem));
                var car = ((PrototypeRoute) _routeSelectedItem).Car;
                this.Image = _imageFactory.Create(_routeSelectedItem, car);
                this.Car = car;
            }
        }

        private BitmapSource _image;

        public BitmapSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private ICar _car;

        public ICar Car
        {
            get { return _car; }
            set
            {
                _car = value;
                OnPropertyChanged(nameof(Car));
            }
        }

        private readonly ImageFactory _imageFactory;

        public MainWindowViewModel(List<IRoute> routes, ImageFactory imageFactory)
        {
            Routes = routes;
            _imageFactory = imageFactory;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
