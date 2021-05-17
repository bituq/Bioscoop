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
            public string Price { get; set; }

            public Food(string name, string price, string vegetarian, string stock)
            {
                Name = name;
                Price = price;

                var title = new TextBuilder(Window, 11, 1)
                    .Color(ConsoleColor.Red)
                    .Text(name)
                    .Result();

                var information = new TextListBuilder(Window, 11, 3)
                    .Color(ConsoleColor.White)
                    .SetItems(price, vegetarian, stock)
                    .Result();


                var goBack = new TextListBuilder(Window, 1, 1)
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
            string[] snackPrice = new string[root.GetArrayLength()];
            var addbuttonarray = new string[snackNames.Length];
            var removebuttonlist = new List<string>() {};
            var cartlist = new List<string>() {};
            
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
                snackPrice[i] = $"${root[i].GetProperty("price").ToString()}";
                addbuttonarray[i] = "Add to cart";
            }
            
            
            /*foreach(var p in snackNames)
            {
                shopcart.Add(p);
            }*/

            
              
            

            

            var snackList = new TextListBuilder(food, 11, 1)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(snackNames)
                .UseNumbers()
                .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                .LinkWindows(snackWindows)
                .Result();

            var snackPrices = new TextListBuilder(food, 35, 1)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(snackPrice)
                .Result();

            var addButton = new TextListBuilder(food, 46, 1)
                .Color(ConsoleColor.Magenta)
                .SetItems(addbuttonarray)
                .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                .Result();

            var shopcart = new TextListBuilder(food, 70, 1)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(cartlist.ToArray())
                .Result();
            
            var removebutton = new TextListBuilder(food, 90, 1)
                    .Color(ConsoleColor.Magenta)
                    .SetItems("")
                    .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                    .Result();

            for (int i = 0; i < addbuttonarray.Length; i++)
            {
                addButton[i].OnClick = () =>
                {
                    var addIndex = addButton.Items.IndexOf(addButton.Items.Find(item => item.Selected));
                    cartlist.Add(snackNames[addIndex]);
                    removebuttonlist.Add("remove");

                    shopcart.Replace(new TextListBuilder(food, 70, 1)
                    .Color(ConsoleColor.DarkMagenta)
                    .SetItems(cartlist.ToArray())
                    .Result());

                    removebutton.Replace(new TextListBuilder(food, 90, 1)
                    .Color(ConsoleColor.Magenta)
                    .SetItems(removebuttonlist.ToArray())
                    .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                    .Result());

                    removebutton[removebutton.Items.Count - 1].OnClick = () =>
                    {
                        var removeIndex = removebutton.Items.IndexOf(removebutton.Items.Find(item => item.Selected));
                        cartlist.RemoveAt(removeIndex);
                        removebuttonlist.RemoveAt(removeIndex);

                        shopcart.Replace(new TextListBuilder(food, 70, 1)
                        .Color(ConsoleColor.Blue)
                        .SetItems(cartlist.ToArray())
                        .Result());

                        removebutton.Replace(new TextListBuilder(food, 90, 1)
                        .Color(ConsoleColor.Green)
                        .SetItems(removebuttonlist.ToArray())
                        .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                        .Result());
                    };
                };
            }
        }
    }
}
