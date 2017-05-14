using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server_From_App_Config
{
    // GitHub repo.
    // https://github.com/Reevolution/CSharp_Research

    /// <summary>
    /// Simple WCF server using Named Pipes. Received data from client.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Registr SimpleServerObject like simgleton.
                // When client connect to host he run SendData method from SimpleServerObject.
                using (ServiceHost host = new ServiceHost(typeof(SimpleServerObject)))
                {
                    host.Open();
                    Console.WriteLine("Server open");
                    Console.WriteLine("Please Enter close the server");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
