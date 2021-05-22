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
                   .LinkWindows(AdminScherm)
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

            var halljson = File.ReadAllText("..\\..\\..\\Halls.json");

            JsonDocument doc = JsonDocument.Parse(halljson);
            JsonElement root = doc.RootElement;

            

            addButton[0].OnClick = () =>
            {
                bool valid = true;
                if (input[0].Value != "" && input[1].Value != "" && input[2].Value != "")
                {
                    var objHall = new addHall();
                    objHall.id = Convert.ToInt32(input[0].Value);
                    objHall.rows = Convert.ToInt32(input[1].Value);
                    objHall.columns = Convert.ToInt32(input[2].Value);

                    for (int i = 0; i < root.GetArrayLength(); i++)
                    {
                        if (Convert.ToInt32(root[i].GetProperty("id")) == objHall.id)
                        {
                            message.Replace(
                            new TextListBuilder(addhallscreen, 1, 10)
                            .Color(ConsoleColor.Green)
                            .SetItems($"There already excist an hall with id {objHall.id}!")
                            .Result());
                            valid = false;
                        }
                    }
                    if (valid == true)
                    {
                        JsonFile.AppendToFile(objHall, "..\\..\\..\\Halls.json");

                        message.Replace(
                            new TextListBuilder(addhallscreen, 1, 10)
                            .Color(ConsoleColor.Green)
                            .SetItems($"Hall {objHall.id} is succesfully added to the list!")
                            .Result());
                    }
                }
                else
                {
                    message.Replace(new TextListBuilder(addhallscreen, 1, 10)
                    .Color(ConsoleColor.Green)
                    .SetItems("Not all fields are filled in!")
                    .Result());
                }
            };

            goBack[0].OnClick = () =>
            {
                /*input.Replace(new TextListBuilder(addhallscreen, 2 + inputOptions.Items[2].Text.Length, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result());*/

                message.Replace(new TextListBuilder(addhallscreen, 1, 10)
                    .Color(ConsoleColor.Green)
                    .SetItems("")
                    .Result()); 
            };
        }
    }
}