using System;
using System.IO;
using System.Collections.Generic;
using JsonHandler;

namespace CinemaApplication
{
    public class Program
    {
        public class Movie
        {
            public string name { get; set; }
            public int duration { get; set; }
            public int releaseDate { get; set; }
            public int rating { get; set; }
            public string[] genres { get; set; }
            public string language { get; set; }
            public string company { get; set; }
            public string[] starring { get; set; }
            public string description { get; set; }
        }
        public static void Main()
        {
            var movies = new List<Movie>();
            bool stop = false;
            while (!stop)
            {
                var movie = new Movie();
                Console.WriteLine("Voeg hier een film toe");
                Console.Write("Naam: ");
                movie.name = Console.ReadLine();
                Console.Write("Lengte (seconden): ");
                movie.duration = Int32.Parse(Console.ReadLine());
                Console.Write("Publicatiedatum (unix): ");
                movie.releaseDate = Int32.Parse(Console.ReadLine());
                Console.Write("Rating: ");
                movie.rating = Int32.Parse(Console.ReadLine());
                Console.Write("Taal: ");
                movie.language = Console.ReadLine();
                Console.Write("Bedrijf: ");
                movie.company = Console.ReadLine();
                Console.Write("Beschrijving: ");
                movie.description = Console.ReadLine();
                Console.WriteLine("Toegevoegd! Stoppen? Druk op 'RightArrow'");
                movies.Add(movie);
                stop = Console.ReadKey().Key == ConsoleKey.RightArrow;
            }


            JsonFile.AppendToFile(movies, "Movies.json");
            Console.WriteLine($"Successfully added {movies.Count} movies.");
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
        }
    }
}