using System;
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
        public class Filter
        {
            public string Name = "";
            public string[] Genres = new string[] { "" };
            public DateTime Date;

            static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
            {
                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                return dtDateTime;
            }

            public Filter(string name, string[] genres, DateTime date)
            {
                if (name != null)
                    this.Name = name.ToLower();
                if (genres != null)
                    for (int i = 0; i < genres.Length; i++)
                    {
                        genres[i] = genres[i].ToLower();
                    }
                this.Genres = genres;
                if (date != null)
                    this.Date = date;
            }

            public List<JsonElement> FilterRoot()
            {

                List<JsonElement> TimeSlotsList = JsonFile.FileAsList("..\\..\\..\\TimeSlots.json");
                List<JsonElement> root = JsonFile.FileAsList("..\\..\\..\\Movies.json");

                // filter name
                root.RemoveAll(x => !x.GetProperty("name").ToString().ToLower().Contains(this.Name));

                // filter genre
                for (int i = 0; i < this.Genres.Length; i++)
                {
                    root.RemoveAll(x => !x.GetProperty("genres").ToString().ToLower().Contains(this.Genres[i].ToLower()));
                }

                // filter time
                if (this.Date != new DateTime())
                {
                    List<JsonElement> toBeRemoved = new List<JsonElement>();
                    foreach (var movie in root) // for every movie
                    {
                        int id = movie.GetProperty("id").GetInt32();
                        List<JsonElement> TimeSlotsForCurrentMovie = TimeSlotsList.FindAll(x => x.GetProperty("movieId").GetInt32() == id);

                        bool isValid = false; // has no matching timeslot

                        foreach (var timeslot in TimeSlotsForCurrentMovie)
                        {
                            var TimeSlotDate = UnixTimeStampToDateTime(timeslot.GetProperty("time").GetInt32());
                            if (TimeSlotDate > this.Date && TimeSlotDate < this.Date.AddDays(1))
                                isValid = true;
                        }
                        if (!isValid)
                            toBeRemoved.Add(movie);
                    }
                    foreach (var item in toBeRemoved)
                        root.Remove(item);
                }

                // return modified movielist
                return root;
            }
        }

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
            JsonElement JsonRoot = doc.RootElement;

            Console.ForegroundColor = ConsoleColor.DarkMagenta;

            var movieObjects = new List<Movie>();
            var movieWindows = new List<Window>();
            var movieNames = new List<String>();
            var timeslotCounts = new List<int>();

            // stukje filter
            Filter filter = new Filter("", new string[] { "" }, new DateTime());
            var root = filter.FilterRoot();

            void GenerateMovieInformation()
            {
                movieObjects.Clear();
                movieWindows.Clear();
                movieNames.Clear();

                for (int i = 0; i < root.Count; i++) // JsonRoot.GetArrayLength()
                {
                    var timeSlotsOfMovie = timeslotsFile.FindAll(timeSlots => timeSlots.GetProperty("movieId").GetInt32() == root[i].GetProperty("id").GetInt32());
                    if (timeSlotsOfMovie.Count >= 0) // terug zetten voor eind build
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
                        movieNames.Add($"{i+1}. " + movieObjects[i].Name);
                        timeslotCounts.Add(timeSlotsOfMovie.Count);

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
            }
            GenerateMovieInformation();

            var movieListTitle = new TextBuilder(listOfFilms, 11, 1)
                .Color(ConsoleColor.Magenta)
                .Text("Available movies:")
                .Result();

            var movieList = new TextListBuilder(listOfFilms, 11, 3)
                .Color(ConsoleColor.White)
                .SetItems(movieNames.ToArray())
                .Selectable(ConsoleColor.White, ConsoleColor.DarkGray)
                .LinkWindows(movieWindows.ToArray())
                .Result();

            var filterInputs = new TextListBuilder(listOfFilms, 80, 3)
                .SetItems("Name: ", "Genre: ", "Date: ")
                .Result();

            var input = new TextListBuilder(listOfFilms, 80 + filterInputs.Items[2].Text.Length, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("", "", "")
                .AsInput(ConsoleColor.Gray, ConsoleColor.Black)
                .Result();

            var submitButton = new TextListBuilder(listOfFilms, 80, 8)
                .Color(ConsoleColor.Green)
                .SetItems("Submit")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .Result();

            var message = new TextListBuilder(listOfFilms)
                .SetItems()
                .Result();

            submitButton[0].OnClick = () =>
            {
                DateTime unUsed;

                if (!DateTime.TryParse(input[2].Value, out unUsed))
                {
                    Filter filter = new Filter(input[0].Value, input[1].Value.Split(' '), new DateTime());
                    root = filter.FilterRoot();
                }
                else
                {
                    Filter filter = new Filter(input[0].Value, input[1].Value.Split(' '), DateTime.Parse(input[2].Value));
                    root = filter.FilterRoot();
                }
                GenerateMovieInformation();
                movieList.Replace(new TextListBuilder(listOfFilms, 11, 3)
                    .Color(ConsoleColor.White)
                    .SetItems(movieNames.ToArray())
                    .Selectable(ConsoleColor.White, ConsoleColor.DarkGray)
                    .LinkWindows(movieWindows.ToArray())
                    .Result()
                );
                listOfFilms.Init();
            };
        }
    }

}