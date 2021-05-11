using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    partial class Program
    {
        public class Reservation
        {
            public Window Window { get; set; } = new Window();
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int ReservationNumber { get; set; }
            public Hall Hall { get; set; }
            public TimeSlot TimeSlot { get; set; } // Wordt nog een object in de toekomst.
            public int DateOfReservation { get; set; }
            public Seat Seat { get; set; }

            public Reservation(TimeSlot TimeSlot, Window PreviousWindow, Seat Seat)
            {
                this.TimeSlot = TimeSlot;
                this.Seat = Seat;

                var goBack = new TextListBuilder(Window, 1, 1)
                    .Color(ConsoleColor.Yellow)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.White, ConsoleColor.DarkGreen)
                    .LinkWindows(PreviousWindow)
                    .Result();

                var title = new TextBuilder(Window, 11, 1)
                    .Color(ConsoleColor.Magenta)
                    .Text("Make a reservation for " + TimeSlot.Movie.Name)
                    .Result();

                var inputInformation = new TextListBuilder(Window, 11, 3)
                    .Color(ConsoleColor.DarkGray)
                    .SetItems("First name:", "Surname:", "Zaal: " + TimeSlot.Hall.Id, $"Seat: row {Seat.Row} seat {Seat.Column}")
                    .Result();

                var input = new TextListBuilder(Window, 24, 3)
                    .Color(ConsoleColor.White)
                    .SetItems("", "")
                    .AsInput(ConsoleColor.White, ConsoleColor.DarkGray)
                    .Result();
            }
        }
    }
}
