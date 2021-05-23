﻿using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{

    partial class Program
    {
        public partial class Movie
        {
            public int Id { get; set; }
            public Window Window = new Window();
            public Window TimeslotEditWindow = new Window();
            public string Name { get; set; }
            public string Description { get; set; }
            public string Rating { get; set; }
            public int Duration { get; set; }
            public string ReleaseDate { get; set; }
            public string Language { get; set; }
            public string Company { get; set; }
            public List<string> Starring { get; set; } = new List<string>();
            public List<string> Genres { get; set; } = new List<string>();

            public Movie(string name, int duration, string releaseDate, string descriptionText, string rating, string language, string company, JsonElement genres, JsonElement starring)
            {
                Name = name;
                Duration = duration;
                ReleaseDate = releaseDate;
                Description = descriptionText;
                Rating = rating;
                Language = language;
                Company = company;

                foreach (JsonElement genre in genres.EnumerateArray())
                    Genres.Add(genre.ToString());
                foreach (JsonElement star in starring.EnumerateArray())
                    Starring.Add(star.ToString());
            }

            public void InitAdminTimeslot()
            {
                var Menu = new TextListBuilder(TimeslotEditWindow, 2, 5)
                .Color(ConsoleColor.Red)
                .SetItems("Add a timeslot", "Go back")
                .UseNumbers()
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(null, editMovieList)
                .Result();

                var Path = new TextBuilder(TimeslotEditWindow, 2, 2)
                    .Color(ConsoleColor.Cyan)
                    .Text("Home/Admin/Movies/List/Timeslot")
                    .Result();

                var Description = new TextBuilder(TimeslotEditWindow, 2, 3)
                    .Color(ConsoleColor.Gray)
                    .Text(Name)
                    .Result();

                var Title = new TextBuilder(TimeslotEditWindow, Description.Position.X + Math.Max(Name.Length, Path.Text.Length) + 3, 3)
                    .Color(ConsoleColor.Magenta)
                    .Text("Timeslots: ")
                    .Result();

                var TimeSlotList = JsonFile.FileAsList("..\\..\\..\\TimeSlots.json").FindAll(n => n.GetProperty("movieId").GetInt32() == Id);
                var TimeSlotNames = new List<string>();
                var TimeSlotDates = new List<DateTime>();
                int MaxLength = 0;

                foreach (JsonElement timeslot in TimeSlotList)
                {
                    DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    DateTime time = unix.AddSeconds(timeslot.GetProperty("time").GetInt32());
                    int hall = timeslot.GetProperty("hall").GetInt32();
                    string text = $"{time.ToString("g")} in hall {hall}";
                    TimeSlotNames.Add(text);
                    TimeSlotDates.Add(time);
                    MaxLength = Math.Max(MaxLength, text.Length);
                }

                var TimeSlots = new TextListBuilder(TimeslotEditWindow, Title.Position.X, 5)
                    .Color(ConsoleColor.White)
                    .SetItems(TimeSlotNames.ToArray())
                    .Result();

                var RemoveButtonList = new List<string>();

                void ChangeColors()
                {
                    for (int i = 0; i < TimeSlots.Items.Count; i++)
                        if (DateTime.Now < TimeSlotDates[i].AddMinutes(Duration) && DateTime.Now >= TimeSlotDates[i])
                        {
                            TimeSlots.Items[i].TextColor = ConsoleColor.Green;
                            RemoveButtonList.Add("");
                        }
                        else
                        {
                            RemoveButtonList.Add("Remove");
                            if (DateTime.Now > TimeSlotDates[i].AddMinutes(Duration))
                                TimeSlots.Items[i].TextColor = ConsoleColor.DarkGray;
                        }
                }
                ChangeColors();

                if (RemoveButtonList.Count == 0)
                    RemoveButtonList.Add("");

                var RemoveButtons = new TextListBuilder(TimeslotEditWindow, Title.Position.X + MaxLength + 1, 5)
                    .Color(ConsoleColor.Red)
                    .SetItems(RemoveButtonList.ToArray())
                    .Selectable(ConsoleColor.White, ConsoleColor.Red)
                    .Result();

                void InitRemoveButtons()
                {
                    foreach (SelectableText button in RemoveButtons.Items)
                    {
                        void OnRemove()
                        {
                            int index = RemoveButtons.Items.IndexOf(button);
                            RemoveButtonList.RemoveAt(index);
                            TimeSlotDates.RemoveAt(index);
                            TimeSlotNames.RemoveAt(index);
                            RemoveButtons.Replace(new TextListBuilder(TimeslotEditWindow, Title.Position.X + MaxLength + 1, 5)
                                .Color(ConsoleColor.Red)
                                .SetItems(RemoveButtonList.ToArray())
                                .Selectable(ConsoleColor.White, ConsoleColor.Red)
                                .Result());
                            TimeSlots.Replace(new TextListBuilder(TimeslotEditWindow, Title.Position.X, 5)
                                .Color(ConsoleColor.White)
                                .SetItems(TimeSlotNames.ToArray())
                                .Result());
                            InitRemoveButtons();
                        }
                        button.OnClick = () => OnRemove();
                    }
                }
                InitRemoveButtons();
            }

            public void InitVisitor()
            {
                var title = new TextBuilder(Window, 18, 1)
                    .Color(ConsoleColor.Red)
                    .Text(Name)
                    .Result();

                var description = new TextBuilder(Window, 18, 2)
                    .Color(ConsoleColor.DarkGray)
                    .Text(Description)
                    .Result();

                var information = new TextListBuilder(Window, 18, 8)
                    .Color(ConsoleColor.White)
                    .SetItems("Duration: " + Duration, "Release date: " + ReleaseDate, "Rating: " + Rating, "Language: " + Language, "Company: " + Company, "Genres: ")
                    .Result();

                var genresInformation = new TextListBuilder(Window, 18 + information.Items[5].Text.Length, 13)
                    .Color(ConsoleColor.White)
                    .SetItems(Genres.ToArray())
                    .Result();

                var information2 = new TextBuilder(Window, 18, 13 + Genres.Count)
                    .Color(ConsoleColor.White)
                    .Text("Starring: ")
                    .Result();

                var starringInformation = new TextListBuilder(Window, 18 + information2.Text.Length, information2.Position.Y)
                    .Color(ConsoleColor.White)
                    .SetItems(Starring.ToArray())
                    .Result();

                var _ = new TextListBuilder(Window, 1, 1)
                    .Color(ConsoleColor.Yellow)
                    .SetItems("Go back", "Make reservation")
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .LinkWindows(listOfFilms, timeSlotWindow)
                    .Result();
            }
        }
        public static Window listOfFilms = new Window();
        static void ListOfFilms()
        {

            var _ = new TextListBuilder(listOfFilms, 1, 1)
                   .Color(ConsoleColor.Yellow)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                   .LinkWindows(mainMenu)
                   .Result();

            var movies = File.ReadAllText("..\\..\\..\\Movies.json");

            JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;

            Console.ForegroundColor
             = ConsoleColor.DarkMagenta;

            var movieObjects = new List<Movie>();
            var movieWindows = new List<Window>();
            var movieNames = new List<String>();
            for (int i = 0; i < moviesFile.Count; i++)
            {
                var timeSlotsOfMovie = timeslotsFile.FindAll(timeSlots => timeSlots.GetProperty("movieId").GetInt32() == root[i].GetProperty("id").GetInt32());
                if (timeSlotsOfMovie.Count > 0)
                {
                    movieObjects.Add(new Movie(
                        root[i].GetProperty("name").ToString(),
                        root[i].GetProperty("duration").GetInt32(),
                        root[i].GetProperty("releaseDate").ToString(),
                        root[i].GetProperty("description").ToString(),
                        root[i].GetProperty("rating").ToString(),
                        root[i].GetProperty("language").ToString(),
                        root[i].GetProperty("company").ToString(),
                        root[i].GetProperty("genres"),
                        root[i].GetProperty("starring")
                        ));
                    movieObjects[i].InitVisitor();
                    movieObjects[i].Id = root[i].GetProperty("id").GetInt32();
                    movieWindows.Add(movieObjects[i].Window);
                    movieNames.Add(movieObjects[i].Name);
                    foreach (JsonElement timeSlot in timeSlotsOfMovie)
                    {
                        var hallElement = hallsFile.Find(hall => hall.GetProperty("id").GetInt32() == timeSlot.GetProperty("hall").GetInt32());
                        var hall = new Hall(hallElement.GetProperty("id").GetInt32(), hallElement.GetProperty("rows").GetInt32(), hallElement.GetProperty("columns").GetInt32());
                        var occupiedSeats = new List<Seat>();
                        foreach (JsonElement seat in timeSlot.GetProperty("occupiedSeats").EnumerateArray())
                            occupiedSeats.Add(new Seat(seat.GetProperty("row").GetInt32(), seat.GetProperty("column").GetInt32()));
                        timeSlots.Add(new TimeSlot(movieObjects[i], timeSlot.GetProperty("time").GetInt32(), hall, occupiedSeats, timeSlot.GetProperty("id").GetInt32()));
                    }
                    movieObjects[i].TimeSlotScreen();
                }
            }

            var movieListTitle = new TextBuilder(listOfFilms, 11, 1)
                .Color(ConsoleColor.Magenta)
                .Text("Available movies:")
                .Result();

            var movieList = new TextListBuilder(listOfFilms, 11, 3)
                .Color(ConsoleColor.White)
                .SetItems(movieNames.ToArray())
                .UseNumbers()
                .Selectable(ConsoleColor.White, ConsoleColor.DarkGray)
                .LinkWindows(movieWindows.ToArray())
                .Result();
        }
    }

}