using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Garage
{
    class Program
    {
        //Antal parkeringar
        static int spotAmount;
        //Skapa objektet gh som är ett lager mellan garage och program
        static GarageHandler gh;
        //index för menyn
        static int index = 0;
        //indrag på huvudmenyn för extra finess
        static int Indrag = 2;
        //Meddelande i huvudmenyn som rapporterar om det gick att spara/ladda eller inte
        static string Msg = "";
        //Dictionary som sköter alternativen i huvudmenyn
        static Dictionary<string, Action<string>> MainMenyDictionary = new Dictionary<string, Action<string>>();
        //Lista av sökresultat vid sökning av parkerade fordon
        static List<Vehicle> searchResult = new List<Vehicle>();

        static void Main(string[] args)
        {
            //Låt användaren skapa nytt garage
            CreateGarage();
            
            //Lägg till alternativ till huvudmenyn
            AddToDictionary();

            //Kör huvudmenyn
            Menu("Main menu", MainMenyDictionary);
        }

        /// <summary>
        /// Låter användaren försöka skapa ett garage, användaren tar sig inte vidare utan att skapa ett garage
        /// </summary>
        static void CreateGarage()
        {
            //Roliga fraser
            string fras1 = "Bu.. But you need a garage to park in the garage...";
            string fras2 = "Lets try this again.. Dont fail..";
            string fras3 = "Sigh.. . Lets start over...";

            //Loop som fortsätter tills användaren lyckats skapa ett garage
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Would you like to create a Garage? (Y/N)");
                if (Console.ReadLine().ToUpper()[0] == 'N')
                {
                    //Skriv ut fraser och sudda med finess
                    foreach (char c in fras1)
                    {
                        Console.Write(c);
                        Thread.Sleep(20);
                    }
                    Thread.Sleep(1000);
                    foreach (char c in fras1)
                    {
                        Console.Write("\b \b");
                        Thread.Sleep(10);
                    }
                    foreach (char c in fras2)
                    {
                        Console.Write(c);
                        Thread.Sleep(20);
                    }
                    Thread.Sleep(1000);
                    foreach (char c in fras2)
                    {
                        Console.Write("\b \b");
                        Thread.Sleep(10);
                    }
                }
                else
                {
                    Console.WriteLine("Parkingspot Quantity: ");
                    int amount;
                    //Kolla om användarens input är ok
                    if (int.TryParse(Console.ReadLine(), out amount))
                    {
                        //Skapa objektet GarageHandler och skicka med antal parkeringar
                        gh = new GarageHandler(amount);
                        spotAmount = amount;
                        //Sätter bredden på fönstet så att sökresultaten alltid får plats på sidan av
                        Console.SetWindowSize(42 + (amount * 4), 40);
                        break;
                    }
                    else
                    {
                        //Skriv ut fras 3 med finess
                        foreach (char c in fras3)
                        {
                            Console.Write(c);
                            Thread.Sleep(20);
                        }
                        Thread.Sleep(1000);
                        foreach (char c in fras3)
                        {
                            Console.Write("\b \b");
                            Thread.Sleep(10);
                        }
                        
                        continue;
                    }
                }
            }
            
            

        }

        /// <summary>
        /// Lägger till meny alternativ med respektive metod
        /// </summary>
        static void AddToDictionary()
        {
            MainMenyDictionary.Add("Show garage", ShowGarage);
            MainMenyDictionary.Add("Exit application", gh.Exit);
            MainMenyDictionary.Add("Save", Save);
            MainMenyDictionary.Add("Load", Load);
        }

        /// <summary>
        /// Kallar Save i GarageHandler och skriver ut om det gick att spara eller inte
        /// </summary>
        /// <param name="kaos"></param>
        static void Save(string kaos)
        {
            if (gh.Save("save"))
                Msg = "Saving success!";
            else
                Msg = "Saving failure!";
        }

        /// <summary>
        /// Kallar Load i GarageHandler och skriver ut om det gick att ladda eller inte
        /// </summary>
        /// <param name="kaos"></param>
        static void Load(string kaos)
        {
            if (gh.Load("load"))
                Msg = "Loading success!";
            else
                Msg = "Loading failure!";
        }

        /// <summary>
        /// Skriver ut en tom rad som är lika bred som fönstret
        /// </summary>
        static void ClearLine()
        {
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }

        /// <summary>
        /// Hanterar navigation i garagevyn. ShowGarage kallar vidare på DrawLayout, VehicleInfo, ShowSearchResult och Search
        /// </summary>
        /// <param name="name"></param>
        static void ShowGarage(string name)
        {
            Console.Clear();
            //pos: Vilken parkering som är fokuserad
            int pos = 1;
            //Antal parkeringsplatser
            int l = gh.GetParkingspots().Length;
            while(true)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(name);
                Console.WriteLine("Parked vehicles:");
                ClearLine();
                //Anpassar indraget efter hur många parkeringar som existerar
                int left = Console.CursorLeft + l*4;
                int top = Console.CursorTop;
                if(left > Console.WindowWidth/2)
                    left = 1;
                ClearLine(); 
                //Skriver ut sökresultaten
                ShowSearchResult(gh.searchQuery);
                //Rita ut Layouten på garaget
                DrawLayout(pos);
                //Skriv ut info om fokuserad parkering
                VehicleInfo(pos);

                //Navigation
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.DownArrow) // Navigera upp/ned
                {
                    if (pos >= 1 && pos <= l/2)
                        pos += l/2;
                    else if (pos >= l/2 && pos <= l)
                        pos -= l/2;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow) //Navigera vänster
                {
                    if(pos > 1 && pos <= l/2 || pos > (l/2)+1 && pos <= l)
                        pos--;
                    else if(pos == 1)
                        pos = l/2;
                    else if(pos == (l/2)+1)
                        pos = l;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow) //Navigera höger
                {
                    if (pos >= 1 && pos < l/2 || pos > l/2 && pos < l)
                        pos++;
                    else if (pos == l/2)
                        pos = 1;
                    else if (pos == l)
                        pos = (l/2)+1;
                }
                else if (keyInfo.Key == ConsoleKey.Escape) //Backa
                    return;
                else if (keyInfo.Key == ConsoleKey.Enter) //Interagera
                {
                    //Kör användaren ett fordon?
                    if(gh.SelectedVehicle != null) 
                    {
                        //Är vald parkering tom?
                        if (gh.GetVehicle(pos) == null) 
                        {
                            Console.WriteLine("Park your vehicle in spot #{0}? (Y/N)", pos);
                            //Kolla om användaren vill parkera
                            if(Console.ReadLine()[0].ToString().ToUpper() == "Y") 
                            {
                                //Parkera fordonet
                                gh.Park(pos, gh.SelectedVehicle);
                                //Nollställ användarens fordon
                                gh.SelectedVehicle = null;
                            }
                            //Börja om loopen
                            continue;
                        }
                        else
                            Console.WriteLine("You need to park your current vehicle before leaving with another!");
                    }
                    //Kör användaren ej?
                    else if (gh.SelectedVehicle == null)
                    {
                        //Innehåller fokuserad parkering ett fordon?
                        if (gh.GetVehicle(pos) != null)
                        {
                            Console.WriteLine("Do you want to take the {0} for a drive? (Y/N)", gh.GetVehicle(pos).Name);
                            //Vill användaren köra fordonet?
                            if (Console.ReadLine()[0].ToString().ToUpper() == "Y")
                                gh.SelectedVehicle = gh.Unpark(pos);   //Backa ut från parkeringen
                            continue;   //Börja om loopen
                        }
                        else
                            Console.WriteLine("You need a vehicle before you can park!");
                    }
                    Console.ReadKey();
                }
                else if (keyInfo.Key == ConsoleKey.Backspace || char.IsLetter(keyInfo.KeyChar)) //Updatera sökfras
                    gh.Search(keyInfo);
            }
        }

        /// <summary>
        /// Tar en sökfras och söker efter parkerade fordon. Fordonen läggstill i en resultat lista som sedan skrivs ut.
        /// </summary>
        /// <param name="sq"></param>
        static void ShowSearchResult(string sq)
        {
            int OriginalLeft = Console.CursorLeft;  //Spara markörens
            int OriginalTop = Console.CursorTop;    //position
            int l = gh.GetParkingspots().Length;    //Antal parkeringar
            int left = Console.WindowWidth - 40;    //Variabel för att sedan flytta markören 40 karaktärer från högra kanten
            int top = 5;                            //Variabel för att flytta markören till rad 5
            //Rensa resultatlistan inför ny sökning
            searchResult.Clear();
            int vehicleAmount = 0;
            //Kollar om sökfrasen inte är tom
            if(sq.Length > 0)
            {
               
                if(sq == "Car")
                {
                    //Sök och lägg till fordon som vars namn börjar med sökfrasen
                    IEnumerable<Vehicle> vQC = gh.GetParkedVehicles().Where(q => q.type == "Car");
                    foreach (Vehicle v in vQC)
                    {
                        vehicleAmount += 1;
                        if (!searchResult.Contains(v))
                            searchResult.Add(v);
                    }
                }
                else if(sq == "Bike")
                {
                    //Sök och lägg till fordon som vars namn börjar med sökfrasen
                    IEnumerable<Vehicle> vQB = gh.GetParkedVehicles().Where(q => q.type == "Bike");
                    foreach (Vehicle v in vQB)
                    {
                        vehicleAmount += 1;
                        if (!searchResult.Contains(v))
                            searchResult.Add(v);
                    }
                }
                else if (sq == "Quad")
                {
                    //Sök och lägg till fordon som vars namn börjar med sökfrasen
                    IEnumerable<Vehicle> vQQ = gh.GetParkedVehicles().Where(q => q.type == "Quad");
                    foreach (Vehicle v in vQQ)
                    {
                        vehicleAmount += 1;
                        if (!searchResult.Contains(v))
                            searchResult.Add(v);
                    }
                }
                else if (sq == "Lawnmower")
                {
                    //Sök och lägg till fordon som vars namn börjar med sökfrasen
                    IEnumerable<Vehicle> vQL = gh.GetParkedVehicles().Where(q => q.type == "Lawnmower");
                    foreach (Vehicle v in vQL)
                    {
                        vehicleAmount += 1;
                        if (!searchResult.Contains(v))
                            searchResult.Add(v);
                    }
                }
                else
                {

                    //Sök och lägg till fordon som vars namn börjar med sökfrasen
                    IEnumerable<Vehicle> vQ = gh.GetParkedVehicles().Where(q => q.Name.StartsWith(sq)).Select(q => q).OrderBy(q => q.type);
                    foreach (Vehicle v in vQ)
                    {
                        if (!searchResult.Contains(v))
                            searchResult.Add(v);
                    }
                    //Sök och lägg till fordon som vars namn innehåller sökfrasen
                    IEnumerable<Vehicle> vQ2 = gh.GetParkedVehicles().Where(q => q.Name.ToLower().Contains(sq.ToLower())).Select(q => q).OrderBy(q => q.type);
                    foreach (Vehicle v in vQ2)
                    {
                        if (!searchResult.Contains(v))
                            searchResult.Add(v);
                    }
                }


            }
            //Skriv ut tomma rader för att ta bort tidigare text
            for (int q = 0; q < 30; q++)
            {
                Console.SetCursorPosition(left, top+q);
                Console.WriteLine("                                         ");
            }

            //Förhindrar att antal sökresultat blir mer än fyra
            int limit = searchResult.Count();
            if (limit > 4)
                limit = 4;

            //Flytta markören och skriv tomma rader
            Console.SetCursorPosition(left, top - 2);
            Console.WriteLine("                                             ");
            Console.SetCursorPosition(left, top - 1);
            Console.WriteLine("                                             ");
            Console.SetCursorPosition(left, top - 2);

            //Om sökfrasen innehåller karaktärer skrivs Search och sökfrasen ut
            if (sq.Length > 0 && vehicleAmount == 0)
                Console.WriteLine("Search: " + sq);
            else if(sq.Length > 0 && vehicleAmount != 0)
                Console.WriteLine("Search: " + sq + " Amount: " + vehicleAmount);

            //Loopa och skriv ut alla eller 4 resultat
            for (int i = 0; i < limit; i+=1  )
            {
                //Är resultatet en parkering?
                if(searchResult[i] != null)
                {
                    //Spara fordonet i lokal variabel
                    Vehicle v = searchResult[i];
                    //Spara fordonets parking som sträng för att kunna byta ut talet till en text
                    var spot = v.spot.ToString();
                    //Om fordonets parkering är 0 innebär det att vi kör fordonet. Dvs fordonet är "In use"
                    if (v.spot == 0)
                        spot = "In use";
                    Console.SetCursorPosition(left, top-1);

                    //Skriv ut rad mellan resultaten
                    for (int z = 0; z < 30; z++)
                    {
                        Console.Write("-");
                    }

                    //Skriv ut resultat
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

                    //Om vi har utrymme för fler resultat flyttar vi markören längre ned
                    if(top < Console.BufferHeight-8)
                        top += 8;
                }
            }
            //Flytta markören till sin orginalposition
            Console.SetCursorPosition(OriginalLeft, OriginalTop);
        }

        /// <summary>
        /// Rita ut layouten på garaget
        /// </summary>
        /// <param name="active"></param>
        static void DrawLayout(int active)
        {
            //Flytta markören längst till vänster
            Console.SetCursorPosition(0, Console.CursorTop);

            //Skriv ut en outline på ovansidan av garaget
            drawTopMidBottom();

            //Antal parkeringar
            int len = gh.GetParkingspots().Length;

            //Skriv ut parkeringar
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
                    Console.Write("#"); //Vänster sida av garaget
                    for (int i = 1; i <= (len / 2); i += 1)
                    {
                        string str = "0";
                        if (x == 2)
                            str = q.ToString();
                        else if(x == xMin || x == xMax)
                            str = " ";
                        ParkLight(str, q, active);
                        q += 1;
                        Console.Write("#"); //Markering efter parkeringar
                    }
                    Console.WriteLine();
                }
                if(z == 1)
                {
                    //Skriv ut tomrum mellan raderna av parkeringarw
                    for (int i = 0; i < 2; i += 1)
                    {
                        Console.Write("#");     //Vänster sida
                        drawTopMidBottom(true); //Skriver ut tomrum i mitten
                        Console.Write("#\n");   //Höger sida
                    }
                }
            }
            //Skriver ut en rad på botten vilket skapar en ram runt garaget
            drawTopMidBottom();
        }

        /// <summary>
        /// Ritar ut ram delar. Skickar man med värdet true skrivs en "mitten rad" ut som bara har ett tecken på vardera sida och resten tomrum
        /// </summary>
        /// <param name="mid"></param>
        static void drawTopMidBottom(bool mid = false)
        {
            //Antal parkeringar
            int len = gh.GetParkingspots().Length;

            //Mitten rad?
            if(mid)
            {
                //Skriv ut tomrum
                for (int t = 0; t < len * 4 - 1; t += 1)
                {
                    Console.Write(" ");
                }
            }
            //Inte mitten?
            else
            {
                //Skriv ut kanter
                for (int f = 0; f < len * 4 + 1; f += 1)
                    Console.Write("#");
                Console.WriteLine();
            }
            
        }

        /// <summary>
        /// Skriver ut information om fokuserad parkering
        /// </summary>
        /// <param name="index"></param>
        static void VehicleInfo(int index)
        {
            Console.WriteLine("\n");
            int x = Console.CursorLeft; //Spara markörens
            int y = Console.CursorTop;  //position

            //Skriver ut 16 tommar rader för att rensa "arean"
            for (int i = 0; i < 16; i+=1)
                Console.WriteLine(new string(' ', spotAmount * 4));

            //Återställ markörens position
            Console.SetCursorPosition(x, y);

            //Är det ett fordon i parkeringen?
            if(gh.GetVehicle(index) != null)
            {
                //Spara fordonet i lokal variabel
                Vehicle v = gh.GetVehicle(index);
                //Skriv ut information
                Console.WriteLine(" {0, -20}{1}\n {2, -20}{3}\n {4, -20}{5}\n {6, -20}{7}\n {8, -20}{9}\n {10, -20}{11}", "Parkingspot:", index, "Vehicle:", v.type, "Name of Vehicle:", v.Name, "Color:", v.Color, "Value:", v.Value, "Nr of wheels:", v.NrOfWheels);
            }
            else
            {
                //Skriv ut mall för information
                Console.WriteLine(" {0, -20}{1}\n {2, -20}{3}\n {4, -20}{5}\n {6, -20}{7}\n {8, -20}{9}\n {10, -20}{11}", "Parkingspot:", index, "Vehicle:", "N/A", "Name of Vehicle:", "N/A", "Color:", "N/A", "Value:", "N/A", "Nr of wheels:", "N/A");
            }
        }

        /// <summary>
        /// Sköter formateringen av layouten på garaget. Skriver också ut visuella fordon på respektive plats
        /// </summary>
        /// <param name="alternativ"></param>
        /// <param name="index"></param>
        /// <param name="active"></param>
        static void ParkLight(string alternativ, int index, int active)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;

            //Är fokuserad parkering samma som denna parkering och vilken rad av parkeringen gäller det?
            if (active == index && alternativ == "0")
            {
                //Markering att den parkeringen är fokuserad
                Console.Write("|     |");
            }
            //Är fokuserad parkering inte samma som denna parkering?
            else if (active != index && alternativ == "0")
            {
                //Skriv ut tomrum
                Console.Write("{0,7}", " ");
            }
            //Är fokuserad parkering samma som denna parkering och är alternativ ej ett mellanslag?
            else if (active == index && alternativ != " ")
            {
                //Skriv ut fokuserad ram med parkeringens siffra
                Console.Write("|{0,-5}|", alternativ);
            }
            //Är fkuserad parkering samma som denna paerkering och är alternativ ett mellanslag?
            else if (active == index && alternativ == " ")
            {
                //Skriv ut toppen/botten på fokusramen
                Console.Write(" {0,-5} ", "-----");
            }
            else
            {
                //Stämmer ingenting in skrivs en tom rad ut med värdet som skickats med alternativ
                Console.Write(" {0, -5} ", alternativ);
            }
            Console.ResetColor();

            //Är det ett fordon i parkeringen?
            if (gh.GetVehicle(index) != null)
            {
                //Fordonets färg
                ConsoleColor Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), gh.GetVehicle(index).Color, true);

                int left = Console.CursorLeft;  //Spara markörens
                int top = Console.CursorTop;    //position
                //Typ av fordon
                string type = gh.GetVehicle(index).type;
                Console.BackgroundColor = Color;

                //Är fordonet en bil?
                if (type == "Car")
                {
                    MoveCursorToSpot(index);        //Skriv ut en visualisering av en bil
                    Console.Write("   ");           //  DVS  ###
                    MoveCursorToSpot(index, 1);     //       ###
                    Console.Write("   ");           //       ###
                    MoveCursorToSpot(index, 2);
                    Console.Write("   ");
                }
                //Är fordonet en cykel/hoj?
                else if(type == "Bike")
                {
                    MoveCursorToSpot(index, 1, 1);  //Skriv ut en visualisering av en cykel/hoj
                    Console.Write(" ");             //  DVS  #
                    MoveCursorToSpot(index, 2, 1);  //       #
                    Console.Write(" ");             //       #
                }
                //Är fordonet en fyrhjuling?
                else if(type == "Quad")
                {
                    MoveCursorToSpot(index);        
                    Console.Write(" ");                         
                    MoveCursorToSpot(index, 0, 2);  //Skriv ut en visualisering av en fyrhjuling
                    Console.Write(" ");             //  DVS  # #
                    MoveCursorToSpot(index, 1, 1);  //        #
                    Console.Write(" ");             //       # #
                    MoveCursorToSpot(index, 2, 0); 
                    Console.Write(" ");
                    MoveCursorToSpot(index, 2, 2);
                    Console.Write(" ");
                }
                //Är fordonet en gräsklippare?
                else if(type == "Lawnmower")
                {
                    MoveCursorToSpot(index, 0, 1);  //Skriv ut en visulisering av en gräsklippare
                    Console.Write("  ");            //  DVS 
                    MoveCursorToSpot(index, 1, 1);  //       ##
                    Console.Write("  ");            //       ##
                }
                Console.ResetColor();
                //Återställ markörens position
                Console.SetCursorPosition(left, top);
            }
        }

        /// <summary>
        /// Flyttar markören till en angiven parkering, left och top skickas med för att kunna manipulera marörens position inom en parkering på ett smidit sätt
        /// </summary>
        /// <param name="index"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        static void MoveCursorToSpot(int index, int top = 0, int left = 0)
        {
            //Antal parkeringar
            int totSpot = gh.GetParkingspots().Length;
            //Max längd för en rad beroende på antalet parkeringar
            int rowMax = totSpot / 2;

            //Är vald parkering under pivoten?
            if(index <= rowMax)
            {
                top += 6; //Flytta markören till radens höjd
                left += 3 + (8 * (index-1)); //Flytta markören till rätt parkering
            }
            else
            {
                top += 13;//Flytta markören till radens höjd
                left += 3 + (8 * ((index - 1) - rowMax));//Flytta markören till rätt parkering
            }
            //Återställ markörensposition
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Huvudmenyn, tar in namn och en dictionary som håller alla menyval
        /// </summary>
        /// <param name="name"></param>
        /// <param name="meny"></param>
        static void Menu(string name, Dictionary<string, Action<string>> meny)
        {
            name = name.ToUpper();
            //Om index är mer än antal alternativ återställs index till 0
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
                    //Lägg till indrag för finess
                    Console.CursorLeft = Indrag;
                    //Är detta alternativ samma som det aktiva alternativet
                    if (index == i)
                    {
                        //Ge det aktiva alternativet en annan bakgrundsfärg
                        HighLight(alternativ.Key);
                        active = alternativ.Key;
                    }
                    else
                        Console.WriteLine(" "+alternativ.Key);
                    i += 1;
                }

                //Meddelande från ladda/spara
                Console.WriteLine("\n" + Msg + "\n");

                //Skriv ut kontrollerna
                Console.WriteLine("Arrowkeys: " + (char)0x18 + (char)0x19 + (char)0x1A + (char)0x1B);
                Console.WriteLine("Enter: Interact" + (char)13);
                Console.WriteLine("Escape: Back");

                //Navigation i huvudmenyn
                if (Navigation(active, name, meny)) { return; }
            }
        }

        /// <summary>
        /// Sköter navigationen i huvudmenyn, tar in ett namn, en dictionary med alla alternativ och det aktiva alternativet
        /// </summary>
        /// <param name="active"></param>
        /// <param name="namn"></param>
        /// <param name="meny"></param>
        /// <returns></returns>
        static bool Navigation(string active, string namn, Dictionary<string, Action<string>> meny)
        {
            //Navigering
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
                //Kör alternativets associerade metod
                meny[active].Invoke(active);
            }
            return false;
        }

        /// <summary>
        /// Tar in en sträng och skriver ut den med vit bakgrund
        /// </summary>
        /// <param name="alternativ"></param>
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
