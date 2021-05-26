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
            var title = new TextBuilder(addSnack, 1, 2)
           .Color(ConsoleColor.Cyan)
           .Text("Home/Admin/Hall Select/Edit Snack")
           .Result();

            var title2 = new TextBuilder(addSnack, 1, 3)
                 .Color(ConsoleColor.White)
                 .Text("Enter the information of the snack you want to add\nMake sure to only input either 'Yes' or 'No' in the Veggie input window")
                 .Result();

            var inputOptions = new TextListBuilder(addSnack, 13, 6)
                .SetItems("Name: ", "Price: ", "Veggie: ", "Stock: ")
                .Result();

            var input = new TextListBuilder(addSnack, 14 + inputOptions.Items[3].Text.Length, 6)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();

            var addButton = new TextListBuilder(addSnack, 13, 11)
                .Color(ConsoleColor.Green)
                .SetItems("Add")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .Result();
            
            var _ = new TextListBuilder(addSnack, 1, 6)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(AdminScherm)
                .Result();
            
            var message = new TextListBuilder(addSnack)
                .SetItems("")
                .Result();

            var snacksAndDrinks = File.ReadAllText("..\\..\\..\\snacksAndDrinks.json");

            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

            addButton[0].OnClick = () =>
            {
                /* Dylan */
                var ErrorList = new List<string> ( );
                string[] n = new string[] { "yes", "no" };
                double price = 0.0;
                int stock = 0;
                if (input[0].Value == "" || input[1].Value == "" || input[2].Value == "" || input[3].Value == "")
                    ErrorList.Add("Input fields may not be empty.");
                if (!Double.TryParse(input[1].Value, out price))
                    ErrorList.Add("Price must be a decimal point value. (ex. 1.00, 5.20, 10.23, etc.)");
                if (!new List<string> { "yes", "no" }.Contains(input[2].Value.ToLower()))
                    ErrorList.Add("Vegetarian must be either 'yes' or 'no'.");
                else
                    input[2].Value = input[2].Value[0].ToString().ToUpper() + input[2].Value.Substring(1, input[2].Value.Length - 1);
                if (!Int32.TryParse(input[3].Value, out stock))
                    ErrorList.Add("Stock must be a number.");

                if (ErrorList.Count == 0)
                {
                    /* Waros */
                    var nSnack = new SnacksAdd();
                    nSnack.name = input[0].Value;
                    nSnack.price = price;
                    nSnack.vegetarian = input[2].Value;
                    nSnack.stock = stock;
                    JsonFile.AppendToFile(nSnack, "..\\..\\..\\snacksAndDrinks.json");
                    message.Replace(
                        new TextListBuilder(addSnack, 1, 13)
                        .Color(ConsoleColor.Green)
                        .SetItems($"You have succesfully added {nSnack.name} to the list!", "If you want to add another snack, fill in the above requirements again.")
                        .Result());
                    snacksWindow.Reset();
                    SnacksWindow();
                    snacksWindow.Init();
                }
                else
                {
                    /* Dylan */
                    ErrorList.Insert(0, "Errors:");
                    message.Replace(
                        new TextListBuilder(addSnack, 1, 13)
                        .Color(ConsoleColor.Red)
                        .SetItems(ErrorList.ToArray())
                        .Result());
                }
            };
        }
    }
}
