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
    }
}