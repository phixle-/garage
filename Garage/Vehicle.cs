using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Vehicle
    {
        public int NrOfWheels { get; set; }
        public string Color { get; set; }
        public double Value { get; set; }
        public string Name { get; set; }
        public string type { get; set; }

        public Vehicle(int NrOfWheels, string Color, double Value, string Name, string type)
        {
            this.NrOfWheels = NrOfWheels;
            this.Color = Color;
            this.Value = Value;
            this.Name = Name;
            this.type = type;
        }

    }
}
