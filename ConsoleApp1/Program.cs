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


            foreach (JsonElement reserverin in doc.RootElement.EnumerateArray())
            {
                foreach (JsonElement reservering in doc.RootElement.EnumerateArray()) {
                    Console.WriteLine(reservering.GetProperty("reserveringNummer"));
                    Console.WriteLine(reservering.GetProperty("film"));
                    Console.WriteLine(reservering.GetProperty("voorNaam"));
                    Console.WriteLine("");
                }
            }
            // Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            // Console.WriteLine(unixTimestamp);
        }
    }
}