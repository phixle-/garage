using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Garage
{
    class UIGarageLayer
    {
        string[] colors = { "Blue", "Yellow", "Green", "Red", "Cyan", "Magenta", "DarkGreen", "DarkBlue", "DarkYellow", "DarkCyan", "DarkCyan" };
        string[] types = { "Car", "Bike", "Lawnmower", "Quad" };
        string[] CarNames = { "Mini Cooper", "Volvo 240", "Renault Scénica", "Opel Vivaro" };
        string[] BikeNames = { "Honda 125hk", "Yamaha Wheelie", "Electric Bicycle" };
        string[] LawnmowerNames = { "Huswvarna Rider", "Stiga Tornado", "Traktor McCulloch" };
        string[] QuadNames = { "Loncin 90-B", "Cobra Automatic", "Loncin QuadSnake" };

        Garage g = new Garage(8);
        Random rand = new Random();

        public UIGarageLayer(bool load = false)
        {
            if (load && !File.Exists(Directory.GetCurrentDirectory() + @"\Save.txt"))
            {
                Load("loading");
            }
            else
            {
                AddVehicles();
            }
        }

        public void Save(string tjoller)
        {
            Vehicle[] vehicles = g.ParkingSpots;
            string lines = "";
            foreach(Vehicle v in vehicles)
            {
                if (v != null)
                    lines += v.type + "/" + v.Name + "!" + v.Color + "#" + v.Value + "|" + v.NrOfWheels + "\r\n";
                else
                    lines += "\r\n";
            }

            StreamWriter file = new StreamWriter(Directory.GetCurrentDirectory() + @"\Save.txt");
            file.WriteLine(lines);

            file.Close();
        }

        public void Load(string tjoller)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Save.txt"))
            {
                StreamReader file = new StreamReader(Directory.GetCurrentDirectory() + @"\Save.txt");
                Vehicle[] vehicles = new Vehicle[8];
                int index = 0;
                while(file.Peek() >= 0)
                {
                    string line = file.ReadLine();
                    string type = line.Split('/')[0];
                    string name = line.Split('!')[0];
                    string color = line.Split('#')[0];
                    string value = line.Split('|')[0];
                    string NoW = line;

                    Vehicle v = new Vehicle(int.Parse(NoW), color, double.Parse(value), name, type);
                    vehicles[index] = v;
                    index += 1;
                }
                g.ParkingSpots = vehicles;
            }
        }

        public Vehicle GetVehicle(int index)
        {
            return g.ParkingSpots[index - 1];
        }

        private void AddVehicles()
        {
            for (int i = 0; i < g.ParkingSpots.Length; i += 1)
            {
                if(rand.Next(2) == 1)
                {
                    string type = types[rand.Next(types.Length)];
                    int wheels = 4;
                    string name = "";

                    if (type == "Car")
                    {
                        name = CarNames[rand.Next(CarNames.Length)];
                    }
                    else if (type == "Bike")
                    {
                        wheels = 2;
                        name = BikeNames[rand.Next(BikeNames.Length)];
                    }
                    else if (type == "Lawnmower")
                    {
                        name = LawnmowerNames[rand.Next(LawnmowerNames.Length)];
                    }
                    else if (type == "Quad")
                    {
                        name = QuadNames[rand.Next(QuadNames.Length)];
                    }

                    string color = colors[rand.Next(colors.Length)];
                    double value = rand.Next(100000);

                    g.ParkingSpots[i] = new Vehicle(wheels, color, value, name, type);
                }
            }
        }

        public void Park(int s, Vehicle v)
        {
            g.Park(s, v);
        }

        public Vehicle Unpark(int s)
        {
            return g.Unpark(s);
        }
    }
}
