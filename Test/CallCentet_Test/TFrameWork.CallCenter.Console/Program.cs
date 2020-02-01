using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TFrameWork.CallCenter.ServiceLib;

namespace TFrameWork.CallCenter.Console
{
    class Program
    {
        // static EmployeeService _employeeService;

        static void Main(string[] args)
        {
            //Timer _timeOut = new Timer(1000);

            //_timeOut.Elapsed += _timeOut_Elapsed; 

            //_timeOut.Start();

            //_timeOut.Stop();



            Test();

            System.Console.ReadLine();
        }

        private static void _timeOut_Elapsed(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine("...");
        }

        private static void Test()
        {
            //var xmlEmployeeSettings = new XmlEmployeeSettings();

            //_employeeService = new EmployeeService(xmlEmployeeSettings);

            //_employeeService.UpdateState(new EmployeeModel() { Category = EmployeeCategory.Operator, Login = "petrov", Status = Status.Free });

            //var employee = _employeeService.GetFreeEmloyee();

            var client = new CallCenterServiceServiceReference.CallCenterServiceClient();

            //client.Login("petrov");

            //client.UpdateState("petrov", CallCenterServiceServiceReference.Status.Free);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);

                    var empl = client.GetFreeEmloyee();

                    System.Console.WriteLine($"Status = {empl.Login} {empl.Status}");
                }
            });

            System.Console.ReadLine();
        }
    }
}
