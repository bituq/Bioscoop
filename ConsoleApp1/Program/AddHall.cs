using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        public class addHall
        {
            public int id { get; set; }
            public int rows { get; set; }
            public int columns { get; set; }
        }

        static Window addhallscreen = new Window();
        static void AddHall()
        {
            var newhall = new List<string>();

            var title = new TextBuilder(addhallscreen, 13, 1)
                 .Color(ConsoleColor.Red)
                 .Text("Add your hall")
                 .Result();


            var goBack = new TextListBuilder(addhallscreen, 1, 1)
                   .Color(ConsoleColor.White)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(mainMenu)
                   .Result();

            var inputOptions = new TextListBuilder(addhallscreen, 1, 3)
                .SetItems("ID: ", "Rows: ", "Columns: ")
                .Result();

            var input = new TextListBuilder(addhallscreen, 2 + inputOptions.Items[2].Text.Length, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();

            var addButton = new TextListBuilder(addhallscreen, 1, 8)
                .Color(ConsoleColor.Green)
                .SetItems("Add")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .Result();

            var message = new TextListBuilder(addhallscreen)
                .SetItems("")
                .Result();

            var snacksAndDrinks = File.ReadAllText("..\\..\\..\\Halls.json");

            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

            /*goBack[0].OnClick = () =>
            {
                input.Replace(new TextListBuilder(addhallscreen, 2 + inputOptions.Items[2].Text.Length, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();)
            };*/

            addButton[0].OnClick = () =>
            {
                var nSnack = new addHall();
                nSnack.id = Convert.ToInt32(input[0].Value);
                nSnack.rows = Convert.ToInt32(input[1].Value);
                nSnack.columns = Convert.ToInt32(input[2].Value);
      
                JsonFile.AppendToFile(nSnack, "..\\..\\..\\Halls.json");
                message.Replace(
                    new TextListBuilder(addhallscreen, 1, 10)
                    .Color(ConsoleColor.Green)
                    .SetItems("You have succesfully added a new snack to the list!")
                    .Result());
            };
        }
    }
}