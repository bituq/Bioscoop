using System;
using System.IO;
using System.Text.Json;
namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:/Users/Brent/.vscode/Bioscoop/Bioscoop/ConsoleApp1/reserveringen.json";
            StreamReader reserveringFile = new StreamReader(filePath);
            var reserveringen = reserveringFile.ReadToEnd();

            JsonDocument doc = JsonDocument.Parse(reserveringen);
            JsonElement root = doc.RootElement;
            Console.WriteLine("1 : reservering zoeken op reserveringscode\n2 : reservering aanmaken");
            Console.Write("Wat wilt U doen?: ");
            if ((Console.ReadLine()).Contains("1") == true) {
                Console.Write("Wat is uw reserveringscode?: ");
                string reserveringscode = Console.ReadLine();
                zoekDoorCode(reserveringscode);
            }
            // Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            // Console.WriteLine(unixTimestamp);
        }

        static void reserveringDoorNaam(string heleNaam) { // reserveringdoornaam mag eigenlijk alleen in het adminscherm staan.
            string filePath = "C:/Users/Brent/.vscode/Bioscoop/Bioscoop/ConsoleApp1/reserveringen.json"; // ik laat hem hier voor nu
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
        }
        static void zoekDoorCode(string code) {
            string filePath = "C:/Users/Brent/.vscode/Bioscoop/Bioscoop/ConsoleApp1/reserveringen.json";
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
        }
    }
}