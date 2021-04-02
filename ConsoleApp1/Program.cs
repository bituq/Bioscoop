using System;
using System.Text.Json;
using System.IO;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = File.ReadAllText("Movies.json");
            var fs = File.CreateText("Movies.json");

            JsonDocument doc = JsonDocument.Parse(movies);

            foreach (JsonElement movie in doc.RootElement.EnumerateArray())
            {
                if (movie.GetProperty("description").ToString() == "This is a test")
                {
                    Console.WriteLine(movie.GetProperty("name"));
                    Console.WriteLine(movie.GetProperty("duration"));
                    foreach (JsonElement starring in movie.GetProperty("starring").EnumerateArray())
                    {
                        Console.WriteLine(starring);
                    }
                    Console.WriteLine(movie.GetProperty("rating"));
                    Console.WriteLine(movie.GetProperty("description"));
                    Console.WriteLine("");
                }
            }
        }
    }
}