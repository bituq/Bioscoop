using System;
using System.Collections.Generic;
using System.Text.Json;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        public static Window adminMovieWindow = new Window();
        static void AdminMovieMenu()
        {
            var Menu = new TextListBuilder(adminMovieWindow, 2, 5)
                .Color(Colors.selection)
                .SetItems("Add Movie","Remove Movie", "Edit Movies/Timeslots", "Go Back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(addMovie, removeMovie, editMovieList, AdminScherm)
                .Result();

            var Path = new TextBuilder(adminMovieWindow, 2, 2)
                .Color(Colors.breadcrumbs)
                .Text("Home/Admin/Movie Options/")
                .Result();
        }
        public static Window editMovieList = new Window();
        static void EditMovies()
        {
            var Menu = new TextListBuilder(editMovieList, 2, 5)
                .Color(Colors.back)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(adminMovieWindow)
                .Result();

            var Path = new TextBuilder(editMovieList, 2, 2)
                .Color(Colors.breadcrumbs)
                .Text("Home/Admin/Movies/List/")
                .Result();

            var Title = new TextBuilder(editMovieList, 26, 2)
                .Color(Colors.text)
                .Text("Select a movie to edit: ")
                .Result();

            var MovieList = JsonFile.FileAsList("../../../Movies.json");
            var TimeslotList = JsonFile.FileAsList("../../../TimeSlots.json");
            var MovieObjects = new List<Movie>();
            var MovieNames = new List<string>();
            var TimeslotCount = new List<int>();
            var MovieInfo = new List<TextList>();
            var MovieAction = new List<SelectableList>();
            for(int i = 0; i < MovieList.Count; i++)
            {
                JsonElement Movie = MovieList[i];
                MovieObjects.Add(new Movie(
                        Movie.GetProperty("name").ToString(),
                        Movie.GetProperty("duration").GetInt32(),
                        Movie.GetProperty("releaseDate").ToString(),
                        Movie.GetProperty("description").ToString(),
                        Movie.GetProperty("rating").ToString(),
                        Movie.GetProperty("language").ToString(),
                        Movie.GetProperty("company").ToString(),
                        Movie.GetProperty("genres"),
                        Movie.GetProperty("starring")
                        ));
                MovieObjects[i].Id = Movie.GetProperty("id").GetInt32();
                MovieObjects[i].InitAdminTimeslot();
                MovieNames.Add(MovieObjects[i].Name);
                TimeslotCount.Add(TimeslotList.FindAll(timeslot => timeslot.GetProperty("movieId").ToString() == Movie.GetProperty("id").ToString()).Count);
                MovieInfo.Add(new TextListBuilder(editMovieList, 26, 4 * (MovieInfo.Count + 1))
                    .Color(Colors.text)
                    .SetItems("Title: " + MovieObjects[i].Name, "Timeslots: " + TimeslotCount[i])
                    .Result());
                MovieAction.Add(new TextListBuilder(editMovieList, 26, 4 * (MovieInfo.Count + 1) - 2)
                    .Color(Colors.selection)
                    .SetItems("Edit timeslots")
                    .Selectable(ConsoleColor.DarkGreen, ConsoleColor.White)
                    .LinkWindows(MovieObjects[i].TimeslotEditWindow)
                    .Result());
            }
        }
    }
}
