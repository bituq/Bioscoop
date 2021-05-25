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

            var title = new TextBuilder(addhallscreen, 1, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/Hall Select/Edit Hall")
                .Result();

            var title2 = new TextBuilder(addhallscreen, 1, 3)
                 .Color(ConsoleColor.White)
                 .Text("Enter the information of the hall to add")
                 .Result();


            var goBack = new TextListBuilder(addhallscreen, 1, 5)
                   .Color(ConsoleColor.Red)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(selecteerHallsScherm)
                   .Result();

            var inputOptions = new TextListBuilder(addhallscreen, 13, 5)
                .Color(ConsoleColor.Red)
                .SetItems("ID: ", "Rows: ", "Columns: ")
                .Result();

            var input = new TextListBuilder(addhallscreen, 14 + inputOptions.Items[2].Text.Length, 5)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();

            var addButton = new TextListBuilder(addhallscreen, 13, 9)
                .Color(ConsoleColor.Green)
                .SetItems("Add")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .Result();

            var message = new TextListBuilder(addhallscreen)
                .SetItems("")
                .Result();

            var halljson = File.ReadAllText("..\\..\\..\\Halls.json");

            JsonDocument doc = JsonDocument.Parse(halljson);
            JsonElement root = doc.RootElement;

            goBack[0].OnClick = () =>
            {
                /*input.Replace(new TextListBuilder(addhallscreen, 2 + inputOptions.Items[2].Text.Length, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result());*/

                message.Replace(new TextListBuilder(addhallscreen, 13, 12)
                    .Color(ConsoleColor.Green)
                    .SetItems("")
                    .Result()); 
            };

            addButton[0].OnClick = () =>
            {
                var ErrorList = new List<string>();
                int id = 0;
                int rows = 0;
                int columns = 0;

               

                

                if (input[0].Value == "" || input[1].Value == "" || input[2].Value == "")
                    ErrorList.Add("Input fields may not be empty.");
                if (!Int32.TryParse(input[0].Value, out id))
                    ErrorList.Add("ID must be an integer (ex. 1, 5, 10 etc.)");
                if (!Int32.TryParse(input[1].Value, out rows))
                    ErrorList.Add("Rows must be an integer (ex. 1, 5, 10 etc.)");
                if (!Int32.TryParse(input[2].Value, out columns))
                    ErrorList.Add("Columns must be an integer (ex. 1, 5, 10 etc.)");
                
                

                if (ErrorList.Count == 0)
                {
                    for (int i = 0; i < root.GetArrayLength(); i++)
                    {
                        if (Convert.ToInt32(input[0].Value) == root[i].GetProperty("id").GetInt32())
                        {
                            ErrorList.Add($"There already exist an hall with the id {input[0].Value}!");
                        }
                    }
                    if (ErrorList.Count == 0)
                    {
                        var objHall = new addHall();
                        objHall.id = Convert.ToInt32(input[0].Value);
                        objHall.rows = Convert.ToInt32(input[1].Value);
                        objHall.columns = Convert.ToInt32(input[2].Value);
                        JsonFile.AppendToFile(objHall, "..\\..\\..\\Halls.json");
                        message.Replace(
                            new TextListBuilder(addhallscreen, 13, 12)
                            .Color(ConsoleColor.Green)
                            .SetItems($"You have succesfully added hall {objHall.id} to the list!", "If you want to add another snack, fill in the above requirements again.")
                            .Result());
                    }
                }
                else
                {
                    ErrorList.Insert(0, "Errors:");
                    message.Replace(
                        new TextListBuilder(addhallscreen, 13, 12)
                        .Color(ConsoleColor.Red)
                        .SetItems(ErrorList.ToArray())
                        .Result());
                }
            };
        }
    }
}