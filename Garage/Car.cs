using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Car : Vehicle
    {
        public int NrOfSeats { get; set; }

        public Car(int NrOfSeats, int NrOfWheels, string Color, double Value, string Name, string type) : base(NrOfWheels, Color, Value, Name, type)
        {
            this.NrOfSeats = NrOfSeats;
        }
    }
}
