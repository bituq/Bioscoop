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
        public class SnacksRemove
        {
            public string name { get; set; }
            public double price { get; set; }
            public string vegetarian { get; set; }
            public int stock { get; set; }

            public void SnackList(string snackName)
            {
                name = snackName;
                var title = new TextBuilder(removeSnack, 11, 6)
                    .Color(ConsoleColor.Red)
                    .Text(name)
                    .Result();

             
            }

        }
        public static Window removeSnack = new Window(true);
        static void RemoveSnack()
        {
            var title = new TextBuilder(removeSnack, 1, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/Hall Select/Remove Snack")
                .Result();

            var _ = new TextListBuilder(removeSnack, 1, 6)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(AdminScherm)
                .Result();

            var snacksAndDrinks = File.ReadAllText("..\\..\\..\\snacksAndDrinks.json");
          
            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

            SnacksRemove[] snackObjects = new SnacksRemove[root.GetArrayLength()];
            var snackWindows = new Window[snackObjects.Length];
            string[] snackNames = new string[root.GetArrayLength()];

            for (int i = 0; i < snackNames.Length; i++)
            {
                snackObjects[i] = new SnacksRemove();
                snackNames[i] = root[i].GetProperty("name").ToString();
                
            };

            var SnackList = new TextListBuilder(removeSnack, 11, 6)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(snackNames)
                .UseNumbers()
                .Result();

            string[] removeName = new string[snackObjects.Length];


            for (int i = 0; i < snackObjects.Length; i++)
            {
                removeName[i] = "Remove";
            }
            
            var removeButtons = new TextListBuilder(removeSnack, 35, 6)
                .Color(ConsoleColor.Green)
                .SetItems(removeName)
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .Result();

            foreach (var button in removeButtons.Items)
            {
                button.OnClick = () =>
                {
                    var snacksAndDrinksList = JsonFile.FileAsList("..\\..\\..\\snacksAndDrinks.json");

                    int index = removeButtons.Items.IndexOf(button);
                    var id = snacksAndDrinksList[index].GetProperty("id").GetInt32();

                    JsonFile.RemoveFromFile("id", id, "..\\..\\..\\snacksAndDrinks.json");
                };
            }


        }
    }
}
