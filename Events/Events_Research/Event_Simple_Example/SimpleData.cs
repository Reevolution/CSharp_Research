using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Event_Simple_Example
{
    /// <summary>
    /// Simple class describe two event actions.
    /// </summary>
    public class SimpleData
    {
        /// <summary>
        /// First event.
        /// </summary>
        public event Action SimpleEvent_0 = delegate { };

        /// <summary>
        /// Second event.
        /// </summary>
        public event Action<int> SimpleEvent_1 = delegate { };

        public SimpleData()
        {

        }

        /// <summary>
        /// First counter call events.
        /// </summary>
        public void Counter_0()
        {
            for (int i = 0; i < 20; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine($"Counter_0 Event happened. Data = {i}.");
                    SimpleEvent_0();
                    Thread.Sleep(150);
                }
            }
        }

        /// <summary>
        /// Second counter call events.
        /// </summary>
        public void Counter_1()
        {
            for (int i = 0; i < 20; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine($"Counter_1 Event happened {i}");
                    SimpleEvent_1(i);
                    Thread.Sleep(150);
                }
            }
        }
    }
}
