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
            public int id { get; set; }
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
            var _ = new TextListBuilder(addMovie, 1, 7)
               .Color(Colors.back)
               .SetItems("Go back")
               .Selectable(ConsoleColor.Black, ConsoleColor.White)
               .LinkWindows(adminMovieWindow)
               .Result();

            var title = new TextBuilder(addMovie, 1, 2)
                .Color(Colors.breadcrumbs)
                .Text("Home/Admin/Movie Options/Add Movie")
                .Result();

            var title2 = new TextBuilder(addMovie, 1, 3)
                 .Color(Colors.description)
                 .Text("Enter the information of the movie you want to add\nRelease date format: 'dd-mm-yyyy' and duration must be in minutes\nAlso make sure to put a ', ' between each actor's full name and between the genres")
                 .Result();

            var inputOptions = new TextListBuilder(addMovie, 13, 7)
                .SetItems("Name: ", "Duration: ", "Release Date: ", "Rating: ", "Genre: ", "Starring: ", "Language: ", "Company: ", "Description: " )
                .Result();

            var input = new TextListBuilder(addMovie, 14 + inputOptions.Items[8].Text.Length, 7)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "", "", "", "", "", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();

            var addButton = new TextListBuilder(addMovie, 13, 17)
                .Color(Colors.submit.Item1)
                .SetItems("Add")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
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
                int duration = 0;
                if (input[0].Value == "" || input[1].Value == "" || input[2].Value == "" || input[3].Value == "" || input[4].Value == "" || input[5].Value == "" || input[6].Value == "" || input[7].Value == "" || input[8].Value == "")
                    ErrorList.Add("Input fields may not be empty.");
                if (!Int32.TryParse(input[1].Value, out duration))
                    ErrorList.Add("Duration must be a number.");

                if (ErrorList.Count == 0)
                {
                   
                    var nMovie = new MovieAdd();
                    nMovie.id = JsonFile.FileAsList("..\\..\\..\\Movies.json")[(JsonFile.FileAsList("..\\..\\..\\Movies.json").Count - 1)].GetProperty("id").GetInt32() + 1;
                    nMovie.name = input[0].Value;
                    nMovie.duration = duration;
                    nMovie.releaseDate = input[2].Value;
                    nMovie.rating = input[3].Value;
                    nMovie.genres = input[4].Value.Split(", ");
                    nMovie.starring = input[5].Value.Split(", ");
                    nMovie.language = input[6].Value;
                    nMovie.company = input[7].Value;
                    nMovie.description = input[8].Value;
                    JsonFile.AppendToFile(nMovie, "..\\..\\..\\Movies.json");
                    message.Replace(
                        new TextListBuilder(addMovie, 1, 19)
                        .Color(ConsoleColor.Green)
                        .SetItems($"You have succesfully added {nMovie.name} to the list!", "If you want to add another Movie, fill in the above requirements again.")
                        .Result());
                    listOfFilms.Reset();
                    ListOfFilms();
                    listOfFilms.Init();
                    editMovieList.Reset();
                    EditMovies();
                    editMovieList.Init();
                }
                else
                {
                    
                    ErrorList.Insert(0, "Errors:");
                    message.Replace(
                        new TextListBuilder(addMovie, 1, 19)
                        .Color(ConsoleColor.Red)
                        .SetItems(ErrorList.ToArray())
                        .Result());
                }
            };
        }
    }
}
