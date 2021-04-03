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
        }

        static void reserveringDoorNaam(string heleNaam) { // reserveringdoornaam mag eigenlijk alleen in het adminscherm staan.
            string filePath = "C:/Users/Brent/.vscode/Bioscoop/Bioscoop/ConsoleApp1/bin/Debug/netcoreapp3.1/Reserveringen.json"; // ik laat hem hier voor nu
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
            string filePath = "Reserveringen.json";
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
    
        static void reserveringMaken(string VoorNaam, string AchterNaam, string film) {
            string filePath = "C:/Users/Brent/.vscode/Bioscoop/Bioscoop/ConsoleApp1/bin/Debug/netcoreapp3.1/Reserveringen.json";
            StreamReader reserveringFile = new StreamReader(filePath);
            var reserveringen = reserveringFile.ReadToEnd();

            JsonDocument doc = JsonDocument.Parse(reserveringen);
            JsonElement root = doc.RootElement;

            Random rd = new Random();
            string CreateString(int Length) {
                const string allowedChars = "0123456789";
                char[] chars = new char[Length];
                for (int i = 0; i < Length; i++) {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
                return new string(chars);
            }

            string randomCode = CreateString(7);
            
            foreach (JsonElement reservering in root.EnumerateArray()) {
                if ((reservering.GetProperty("reserveringNummer").ToString()) == randomCode) {
                    reserveringMaken(VoorNaam, AchterNaam, film);
                    return;
                }
                else {
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    string unixTime = unixTimestamp.ToString();
                    string datum = "11 april 2021 om 16:00";
                    string[] lijstReserveringen = reserveringen.Split('}');
                    int len = lijstReserveringen.Length;
                    for (int i = 0; i < len-1; i++) {
                        if (i == (len-2)) {
                            lijstReserveringen[i] = lijstReserveringen[i] + "},\n";
                        }
                        else {
                            lijstReserveringen[i] = lijstReserveringen[i] + "}";
                        }
                    }

                    string toevoegen = "\t{\n" + $"\t\t\"voorNaam\" : \"{VoorNaam}\",\n" + $"\t\t\"achterNaam\" : \"{AchterNaam}\",\n" + $"\t\t\"reserveringNummer\" : \"{randomCode}\",\n" + $"\t\t\"zaal\" : \"{1}\",\n" + $"\t\t\"film\" : \"{film}\",\n" + $"\t\t\"datumVanReservatie\" : \"{unixTime}\",\n" + $"\t\t\"stoel\" : \"{0}\",\n" + $"\t\t\"datum\" : \"{datum}\"\n" + "\t}";
                    string newJson = "";
                    for (int j = 0; j < len-1; j++) {
                        newJson = newJson + lijstReserveringen[j];
                    }
                    newJson = newJson + toevoegen + lijstReserveringen[len-1];
                    reserveringFile.Close();
                    File.WriteAllText(filePath, newJson);
                }
            }
        }
    }
}