using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using CinemaUI;
using CinemaUI.Builder;
namespace CinemaApplication
{
    partial class Program
    {
        static void Main()
        {
            selectieSchermBrent();
            reserveringMaakScherm();
            reserveringZoekScherm();

            InputHandler.WaitForInput();
        }

        static void reserveringDoorNaam(string heleNaam) { // reserveringdoornaam mag eigenlijk alleen in het adminscherm staan.
            string filePath = @"bin\Debug\netcoreapp3.1\Reserveringen.json"; // ik laat hem hier voor nu
            StreamReader reserveringFile = new StreamReader(filePath);       // tot we beginnen aan het adminscherm.
            var reserveringen = reserveringFile.ReadToEnd();

            JsonDocument doc = JsonDocument.Parse(reserveringen);
            JsonElement root = doc.RootElement;
            string voornaam = "";
            string achternaam = "";
            foreach (JsonElement reservering in root.EnumerateArray()) {
                voornaam = reservering.GetProperty("voorNaam").ToString();
                achternaam = reservering.GetProperty("achterNaam").ToString();
                if ((voornaam + " " + achternaam) == heleNaam) {
                    Console.WriteLine(reservering.GetProperty("reserveringNummer"));
                }
            }
            reserveringFile.Close();
        }
        static void zoekDoorCode() {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Wat is uw reserveringscode?");
            Console.ForegroundColor = ConsoleColor.White;
            string code = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Green;
            string filePath = @"bin\Debug\netcoreapp3.1\Reserveringen.json";
            StreamReader reserveringFile = new StreamReader(filePath);
            var reserveringen = reserveringFile.ReadToEnd();

            JsonDocument doc = JsonDocument.Parse(reserveringen);
            JsonElement root = doc.RootElement;
            
            foreach (JsonElement reservering in root.EnumerateArray()) {
                if ((reservering.GetProperty("reserveringNummer").ToString()) == code) {
                    string voornaam = (reservering.GetProperty("voorNaam").ToString());
                    string achternaam = (reservering.GetProperty("achterNaam").ToString());
                    string zaal = (reservering.GetProperty("zaal").ToString());
                    string stoel = (reservering.GetProperty("stoelen").ToString());
                    string film = (reservering.GetProperty("film").ToString());
                    string datum = (reservering.GetProperty("datum").ToString());
                    Console.WriteLine($"Uw reservering staat onder de naam {voornaam + " " + achternaam}. U gaat naar de film {film} in zaal {zaal} op stoel {stoel}. De film speelt op {datum}. Tot dan!");
                }
            }
            reserveringFile.Close();
            Console.ForegroundColor = ConsoleColor.White;
        }
    
        static void reserveringMaken() { // functie voor het maken van reserveringen.
            string filePath = @"bin\Debug\netcoreapp3.1\Reserveringen.json"; // pakt de filepath.
            StreamReader reserveringFile = new StreamReader(filePath); // leest het json bestand.
            var reserveringen = reserveringFile.ReadToEnd(); // maakt een string van het jsonbestand.

            JsonDocument doc = JsonDocument.Parse(reserveringen); // omzetten in JsonDocument.
            JsonElement root = doc.RootElement; // root ofzo.

            Random rd = new Random(); // random, zorgt ervoor dat string creatie mogelijk is.
            string CreateString(int Length) { // maakt een string met gegeven lengte.
                const string allowedChars = "0123456789"; // karakters waar de string uit mag bestaan.
                char[] chars = new char[Length]; // lijst van karakters met gegeven lengte.
                for (int i = 0; i < Length; i++) { // loop die random karakters toevoegt aan de character array.
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)]; // voegt random karakter toe aan array.
                }
                return new string(chars); // zet de lijst om in een string en stuurt het terug naar de functioncall.
            }

            string randomCode = CreateString(7); // maakt een random string uit nummers met lengte van 7.
            
            foreach (JsonElement reservering in root.EnumerateArray()) { // gaat door alle reserveringen heen.
                if ((reservering.GetProperty("reserveringNummer").ToString()) == randomCode) { // kijkt of nieuwe code gelijk is aan een oudere.
                    reserveringMaken(); // stuurt de data terug als de reserveringscode hetzelfde is zodat er een nieuwe gemaakt kan worden.
                    return; // stopt de functie zodat die niet op wonderbaarlijke wijze doorgaat.
                }
            }
             // nieuwe reserveringscode is niet hetzelfde als een voorgaande, dus kan doorgaan.
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; // unixtijd.
            string unixTime = unixTimestamp.ToString(); // tijd in unix formaat op dit huidige moment, omgezet in een string.
            string datum = "11 april 2021 om 16:00"; // datum

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Wat is uw voornaam?: ");
            Console.ForegroundColor = ConsoleColor.White;
            string VoorNaam = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Wat is uw achternaam (inclusief tussenvoegsel?: ");
            Console.ForegroundColor = ConsoleColor.White;
            string AchterNaam = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Naar welke film wilt U heen gaan?: ");
            Console.ForegroundColor = ConsoleColor.White;
            string film = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Typ welke stoel U wilt reserveren (als U meerdere wilt, moet U ze onderschijden met spaties): "); // vragen welke stoelen.
            Console.ForegroundColor = ConsoleColor.White;
            string stringstoel = Console.ReadLine(); // leest welke stoelen iemand reserveerd.
            string[] stoelen = stringstoel.Split(' '); // maakt een lijst met de gereserveerde stoelen.
            int length2 = stoelen.Length; // pakt het aantal genoemde stoelen.
            string stoelen2 = "["; // maakt de base voor het begin van de array.
            for (int x = 0; x < length2; x++) { // loop die de gereserveerde stoelen in een lijst zet.
                if (x == length2-1) {
                    stoelen2 = stoelen2 + stoelen[x] + "]"; // einde van de loop.
                }
                else {
                    stoelen2 = stoelen2 + stoelen[x] + ", "; // voegt een komma toe aan het einde van een stoelreservering.
                }
            }

            string[] lijstReserveringen = reserveringen.Split('}'); // splitst de json string naar een list per reservering.
            int len = lijstReserveringen.Length; // pakt de lengte van de array.
            for (int i = 0; i < len-1; i++) { // loop om de curly brackets terug toe te voegen.
                if (i == (len-2)) { // pakt de laatste reservering (niet degene die nu word gemaakt)
                lijstReserveringen[i] = lijstReserveringen[i] + "},\n"; // voegt een curly bracket toe aan de laatste reservering samen met een komma.
                }
                else {
                    lijstReserveringen[i] = lijstReserveringen[i] + "}"; // geeft een curly bracket aan de reserveringen terug
                }
            }
            // string toevoegen gooit alle data die opgepakt is van de reservering in het reservering formaat van het json bestand.
            string toevoegen = "\t{\n" + $"\t\t\"voorNaam\" : \"{VoorNaam}\",\n" + $"\t\t\"achterNaam\" : \"{AchterNaam}\",\n" + $"\t\t\"reserveringNummer\" : \"{randomCode}\",\n" + $"\t\t\"zaal\" : \"{1}\",\n" + $"\t\t\"film\" : \"{film}\",\n" + $"\t\t\"datumVanReservatie\" : \"{unixTime}\",\n" + $"\t\t\"stoelen\" : {stoelen2},\n" + $"\t\t\"datum\" : \"{datum}\"\n" + "\t}";
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"U gaat naar de film {film}. U heeft {stoelen.Length} stoel(en) met nummer(s) {stoelen2} onder uw naam staan. Uw reserveringscode is {randomCode}. Sla de code goed op!");
            string newJson = ""; // definiert de string waar het bestand uit gaat bestaan.
            for (int j = 0; j < len-1; j++) {
                newJson = newJson + lijstReserveringen[j]; // voegt de oude reserveringen weer samen.
            }
            newJson = newJson + toevoegen + lijstReserveringen[len-1]; // voegt de nieuwe reservering aan de oude reserveringen toe.
            reserveringFile.Close(); // sluit het bestand zodat de data geschreven kan worden naar het bestand.
            File.WriteAllText(filePath, newJson); // herschrijft het bestand met niewe reserveringsinformatie.
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}