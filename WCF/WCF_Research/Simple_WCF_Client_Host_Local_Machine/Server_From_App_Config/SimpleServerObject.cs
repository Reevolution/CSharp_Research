using SimpleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_From_App_Config
{
    /// <summary>
    /// Class implemented IService interface received data from client and run method.
    /// </summary>
    public class SimpleServerObject : IService
    {
        public void SendData(string data)
        {
            Console.WriteLine("Method run on server.");
            Console.WriteLine($"Data received {data}.");
            Console.WriteLine("Method run on server.");
        }
    }
}
