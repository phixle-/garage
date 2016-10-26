using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    class Program
    {
        static UIGarageLayer layer = new UIGarageLayer(7);
        static int index = 0;
        static int Indrag = 2;
        static string Msg = "";
        static Dictionary<string, Action<string>> MainMenyDictionary = new Dictionary<string, Action<string>>();

        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth, 40);
            AddToDictionary();
            while(true)
                Menu("Main menu", MainMenyDictionary);
        }

        

        static void AddToDictionary()
        {
            MainMenyDictionary.Add("Show garage", ShowGarage);
            MainMenyDictionary.Add("Exit application", Exit);
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

        static void Exit(string name)
        {
            Environment.Exit(0);
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
                //Console.Clear();
                Console.SetCursorPosition(0, 0);
                
                Console.WriteLine(name + "\nParked vehicles:");
                ClearLine();
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
                else
                {
                    Search();
                }
            }
        }

        static void Search()
        {
            layer.Search();
        }

        static void OldDrawLayout(int active)
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

        static void DrawLayout(int active)
        {
            
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

            for (int i = 0; i < 8; i+=1)
                Console.WriteLine(new string(' ', Console.WindowWidth));

                Console.SetCursorPosition(x, y);

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
                string[,] figures = new string[,] { { " # # ", "  #  ", " # # " },
                                                    { "  #  ", " |#| ", "  #  " },
                                                    { "  #  ", "  |  ", "  #  " }, 
                                                    { "     ", " ##  ", " ##  " } };

                

                //Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), layer.GetVehicle(index).Color, true);
                string type = layer.GetVehicle(index).type;
                //type = "Car";
                if (type == "Car")
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    ConsoleColor Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), layer.GetVehicle(index).Color, true);
                    if (active == index && alternativ == "0")
                    {
                        Console.Write("| ");
                        Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" |");
                    }
                    else if (active != index && alternativ == "0")
                    {
                        Console.Write("  ");
                        Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    else if (active == index && alternativ != " ")
                    {
                        Console.Write("|{0,1}", alternativ);
                        Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" |");
                    }
                    else if (active == index && alternativ == " ")
                    {
                        Console.Write(" {0,-5} ", "-----");
                    }
                    else if (active != index && alternativ == " ")
                    {

                        Console.Write(" {0,-5} ", "     ");
                    }
                    else
                    {
                        Console.Write("{0,2}", alternativ);
                        Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    Console.ResetColor();
                }
                else if (type == "Quad")
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    ConsoleColor Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), layer.GetVehicle(index).Color, true);
                    if (active == index && alternativ == "0")
                    {
                        Console.Write("|  ");
                        Console.BackgroundColor = Color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  |");
                    }
                    else if (active != index && alternativ == "0")
                    {
                        Console.Write("   ");
                        Console.BackgroundColor = Color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("   ");
                    }
                    else if (active != index && alternativ == "-1")
                    {
                        Console.Write("  ");
                        Console.BackgroundColor = Color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" ");
                        Console.BackgroundColor = Color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    else if (active == index && alternativ != " ")
                    {
                        Console.Write("|{0,1}", alternativ);
                        Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" |");
                    }
                    else if (active == index && alternativ == " ")
                    {
                        Console.Write(" {0,-5} ", "-----");
                    }
                    else if (active != index && alternativ == " ")
                    {

                        Console.Write(" {0,-5} ", "     ");
                    }
                    else
                    {
                        Console.Write("{0,2}", alternativ);
                        Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    Console.ResetColor();
                }
                else if (type == "Lawnmower")
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    ConsoleColor Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), layer.GetVehicle(index).Color, true);
                    if (active == index && alternativ == "0")
                    {
                        Console.Write("| ");
                        Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" |");
                    }
                    else if (active != index && alternativ == "0")
                    {
                        Console.Write("  ");
                        Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    else if (active == index && alternativ != " ")
                    {
                        Console.Write("|{0,1}", alternativ);
                        //Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" |");
                    }
                    else if (active == index && alternativ == " ")
                    {
                        Console.Write(" {0,-5} ", "-----");
                    }
                    else if (active != index && alternativ == " ")
                    {

                        Console.Write(" {0,-5} ", "     ");
                    }
                    else
                    {
                        Console.Write("{0,2}", alternativ);
                       // Console.BackgroundColor = Color;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    Console.ResetColor();
                }
                else if (type == "Bike")
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    ConsoleColor Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), layer.GetVehicle(index).Color, true);
                    if (active == index && alternativ == "0")
                    {
                        Console.Write("|  ");
                        Console.BackgroundColor = Color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  |");
                    }
                    else if (active != index && alternativ == "0")
                    {
                        Console.Write("   ");
                        Console.BackgroundColor = Color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("   ");
                    }
                    else if (active == index && alternativ != " ")
                    {
                        Console.Write("|{0,1} ", alternativ);
                        Console.BackgroundColor = Color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  |");
                    }
                    else if (active == index && alternativ == " ")
                    {
                        Console.Write(" {0,-5} ", "-----");
                    }
                    else if (active != index && alternativ == " ")
                    {

                        Console.Write(" {0,-5} ", "     ");
                    }
                    else
                    {
                        Console.Write("{0,2} ", alternativ);
                        Console.BackgroundColor = Color;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("   ");
                    }
                    Console.ResetColor();
                }
            }
            else
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
            }

            //string type = layer.GetVehicle(index).type;

            

            


            
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
