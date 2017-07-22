using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_BinaryReadWrite
{
    // GitHub repo.
    // https://github.com/Reevolution/CSharp_Research

    /// <summary>
    /// Simple example read write binary data file.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Write simple data string to file.
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(@"D:\SimpleData.dat")))
            {
                for (int i = 0; i < 5; i++)
                {
                    bw.Write($"Simple string {i}");
                }
            }

            // Read simple data string from file.
            using (BinaryReader br = new BinaryReader(File.OpenRead(@"D:\SimpleData.dat")))
            {
                for (int i = 0; i < 5; i++)
                {
                    string data = br.ReadString();
                    Console.WriteLine(data);
                }
            };
        }
    }
}
