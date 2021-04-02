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
            using (StreamReader r= new StreamReader(filePath))
            {
                String s = "";
                while ((s = r.ReadLine()) != null)
                {
                    if (s.Contains("voorNaam")) {
                        Console.WriteLine(s);
                    }
                }
            }
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Console.WriteLine(unixTimestamp);
        }
    }
}