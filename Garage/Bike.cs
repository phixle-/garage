using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Bike : Vehicle
    {
        public int Gears { get; set; }

        public Bike (int Gears, int NrOfWheels, string Color, double Value, string Name, string type) : base(NrOfWheels, Color, Value, Name, type)
        {
            this.Gears = Gears;
        }
    }
}
