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

                var _ = new TextListBuilder(Window, 1, 1)
                   .Color(Colors.back)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(snacksWindow)
                   .Result();

                var title = new TextBuilder(Window, 11, 1)
                    .Color(ConsoleColor.Red)
                    .Text(name)
                    .Result();

                var information = new TextListBuilder(Window, 11, 3)
                    .Color(Colors.description)
                    .SetItems(price, vegetarian, stock)
                    .Result();

            }
        }

        public static Window snacksWindow = new Window();
        static void SnacksWindow()
        {
            var _ = new TextListBuilder(snacksWindow, 1, 1)
                   .Color(Colors.back)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(mainMenu)
                   .Result();

            var snacksAndDrinks = File.ReadAllText("../../../snacksAndDrinks.json");

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

            var snackList = new TextListBuilder(snacksWindow, 11, 1)
                .Color(Colors.selection)
                .SetItems(snackNames)
                .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                .LinkWindows(snackWindows)
                .Result();
        }
    }
}
