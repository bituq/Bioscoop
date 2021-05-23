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
            public Window Window = new Window();
            public string Name { get; set; }
            public string Rows { get; set; }
            public string Columns { get; set; }

            public hall(string name, string rows, string columns)
            {
                Name = name;
                Rows = rows;
                Columns = columns;

                var selectableList = new TextListBuilder(Window, 3, 1)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                    .LinkWindows(hallscreen)
                    .Result();

                var title = new TextBuilder(Window, 3, 3)
                    .Color(ConsoleColor.Red)
                    .Text(name)
                    .Result();

                var description = new TextListBuilder(Window, 3, 5)
                    .Color(ConsoleColor.Gray)
                    .SetItems($"Rows: {rows}", $"Columns: {columns}")
                    .Result();
            }
        }
        static Window hallscreen = new Window();
        static void Halls()
        {
            var halls = File.ReadAllText("..\\..\\..\\Halls.json");

            JsonDocument doc = JsonDocument.Parse(halls);

            JsonElement root = doc.RootElement;

            Console.ForegroundColor= ConsoleColor.DarkMagenta;
            var hallObjects = new hall[root.GetArrayLength()];
            var hallWindows = new Window[hallObjects.Length];
            string[] hallNames = new string[root.GetArrayLength()];
            for (int i = 0; i < hallNames.Length; i++)
            {
                hallObjects[i] = new hall(
                    "Hall number " + root[i].GetProperty("id").ToString(),
                    root[i].GetProperty("rows").ToString(),
                    root[i].GetProperty("columns").ToString()
                    );

                hallWindows[i] = hallObjects[i].Window;
                hallNames[i] = hallObjects[i].Name;
                //Console.WriteLine($"{i} : {movieNames[i]}");
            }
            var introtexthall = new TextListBuilder(hallscreen, 2, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("Select which hall you would like to see.")
                .Result();

            var GoBackHallsScherm2 = new TextListBuilder(hallscreen, 2, 5)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(selecteerHallsScherm)
                .Result();

            var showhall = new TextListBuilder(hallscreen, 14, 5)
                .Color(ConsoleColor.Red)
                .SetItems(hallNames)
                .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .LinkWindows(hallWindows)
                .Result();

            var title = new TextBuilder(hallscreen, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/Hall Select/Halls/")
                .Result();
        }
    }
}