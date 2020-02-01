using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TFrameWork.CallCenter.UI.Model;

namespace TFrameWork.CallCenter.UI.Model
{
    public class MainWindowViewModel : INotifyPropertyChanged 
    {
        readonly ObservableCollection<EmployeeModel> _employees = new ObservableCollection<EmployeeModel>();
        readonly ObservableCollection<CustomerModel> _customers = new ObservableCollection<CustomerModel>();

        readonly IEmployeesModel _model;

        public MainWindowViewModel() : this(new EmployeesModel())
        {

        }

        public MainWindowViewModel(IEmployeesModel model)
        {
            _model = model;

            _model.EmployeesLoadCompleted += _model_EmployeesLoadCompleted;

            Init();
        }

        public IEnumerable<EmployeeModel> Employees { get { return _employees; } }
        public IEnumerable<CustomerModel> Customers { get { return _customers; } }

        private CustomerModel _selectedCustomer;

        public CustomerModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;

                    OnPropertyChanged(nameof(SelectedCustomer));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddCustomer()
        {
            var customer = new CustomerModel()
            {
                Guid = Guid.NewGuid().ToString()
            };

            var login = _model.GetFreeEmployee();

            if (!string.IsNullOrEmpty(login))
            {
                var employee = _employees.Single(n => n.Login == login);
                employee.Status = EmployeeStatus.Busy;
                customer.Employee = employee;

                _customers.Add(customer);

                MessageBox.Show($"С вами на связи {employee.Login} {employee.Status}", "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Нет свободных сотрудников", "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
            }        
        }

        public void RemoveCustomer()
        {
            if (SelectedCustomer != null)
            {
                var employee = SelectedCustomer.Employee;
                employee.Status = EmployeeStatus.Free;

                _customers.Remove(SelectedCustomer);
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Init()
        {
            _model.InitEmploees();
        }

        private void _model_EmployeesLoadCompleted(object sender, EventArgs e)
        {
            var employees = _model.GetEmployees();

            foreach (var employee in employees)
            {
                _employees.Add(employee);
            }
        }
    }
}
