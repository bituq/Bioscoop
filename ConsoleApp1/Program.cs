using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = File.ReadAllText("Movies.json");

            JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;


            string[] movieNames = new string[root.GetArrayLength()];
            for (int i = 0; i < movieNames.Length; i++)
            {
                movieNames[i] = root[i].GetProperty("name").ToString();
                Console.WriteLine($"{i + 1} : {movieNames[i]}");
            }


            Console.Write("\nMaak uw Keuze: ");
            int movieNumber = Int32.Parse(Console.ReadLine());
            if (!root[movieNumber].Equals(null))
            {
                Console.WriteLine(
                    $"Naam: {root[movieNumber].GetProperty("name")}\n" +
                    $"Rating: {root[movieNumber].GetProperty("rating")}\n"
                    );
            }
            else
                Console.WriteLine($"Film {movieNumber} bestaat niet!");

        }
    }

}