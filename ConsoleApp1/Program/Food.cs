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
        public class Food
        {
            public Window Window = new Window();
            public string Name { get; set; }


            public Food(string name, string price, string vegetarian, string stock)
            {
                Name = name;

                var title = new TextBuilder(Window, 11, 1)
                    .Color(ConsoleColor.Red)
                    .Text(name)
                    .Result();

                var information = new TextListBuilder(Window, 11, 3)
                    .Color(ConsoleColor.White)
                    .SetItems(price, vegetarian, stock)
                    .Result();


                var _ = new TextListBuilder(Window, 1, 1)
                   .Color(ConsoleColor.White)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(food)
                   .Result();

            }
        }

        public static Window food = new Window(true);
        static void FoodWindow()
        {
            var _ = new TextListBuilder(food, 1, 1)
                   .Color(ConsoleColor.White)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(mainMenu)
                   .Result();

            var snacksAndDrinks = File.ReadAllText("..\\..\\..\\snacksAndDrinks.json");

            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

            Food[] snackObjects = new Food[root.GetArrayLength()];
            var snackWindows = new Window[snackObjects.Length];
            string[] snackNames = new string[root.GetArrayLength()];
            var addbuttonarray = new string[snackNames.Length];

            for (int i = 0; i < snackNames.Length; i++)
            {
                snackObjects[i] = new Food(
                    root[i].GetProperty("name").ToString(),
                    $"Price: ${root[i].GetProperty("price")}",
                    $"Vegetarian: {root[i].GetProperty("vegetarian")}",
                    $"Stock: {root[i].GetProperty("stock")}"

                    );

                snackWindows[i] = snackObjects[i].Window;
                snackNames[i] = snackObjects[i].Name;
                addbuttonarray[i] = "Add to cart";
            }
            
            var shopcart = new List<string>() { "voedsel" };
            /*foreach(var p in snackNames)
            {
                shopcart.Add(p);
            }*/

            var cartlist = new TextListBuilder(food, 60, 1)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(shopcart.ToArray())
                .Result();
              
            

            

            var snackList = new TextListBuilder(food, 11, 1)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(snackNames)
                .UseNumbers()
                .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                .LinkWindows(snackWindows)
                .Result();

            

            var addButton = new TextListBuilder(food, 35, 1)
                .Color(ConsoleColor.Magenta)
                .SetItems(addbuttonarray)
                .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                .Result();
            
            addButton.Items[0].OnClick = () =>
            {
                shopcart.Add(snackNames[0]);
                
                cartlist.Replace(new TextListBuilder(food, 60, 1)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(shopcart.ToArray())
                .Result());
            };
        }
    }
}
