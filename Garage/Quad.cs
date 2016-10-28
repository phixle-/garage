using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Quad : Vehicle
    {
        //Antal hästkrafter
        public int HP { get; set; }

        /// <summary>
        /// Konstruktor för fyrhjuling som tar in antal hästkrafter och sedan hjul, färg, värde, namn och typ som skickas till baskonstruktorn.
        /// </summary>
        /// <param name="HP"></param>
        /// <param name="NrOfWheels"></param>
        /// <param name="Color"></param>
        /// <param name="Value"></param>
        /// <param name="Name"></param>
        /// <param name="type"></param>
        public Quad (int HP, int NrOfWheels, string Color, double Value, string Name, string type) : base(NrOfWheels, Color, Value, Name, type)
        {
            this.HP = HP;
        }
    }
}
