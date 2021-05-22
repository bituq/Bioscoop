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
            var inputOptions = new TextListBuilder(addSnack, 1, 3)
                .SetItems("Name: ", "Price: ", "Veggie: ", "Stock: ")
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
            
            var _ = new TextListBuilder(addSnack, 1, 15)
                .Color(ConsoleColor.White)
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
                var ErrorList = new List<string> ( );
                if (input[0].Value == "" && input[1].Value == "" && input[2].Value == "" && input[3].Value == "")
                {
                    ErrorList.Add("Input fields may not be empty.");
                }
                if (Double.TryParse(input[1].Value, out double _))
                {

                }
                var nSnack = new SnacksAdd();
                nSnack.name = input[0].Value;
                nSnack.price = Convert.ToDouble(input[1].Value);
                nSnack.vegetarian = input[2].Value;
                nSnack.stock = Convert.ToInt32(input[3].Value);
                JsonFile.AppendToFile(nSnack, "..\\..\\..\\snacksAndDrinks.json");
                message.Replace(
                    new TextListBuilder(addSnack, 1, 10)
                    .Color(ConsoleColor.Green)
                    .SetItems($"You have succesfully added {nSnack.name} to the list!","If you want to add another snack, fill in the above requirements again.")
                    .Result()); 

            };
        }
    }
}
