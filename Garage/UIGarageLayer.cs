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
        Garage<Vehicle> g;
        public Vehicle SelectedVehicle;
        Random rand = new Random();

        public UIGarageLayer(int size, bool load = false)
        {
            g = new Garage<Vehicle>(size);
            SelectedVehicle = new Car(4, 4, "Yellow", 53000, "MiniCoopern", "Car");
            if (load && !File.Exists(Directory.GetCurrentDirectory() + @"\Save.txt"))
            {
                Load("loading");
            }
            else
            {
                AddVehicles();
            }
        }

        public Vehicle[] GetParkingspots()
        {
            return g.ParkingSpots;
        }

        public bool Save(string tjoller)
        {
            try
            {
                Vehicle[] vehicles = g.ParkingSpots;
                string lines = "";
                if (SelectedVehicle != null)
                    lines = SelectedVehicle.type + "," + SelectedVehicle.Name + "," + SelectedVehicle.Color + "," + SelectedVehicle.Value + "," + SelectedVehicle.NrOfWheels + "\r\n";
                else
                    lines = "\r\n";
                for (int i = 0; i < vehicles.Length; i += 1)
                {
                    Vehicle v = vehicles[i];

                    if (v != null)
                        lines += v.type + "," + v.Name + "," + v.Color + "," + v.Value + "," + v.NrOfWheels + "\r\n";
                    else
                        lines += "\r\n";
                }
                StreamWriter file = new StreamWriter(Directory.GetCurrentDirectory() + @"\Save.txt");
                lines.TrimEnd();
                file.WriteLine(lines);

                file.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Load(string tjoller)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Save.txt"))
            {
                StreamReader file = new StreamReader(Directory.GetCurrentDirectory() + @"\Save.txt");
                List<Vehicle> vehicles = new List<Vehicle>();
                int index = 0;
                while(file.Peek() >= 0)
                {
                    string line = file.ReadLine();
                    string[] splitted = line.Split(',');
                    if (splitted.Length > 1)
                    {
                        string type = splitted[0];
                        string name = splitted[1];
                        string color = splitted[2];
                        string value = splitted[3];
                        string NoW = splitted[4];

                        Vehicle v = new Vehicle(int.Parse(NoW), color, double.Parse(value), name, type);
                        if (index == 0)
                            SelectedVehicle = v;
                        else
                            vehicles.Add(v);
                    }
                    else
                        if (index != 0)
                            vehicles.Add(null);
                    
                    index += 1;
                }
                Vehicle[] newArr = new Vehicle[vehicles.Count()-1];
                for (int i = 0; i < vehicles.Count()-1; i += 1)
                {
                    newArr[i] = vehicles[i];
                }
                g.ParkingSpots = newArr;
                file.Close();
                return true;
            }
            return false;
            
        }

        public void Search()
        {

        }

        public Vehicle GetVehicle(int index)
        {
            return g.ParkingSpots[index -1];
        }

        public List<Vehicle> GetParkedVehicles()
        {
            return g.GetParkedVehicles();
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
                    Vehicle v = new Vehicle(wheels, color, value, name, type);
                    v.spot = i + 1;
                    g.ParkingSpots[i] = v;
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
