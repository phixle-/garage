using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Bike : Vehicle
    {
        //Antal växlar
        public int Gears { get; set; }

        /// <summary>
        /// Konstruktor för motor/cykel som tar in växlar och sedan hjul, färg, värde, namn och typ som skickas till baskonstruktorn.
        /// </summary>
        /// <param name="Gears"></param>
        /// <param name="NrOfWheels"></param>
        /// <param name="Color"></param>
        /// <param name="Value"></param>
        /// <param name="Name"></param>
        /// <param name="type"></param>
        public Bike (int Gears, int NrOfWheels, string Color, double Value, string Name, string type) : base(NrOfWheels, Color, Value, Name, type)
        {
            this.Gears = Gears;
        }
    }
}
