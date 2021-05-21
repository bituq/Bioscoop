using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;
using System.Text.Json;
using System.IO;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        public class SnacksAdd
        {
            public string name { get; set; }
            public double price { get; set; }
            public string vegetarian { get; set; }
            public int stock { get; set; }
        }

        public static Window addSnack = new Window(false);
        static void AddSnack()
        {
            var _ = new TextListBuilder(addSnack, 1, 1)
                   .Color(ConsoleColor.White)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(mainMenu)
                   .Result();

            var inputOptions = new TextListBuilder(addSnack, 1, 3)
                .SetItems("Name: ", "Price: ", "Vegetarian: ", "Stock: ")
                .Result();

            var input = new TextListBuilder(addSnack, 2 + inputOptions.Items[3].Text.Length, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();

            var addButton = new TextListBuilder(addSnack, 1, 8)
                   .Color(ConsoleColor.Green)
                   .SetItems("Add")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .Result();

            var snacksAndDrinks = File.ReadAllText("..\\..\\..\\snacksAndDrinks.json");

            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

            addButton[0].OnClick = () =>
            {
                var nSnack = new SnacksAdd();
                nSnack.name = input[0].Value;
                nSnack.price = 20.0;
                nSnack.vegetarian = input[2].Value;
                nSnack.stock = 5;
                JsonFile.AppendToFile(nSnack, "..\\..\\..\\snacksAndDrinks.json");
            };
        }

    }
}
