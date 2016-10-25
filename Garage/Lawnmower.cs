using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Lawnmower : Vehicle
    {
        public bool Collector { get; set; }

        public Lawnmower(bool Collector, int NrOfWheels, string Color, double Value, string Name, string type) : base(NrOfWheels, Color, Value, Name, type)
        {
            this.Collector = Collector;
        }
    }
}
