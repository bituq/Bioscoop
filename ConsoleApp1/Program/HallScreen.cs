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
                .Color(Colors.breadcrumbs)
                .Text($"Home/Admin/Hall Select/Halls/{Name}")
                .Result();

                var selectableList = new TextListBuilder(Window, 1, 5)
                    .Color(Colors.selection)
                    .SetItems("Go back")
                    .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                    .LinkWindows(hallscreen)
                    .Result();

                var title2 = new TextBuilder(Window, 13, 5)
                    .Color(Colors.title)
                    .Text(name)
                    .Result();

                var description = new TextListBuilder(Window, 13, 7)
                    .Color(Colors.undertitle)
                    .SetItems($"Rows: {rows}", $"Columns: {columns}")
                    .Result();

            }
        }
        static Window hallscreen = new Window();
        static void Halls()
        {
            var halls = File.ReadAllText("../../../Halls.json");

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
                .Color(Colors.undertitle)
                .SetItems("Select an hall you would like to see.")
                .Result();

            var goBack = new TextListBuilder(hallscreen, 2, 5)
                .Color(Colors.selection)
                .SetItems("Go back")
                .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                .LinkWindows(selecteerHallsScherm)
                .Result();

            var removeButton = new TextListBuilder(hallscreen, 14, 5)
                .Color(ConsoleColor.DarkRed)
                .SetItems(removeButtonlist.ToArray())
                .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .Result();

            var showhall = new TextListBuilder(hallscreen, 24, 5)
                .Color(Colors.selection)
                .SetItems(hallNames.ToArray())
                .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                .LinkWindows(hallWindows)
                .Result();

            var title = new TextBuilder(hallscreen, 2, 2)
                .Color(Colors.breadcrumbs)
                .Text("Home/Admin/Hall Select/Halls/")
                .Result();

            var Errormessage = new TextListBuilder(hallscreen, 43, 5)
                .Color(ConsoleColor.Cyan)
                .SetItems("")
                .Result();




            void onRemove()
            {
                foreach (var item in removeButton.Items)
                {
                    bool valid = true;
                    int themovie = 0;
                    List<JsonElement> HallList = JsonFile.FileAsList("../../../Halls.json");
                    int index = removeButton.Items.IndexOf(item);
                    int id = HallList[index].GetProperty("id").GetInt32();

                    item.OnClick = () =>
                    {
                        List<JsonElement> listylist = JsonFile.FileAsList("../../../TimeSlots.json");
                        for (int i = 0; i < listylist.Count; i++)
                        {
                            if (id == listylist[i].GetProperty("hall").GetInt32())
                            {
                                valid = false;
                                themovie = listylist[i].GetProperty("id").GetInt32();
                            }
                        }
                        if(valid)
                        {
                            JsonFile.RemoveFromFile("id", id, "../../../Halls.json");

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

                            showhall.Replace(new TextListBuilder(hallscreen, 24, 5)
                            .Color(Colors.selection)
                            .SetItems(hallNames.ToArray())
                            .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                            .LinkWindows(hallWindows)
                            .Result());

                            removeButton.Replace(new TextListBuilder(hallscreen, 14, 5)
                            .Color(Colors.selection)
                            .SetItems(removeButtonlist.ToArray())
                            .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                            .Result());

                            Errormessage.Replace(new TextListBuilder(hallscreen, 43, 5)
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
                            onRemove();
                        }
                        else
                        {
                            Errormessage.Replace(new TextListBuilder(hallscreen, 43, 5)
                            .Color(ConsoleColor.DarkRed)
                            .SetItems($"Not able to remove hall {id}, this hall is used in timeslot id {themovie}")
                            .Result());
                        }
                    };
                }
            }
            onRemove();
            goBack[0].OnClick = () =>
            {
                Errormessage.Replace(new TextListBuilder(hallscreen, 43, 5)
                            .Color(ConsoleColor.Green)
                            .SetItems("")
                            .Result());
            };
        }
    }
}