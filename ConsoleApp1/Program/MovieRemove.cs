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
        public static Window removeMovie = new Window(true);
        static void RemoveMovie()
        {
            var title = new TextBuilder(removeMovie, 1, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/Remove Movie")
                .Result();

            var back = new TextListBuilder(removeMovie, 1, 6)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(AdminScherm)
                .Result();

            var movieMan = File.ReadAllText("..\\..\\..\\Movies.json");

            JsonDocument doc = JsonDocument.Parse(movieMan);
            JsonElement root = doc.RootElement;

            List<string> movieNames = new List<string>();

            for (int i = 0; i < root.GetArrayLength(); i++)
            {
                movieNames.Add(root[i].GetProperty("name").ToString());

            };

            var MovieList = new TextListBuilder(removeMovie, 19, 6)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems(movieNames.ToArray())
                .Result();

            List<string> removeName = new List<string>();


            for (int i = 0; i < movieNames.Count; i++)
            {
                removeName.Add("Remove");
            }

            var removeButtons = new TextListBuilder(removeMovie, 11, 6)
                .Color(ConsoleColor.Red)
                .SetItems(removeName.ToArray())
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .Result();

            void UpdateClick()
            {

                foreach (var button in removeButtons.Items)
                {
                    button.OnClick = () =>
                    {
                        List<JsonElement> movieMan = JsonFile.FileAsList("..\\..\\..\\Movies.json");

                        int index = removeButtons.Items.IndexOf(button);
                        int id = movieMan[index].GetProperty("id").GetInt32();

                        JsonFile.RemoveFromFile("id", id, "..\\..\\..\\Movies.json");

                        movieNames.RemoveAt(index);
                        removeName.RemoveAt(0);


                        bool isEmpty = false;
                        if (removeName.Count == 0)
                        {
                            isEmpty = true;
                            removeName.Add("");
                            removeButtons[0].Unselect();
                        }

                        removeButtons.Replace(new TextListBuilder(removeMovie, 11, 6)
                            .Color(ConsoleColor.Red)
                            .SetItems(removeName.ToArray())
                            .Selectable(ConsoleColor.Black, ConsoleColor.White)
                            .Result());


                        MovieList.Replace(new TextListBuilder(removeMovie, 19, 6)
                            .Color(ConsoleColor.DarkMagenta)
                            .SetItems(movieNames.ToArray())
                            .Result());


                        if (!isEmpty)
                        {
                            var _ = removeButtons.Items.Count;
                            removeButtons[0].Unselect();
                            removeButtons[Math.Min(index, removeButtons.Items.Count - 1)].Select();

                            foreach (SelectableText Item in removeButtons.Items)
                                Item.OnClick = UpdateClick;
                        }
                        else
                        {
                            removeMovie.ActiveSelectable = back;
                            back[0].Select();
                        }



                    };
                }


            }
            UpdateClick();
        }
    }
}

