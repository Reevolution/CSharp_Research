using System;
using System.Collections.Generic;
using Map.Framework;
using Map.Math;

namespace Map.Prototype.Factories
{
    public class CarFactory
    {
        private readonly int _width;
        private readonly int _height;

        public CarFactory(int width, int height)
        {
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Creates cars.
        /// </summary>
        /// <returns></returns>
        public List<ICar> Create(int count)
        {
            var cars = new List<ICar>();

            var rand = new Random((int)DateTime.Now.Ticks);
            var vector2S = new HashSet<Vector2>();

            for (int i = 0; i < count; i++)
            {
                var vector2 = new Vector2((float)rand.Next(0, _width), (float)rand.Next(0, _height));
                vector2S.Add(vector2);
            }

            foreach (var n in vector2S)
            {
                var doc = new PrototypeCar(n.Y, n.X, (float)rand.Next(1024, 2048));
                cars.Add(doc);
            }

            return cars;
        }
    }
}
