using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Quad : Vehicle
    {
        public int HP { get; set; }

        public Quad (int HP, int NrOfWheels, string Color, double Value, string Name, string type) : base(NrOfWheels, Color, Value, Name, type)
        {
            this.HP = HP;
        }
    }
}
