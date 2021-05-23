﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        public static Window editMovieList = new Window(true);
        static void EditMovies()
        {
            var Menu = new TextListBuilder(editMovieList, 2, 5)
                .Color(ConsoleColor.Yellow)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(AdminScherm)
                .Result();

            var Path = new TextBuilder(editMovieList, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/Movies/List/")
                .Result();

            var Title = new TextBuilder(editMovieList, 26, 2)
                .Color(ConsoleColor.Magenta)
                .Text("Select a movie to edit: ")
                .Result();

            var MovieList = JsonFile.FileAsList("..\\..\\..\\Movies.json");
            var TimeslotList = JsonFile.FileAsList("..\\..\\..\\TimeSlots.json");
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
                MovieNames.Add(MovieObjects[i].Name);
                TimeslotCount.Add(TimeslotList.FindAll(timeslot => timeslot.GetProperty("movieId").ToString() == Movie.GetProperty("id").ToString()).Count);
                MovieInfo.Add(new TextListBuilder(editMovieList, 26, 4 * (MovieInfo.Count + 1))
                    .Color(ConsoleColor.Red)
                    .SetItems("Title: " + MovieObjects[i].Name, "Timeslots: " + TimeslotCount[i])
                    .Result());
                MovieAction.Add(new TextListBuilder(editMovieList, 26, 4 * (MovieInfo.Count + 1) - 2)
                    .Color(ConsoleColor.White)
                    .SetItems("Edit timeslots")
                    .Selectable(ConsoleColor.DarkGreen, ConsoleColor.White)
                    .Result());
                MovieAction.Add(new TextListBuilder(editMovieList, 42, 4 * (MovieInfo.Count + 1) - 2)
                    .Color(ConsoleColor.White)
                    .SetItems("Remove Movie")
                    .Selectable(ConsoleColor.DarkGreen, ConsoleColor.White)
                    .Result());
            }
        }
    }
}
