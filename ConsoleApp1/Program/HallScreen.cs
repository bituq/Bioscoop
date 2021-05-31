using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;
using System.Text.Json;
using System.IO;

namespace CinemaApplication
{
    partial class Program
    {
        public class hall
        {
            public Window Window = new Window();
            public string Name { get; set; }
            public string Rows { get; set; }
            public string Columns { get; set; }

            public hall(string name, string rows, string columns)
            {
                Name = name;
                Rows = rows;
                Columns = columns;

                var title1 = new TextBuilder(Window, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text($"Home/Admin/Hall Select/Halls/{Name}")
                .Result();

                var selectableList = new TextListBuilder(Window, 1, 5)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                    .LinkWindows(hallscreen)
                    .Result();

                var title2 = new TextBuilder(Window, 13, 5)
                    .Color(ConsoleColor.Red)
                    .Text(name)
                    .Result();

                var description = new TextListBuilder(Window, 13, 7)
                    .Color(ConsoleColor.Gray)
                    .SetItems($"Rows: {rows}", $"Columns: {columns}")
                    .Result();

            }
        }
        static Window hallscreen = new Window(true);
        static void Halls()
        {
            var halls = File.ReadAllText("..\\..\\..\\Halls.json");

            JsonDocument doc = JsonDocument.Parse(halls);

            JsonElement root = doc.RootElement;

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            var hallObjects = new hall[root.GetArrayLength()];
            var hallWindows = new Window[hallObjects.Length];
            var hallNames = new List<string> { };
            var removeButtonlist = new List<string> { };
            for (int i = 0; i < hallObjects.Length; i++)
            {
                hallObjects[i] = new hall(
                    "Hall number " + root[i].GetProperty("id").ToString(),
                    root[i].GetProperty("rows").ToString(),
                    root[i].GetProperty("columns").ToString()
                    );

                hallWindows[i] = hallObjects[i].Window;
                hallNames.Add(hallObjects[i].Name);
                removeButtonlist.Add("Remove");
                //Console.WriteLine($"{i} : {movieNames[i]}");
            }
            var introtexthall = new TextListBuilder(hallscreen, 2, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("Select an hall you would like to see.")
                .Result();

            var goBack = new TextListBuilder(hallscreen, 2, 5)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(selecteerHallsScherm)
                .Result();

            var showhall = new TextListBuilder(hallscreen, 14, 5)
                .Color(ConsoleColor.Red)
                .SetItems(hallNames.ToArray())
                .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .LinkWindows(hallWindows)
                .Result();

            var removeButton = new TextListBuilder(hallscreen, 31, 5)
                .Color(ConsoleColor.DarkRed)
                .SetItems(removeButtonlist.ToArray())
                .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .Result();

            var title = new TextBuilder(hallscreen, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/Hall Select/Halls/")
                .Result();

            var sure = new TextListBuilder(hallscreen, 43, 5)
                .Color(ConsoleColor.Cyan)
                .SetItems("")
                .Result();



            foreach (SelectableText item in removeButton.Items)
            {
                item.OnClick = onRemove;
            }
            void onRemove()
            {
                sure.Replace(new TextListBuilder(hallscreen, 43, 5)
                .Color(ConsoleColor.Red)
                .SetItems("Warning! : Removing halls might have conflicts with movies.")
                .Result());

                foreach (var item in removeButton.Items)
                {
                    item.OnClick = () =>
                    {
                        {

                            List<JsonElement> snacksAndDrinksList = JsonFile.FileAsList("..\\..\\..\\Halls.json");

                            int index = removeButton.Items.IndexOf(item);
                            int id = snacksAndDrinksList[index].GetProperty("id").GetInt32();

                            JsonFile.RemoveFromFile("id", id, "..\\..\\..\\Halls.json");


                            var removeIndex = removeButton.SelectedIndex;

                            removeButtonlist.RemoveAt(removeIndex);
                            hallNames.RemoveAt(removeIndex);

                            bool isEmpty = false;
                            if (removeButtonlist.Count == 0)
                            {
                                isEmpty = true;
                                removeButtonlist.Add("");
                                removeButton[0].Unselect();
                            }

                            removeButton.Replace(new TextListBuilder(hallscreen, 31, 5)
                            .Color(ConsoleColor.DarkRed)
                            .SetItems(removeButtonlist.ToArray())
                            .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                            .Result());

                            showhall.Replace(new TextListBuilder(hallscreen, 14, 5)
                            .Color(ConsoleColor.Red)
                            .SetItems(hallNames.ToArray())
                            .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                            .LinkWindows(hallWindows)
                            .Result());

                            sure.Replace(new TextListBuilder(hallscreen, 43, 5)
                .Color(ConsoleColor.Green)
                .SetItems($"Hall {id} is succesfully removed!")
                .Result());

                            if (!isEmpty)
                            {
                                var _ = removeButton.Items.Count;
                                removeButton[0].Unselect();
                                removeButton[Math.Min(removeIndex, removeButton.Items.Count - 1)].Select();

                                foreach (SelectableText Item in removeButton.Items)
                                    Item.OnClick = onRemove;
                            }
                            else
                            {
                                hallscreen.ActiveSelectable = goBack;
                                goBack[0].Select();
                            }
                        }
                    };
                }

            }
        }
    }
}