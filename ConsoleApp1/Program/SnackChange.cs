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

            var snacksAndDrinks = File.ReadAllText("..\\..\\..\\snacksAndDrinks.json");

            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

            var nSnack = new SnacksAdd();
            nSnack.name = "Fanta(750ml)";
            nSnack.price = 1000.99;
            nSnack.vegetarian = "Yes";
            nSnack.stock = 5;
            

            JsonFile.AppendToFile(new List<SnacksAdd> { nSnack }, "..\\..\\..\\snacksAndDrinks.json");





        }

    }
}
