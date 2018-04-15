using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map.Framework;

namespace Map.Prototype
{
    public class PrototypeDoc : IDoc
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public double Lat { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public double Lon { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="PrototypeDoc"/> class.
        /// </summary>
        /// <param name="lat">Географическая широта</param>
        /// <param name="lon">Географическая долгота</param>
        /// <param name="weight">Вес товара</param>
        public PrototypeDoc(double lat, double lon, double weight)
        {
            Lat = lat;
            Lon = lon;
            Weight = weight;
        }
    }
}
