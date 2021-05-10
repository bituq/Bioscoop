using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;
using System.Text.Json;
using System.IO;

namespace CinemaApplication
{
    partial class Program
    {
        public class Snacks
        {
            public Window Window = new Window();
            public string Name { get; set; }


            public Snacks(string name, string price, string vegetarian, string stock)
            {
                Name = name;

                var title = new TextBuilder(Window, 3, 3)
                    .Color(ConsoleColor.Red)
                    .Text(name)
                    .Result();

                var information = new TextListBuilder(Window, 3, 4)
                    .Color(ConsoleColor.White)
                    .SetItems(price, vegetarian, stock)
                    .Result();

                var _ = new TextListBuilder(Window, 3, 1)
                   .Color(ConsoleColor.White)
                   .Selectable(new Color(ConsoleColor.Black, ConsoleColor.White), false, "Go back")
                   .LinkWindows(snacksWindow)
                   .Result();

            }
        }

        public static Window snacksWindow = new Window();
        static void SnacksWindow()
        {
            var snacksAndDrinks = File.ReadAllText("snacksAndDrinks.json");

            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

            Snacks[] snackObjects = new Snacks[root.GetArrayLength()];
            var snackWindows = new Window[snackObjects.Length];
            string[] snackNames = new string[root.GetArrayLength()];

            for (int i = 0; i < snackNames.Length; i++)
            {
                snackObjects[i] = new Snacks(
                    root[i].GetProperty("name").ToString(),
                    $"Price: ${root[i].GetProperty("price")}",
                    $"Vegetarian: {root[i].GetProperty("vegetarian")}",
                    $"Stock: {root[i].GetProperty("stock")}"

                    );

                snackWindows[i] = snackObjects[i].Window;
                snackNames[i] = snackObjects[i].Name;
                
            }

            var snackList = new TextListBuilder(snacksWindow, 4, 4)
                .Color(ConsoleColor.DarkMagenta)
                .Selectable(new Color(ConsoleColor.Cyan, ConsoleColor.DarkMagenta), true, snackNames)
                .LinkWindows(snackWindows)
                .Result();

        }

    }
}
