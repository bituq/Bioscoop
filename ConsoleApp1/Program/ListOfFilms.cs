using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    partial class Program
    {
        public static Window listOfFilms = new Window();
        static void ListOfFilms()
        {
            var movies = File.ReadAllText("Movies.json");

            JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;

            Console.ForegroundColor
             = ConsoleColor.DarkMagenta;
      
            string[] movieNames = new string[root.GetArrayLength()];
            for (int i = 0; i < movieNames.Length; i++)
            {
                movieNames[i] = root[i].GetProperty("name").ToString();
                Console.WriteLine($"{i} : {movieNames[i]}");
            }


            Console.Write("\nMake your choice: \n");
            int movieNumber = Int32.Parse(Console.ReadLine());
            if (!root[movieNumber].Equals(null))
            {
                Console.ForegroundColor
                 = ConsoleColor.Green;
                Console.WriteLine(
                    $"{root[movieNumber].GetProperty("name")}\n\n" +
                    $"   Duration: {root[movieNumber].GetProperty("duration")} minutes\n" +
                    $"   Release Date: {root[movieNumber].GetProperty("releaseDate")}\n" +
                    $"   Rating: {root[movieNumber].GetProperty("rating")}\n" +
                    $"   Language: {root[movieNumber].GetProperty("language")}\n" +
                    $"   Company: {root[movieNumber].GetProperty("company")}"
                    );

                Console.WriteLine("   Genre(s):");
                foreach (JsonElement genres in root[movieNumber].GetProperty("genres").EnumerateArray())
                {
                    Console.WriteLine($"\t{genres}");
                }

                Console.WriteLine("   Starring:");
                foreach (JsonElement starring in root[movieNumber].GetProperty("starring").EnumerateArray())
                {
                    Console.WriteLine($"\t{starring}");
                }

                Console.WriteLine("   Description:");
                Console.WriteLine($"   {root[movieNumber].GetProperty("description")}");
            }
            else
                Console.WriteLine($"Film {movieNumber} does not exist!");
           
            Console.ForegroundColor
             = ConsoleColor.White;
        }
    }

}