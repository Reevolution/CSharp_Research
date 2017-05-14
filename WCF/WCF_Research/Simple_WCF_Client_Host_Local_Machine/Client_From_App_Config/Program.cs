using SimpleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client_From_App_Config
{
    // GitHub repo.
    // https://github.com/Reevolution/CSharp_Research

    /// <summary>
    /// Simple WCF client using Named Pipes. Call method from server and send data.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChannelFactory<IService> myChannelFactory = new ChannelFactory<IService>("ProductServiceEndPoint");
                IService proxy = myChannelFactory.CreateChannel();

                proxy.SendData("SIMPLE DATA FROM CLIENT");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}
