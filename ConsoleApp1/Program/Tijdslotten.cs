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
        public class TimeSlot
        {
            public Window Window { get; set; } = new Window();
            public string Movie { get; set; } // Wordt uiteindelijk een Movie object.
            public int Time { get; set; }
            public Hall Hall { get; set; }
            public List<Seat> occupiedSeats { get; set; } = new List<Seat>();

            public TimeSlot(string Movie, int Time, Hall Hall)
            {
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
                    .Text("Film: " + this.Movie)
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
                    {
                        seats[row] = $"s{(column + 1) + (row * this.Hall.Columns)}";
                    }
                    var _ = new TextListBuilder(this.Window, 9 + column * 4, 5)
                        .Color(ConsoleColor.White)
                        .SetItems(seats)
                        .Selectable(ConsoleColor.Black, ConsoleColor.White)
                        .Result();
                }

                var goBack = new TextListBuilder(this.Window, 1, 7 + this.Hall.Rows)
                    .Color(ConsoleColor.Green)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.White, ConsoleColor.DarkGreen)
                    .LinkWindows(timeSlotWindow)
                    .Result();
            }
        }

        static List<JsonElement> timeslotsFile = JsonFile.FileAsList("..\\..\\..\\TimeSlots.json");
        static List<JsonElement> hallsFile = JsonFile.FileAsList("..\\..\\..\\Halls.json");
        static List<JsonElement> moviesFile = JsonFile.FileAsList("..\\..\\..\\Movies.json");
        static List<TimeSlot> timeSlots = new List<TimeSlot>();

        static Window timeSlotWindow = new Window(true);
        static void TimeSlotScreen()
        {
            foreach (JsonElement timeSlot in timeslotsFile)
            {
                var hallElement = hallsFile.Find(hall => hall.GetProperty("id").GetInt32() == timeSlot.GetProperty("hall").GetInt32());
                var hall = new Hall(hallElement.GetProperty("id").GetInt32(), hallElement.GetProperty("rows").GetInt32(), hallElement.GetProperty("columns").GetInt32());
                timeSlots.Add(new TimeSlot(timeSlot.GetProperty("movie").GetString(), timeSlot.GetProperty("time").GetInt32(), hall));
            }

            var current = moviesFile[0].GetProperty("name").GetString();
            var validTimeSlots = timeSlots.FindAll(t => t.Movie == current);
            var validTimeSlotNames = new List<string>();
            var validTimeSlotWindows = new List<Window>();
            foreach (TimeSlot timeSlot in validTimeSlots)
                if (timeSlot.Movie == current)
                {
                    validTimeSlotNames.Add("Tijd: " + timeSlot.Time);
                    validTimeSlotWindows.Add(timeSlot.Window);
                }

            var title = new TextBuilder(timeSlotWindow, 1, 1)
                .Color(ConsoleColor.Magenta)
                .Text("Beschikbare tijdslotten voor " + current)
                .Result();

            var listOfTimeslots = new TextListBuilder(timeSlotWindow, 1, 3)
                .Color(ConsoleColor.White)
                .SetItems(validTimeSlotNames.ToArray())
                .UseNumbers()
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(validTimeSlotWindows.ToArray())
                .Result();
        }
    }
}
