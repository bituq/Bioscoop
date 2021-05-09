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
        public class hall
        {
            public string name { get; set; }
            public int rows { get; set; }
            public int columns { get; set; }
        }
        static Window hallscreen = new Window(true);
        static void Halls()
        {
            var halls = File.ReadAllText("halls.json");

            JsonDocument doc = JsonDocument.Parse(halls);
            JsonElement root = doc.RootElement;



            Console.ForegroundColor
             = ConsoleColor.DarkMagenta;



            var movieObjects = new hall[root.GetArrayLength()];
            var movieWindows = new Window[movieObjects.Length];
            string[] movieNames = new string[root.GetArrayLength()];
            for (int i = 0; i < movieNames.Length; i++)
            {
                movieObjects[i] = new hall(
                    root[i].GetProperty("name").ToString(),
                    $"Duration: {root[i].GetProperty("rows")} minutes",
                    $"Release Date: {root[i].GetProperty("columns")}"
                    );

                movieWindows[i] = movieObjects[i].Window;
                movieNames[i] = movieObjects[i].Name;
                //Console.WriteLine($"{i} : {movieNames[i]}");
            }
            var showhall = new TextListBuilder(hallscreen, 3, 2)
                .Color(ConsoleColor.Red)
                .SetItems()
                .Selectable(ConsoleColor.Yellow, ConsoleColor.White)
                .Result();
        }
    }
}