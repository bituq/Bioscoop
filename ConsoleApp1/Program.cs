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
            var movie = new Movie();
            movie.name = "Hacksaw Ridge";
            movie.duration = 5000;
            movie.releaseDate = 1478127600;
            movie.rating = 5;
            movie.language = "Engels";
            movie.company = "Lionsgate";
            movie.description = "Nothing to see here...";

            JsonFile.AppendToFile(new List<Movie> { movie }, "Movies.json");
            Console.WriteLine($"Successfully added \"{movie.name}\"");
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();

            JsonFile.RemoveFromFile("name", "Hacksaw Ridge", "Movies.json");
            Console.WriteLine($"Successfully removed \"{movie.name}\"");

            /*Console.WriteLine("Voeg hier een film toe");
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
            JsonHandler.JsonFile.AppendToFile(new List<Movie> { movie }, "Movies.json");
            Console.WriteLine($"Successfully added \"{movie.name}\"");
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();*/
        }
    }
}