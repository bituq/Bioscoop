using System;
using System.Collections.Generic;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;
using System.Text.Json;

namespace CinemaApplication
{
    partial class Program
    {
        public struct Seat
        {
            public int Row;
            public int Column;

            public Seat(int row, int column)
            {
                Row = row;
                Column = column;
            }
        }

        public class Hall
        {
            public int Id { get; set; }
            public int Rows { get; set; }
            public int Columns { get; set; }
            public List<Seat> Seats { get; set; } = new List<Seat>();

            public Hall(int Id, int Rows, int Columns)
            {
                this.Id = Id;
                this.Rows = Rows;
                this.Columns = Columns;
                for (int row = 1; row <= Rows; row++)
                    for (int column = 1; column <= Columns; column++)
                        this.Seats.Add(new Seat(row, column));
            }
        }

        public class TimeSlot
        {
            public string Movie { get; set; } // Wordt uiteindelijk een Movie object.
            public int Time { get; set; }
            public Hall Hall { get; set; }
            public List<Seat> occupiedSeats { get; set; } = new List<Seat>();

            public TimeSlot(string Movie, int Time, Hall Hall)
            {
                this.Movie = Movie;
                this.Time = Time;
                this.Hall = Hall;
            }
        }

        static Window seatWindow = new Window(true);
        static void SeatScreen()
        {
            var timeslotsFile = JsonFile.FileAsList("..\\..\\..\\TimeSlots.json");
            var hallsFile = JsonFile.FileAsList("..\\..\\..\\Halls.json");

            var timeSlots = new List<TimeSlot>();

            foreach (JsonElement timeSlot in timeslotsFile)
            {
                var hallElement = hallsFile.Find(hall => hall.GetProperty("id").GetInt32() == timeSlot.GetProperty("hall").GetInt32());
                var hall = new Hall(hallElement.GetProperty("id").GetInt32(), hallElement.GetProperty("rows").GetInt32(), hallElement.GetProperty("columns").GetInt32());
                timeSlots.Add(new TimeSlot(timeSlot.GetProperty("movie").GetString(), timeSlot.GetProperty("time").GetInt32(), hall));
            }

            var title = new TextBuilder(seatWindow, 1, 1)
                .Color(ConsoleColor.Magenta);
        }
    }
}
