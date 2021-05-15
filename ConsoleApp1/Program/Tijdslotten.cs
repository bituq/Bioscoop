using System;
using System.Collections.Generic;
using System.Text.Json;
using JsonHandler;
using CinemaUI;
using CinemaUI.Builder;
using System.IO;

namespace CinemaApplication
{
    partial class Program
    {
        static List<JsonElement> timeslotsFile = JsonFile.FileAsList("..\\..\\..\\TimeSlots.json");
        static List<JsonElement> hallsFile = JsonFile.FileAsList("..\\..\\..\\Halls.json");
        static List<JsonElement> moviesFile = JsonFile.FileAsList("..\\..\\..\\Movies.json");
        static List<TimeSlot> timeSlots = new List<TimeSlot>();
        public class TimeSlot
        {
            public class Information
            {
                public int id { get; set; }
                public int movieId { get; set; }
                public int time { get; set; }
                public int hall { get; set; }
                public JsonElement[] occupiedSeats { get; set; }
            }
            public Window Window { get; set; } = new Window();
            public Movie Movie { get; set; }
            public int id { get; set; }
            public int Time { get; set; }
            public Hall Hall { get; set; }
            public List<Seat> occupiedSeats { get; set; } = new List<Seat>();
            public List<Window> reservationWindows { get; set; } = new List<Window>();
            public List<Seat> selectedSeats = new List<Seat>();

            public TimeSlot(Movie Movie, int Time, Hall Hall, List<Seat> OccupiedSeats, int id)
            {
                this.occupiedSeats = OccupiedSeats;
                this.Movie = Movie;
                this.Time = Time;
                this.Hall = Hall;
                this.id = id;
                Init();
            }

            public void Init()
            {
                var file = File.ReadAllText("..\\..\\..\\TimeSlots.json");
                JsonDocument doc = JsonDocument.Parse(file);
                JsonElement root = doc.RootElement;

                occupiedSeats = new List<Seat>();
                foreach (JsonElement timeslot in root.EnumerateArray())
                    if (timeslot.GetProperty("id").GetInt32() == id)
                        foreach (JsonElement seat in timeslot.GetProperty("occupiedSeats").EnumerateArray())
                            occupiedSeats.Add(new Seat(seat.GetProperty("row").GetInt32(), seat.GetProperty("column").GetInt32()));

                var title = new TextBuilder(this.Window, 1, 0)
                .Color(ConsoleColor.Magenta)
                .Text($"Available seats at {this.Time}")
                .Result();

                var subtitle = new TextBuilder(this.Window, 1, 1)
                    .Color(ConsoleColor.DarkMagenta)
                    .Text("Movie: " + this.Movie.Name)
                    .Result();

                var subtitle2 = new TextBuilder(this.Window, 1, 2)
                    .Color(ConsoleColor.DarkMagenta)
                    .Text("Hall: " + this.Hall.Id)
                    .Result();

                string[] rows = new string[this.Hall.Rows];
                for (int i = 0; i < rows.Length; i++)
                    rows[i] = $"Row {i + 1} - ";

                var rowList = new TextListBuilder(this.Window, 1, 5)
                    .Color(ConsoleColor.DarkGray)
                    .SetItems(rows)
                    .Result();

                var columns = new List<SelectableList>();

                for (int column = 0; column < this.Hall.Columns; column++)
                {
                    string[] seats = new string[this.Hall.Rows];
                    for (int row = 0; row < this.Hall.Rows; row++)
                    {
                        int n = (column + 1) + (row * this.Hall.Columns);
                        seats[row] = "s" + n;
                    }

                    var _ = new TextListBuilder(this.Window, 9 + column * 4, 5)
                        .Color(ConsoleColor.DarkGreen)
                        .SetItems(seats)
                        .Selectable(ConsoleColor.White, ConsoleColor.DarkGray)
                        .LinkWindows() //reservationWindows.ToArray()
                        .DisabledColor(ConsoleColor.DarkRed)
                        .Result();

                    for (int row = 0; row < _.Items.Count; row++)
                        if (occupiedSeats.Exists(seat => seat.Column - 1 == column && seat.Row - 1 == row))
                            _[row].Disable();

                    columns.Add(_);
                }

                var goBack = new TextListBuilder(this.Window, 1, 7 + this.Hall.Rows)
                    .Color(ConsoleColor.Yellow)
                    .SetItems("Go back", "Make reservation")
                    .Selectable(ConsoleColor.White, ConsoleColor.DarkGreen)
                    .LinkWindows(Movie.timeSlotWindow)
                    .Result();

                goBack[1].Disable();

                var hoverDict = new Dictionary<SelectableText, bool>();
                for (int col = 0; col < columns.Count; col++)
                    for (int row = 0; row < columns[col].Items.Count; row++)
                    {
                        var item = columns[col][row];
                        hoverDict[item] = false;
                        item.OnClick = () =>
                        {
                            Seat currentSeat = Hall.Seats[Int32.Parse(item.Text.Remove(0, 1)) - 1];
                            if (!hoverDict[item] && !item.Disabled)
                            {
                                hoverDict[item] = true;
                                item.TextColor = ConsoleColor.Yellow;
                                selectedSeats.Add(currentSeat);
                            }
                            else if (hoverDict[item] && !item.Disabled)
                            {
                                hoverDict[item] = false;
                                item.TextColor = item._defaultTextColor;
                                selectedSeats.Remove(currentSeat);
                            }
                            if (hoverDict.ContainsValue(true))
                            {
                                goBack[1].Enable();
                            }
                            else
                            {
                                goBack[1].Disable();
                            }
                        };
                    }

                goBack[1].OnClick = () =>
                {
                    var reservation = new Reservation(this, this.Window, selectedSeats);
                    goBack[1].Referral = reservation.Window;
                    goBack[1].ActivateReferral();
                };
            }
        }
        public partial class Movie
        {
            public Window timeSlotWindow = new Window();
            public void TimeSlotScreen()
            {
                var validTimeSlots = timeSlots.FindAll(t => t.Movie == this);
                var validTimeSlotNames = new List<string>();
                var validTimeSlotWindows = new List<Window>();
                foreach (TimeSlot timeSlot in validTimeSlots)
                {
                    validTimeSlotNames.Add("Time: " + timeSlot.Time);
                    validTimeSlotWindows.Add(timeSlot.Window);
                }

                var goBack = new TextListBuilder(timeSlotWindow, 1, 1)
                    .Color(ConsoleColor.Yellow)
                    .SetItems("Go Back")
                    .Selectable(ConsoleColor.White, ConsoleColor.DarkGray)
                    .LinkWindows(Window)
                    .Result();

                var title = new TextBuilder(timeSlotWindow, 11, 1)
                    .Color(ConsoleColor.Magenta)
                    .Text("Available times for " + Name)
                    .Result();

                SelectableList listOfTimeslots;
                if (validTimeSlots.Count > 0)
                    listOfTimeslots = new TextListBuilder(timeSlotWindow, 11, 3)
                        .Color(ConsoleColor.White)
                        .SetItems(validTimeSlotNames.ToArray())
                        .UseNumbers()
                        .Selectable(ConsoleColor.Black, ConsoleColor.White)
                        .LinkWindows(validTimeSlotWindows.ToArray())
                        .Result();
            }
        }
    }
}
