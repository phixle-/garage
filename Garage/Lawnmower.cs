using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Lawnmower : Vehicle
    {
        //Har gräsklipparen uppsamlare?
        public bool Collector { get; set; }

        /// <summary>
        /// Konstruktor för gräsklippare som tar in bool Collector och sedan hjul, färg, värde, namn och typ som skickas till baskonstruktorn.
        /// </summary>
        /// <param name="Collector"></param>
        /// <param name="NrOfWheels"></param>
        /// <param name="Color"></param>
        /// <param name="Value"></param>
        /// <param name="Name"></param>
        /// <param name="type"></param>
        public Lawnmower(bool Collector, int NrOfWheels, string Color, double Value, string Name, string type) : base(NrOfWheels, Color, Value, Name, type)
        {
            this.Collector = Collector;
        }
    }
}
