using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Simple_Example
{
    // GitHub repo.
    // https://github.com/Reevolution/CSharp_Research

    /// <summary>
    /// Simple example two events actions.
    /// First simple event action just call action.
    /// Second simple event action call action and send data from event.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            SimpleData simpleData = new SimpleData();

            simpleData.SimpleEvent_0 += SimpleAction_0;
            simpleData.SimpleEvent_1 += SimpleAction_1;

            Console.WriteLine("Second event counter");
            simpleData.Counter_0();

            Console.WriteLine("First event counter");
            simpleData.Counter_1();
        }

        /// <summary>
        /// Simple action no data.
        /// </summary>
        static void SimpleAction_0()
        {
            Console.WriteLine("Simple action after event.");
        }

        /// <summary>
        /// Simple action get data from event.
        /// </summary>
        /// <param name="i"></param>
        static void SimpleAction_1(int i)
        {
            Console.WriteLine($"Simple action after event. Data from event {i}");
        }
    }
}
