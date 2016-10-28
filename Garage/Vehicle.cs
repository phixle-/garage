using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Vehicle
    {
        //Antal hjul
        public int NrOfWheels { get; set; }
        //Vilken färg på fordonet
        public string Color { get; set; }
        //Fordonets värde
        public double Value { get; set; }
        //Namnet på fordonet
        public string Name { get; set; }
        //Vilken typ av fordon
        public string type { get; set; }
        //Vilken parkering den står på
        public int spot { get; set; }

        /// <summary>
        /// Konstruktor för fordon, tar in antal hjul, vilken färg, värde, namn och typ.
        /// </summary>
        /// <param name="NrOfWheels"></param>
        /// <param name="Color"></param>
        /// <param name="Value"></param>
        /// <param name="Name"></param>
        /// <param name="type"></param>
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
