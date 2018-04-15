using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Map.Framework;
using Map.Math;
using Map.Prototype;
using Map.Prototype.Factories;

namespace WpfApp_ImageSource
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WIDTH = 128;
        private const int HEIGHT = 128;

        public MainWindow()
        {
            InitializeComponent();

            var carFactory = new CarFactory(WIDTH, HEIGHT);
            var cars = carFactory.Create(5);

            var docFactory = new DocFactory(WIDTH, HEIGHT);
            var docs = docFactory.Create(500);

            var calculator = new Calculator();
            var routes = calculator.CalcRoutes(docs, cars);

            var imageFactory = new ImageFactory(WIDTH, HEIGHT);

            var mainWindowViewModel = new MainWindowViewModel(routes, imageFactory);
            this.DataContext = mainWindowViewModel;
        }
    }
}
