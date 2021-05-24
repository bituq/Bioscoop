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
                int duration = 0;
                if (input[0].Value == "" || input[1].Value == "" || input[2].Value == "" || input[3].Value == "" || input[4].Value == "" || input[5].Value == "" || input[6].Value == "" || input[7].Value == "" || input[8].Value == "")
                    ErrorList.Add("Input fields may not be empty.");
                if (!Int32.TryParse(input[1].Value, out duration))
                    ErrorList.Add("Duration must be a number.");

                if (ErrorList.Count == 0)
                {
                   
                    var nMovie = new MovieAdd();
                    nMovie.name = input[0].Value;
                    nMovie.duration = duration;
                    nMovie.releaseDate = input[2].Value;
                    nMovie.rating = input[3].Value;
                    nMovie.genres = input[4].Value.Split(' ');
                    nMovie.starring = input[5].Value.Split(' ');
                    nMovie.language = input[6].Value;
                    nMovie.company = input[7].Value;
                    nMovie.description = input[8].Value;
                    JsonFile.AppendToFile(nMovie, "..\\..\\..\\Movies.json");
                    message.Replace(
                        new TextListBuilder(addMovie, 1, 15)
                        .Color(ConsoleColor.Green)
                        .SetItems($"You have succesfully added {nMovie.name} to the list!", "If you want to add another Movie, fill in the above requirements again.")
                        .Result());
                }
                else
                {
                    
                    ErrorList.Insert(0, "Errors:");
                    message.Replace(
                        new TextListBuilder(addMovie, 1, 15)
                        .Color(ConsoleColor.Red)
                        .SetItems(ErrorList.ToArray())
                        .Result());
                }
            };
        }
    }
}
