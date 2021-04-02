using System;
using System.Text.Json;
using System.IO;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader moviesFile = new StreamReader("Movies.json");
            var movies = moviesFile.ReadToEnd();

            JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;


            foreach (JsonElement movie in doc.RootElement.EnumerateArray())
            {
                Console.WriteLine(movie.GetProperty("name"));
                Console.WriteLine(movie.GetProperty("duration"));
                foreach (JsonElement starring in movie.GetProperty("starring").EnumerateArray())
                {
                    Console.WriteLine(starring);
                }
                Console.WriteLine(movie.GetProperty("rating"));
                Console.WriteLine("");
            }
        }
    }
}