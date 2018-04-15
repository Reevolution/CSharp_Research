using System;
using System.Collections.Generic;
using Map.Framework;
using Map.Math;

namespace Map.Prototype.Factories
{
    public class DocFactory
    {
        private readonly int _width;
        private readonly int _height;

        public DocFactory(int width, int height)
        {
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Creates docs.
        /// </summary>
        /// <returns></returns>
        public List<IDoc> Create(int count)
        {
            var docs = new List<IDoc>();

            var rand = new Random((int) DateTime.Now.Ticks);
            var vector2S = new HashSet<Vector2>();

            for (int i = 0; i < count; i++)
            {
                var vector2 = new Vector2((float) rand.Next(0, _width), (float) rand.Next(0, _height));
                vector2S.Add(vector2);
            }

            foreach (var n in vector2S)
            {
                var doc = new PrototypeDoc(n.Y, n.X, (float) rand.Next(128, 256));
                docs.Add(doc);
            }

            return docs;
        }
    }
}
