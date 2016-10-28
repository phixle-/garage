using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Garage
{
    class GarageHandler
    {
        //colors innehåller olika färger
        string[] colors = { "Blue", "Yellow", "Green", "Red", "Cyan", "Magenta", "DarkGreen", "DarkBlue", "DarkYellow", "DarkCyan", "DarkCyan" };
        //types består av olika fordonstyper
        string[] types = { "Car", "Bike", "Lawnmower", "Quad" };
        //CarNames innehåller olika bilnamn
        string[] CarNames = { "Mini Cooper", "Volvo 240", "Renault Scénica", "Opel Vivaro" };
        //BikeNames innehåler olika motor/cykelnamn
        string[] BikeNames = { "Honda 125hk", "Yamaha Wheelie", "Electric Bicycle" };
        //LawnmowerNames består av olika gräsklipparnamn
        string[] LawnmowerNames = { "Huswvarna Rider", "Stiga Tornado", "Traktor McCulloch" };
        //QuadNames innehåller olika fyrhjulingsnamn
        string[] QuadNames = { "Loncin 90-B", "Cobra Automatic", "Loncin QuadSnake" };
        
        //Fordonet användaren kör just nu
        public Vehicle SelectedVehicle;
        //Sökfrasen som används för att söka bland de parkerade fordonen
        public string searchQuery = "";

        //Variabel för att hålla det aktiva garaget
        Garage<Vehicle> g;
        
        /// <summary>
        /// Konstruktor som tar in antal parkeringar och om vi ska ladda från en sparfil
        /// </summary>
        /// <param name="size"></param>
        /// <param name="load"></param>
        public GarageHandler(int size, bool load = false)
        {
            //Skapa nytt garage med specificerad storlek
            g = CreateGarage(size);
            //Skapa ny bil åt användaren
            SelectedVehicle = new Car(4, 4, "Yellow", 53000, "Mini Coopern", "Car");

            //Kolla om vi ska ladda sparning från en fil och i så fall kolla om filen i fråga existerar.
            if (load && !File.Exists(Directory.GetCurrentDirectory() + @"\Save.txt"))
            {
                //Ladda information
                Load("loading");
            }
            else
            {
                //Generera fordon
                AddVehicles();
            }
        }

        /// <summary>
        /// Returnerar ett nyskapat garage med specificerad storlek
        /// </summary>
        /// <param name="spots"></param>
        /// <returns></returns>
        public Garage<Vehicle> CreateGarage(int spots)
        {
            return new Garage<Vehicle>(spots);
        }

        /// <summary>
        /// Stänger ned programmet
        /// </summary>
        /// <param name="name"></param>
        public void Exit(string name)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Returnera en array av alla parkeringar
        /// </summary>
        /// <returns></returns>
        public Vehicle[] GetParkingspots()
        {
            List<Vehicle> spots = new List<Vehicle>();
            foreach (Vehicle v in g)
            {
                spots.Add(v);
            }
            return spots.ToArray();
        }

        /// <summary>
        /// Hämtar alla parkeringar och formaterar varje rad som sedan skrivs in i Save.txt. Returnerar true om det gick att spara, false om det inte gick
        /// </summary>
        /// <param name="tjoller"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returnerar true om det gick att ladda, false om det inte gick. Load kollar varje rad i Save.txt och skapar nya fordon av informationen i Save.txt 
        /// </summary>
        /// <param name="tjoller"></param>
        /// <returns></returns>
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
                        else if (index == 0)
                            SelectedVehicle = null;
                    
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

        /// <summary>
        /// Tar emot en tangent. Om det är en backspace tar vi bort sista bokstaven i searchQuery. Om det inte var backspace är det en bokstav och i det fallet läggs bokstaven till på slutet av searchQuery
        /// </summary>
        /// <param name="key"></param>
        public void Search(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                if (searchQuery.Length > 0)
                {
                    searchQuery = searchQuery.Substring(0, searchQuery.Length - 1);
                }
            }
            else
            {
                searchQuery += key.KeyChar.ToString();
            }
        }

        /// <summary>
        /// Returnerar fordonet för specificerad parkering
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vehicle GetVehicle(int index)
        {
            return g.ParkingSpots[index -1];
        }

        /// <summary>
        /// Returnerar en lista av alla parkerade fordon
        /// </summary>
        /// <returns></returns>
        public List<Vehicle> GetParkedVehicles()
        {
            return g.GetParkedVehicles();
        }

        public void CreateVehicle(int NrOfWheels, string Color, double Value, string Name, string type, int NrOfSeats, int HP, int Gears, bool Collector)
        {
            g.CreateVehicle(NrOfWheels, Color, Value, Name, type, NrOfSeats, HP, Gears, Collector);
        }

        /// <summary>
        /// Generar slumpmässigt nya fordon baserat på hur många parkeringar som existerar
        /// </summary>
        private void AddVehicles()
        {
            for (int i = 0; i < g.ParkingSpots.Length; i += 1)
            {
                if(g.rand.Next(2) == 1)
                {
                    string type = types[g.rand.Next(types.Length)];
                    int wheels = 4;
                    string name = "";

                    if (type == "Car")
                    {
                        name = CarNames[g.rand.Next(CarNames.Length)];
                    }
                    else if (type == "Bike")
                    {
                        wheels = 2;
                        name = BikeNames[g.rand.Next(BikeNames.Length)];
                    }
                    else if (type == "Lawnmower")
                    {
                        name = LawnmowerNames[g.rand.Next(LawnmowerNames.Length)];
                    }
                    else if (type == "Quad")
                    {
                        name = QuadNames[g.rand.Next(QuadNames.Length)];
                    }

                    string color = colors[g.rand.Next(colors.Length)];
                    double value = g.rand.Next(100000);
                    Vehicle v = new Vehicle(wheels, color, value, name, type);
                    v.spot = i + 1;
                    g.ParkingSpots[i] = v;
                }
            }
        }

        /// <summary>
        /// Park kallas från UIn som sedan kallar på Park i Garage
        /// </summary>
        /// <param name="s"></param>
        /// <param name="v"></param>
        public void Park(int s, Vehicle v)
        {
            g.Park(s, v);
        }

        /// <summary>
        /// Unpark kallas från UIn som sedan kallar på Unpark i Garage
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Vehicle Unpark(int s)
        {
            return g.Unpark(s);
        }
    }
}
