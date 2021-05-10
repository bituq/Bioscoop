using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
     public class hall
    {
        public Window Window = new Window(true);
        public string Name { get; set; }
        public string Rows { get; set; }
        public string Columns { get; set; }

        public hall(string name, string rows, string columns)
        {
            var Name = name;
            var Rows = rows;
            var Columns = columns;

            var title = new TextBuilder(Window, 3, 3)
                .Color(ConsoleColor.Red)
                .Text(name)
                .Result();

            var description = new TextBuilder(Window, 3, 4)
                .Color(ConsoleColor.DarkGray)
                .Text(rows)
                .Result();

            var information = new TextBuilder(Window, 3, 12)
                .Color(ConsoleColor.White)
                .Text(columns)
                .Result();

      
        }
    }
    partial class Program
    {
        static Window hallscreen = new Window();
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