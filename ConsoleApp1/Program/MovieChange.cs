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
        public class MovieAdd
        {
            public string id { get; set; }
            public string name { get; set; }
            public int duration { get; set; }
            public string releaseDate { get; set; }
            public string rating { get; set; }
            public string[] genres { get; set; }
            public string[] starring { get; set; }
            public string language { get; set; }
            public string company { get; set; }
            public string description { get; set; }
        }

        public static Window addMovie = new Window(false);
        static void AddMovie()
        {
            var inputOptions = new TextListBuilder(addMovie, 1, 3)
                .SetItems("Name: ", "Duration: ", "Release Date: ", "Rating: ", "Genre: ", "Starring: ", "Language: ", "Company: ", "Description: " )
                .Result();

            var input = new TextListBuilder(addMovie, 2 + inputOptions.Items[8].Text.Length, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "", "", "", "", "", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();

            var addButton = new TextListBuilder(addMovie, 1, 20)
                .Color(ConsoleColor.Green)
                .SetItems("Add")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .Result();

            var _ = new TextListBuilder(addMovie, 1, 25)
                .Color(ConsoleColor.White)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(AdminScherm)
                .Result();

            var message = new TextListBuilder(addMovie)
                .SetItems("")
                .Result();

            var movieFile = File.ReadAllText("..\\..\\..\\Movies.json");

            JsonDocument doc = JsonDocument.Parse(movieFile);
            JsonElement root = doc.RootElement;

            addButton[0].OnClick = () =>
            {
                
                var ErrorList = new List<string>();
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
                   
                    var nSnack = new SnacksAdd();
                    nSnack.name = input[0].Value;
                    nSnack.price = price;
                    nSnack.vegetarian = input[2].Value;
                    nSnack.stock = stock;
                    JsonFile.AppendToFile(nSnack, "..\\..\\..\\snacksAndDrinks.json");
                    message.Replace(
                        new TextListBuilder(addSnack, 1, 10)
                        .Color(ConsoleColor.Green)
                        .SetItems($"You have succesfully added {nSnack.name} to the list!", "If you want to add another snack, fill in the above requirements again.")
                        .Result());
                }
                else
                {
                    
                    ErrorList.Insert(0, "Errors:");
                    message.Replace(
                        new TextListBuilder(addSnack, 1, 10)
                        .Color(ConsoleColor.Red)
                        .SetItems(ErrorList.ToArray())
                        .Result());
                }
            };
        }
    }
}
