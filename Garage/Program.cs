using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Program
    {
        static UIGarageLayer layer = new UIGarageLayer();
        static Vehicle SelectedVehicle = new Car(4, 4, "Yellow", 53000, "MiniCoopern", "Car");
        static int index = 0;
        static int Indrag = 2;

        static Dictionary<string, Action<string>> MainMenyDictionary = new Dictionary<string, Action<string>>();

        static void Main(string[] args)
        {
            AddToDictionary();
            while(true)
                Menu("Main menu", MainMenyDictionary);
        }

        static void AddToDictionary()
        {
            MainMenyDictionary.Add("Show garage", ShowGarage);
            MainMenyDictionary.Add("Exit application", Exit);
            MainMenyDictionary.Add("Save", layer.Save);
            MainMenyDictionary.Add("Load", layer.Load);
        }

        static void Exit(string name)
        {
            Environment.Exit(0);
        }

        static void ShowGarage(string name)
        {
            int pos = 1;
            while(true)
            {
                Console.Clear();
                Console.WriteLine(name);
                Console.WriteLine("Parked vehicles: \n");
                DrawLayout(pos);
                VehicleInfo(pos);
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (pos >= 1 && pos <= 4)
                        pos += 4;
                    else if (pos >= 4 && pos <= 8)
                        pos -= 4;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if(pos > 1 && pos <= 4 || pos > 5 && pos <= 8)
                        pos--;
                    else if(pos == 1)
                        pos = 4;
                    else if(pos == 5)
                        pos = 8;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (pos >= 1 && pos < 4 || pos > 4 && pos < 8)
                        pos++;
                    else if (pos == 4)
                        pos = 1;
                    else if (pos == 8)
                        pos = 5;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                    return;
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if(SelectedVehicle != null)
                    {
                        if (layer.GetVehicle(pos) == null)
                        {
                            Console.WriteLine("Park your vehicle in spot #{0}? (Y/N)", pos);
                            if(Console.ReadLine()[0].ToString().ToUpper() == "Y")
                            {
                                layer.Park(pos, SelectedVehicle);
                                SelectedVehicle = null;
                            }
                            continue;
                        }
                        else
                            Console.WriteLine("You need to park your current vehicle before leaving with another!");
                    }
                    else if(SelectedVehicle == null)
                    {
                        if (layer.GetVehicle(pos) == null)
                            Console.WriteLine("You need a vehicle before you can park!");
                        else
                        {
                            Console.WriteLine("Do you want to take the {0} for a drive? (Y/N)", layer.GetVehicle(pos).Name);
                            if (Console.ReadLine()[0].ToString().ToUpper() == "Y")
                                SelectedVehicle = layer.Unpark(pos);
                            continue;
                        }
                    }
                    Console.ReadKey();
                }
            }
        }

        static void DrawLayout(int active)
        {
            Console.WriteLine("#################");
            Console.Write("#"); ParkLight(" ", 1, active); Console.Write("#"); ParkLight(" ", 2, active); Console.Write("#"); ParkLight(" ", 3, active); Console.Write("#"); ParkLight(" ", 4, active); Console.WriteLine("#");
            Console.Write("#"); ParkLight("1", 1, active); Console.Write("#"); ParkLight("2", 2, active); Console.Write("#"); ParkLight("3", 3, active); Console.Write("#"); ParkLight("4", 4, active); Console.WriteLine("#");
            Console.Write("#"); ParkLight(" ", 1, active); Console.Write("#"); ParkLight(" ", 2, active); Console.Write("#"); ParkLight(" ", 3, active); Console.Write("#"); ParkLight(" ", 4, active); Console.WriteLine("#");
            Console.WriteLine("#               #");
            Console.WriteLine("#               #");
            Console.WriteLine("#               #");
            Console.Write("#"); ParkLight(" ", 5, active); Console.Write("#"); ParkLight(" ", 6, active); Console.Write("#"); ParkLight(" ", 7, active); Console.Write("#"); ParkLight(" ", 8, active); Console.WriteLine("#");
            Console.Write("#"); ParkLight("5", 5, active); Console.Write("#"); ParkLight("6", 6, active); Console.Write("#"); ParkLight("7", 7, active); Console.Write("#"); ParkLight("8", 8, active); Console.WriteLine("#");
            Console.Write("#"); ParkLight(" ", 5, active); Console.Write("#"); ParkLight(" ", 6, active); Console.Write("#"); ParkLight(" ", 7, active); Console.Write("#"); ParkLight(" ", 8, active); Console.WriteLine("#");
            Console.WriteLine("#################");
        }

        static void VehicleInfo(int index)
        {
            if(layer.GetVehicle(index) != null)
            {
                Vehicle v = layer.GetVehicle(index);
                Console.WriteLine("Parkingspot: {0}\nVehicle: {1}\nName of Vehicle: {2}\nColor: {3}\nValue: {4}\nNr of wheels: {5}", index, v.type, v.Name, v.Color, v.Value, v.NrOfWheels);
            }
            else
            {
                Console.WriteLine("Parkingspot: {0}\nVehicle: {1}\nName of Vehicle: {2}\nColor: {3}\nValue: {4}\nNr of wheels: {5}", index, "N/A", "N/A", "N/A", "N/A", "N/A");
            }
        }

        static void ParkLight(string alternativ, int index, int active)
        {
            if (layer.GetVehicle(index) != null)
            {
                Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), layer.GetVehicle(index).Color, true);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Gray;
            }

            Console.ForegroundColor = ConsoleColor.Black;
            if (active == index && alternativ != " ")
            {
                Console.Write("|" + alternativ + "|");
            }
            else if(active == index && alternativ == " ")
            {
                Console.Write(" - ");
            }
            else
            {
                Console.Write(" " + alternativ + " ");
            }
            Console.ResetColor();
        }

        static void Menu(string name, Dictionary<string, Action<string>> meny)
        {
            name = name.ToUpper();
            if (index >= meny.Count()) { index = 0; }
            Console.Clear();
            int i = 0;
            string active = "";
            while (true)
            {
                Console.Clear();
                Console.WriteLine(name);
                i = 0;
                active = "";
                foreach (KeyValuePair<string, Action<string>> alternativ in meny)
                {

                    Console.WriteLine();
                    Console.CursorLeft = Indrag;
                    if (index == i)
                    {
                        HighLight(alternativ.Key);
                        active = alternativ.Key;
                    }
                    else
                        Console.WriteLine(" "+alternativ.Key);
                    i += 1;
                }

                if (Navigation(active, name, meny)) { return; }
            }
        }

        static bool Navigation(string active, string namn, Dictionary<string, Action<string>> meny)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
                if (index == 0)
                    index = meny.Count() - 1;
                else
                    index -= 1;
            else if (keyInfo.Key == ConsoleKey.DownArrow)
                if (index == meny.Count() - 1)
                    index = 0;
                else
                    index += 1;
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (active == "Backa")
                {
                    index = 0;
                    return true;
                }
                meny[active].Invoke(active);
            }
            return false;
        }

        static void HighLight(string alternativ)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" " + alternativ + " ");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
