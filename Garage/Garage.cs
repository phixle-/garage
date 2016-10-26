using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Garage
    {
        public Vehicle[] ParkingSpots;
        Random rand = new Random();

        public Garage(int num)
        {
            if (num % 2 != 0)
                num -= 1;
            this.ParkingSpots = new Vehicle[num];
        }

        public List<int> GetFreeSpots()
        {
            List<int> freeSlots = new List<int>();

            for (int i = 1; i < ParkingSpots.Length+1; i += 1 )
            {
                if(ParkingSpots[i-1] == null)
                {
                    freeSlots.Add(i);
                }
            }
            return freeSlots;
        }

        public bool Park(int spot, Vehicle v)
        {
            List<int> freeSlots = GetFreeSpots();
            if(freeSlots.Contains(spot))
            {
                ParkingSpots[spot-1] = v;
                return true;
            }
            return false;
        }

        public Vehicle Unpark(int spot)
        {
            spot -= 1;
            Vehicle v = null;
            if(ParkingSpots[spot] != null)
            {
                v = ParkingSpots[spot];
                ParkingSpots[spot] = null;
            }
            return v;
        }

        public List<Vehicle> GetParkedVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            for (int i = 0; i < ParkingSpots.Length; i+=1 )
            {
                if(ParkingSpots[i] != null)
                {
                    vehicles.Add(ParkingSpots[i]);
                }
            }
            return vehicles;
        }

    }
}
