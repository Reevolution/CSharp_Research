using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map.Framework;
using Map.Math;
using Map.Prototype;
using Map.Prototype.Factories;

namespace ConsoleApp1
{
    // http://allcalc.ru/node/752
    // http://ru.onlinemschool.com/math/assistance/vector/angl/
    // http://ru.onlinemschool.com/math/library/vector/angl/

    class Program
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;

        static void Main(string[] args)
        {
            var carFactory = new CarFactory(WIDTH, HEIGHT);
            var cars = carFactory.Create(5);

            var docFactory = new DocFactory(WIDTH, HEIGHT);
            var docs = docFactory.Create(500);

            var calculator = new Calculator();
            var routes = calculator.CalcRoutes(docs, cars);

            var array = new int[WIDTH, HEIGHT];

            int index = 1;
            foreach (var route in routes)
            {
                foreach (var doc in route.Docs)
                {
                    array[(int) doc.Lon, (int) doc.Lat] = index;
                }
                index++;
            }

            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    Console.Write($"{array[i,j]} ");
                }

                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
