using System;
using System.Collections.Generic;
using System.Text.Json;
using JsonHandler;
using CinemaUI;
using CinemaUI.Builder;

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
            public Window Window { get; set; } = new Window();
            public Movie Movie { get; set; }
            public int Time { get; set; }
            public Hall Hall { get; set; }
            public List<Seat> occupiedSeats { get; set; } = new List<Seat>();

            public TimeSlot(Movie Movie, int Time, Hall Hall, List<Seat> OccupiedSeats)
            {
                this.occupiedSeats = OccupiedSeats;
                this.Movie = Movie;
                this.Time = Time;
                this.Hall = Hall;
                Init();
            }

            public void Init()
            {
                var title = new TextBuilder(this.Window, 1, 0)
                .Color(ConsoleColor.Magenta)
                .Text($"Beschikbare stoelen op {this.Time}")
                .Result();

                var subtitle = new TextBuilder(this.Window, 1, 1)
                    .Color(ConsoleColor.DarkMagenta)
                    .Text("Film: " + this.Movie.Name)
                    .Result();

                var subtitle2 = new TextBuilder(this.Window, 1, 2)
                    .Color(ConsoleColor.DarkMagenta)
                    .Text("Zaal: " + this.Hall.Id)
                    .Result();

                string[] rows = new string[this.Hall.Rows];
                for (int i = 0; i < rows.Length; i++)
                    rows[i] = $"Rij {i + 1} - ";

                var rowList = new TextListBuilder(this.Window, 1, 5)
                    .Color(ConsoleColor.DarkGray)
                    .SetItems(rows)
                    .Result();

                for (int column = 0; column < this.Hall.Columns; column++)
                {
                    string[] seats = new string[this.Hall.Rows];
                    for (int row = 0; row < this.Hall.Rows; row++)
                        seats[row] = $"s{(column + 1) + (row * this.Hall.Columns)}";

                    var reservations = new List<Reservation>();
                    var reservationWindows = new List<Window>();
                    for (int i = 0; i < seats.Length; i++)
                    {
                        reservations.Add(new Reservation(this, Window, Hall.Seats.Find(s => s.Column == column + 1 && s.Row == i + 1)));
                        reservationWindows.Add(reservations[i].Window);
                    }

                    var _ = new TextListBuilder(this.Window, 9 + column * 4, 5)
                        .Color(ConsoleColor.DarkGreen)
                        .SetItems(seats)
                        .Selectable(ConsoleColor.White, ConsoleColor.DarkGray)
                        .LinkWindows(reservationWindows.ToArray())
                        .DisabledColor(ConsoleColor.DarkRed)
                        .Result();

                    for (int row = 0; row < _.Items.Count; row++)
                        if (occupiedSeats.Exists(seat => seat.Column == column && seat.Row == row))
                            _[row].Disable();
                }

                var goBack = new TextListBuilder(this.Window, 1, 7 + this.Hall.Rows)
                    .Color(ConsoleColor.Cyan)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.White, ConsoleColor.DarkGreen)
                    .LinkWindows(Movie.timeSlotWindow)
                    .Result();
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
                    validTimeSlotNames.Add("Tijd: " + timeSlot.Time);
                    validTimeSlotWindows.Add(timeSlot.Window);
                }

                var title = new TextBuilder(timeSlotWindow, 1, 1)
                    .Color(ConsoleColor.Magenta)
                    .Text("Beschikbare tijdslotten voor " + Name)
                    .Result();

                SelectableList listOfTimeslots;
                if (validTimeSlots.Count > 0)
                    listOfTimeslots = new TextListBuilder(timeSlotWindow, 1, 3)
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
