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

            JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;

            var u1 = root[0];
            var u2 = root[1];
            var u3 = root[2];

            Console.WriteLine("1 : Harry Potter and the Deathly Hallows: Part 1\n2 : Lord of the Rings: Fellowship of the Ring\n3 : 1917");
            Console.Write("Maak uw Keuze: ");
            string movieList = Console.ReadLine();
            if (movieList.Contains("1") == true)
            {
                Console.WriteLine(u1.GetProperty("name"));
                Console.WriteLine(u1.GetProperty("duration"));
                Console.WriteLine(u1.GetProperty("language"));
                Console.WriteLine(u1.GetProperty("company"));
                Console.WriteLine("starring:");
                foreach (JsonElement starring in u1.GetProperty("starring").EnumerateArray())
                {
                    Console.WriteLine(starring);
                }
                Console.WriteLine("genre(s):");
                foreach (JsonElement starring in u1.GetProperty("genres").EnumerateArray())
                {
                    Console.WriteLine(starring);
                }
                Console.WriteLine(u1.GetProperty("releaseDate"));
                Console.WriteLine(u1.GetProperty("rating"));
            }
            else if (movieList.Contains("2") == true)
            {
                Console.WriteLine(u2.GetProperty("name"));
                Console.WriteLine(u2.GetProperty("duration"));
                Console.WriteLine(u2.GetProperty("language"));
                Console.WriteLine(u2.GetProperty("company"));
                Console.WriteLine("starring:");
                foreach (JsonElement starring in u2.GetProperty("starring").EnumerateArray())
                {
                    Console.WriteLine(starring);
                }
                Console.WriteLine("genre(s):");
                foreach (JsonElement starring in u2.GetProperty("genres").EnumerateArray())
                {
                    Console.WriteLine(starring);
                }
                Console.WriteLine(u2.GetProperty("releaseDate"));
                Console.WriteLine(u2.GetProperty("rating"));
            }
            else if (movieList.Contains("3") == true)
            {
                Console.WriteLine(u3.GetProperty("name"));
                Console.WriteLine(u3.GetProperty("duration"));
                Console.WriteLine(u3.GetProperty("language"));
                Console.WriteLine(u3.GetProperty("company"));
                Console.WriteLine("starring:");
                foreach (JsonElement starring in u3.GetProperty("starring").EnumerateArray())
                {
                    Console.WriteLine(starring);
                }
                Console.WriteLine("genre(s):");
                foreach (JsonElement starring in u3.GetProperty("genres").EnumerateArray())
                {
                    Console.WriteLine(starring);
                }
                Console.WriteLine(u3.GetProperty("releaseDate"));
                Console.WriteLine(u3.GetProperty("rating"));
            }
        }
    }

}