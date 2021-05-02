using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    public class Movie
    {
        public Window Window = new Window();
        public string Name { get; set; }

        public Movie(string name, string duration, string releaseDate)
        {
            Name = name;
            var title = new TextBuilder(Window, 3, 3)
                .Color(ConsoleColor.Red)
                .Result($"This is {name}");

            var description = new TextBuilder(Window, 3, 4)
                .Color(ConsoleColor.DarkGray)
                .Result($"This is a description for {name}.");

            var information = new TextListBuilder(Window, 3, 6)
                .Color(ConsoleColor.White)
                .Result(false, duration, releaseDate);

            var _ = new TextListBuilder(Window, 3, 16)
                .Color(ConsoleColor.White)
                .Selectable(new Color(ConsoleColor.Black, ConsoleColor.White), false, "Go back")
                .LinkWindows(Program.listOfFilms)
                .Result();
        }
    }

    partial class Program
    {
        public static Window listOfFilms = new Window(false);
        static void ListOfFilms()
        {
            var movies = File.ReadAllText("Movies.json");

            JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;

            Console.ForegroundColor
             = ConsoleColor.DarkMagenta;

            var movieObjects = new Movie[root.GetArrayLength()];
            var movieWindows = new Window[movieObjects.Length];
            string[] movieNames = new string[root.GetArrayLength()];
            for (int i = 0; i < movieNames.Length; i++)
            {
                movieObjects[i] = new Movie(
                    root[i].GetProperty("name").ToString(),
                    $"Duration: {root[i].GetProperty("duration")} minutes",
                    $"Release Date: {root[i].GetProperty("releaseDate")}"
                    );
                movieWindows[i] = movieObjects[i].Window;
                movieNames[i] = movieObjects[i].Name;
                //Console.WriteLine($"{i} : {movieNames[i]}");
            }
            var movieList = new TextListBuilder(listOfFilms, 4, 4)
                .Color(ConsoleColor.DarkMagenta)
                .Selectable(new Color(ConsoleColor.Cyan, ConsoleColor.DarkMagenta), true, movieNames)
                .LinkWindows(movieWindows)
                .Result();
            /*
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
            */
        }
    }

}