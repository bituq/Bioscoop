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
            public DateTime Time { get; set; }
            public Hall Hall { get; set; }
            public List<Seat> occupiedSeats { get; set; } = new List<Seat>();
            public List<Window> reservationWindows { get; set; } = new List<Window>();
            public List<Seat> selectedSeats = new List<Seat>();
            double sum = 0;

            public TimeSlot(Movie Movie, int Time, Hall Hall, List<Seat> OccupiedSeats, int id)
            {
                this.occupiedSeats = OccupiedSeats;
                this.Movie = Movie;
                DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                this.Time = unix.AddSeconds(Time);
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
                .Color(Colors.title)
                .Text($"Available seats at {Time.ToString("g")}")
                .Result();

                var subtitle = new TextBuilder(this.Window, 1, 1)
                    .Color(Colors.title)
                    .Text("Movie: " + this.Movie.Name)
                    .Result();

                var subtitle2 = new TextBuilder(this.Window, 1, 2)
                    .Color(Colors.undertitle)
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
                var real = new List<List<int>>();
                var seatNumbers = new List<int>();

                for (int column = 0; column < this.Hall.Columns; column++)
                {
                    var temp = new List<int>();
                    string[] seats = new string[this.Hall.Rows];
                    for (int row = 0; row < this.Hall.Rows; row++)
                    {
                        int n = (column + 1);
                        temp.Add(n + (row * this.Hall.Columns));
                        seats[row] = "s" + n;
                    }

                    var _ = new TextListBuilder(this.Window, 9 + column * 4, 5)
                        .Color(ConsoleColor.DarkGreen)
                        .SetItems(seats)
                        .Selectable(Colors.backBg.Item1, Colors.backBg.Item2)
                        .LinkWindows()
                        .DisabledColor(ConsoleColor.DarkRed)
                        .Result();

                    for (int row = 0; row < _.Items.Count; row++)
                    {
                        if (occupiedSeats.Exists(seat => seat.Column - 1 == column && seat.Row - 1 == row))
                            _[row].Disable();
                        _[row].OnClickKeys.Add(ConsoleKey.Spacebar);
                    }

                    columns.Add(_);
                    real.Add(temp);
                }
                
                var goBack = new TextListBuilder(this.Window, 1, 7 + this.Hall.Rows)
                    .Color(Colors.back)
                    .SetItems("Go back", "Make reservation")
                    .Selectable(Colors.backBg.Item1, Colors.backBg.Item2)
                    .LinkWindows(Movie.timeSlotWindow)
                    .Result();

                var priceText = new TextListBuilder(this.Window, 1, 10 + this.Hall.Rows)
                    .Color(Colors.text)
                    .SetItems($"Price: ${sum}")
                    .Result();

                goBack[1].Disable();

                var dict = new Dictionary<SelectableText, int>();
                var hoverDict = new Dictionary<SelectableText, bool>();
                for (int col = 0; col < columns.Count; col++)
                    for (int row = 0; row < columns[col].Items.Count; row++)
                    {
                        var item = columns[col][row];
                        dict[item] = real[col][row] - 1;
                        hoverDict[item] = false;
                        item.OnClick = () =>
                        {
                            double price = 10.0;
                            Seat currentSeat = Hall.Seats[dict[item]];
                            if (!hoverDict[item] && !item.Disabled)
                            {
                                hoverDict[item] = true;
                                item.TextColor = ConsoleColor.Yellow;
                                selectedSeats.Add(currentSeat);
                                sum += price;
                            }
                            else if (hoverDict[item] && !item.Disabled)
                            {
                                hoverDict[item] = false;
                                item.TextColor = item._defaultTextColor;
                                selectedSeats.Remove(currentSeat);
                                sum -= price;
                            }
                            if (hoverDict.ContainsValue(true))
                            {
                                goBack[1].Enable();
                            }
                            else
                            {
                                goBack[1].Disable();
                            }
                            priceText.Replace(new TextListBuilder(this.Window, 1, 10 + this.Hall.Rows)
                                    .Color(Colors.text)
                                    .SetItems($"Price: ${sum}")
                                    .Result()
                            );
                        };
                    }

                goBack[1].OnClick = () =>
                {
                    var reservation = new Reservation(this, this.Window, selectedSeats, sum);
                    goBack[1].Referral = reservation.FoodWindow;
                    goBack[1].ActivateReferral();
                };
            }
        }
        public class SortTimeSlots : IComparer<TimeSlot>
        {
            public int Compare(TimeSlot x, TimeSlot y)
            {
                if (x.Time > y.Time)
                {
                    return 1;
                }
                else if (x.Time < y.Time)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public partial class Movie
        {
            public Window timeSlotWindow = new Window();
            public void TimeSlotScreen()
            {
                SortTimeSlots comparer = new SortTimeSlots();
                var validTimeSlots = timeSlots.FindAll(t => t.Movie == this);
                validTimeSlots.Sort(new SortTimeSlots());
                var validTimeSlotNames = new List<string>();
                var validTimeSlotWindows = new List<Window>();
                foreach (TimeSlot timeSlot in validTimeSlots)
                {
                    validTimeSlotNames.Add("Time: " + timeSlot.Time.ToString("g"));
                    validTimeSlotWindows.Add(timeSlot.Window);
                }

                var goBack = new TextListBuilder(timeSlotWindow, 1, 1)
                    .Color(Colors.back)
                    .SetItems("Go Back")
                    .Selectable(Colors.backBg.Item1, Colors.backBg.Item2)
                    .LinkWindows(Window)
                    .Result();

                var title = new TextBuilder(timeSlotWindow, 11, 1)
                    .Color(ConsoleColor.Red)
                    .Text("Available times for " + Name)
                    .Result();

                SelectableList listOfTimeslots;
                if (validTimeSlots.Count > 0)
                    listOfTimeslots = new TextListBuilder(timeSlotWindow, 11, 3)
                        .Color(ConsoleColor.White)
                        .SetItems(validTimeSlotNames.ToArray())
                        .Selectable(Colors.backBg.Item1, Colors.backBg.Item2)
                        .LinkWindows(validTimeSlotWindows.ToArray())
                        .Result();
            }
        }
    }
}
