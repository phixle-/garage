using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Garage<T> : IEnumerable<T> where T : Vehicle
    {
        //ParkingSpots innehåller garagets parkeringar. Antingen innehåller en parkering ett fordon eller värdet null
        public T[] ParkingSpots;
        //Skapa nytt random objekt för att kunna slumpa tal
        public Random rand = new Random();

        
        /// <summary>
        /// Konstruktor för Garage, tar in en integer som anger hur många parkeringar garagets ska innehålla.
        /// </summary>
        /// <param name="size"></param>
        public Garage(int size)
        {
            //För att göra det lättare att hantera parkeringarna i UI'n ser jag till att det är ett jämt antal parkeringar.
            if (size % 2 != 0)
                size -= 1;
            this.ParkingSpots = new T[size];
        }

        /// <summary>
        /// Returnerar en lista av tomma parkeringar.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Försöker parkera på vald parkering med det fordon du kör. Returnerar True om det gick parkera och false om det inte gick.
        /// </summary>
        /// <param name="spot"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Park(int spot, T v)
        {
            List<int> freeSlots = GetFreeSpots();
            if(freeSlots.Contains(spot))
            {
                ParkingSpots[spot-1] = v;
                v.spot = spot;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Försök ta en bil och lämna parkeringen.
        /// </summary>
        /// <param name="spot"></param>
        /// <returns></returns>
        public T Unpark(int spot)
        {
            //Måste ta spot - 1 för att det visuella indexet för t.ex parking 1 är 1.
            spot -= 1;
            T v = null;
            //Kolla om det står ett fordon på parkeringen.
            if(ParkingSpots[spot] != null)
            {
                v = ParkingSpots[spot];
                ParkingSpots[spot] = null;
            }
            return v;
        }

        public void RemoveVehicle(int spot)
        {
            spot -= 1;
            if (ParkingSpots[spot] != null)
                ParkingSpots[spot] = null;
        }
        
        public void CreateVehicle(int NrOfWheels, string Color, double Value, string Name, string type, int NrOfSeats = 0, int HP = 0, int Gears = 0, bool Collector = false)
        {
            if(type == "Car")
            {
                Vehicle v = new Car(NrOfSeats, NrOfWheels, Color, Value, Name, type);
                v.spot = GetFreeSpots()[rand.Next(GetFreeSpots().Count)];
            }
            else if(type == "Bike")
            {
                Vehicle v = new Bike(Gears, NrOfWheels, Color, Value, Name, type);
                v.spot = GetFreeSpots()[rand.Next(GetFreeSpots().Count)];
            }
            else if(type == "Quad")
            {
                Vehicle v = new Quad(HP, NrOfWheels, Color, Value, Name, type);
                v.spot = GetFreeSpots()[rand.Next(GetFreeSpots().Count)];
            }
            else if(type == "Lawnmower")
            {
                Vehicle v = new Lawnmower(Collector, NrOfWheels, Color, Value, Name, type);
                v.spot = GetFreeSpots()[rand.Next(GetFreeSpots().Count)];
            }
        }

        /// <summary>
        /// Returnerar en lista på alla parkerade fordon.
        /// </summary>
        /// <returns></returns>
        public List<T> GetParkedVehicles()
        {
            List<T> vehicles = new List<T>();

            for (int i = 0; i < ParkingSpots.Length; i+=1 )
            {
                if(ParkingSpots[i] != null)
                {
                    vehicles.Add(ParkingSpots[i]);
                }
            }
            return vehicles;
        }

        /// <summary>
        /// Loopar igenom alla parkeringar och returnerar varje parkering.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in ParkingSpots)
            {
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
