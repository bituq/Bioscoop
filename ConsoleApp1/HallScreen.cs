using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        static Window hallscreen = new Window(true);
        static void HallScreen()
        {
            var halls = File.ReadAllText("halls.json");

            JsonDocument doc = JsonDocument.Parse(halls);
            JsonElement root = doc.RootElement;
            

            Console.ForegroundColor
             = ConsoleColor.DarkMagenta;

            var showhall = new TextListBuilder(hallscreen, 3, 2)
                .Color(ConsoleColor.Red)
                .SetItems(root)
                .Selectable(ConsoleColor.Yellow, ConsoleColor.White)
                .Result();
          

            /*var movieObjects = new Movie[root.GetArrayLength()];
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
            var movieList = new TextListBuilder(hallscreen, 4, 4)
                .Color(ConsoleColor.DarkMagenta)
                .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                .LinkWindows(movieWindows)
                .Result();*/
            }
        }
}