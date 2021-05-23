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
                    this.Genres = genres; // hier ook
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
                    root.RemoveAll(x => !x.GetProperty("genres").ToString().Contains(this.Genres[i]));
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
            public string Name { get; set; }
            public List<string> Starring { get; set; } = new List<string>();
            public List<string> Genres { get; set; } = new List<string>();

            public Movie(string name, string duration, string releaseDate, string descriptionText, string rating, string language, string company, JsonElement genres, JsonElement starring)
            {
                Name = name;

                foreach (JsonElement genre in genres.EnumerateArray())
                    Genres.Add(genre.ToString());
                foreach (JsonElement star in starring.EnumerateArray())
                    Starring.Add(star.ToString());

                var title = new TextBuilder(Window, 18, 1)
                    .Color(ConsoleColor.Red)
                    .Text(name)
                    .Result();

                var description = new TextBuilder(Window, 18, 2)
                    .Color(ConsoleColor.DarkGray)
                    .Text(descriptionText)
                    .Result();

                var information = new TextListBuilder(Window, 18, 8)
                    .Color(ConsoleColor.White)
                    .SetItems(duration, releaseDate, rating, language, company, "Genres: ")
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

            // stukje filter


            Filter filter = new Filter("", new string[] { "" }, new DateTime());
            var root = filter.FilterRoot();

            for (int i = 0; i < root.Count; i++) // JsonRoot.GetArrayLength()
            {
                var timeSlotsOfMovie = timeslotsFile.FindAll(timeSlots => timeSlots.GetProperty("movieId").GetInt32() == root[i].GetProperty("id").GetInt32());
                if (timeSlotsOfMovie.Count > 0)
                {
                    movieObjects.Add(new Movie(
                        root[i].GetProperty("name").ToString(),
                        $"Duration: {root[i].GetProperty("duration")} minutes",
                        $"Release Date: {root[i].GetProperty("releaseDate")}",
                        $"   {root[i].GetProperty("description")}",
                        $"Rating: {root[i].GetProperty("rating")}",
                        $"Language: {root[i].GetProperty("language")}",
                        $"Company: {root[i].GetProperty("company") }",
                        root[i].GetProperty("genres"),
                        root[i].GetProperty("starring")
                    ));

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