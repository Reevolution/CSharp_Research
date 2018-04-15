using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map.Framework;

namespace Map.Prototype
{
    public class PrototypeRoute : IRoute
    {
        public string Id { get; set; }
        public List<IDoc> Docs { get; set; }
        public ICar Car { get; set; }

        public PrototypeRoute(string id, ICar car)
        {
            Id = id;
            Car = car;
            Docs = new List<IDoc>();
        }
    }
}
