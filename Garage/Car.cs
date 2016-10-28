using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Car : Vehicle
    {
        //Antal säten i bilen
        public int NrOfSeats { get; set; }

        /// <summary>
        /// Konstruktor för bil som tar in antal säten och sedan hjul, färg, värde, namn och typ som skickas till baskonstruktorn.
        /// </summary>
        /// <param name="NrOfSeats"></param>
        /// <param name="NrOfWheels"></param>
        /// <param name="Color"></param>
        /// <param name="Value"></param>
        /// <param name="Name"></param>
        /// <param name="type"></param>
        public Car(int NrOfSeats, int NrOfWheels, string Color, double Value, string Name, string type) : base(NrOfWheels, Color, Value, Name, type)
        {
            this.NrOfSeats = NrOfSeats;
        }
    }
}
