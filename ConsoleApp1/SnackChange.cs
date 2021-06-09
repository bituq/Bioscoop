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
           .Color(Colors.breadcrumbs)
           .Text("Home/Admin/Snack Options/Add Snack")
           .Result();

            var title2 = new TextBuilder(addSnack, 1, 3)
                 .Color(Colors.description)
                 .Text("Enter the information of the snack you want to add\nMake sure to only input either 'Yes' or 'No' in the Veggie input field\nPrice must be in dollars")
                 .Result();

            var inputOptions = new TextListBuilder(addSnack, 13, 7)
                .SetItems("Name: ", "Price: ", "Veggie: ", "Stock: ")
                .Result();

            var input = new TextListBuilder(addSnack, 14 + inputOptions.Items[3].Text.Length, 7)
                .Color(Colors.text)
                .SetItems("", "", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();

            var addButton = new TextListBuilder(addSnack, 13, 12)
                .Color(Colors.submit.Item1)
                .SetItems("Add")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .Result();
            
            var _ = new TextListBuilder(addSnack, 1, 7)
                .Color(Colors.back)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(snackOptions)
                .Result();
            
            var message = new TextListBuilder(addSnack)
                .SetItems("")
                .Result();

            var snacksAndDrinks = File.ReadAllText("../../../snacksAndDrinks.json");

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
                    JsonFile.AppendToFile(nSnack, "../../../snacksAndDrinks.json");
                    message.Replace(
                        new TextListBuilder(addSnack, 1, 14)
                        .Color(Colors.text)
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
                        new TextListBuilder(addSnack, 1, 14)
                        .Color(ConsoleColor.Red)
                        .SetItems(ErrorList.ToArray())
                        .Result());
                }
            };
        }
    }
}