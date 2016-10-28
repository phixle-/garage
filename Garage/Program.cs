using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Program
    {
        static int spotAmount = 8;
        static UIGarageLayer layer = new UIGarageLayer(spotAmount);
        static int index = 0;
        static int Indrag = 2;
        static string Msg = "";
        static Dictionary<string, Action<string>> MainMenyDictionary = new Dictionary<string, Action<string>>();
        static List<Vehicle> searchResult = new List<Vehicle>();

        static void Main(string[] args)
        {
            
            Console.SetWindowSize(42 + (spotAmount * 4), 40);
            AddToDictionary();
            while(true)
                Menu("Main menu", MainMenyDictionary);
        }

        static void AddToDictionary()
        {
            MainMenyDictionary.Add("Show garage", ShowGarage);
            MainMenyDictionary.Add("Exit application", layer.Exit);
            MainMenyDictionary.Add("Save", Save);
            MainMenyDictionary.Add("Load", Load);
        }

        static void Save(string kaos)
        {
            if (layer.Save("save"))
                Msg = "Saving success!";
            else
                Msg = "Saving failure!";
        }

        static void Load(string kaos)
        {
            if (layer.Load("load"))
                Msg = "Loading success!";
            else
                Msg = "Loading failure!";
        }

        

        static void ClearLine()
        {
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }

        static void ShowGarage(string name)
        {

            int pos = 1;
            int l = layer.GetParkingspots().Length;
            while(true)
            {
                
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(name);
                Console.WriteLine("Parked vehicles:");
                ClearLine();
                int left = Console.CursorLeft + l*4;
                int top = Console.CursorTop;
                if(left > Console.WindowWidth/2)
                    left = 1;
                
                int tempL = Console.CursorLeft;
                int tempT = Console.CursorTop;
                ClearLine(); 
                string search = layer.searchQuery;
                //Console.SetCursorPosition(left-1, top);
                
                ShowSearchResult(search);
                DrawLayout(pos);
                VehicleInfo(pos);
                
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (pos >= 1 && pos <= l/2)
                        pos += l/2;
                    else if (pos >= l/2 && pos <= l)
                        pos -= l/2;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if(pos > 1 && pos <= l/2 || pos > (l/2)+1 && pos <= l)
                        pos--;
                    else if(pos == 1)
                        pos = l/2;
                    else if(pos == (l/2)+1)
                        pos = l;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (pos >= 1 && pos < l/2 || pos > l/2 && pos < l)
                        pos++;
                    else if (pos == l/2)
                        pos = 1;
                    else if (pos == l)
                        pos = (l/2)+1;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                    return;
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if(layer.SelectedVehicle != null)
                    {
                        if (layer.GetVehicle(pos) == null)
                        {
                            Console.WriteLine("Park your vehicle in spot #{0}? (Y/N)", pos);
                            if(Console.ReadLine()[0].ToString().ToUpper() == "Y")
                            {
                                layer.Park(pos, layer.SelectedVehicle);
                                layer.SelectedVehicle = null;
                            }
                            continue;
                        }
                        else
                            Console.WriteLine("You need to park your current vehicle before leaving with another!");
                    }
                    else if (layer.SelectedVehicle == null)
                    {
                        if (layer.GetVehicle(pos) == null)
                            Console.WriteLine("You need a vehicle before you can park!");
                        else
                        {
                            Console.WriteLine("Do you want to take the {0} for a drive? (Y/N)", layer.GetVehicle(pos).Name);
                            if (Console.ReadLine()[0].ToString().ToUpper() == "Y")
                                layer.SelectedVehicle = layer.Unpark(pos);
                            continue;
                        }
                    }
                    Console.ReadKey();
                }
                else if (keyInfo.Key == ConsoleKey.Backspace || char.IsLetter(keyInfo.KeyChar))
                    layer.Search(keyInfo);
            }
        }

        static void ShowSearchResult(string sq)
        {
            int OriginalLeft = Console.CursorLeft;
            int OriginalTop = Console.CursorTop;
            int l = layer.GetParkingspots().Length;
            int left = Console.WindowWidth - 40;
            int top = 5;
            searchResult.Clear();
            if(sq.Length > 0)
            {
                IEnumerable<Vehicle> vQ = layer.GetParkedVehicles().Where(q => q.Name.StartsWith(sq)).Select(q => q).OrderBy(q => q.type);
                foreach (Vehicle v in vQ)
                {
                    if (!searchResult.Contains(v))
                        searchResult.Add(v);
                }
                IEnumerable<Vehicle> vQ2 = layer.GetParkedVehicles().Where(q => q.Name.ToLower().Contains(sq.ToLower())).Select(q => q).OrderBy(q => q.type);
                foreach (Vehicle v in vQ2)
                {
                    if (!searchResult.Contains(v))
                        searchResult.Add(v);
                }
            }
            for (int q = 0; q < 30; q++)
            {
                Console.SetCursorPosition(left, top+q);
                Console.WriteLine("                                         ");
            }
            int limit = searchResult.Count();
            if (limit > 4)
                limit = 4;
            Console.SetCursorPosition(left, top - 2);
            Console.WriteLine("                                             ");
            Console.SetCursorPosition(left, top - 1);
            Console.WriteLine("                                             ");
            Console.SetCursorPosition(left, top - 2);
            if (sq.Length > 0)
                Console.WriteLine("Search: " + sq);
            for (int i = 0; i < limit; i+=1  )
            {
                if(searchResult[i] != null)
                {
                    Console.MoveBufferArea(0, 0, Console.WindowWidth, Console.WindowHeight, 0, 0);
                    Vehicle v = searchResult[i];
                    string spot = v.spot.ToString();
                    if (v.spot == 0)
                        spot = "In use";
                    Console.SetCursorPosition(left, top-1);
                    for (int z = 0; z < 30; z++)
                    {
                        Console.Write("-");
                    }
                    Console.SetCursorPosition(left, top);
                    Console.WriteLine("{0, -18}{1}", "Parkingspot:", spot);
                    Console.SetCursorPosition(left, top + 1);
                    Console.WriteLine("{0, -18}{1}", "Name of Vehicle:", v.Name);
                    Console.SetCursorPosition(left, top + 2);
                    Console.WriteLine("{0, -18}{1}", "Vehicle:", v.type);
                    Console.SetCursorPosition(left, top + 3);
                    Console.WriteLine("{0, -18}{1}", "Color:", v.Color);
                    Console.SetCursorPosition(left, top + 4);
                    Console.WriteLine("{0, -18}{1}", "Value:", v.Value);
                    Console.SetCursorPosition(left, top + 5);
                    Console.WriteLine("{0, -18}{1}", "Nr of wheels:", v.NrOfWheels);
                    Console.SetCursorPosition(left, top);
                    if(top < Console.BufferHeight-8)
                        top += 8;
                }
            }
            Console.SetCursorPosition(OriginalLeft, OriginalTop);
        }

        static void DrawLayout(int active)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            drawTopMidBottom();
            int len = layer.GetParkingspots().Length;
            if (len % 2 != 0)
                len -= 1;
            for (int z = 1; z <= 2; z += 1)
            {
                int q = 1;
                int xMin = 1;
                int xMax = 5;
                for (int x = xMin; x <= xMax; x += 1)
                {
                    if (z == 1)
                        q = 1;
                    else if (z == 2)
                        q = (len / 2) + 1;
                    Console.Write("#");
                    for (int i = 1; i <= (len / 2); i += 1)
                    {
                        string str = "0";
                        if (x == 2)
                            str = q.ToString();
                        else if(x == xMin || x == xMax)
                            str = " ";
                        ParkLight(str, q, active);
                        q += 1;
                        Console.Write("#");
                    }
                    Console.WriteLine();
                }
                if(z == 1)
                {
                    for (int i = 0; i < 2; i += 1)
                    {
                        Console.Write("#");
                        drawTopMidBottom(true);
                        Console.Write("#\n");
                    }
                }
            }
            drawTopMidBottom();
        }

        static void drawTopMidBottom(bool mid = false)
        {
            int len = layer.GetParkingspots().Length;
            if (len % 2 != 0)
                len -= 1;
            if(mid)
            {
                for (int t = 0; t < len * 4 - 1; t += 1)
                {
                    Console.Write(" ");
                }
            }
            else
            {
                for (int f = 0; f < len * 4 + 1; f += 1)
                    Console.Write("#");
                Console.WriteLine();
            }
            
        }

        static void VehicleInfo(int index)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            for (int i = 0; i < 16; i+=1)
                Console.WriteLine(new string(' ', spotAmount * 4));

                Console.SetCursorPosition(x, y);

            if(layer.GetVehicle(index) != null)
            {
                Vehicle v = layer.GetVehicle(index);
                Console.WriteLine(" {0, -20}{1}\n {2, -20}{3}\n {4, -20}{5}\n {6, -20}{7}\n {8, -20}{9}\n {10, -20}{11}", "Parkingspot:", index, "Vehicle:", v.type, "Name of Vehicle:", v.Name, "Color:", v.Color, "Value:", v.Value, "Nr of wheels:", v.NrOfWheels);
            }
            else
            {
                Console.WriteLine(" {0, -20}{1}\n {2, -20}{3}\n {4, -20}{5}\n {6, -20}{7}\n {8, -20}{9}\n {10, -20}{11}", "Parkingspot:", index, "Vehicle:", "N/A", "Name of Vehicle:", "N/A", "Color:", "N/A", "Value:", "N/A", "Nr of wheels:", "N/A");
            }
        }

        static void ParkLight(string alternativ, int index, int active)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            if (active == index && alternativ == "0")
            {
                Console.Write("|     |");
            }
            else if (active != index && alternativ == "0")
            {
                Console.Write("{0,7}", " ");
            }
            else if (active == index && alternativ != " ")
            {
                Console.Write("|{0,-5}|", alternativ);
            }
            else if (active == index && alternativ == " ")
            {
                Console.Write(" {0,-5} ", "-----");
            }
            else
            {
                Console.Write(" {0, -5} ", alternativ);
            }
            Console.ResetColor();

            if (layer.GetVehicle(index) != null)
            {
                ConsoleColor Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), layer.GetVehicle(index).Color, true);
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                string type = layer.GetVehicle(index).type;
                Console.BackgroundColor = Color;
                if (type == "Car")
                {
                    MoveCursorToSpot(index);
                    Console.Write("   ");
                    MoveCursorToSpot(index, 1);
                    Console.Write("   ");
                    MoveCursorToSpot(index, 2);
                    Console.Write("   ");
                }
                else if(type == "Bike")
                {
                    MoveCursorToSpot(index, 1, 1);
                    Console.Write(" ");
                    MoveCursorToSpot(index, 2, 1);
                    Console.Write(" ");
                }
                else if(type == "Quad")
                {
                    MoveCursorToSpot(index);        //Top
                    Console.Write(" ");
                    MoveCursorToSpot(index, 0, 2);
                    Console.Write(" ");             
                    MoveCursorToSpot(index, 1, 1); //MIddle
                    Console.Write(" ");
                    MoveCursorToSpot(index, 2, 0); //Bottom
                    Console.Write(" ");
                    MoveCursorToSpot(index, 2, 2);
                    Console.Write(" ");
                }
                else if(type == "Lawnmower")
                {
                    MoveCursorToSpot(index, 0, 1);
                    Console.Write("  ");
                    MoveCursorToSpot(index, 1, 1);
                    Console.Write("  ");
                }
                Console.ResetColor();
                Console.SetCursorPosition(left, top);
            }
        }

        static void MoveCursorToSpot(int index, int top = 0, int left = 0)
        {
            int totSpot = layer.GetParkingspots().Length;
            int rowMax = totSpot / 2;
            if(index <= rowMax)
            {
                top += 6;
                left += 3 + (8 * (index-1));
            }
            else
            {
                top += 13;
                left += 3 + (8 * ((index -1 ) - rowMax));
            }
            Console.SetCursorPosition(left, top);
        }

        static void Menu(string name, Dictionary<string, Action<string>> meny)
        {
           /* Garage<Bike> gar = new Garage<Bike>(21);
            Bike bike = new Bike(0, 5, "g", 23, "Tyep", "Vehicle");
            gar.Park(2, bike);
            gar.GetParkedVehicles()[0].
            Vehicle ve = new Vehicle(5, "g", 23, "Tyep", "Vehicle");
            
            ve = bike;*/




            name = name.ToUpper();
            if (index >= meny.Count()) { index = 0; }
            Console.Clear();
            int i = 0;
            string active = "";
            while (true)
            {
                Console.SetCursorPosition(0, 0);
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
                Console.WriteLine("\n" + Msg + "\n");
                Console.WriteLine("Arrowkeys: " + (char)0x18 + (char)0x19 + (char)0x1A + (char)0x1B);
                Console.WriteLine("Enter: Interact" + (char)13);
                Console.WriteLine("Escape: Back");
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
