﻿using System;
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
        public List<string> Starring { get; set; } = new List<string>();
        public List<string> Genres { get; set; } = new List<string>();

        public Movie(string name, string duration, string releaseDate, string descriptionText, string rating, string language, string company, JsonElement genres, JsonElement starring)
        {
            Name = name;

            foreach (JsonElement genre in genres.EnumerateArray())
                Genres.Add(genre.ToString());
            foreach (JsonElement star in starring.EnumerateArray())
                Starring.Add(star.ToString());

            var title = new TextBuilder(Window, 3, 3)
                .Color(ConsoleColor.Red)
                .Result(name);

            var description = new TextBuilder(Window, 3, 4)
                .Color(ConsoleColor.DarkGray)
                .Result(descriptionText);

            var information = new TextListBuilder(Window, 3, 12)
                .Color(ConsoleColor.White)
                .Result(false, duration, releaseDate, rating, language, company, "Genres: ");

            var genresInformation = new TextListBuilder(Window, 3 + information.Items[5].Text.Length, 17)
                .Color(ConsoleColor.White)
                .Result(false, Genres.ToArray());

            var information2 = new TextBuilder(Window, 3, 17 + Genres.Count)
                .Color(ConsoleColor.White)
                .Result("Starring: ");

            var starringInformation = new TextListBuilder(Window, 3 + information2.Text.Length, information2.Position.Y)
                .Color(ConsoleColor.White)
                .Result(false, Starring.ToArray());

            var _ = new TextListBuilder(Window, 3, 1)
                .Color(ConsoleColor.White)
                .Selectable(new Color(ConsoleColor.Black, ConsoleColor.White), false, "Go back")
                .LinkWindows(Program.listOfFilms)
                .Result();
        }
    }

    partial class Program
    {
        public static Window listOfFilms = new Window(true);
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
                    $"Release Date: {root[i].GetProperty("releaseDate")}",
                    $"   {root[i].GetProperty("description")}",
                    $"Rating: {root[i].GetProperty("rating")}",
                    $"Language: {root[i].GetProperty("language")}",
                    $"Company: {root[i].GetProperty("company") }",
                    root[i].GetProperty("genres"),
                    root[i].GetProperty("starring")
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