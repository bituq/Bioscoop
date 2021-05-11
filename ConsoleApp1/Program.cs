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
    }
}