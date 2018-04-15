using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map.Framework;

namespace Map.Prototype
{
    public class PrototypeCar : ICar
    {
        public double StartLat { get; }
        public double StartLon { get; }
        public double MaxWeight { get; }

        private PrototypeRoute _prototypeRoute;

        public PrototypeCar(double startLat, double startLon, double maxWeight)
        {
            StartLat = startLat;
            StartLon = startLon;
            MaxWeight = maxWeight;
        }

        public IRoute CreateRoute()
        {
            _prototypeRoute = new PrototypeRoute(Guid.NewGuid().ToString(), this);
            return _prototypeRoute;
        }
    }
}
