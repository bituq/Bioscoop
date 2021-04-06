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
                Console.WriteLine("");
                Console.WriteLine($"   Duration: {u1.GetProperty("duration")} minutes");
                Console.WriteLine("");
                Console.WriteLine($"   Language: {u1.GetProperty("language")}");
                Console.WriteLine("");
                Console.WriteLine($"   Company: {u1.GetProperty("company")}");
                Console.WriteLine("");
                Console.WriteLine("   Starring:");
                foreach (JsonElement starring in u1.GetProperty("starring").EnumerateArray())
                {
                    Console.WriteLine($"   {starring}");
                }
                Console.WriteLine("");
                Console.WriteLine("   Genre(s):");
                foreach (JsonElement starring in u1.GetProperty("genres").EnumerateArray())
                {
                    Console.WriteLine($"   {starring}");
                }
                Console.WriteLine("");
                Console.WriteLine($"   Release Date: {u1.GetProperty("releaseDate")}");
                Console.WriteLine("");
                Console.WriteLine($"   Rating: {u1.GetProperty("rating")}");
            }
            else if (movieList.Contains("2") == true)
            {
                Console.WriteLine(u2.GetProperty("name"));
                Console.WriteLine("");
                Console.WriteLine($"   Duration: {u2.GetProperty("duration")} minutes");
                Console.WriteLine("");
                Console.WriteLine($"   Language: {u2.GetProperty("language")}");
                Console.WriteLine("");
                Console.WriteLine($"   Company: {u2.GetProperty("company")}");
                Console.WriteLine("");
                Console.WriteLine("   Starring:");
                foreach (JsonElement starring in u2.GetProperty("starring").EnumerateArray())
                {
                    Console.WriteLine($"   {starring}");
                }
                Console.WriteLine("");
                Console.WriteLine("   Genre(s):");
                foreach (JsonElement starring in u2.GetProperty("genres").EnumerateArray())
                {
                    Console.WriteLine($"   {starring}");
                }
                Console.WriteLine("");
                Console.WriteLine($"   Release Date: {u2.GetProperty("releaseDate")}");
                Console.WriteLine("");
                Console.WriteLine($"   Rating: {u2.GetProperty("rating")}");
            }
            else if (movieList.Contains("3") == true)
            {
                Console.WriteLine(u3.GetProperty("name"));
                Console.WriteLine("");
                Console.WriteLine($"   Duration: {u3.GetProperty("duration")} minutes");
                Console.WriteLine("");
                Console.WriteLine($"   Language: {u3.GetProperty("language")}");
                Console.WriteLine("");
                Console.WriteLine($"   Company: {u3.GetProperty("company")}");
                Console.WriteLine("");
                Console.WriteLine("   Starring:");
                foreach (JsonElement starring in u3.GetProperty("starring").EnumerateArray())
                {
                    Console.WriteLine($"   {starring}");
                }
                Console.WriteLine("");
                Console.WriteLine("   Genre(s):");
                foreach (JsonElement starring in u3.GetProperty("genres").EnumerateArray())
                {
                    Console.WriteLine($"   {starring}");
                }
                Console.WriteLine("");
                Console.WriteLine($"   Release Date: {u3.GetProperty("releaseDate")}");
                Console.WriteLine("");
                Console.WriteLine($"   Rating: {u3.GetProperty("rating")}");
            }
        }
    }

}