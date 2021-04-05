using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1 : reservering zoeken op reserveringscode\n2 : reservering aanmaken");
            Console.Write("Wat wilt U doen?: ");
            string antwoord = Console.ReadLine();
            if (antwoord.Contains("1") == true) {
                Console.Write("Wat is uw reserveringscode?: ");
                string reserveringscode = Console.ReadLine();
                zoekDoorCode(reserveringscode);
            }
            else if (antwoord.Contains("2") == true) {
                Console.Write("Wat is uw voornaam?: ");
                string voornaam = Console.ReadLine();
                Console.Write("Wat is uw achternaam? (inclusief tussenvoegsel): ");
                string achternaam = Console.ReadLine();
                Console.Write("Welke film wilt U zien?: ");
                string film = Console.ReadLine();
                reserveringMaken(voornaam, achternaam, film);
            }
            else if (antwoord.Contains("5") == true) {
                Console.Write("Wat is uw hele naam? (inclusief tussenvoegsel): ");
                string naam = Console.ReadLine();
                reserveringDoorNaam(naam);
            }
        }

        static void reserveringDoorNaam(string heleNaam) { // reserveringdoornaam mag eigenlijk alleen in het adminscherm staan.
            string filePath = @"bin\Debug\netcoreapp3.1\Reserveringen.json"; // ik laat hem hier voor nu
            StreamReader reserveringFile = new StreamReader(filePath);                        //tot we beginnen aan het adminscherm.
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
        static void zoekDoorCode(string code) {
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
                    string stoel = (reservering.GetProperty("stoel").ToString());
                    string film = (reservering.GetProperty("film").ToString());
                    string datum = (reservering.GetProperty("datum").ToString());
                    Console.WriteLine($"Uw reservering staat onder de naam {voornaam + " " + achternaam}. U gaat naar de film {film} in zaal {zaal} op stoel {stoel}. De film speelt op {datum}. Tot dan!");
                }
            }
            reserveringFile.Close();
        }
    
        static void reserveringMaken(string VoorNaam, string AchterNaam, string film) { // functie voor het maken van reserveringen.
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
                    reserveringMaken(VoorNaam, AchterNaam, film); // stuurt de data terug als de reserveringscode hetzelfde is zodat er een nieuwe gemaakt kan worden.
                    return; // stopt de functie zodat die niet op wonderbaarlijke wijze doorgaat.
                }
                else { // nieuwe reserveringscode is niet hetzelfde als een voorgaande, dus kan doorgaan.
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; // unixtijd.
                    string unixTime = unixTimestamp.ToString(); // tijd in unix formaat op dit huidige moment, omgezet in een string.
                    string datum = "11 april 2021 om 16:00"; // datum
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
                    string toevoegen = "\t{\n" + $"\t\t\"voorNaam\" : \"{VoorNaam}\",\n" + $"\t\t\"achterNaam\" : \"{AchterNaam}\",\n" + $"\t\t\"reserveringNummer\" : \"{randomCode}\",\n" + $"\t\t\"zaal\" : \"{1}\",\n" + $"\t\t\"film\" : \"{film}\",\n" + $"\t\t\"datumVanReservatie\" : \"{unixTime}\",\n" + $"\t\t\"stoel\" : \"{0}\",\n" + $"\t\t\"datum\" : \"{datum}\"\n" + "\t}";
                    string newJson = ""; // definiert de string waar het bestand uit gaat bestaan.
                    for (int j = 0; j < len-1; j++) {
                        newJson = newJson + lijstReserveringen[j]; // voegt de oude reserveringen weer samen.
                    }
                    newJson = newJson + toevoegen + lijstReserveringen[len-1]; // voegt de nieuwe reservering aan de oude reserveringen toe.
                    reserveringFile.Close(); // sluit het bestand zodat de data geschreven kan worden naar het bestand.
                    File.WriteAllText(filePath, newJson); // herschrijft het bestand met niewe reserveringsinformatie.
                }
            }
        }
    }
}