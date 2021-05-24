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
            var goBack = new TextListBuilder(food, 1, 1)
                   .Color(ConsoleColor.White)
                   .SetItems("Go back", "Continue")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(mainMenu)
                   .Result();

            var snacksAndDrinks = File.ReadAllText("..\\..\\..\\snacksAndDrinks.json");

            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

            Food[] snackObjects = new Food[root.GetArrayLength()];
            Window[] foodWindows = new Window[snackObjects.Length];
            string[] snackNames = new string[root.GetArrayLength()];
            string[] snackPrice = new string[root.GetArrayLength()];
            var addbuttonarray = new string[snackNames.Length];
            var removebuttonlist = new List<string>() {};
            var cartlist = new List<string>() {};
            var cartpricelist = new List<string> {};
            var infobuttonlist = new List<string> {};
            var sumpricelist = new List<int> {};
            int sum = 0;
            for (int i = 0; i < snackNames.Length; i++)
            {
                snackObjects[i] = new Food(
                    root[i].GetProperty("name").ToString(),
                    $"Price: ${root[i].GetProperty("price")}",
                    $"Vegetarian: {root[i].GetProperty("vegetarian")}",
                    $"Stock: {root[i].GetProperty("stock")}"

                    );

                foodWindows[i] = snackObjects[i].Window;
                snackNames[i] = snackObjects[i].Name;
                snackPrice[i] = $"${root[i].GetProperty("price").ToString()}";
                addbuttonarray[i] = "Add to cart";
                infobuttonlist.Add("Info");
            }

            var title1 = new TextBuilder(food, 11, 1)
                .Color(ConsoleColor.Red)
                .Text("Snacks and drinks:")
                .Result();
            
            var title2 = new TextBuilder(food, 70, 1)
                .Color(ConsoleColor.Red)
                .Text("My cart:")
                .Result();

            var title3 = new TextBuilder(food, 11, 20)
                .Color(ConsoleColor.Red)
                .Text("Total :")
                .Result();

            var snackList = new TextListBuilder(food, 11, 4)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(snackNames)
                .Result();

            var snackPrices = new TextListBuilder(food, 35, 4)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(snackPrice)
                .Result();

            var addButton = new TextListBuilder(food, 46, 4)
                .Color(ConsoleColor.Magenta)
                .SetItems(addbuttonarray)
                .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                .Result();
            
            var infoButton = new TextListBuilder(food, 61, 4)
                .Color(ConsoleColor.Magenta)
                .SetItems(infobuttonlist.ToArray())
                .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                .LinkWindows(foodWindows)
                .Result();

            var shopcart = new TextListBuilder(food, 70, 4)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(cartlist.ToArray())
                .Result();

            var shopcartprice = new TextListBuilder(food, 94, 4)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(cartpricelist.ToArray())
                .Result();

            var removebutton = new TextListBuilder(food, 105, 4)
                    .Color(ConsoleColor.Magenta)
                    .SetItems("")
                    .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                    .Result();

            var total = new TextListBuilder(food, 21, 20)
                .Color(ConsoleColor.White)
                .SetItems($"${sum.ToString()}")
                .Result();
            
            
            
            void onRemove()
            {
                var removeIndex = removebutton.SelectedIndex;

                if (cartpricelist.Count > 0)
                    sum -= Convert.ToInt32(Convert.ToDouble(cartpricelist[removeIndex].Trim('$')));
                else
                    sum = 0;

                cartlist.RemoveAt(removeIndex);
                cartpricelist.RemoveAt(removeIndex);
                removebuttonlist.RemoveAt(removeIndex);

                bool isEmpty = false;
                if (removebuttonlist.Count == 0)
                {
                    isEmpty = true;
                    removebuttonlist.Add("");
                    removebutton[0].Unselect();
                }

                shopcart.Replace(new TextListBuilder(food, 70, 4)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(cartlist.ToArray())
                .Result());

                shopcartprice.Replace(new TextListBuilder(food, 94, 4)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(cartpricelist.ToArray())
                .Result());
                
                removebutton[removeIndex].Unselect();

                removebutton.Replace(new TextListBuilder(food, 105, 4)
                .Color(ConsoleColor.Magenta)
                .SetItems(removebuttonlist.ToArray())
                .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                .Result());

                total.Replace(new TextListBuilder(food, 21, 20)
                .Color(ConsoleColor.White)
                .SetItems($"${(sum / 100).ToString()}" + "." + ((sum % 100 < 10) ? $"0{sum % 100}" : (sum % 100).ToString()))
                .Result());

                if (!isEmpty)
                {
                    var _ = removebutton.Items.Count;
                    removebutton[0].Unselect();
                    removebutton[Math.Min(removeIndex, removebutton.Items.Count - 1)].Select();

                    foreach (SelectableText Item in removebutton.Items)
                        Item.OnClick = onRemove;
                }
                else
                {
                    food.ActiveSelectable = addButton;
                    addButton[0].Select();
                }
            }

            for (int i = 0; i < addbuttonarray.Length; i++)
            {
                void OnAdd()
                {
                    var addIndex = addButton.SelectedIndex;
                    cartlist.Add(snackNames[addIndex]);
                    cartpricelist.Add(snackPrice[addIndex]);
                    if (removebuttonlist.Contains(""))
                        removebuttonlist.Remove("");

                    removebuttonlist.Add("Remove");

                    sum += Convert.ToInt32(Convert.ToDouble(snackPrice[addIndex].Trim('$', '.', ' ', ',')));

                    shopcart.Replace(new TextListBuilder(food, 70, 4)
                    .Color(ConsoleColor.DarkMagenta)
                    .SetItems(cartlist.ToArray())
                    .Result());

                    shopcartprice.Replace(new TextListBuilder(food, 94, 4)
                    .Color(ConsoleColor.DarkMagenta)
                    .SetItems(cartpricelist.ToArray())
                    .Result());

                    removebutton.Replace(new TextListBuilder(food, 105, 4)
                    .Color(ConsoleColor.Magenta)
                    .SetItems(removebuttonlist.ToArray())
                    .Selectable(ConsoleColor.Cyan, ConsoleColor.DarkMagenta)
                    .Result());

                    total.Replace(new TextListBuilder(food, 21, 20)
                    .Color(ConsoleColor.White)
                    .SetItems($"${(sum / 100).ToString()}" + "." + ((sum % 100 < 10) ? $"0{sum % 100}" : (sum % 100).ToString()))
                    .Result());

                    food.ActiveSelectable = addButton;

                    foreach(SelectableText item in removebutton.Items)
                        item.OnClick = onRemove;
                }
                addButton[i].OnClick = OnAdd;
            }
        }
    }
}
