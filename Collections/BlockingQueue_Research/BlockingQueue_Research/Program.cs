using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prototype.System.Collections.Concurrent;

namespace BlockingQueue_Research
{
    class Program
    {
        static void Main(string[] args)
        {
            BlockingQueue<int> blockingQueue = new BlockingQueue<int>();

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                blockingQueue.Enqueue(42);
            });

            if (blockingQueue.TryDequeue(out int item, 3000))
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
