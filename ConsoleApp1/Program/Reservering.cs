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

                var title = new TextBuilder(Window, 1, 1)
                    .Color(ConsoleColor.Magenta)
                    .Text("Maak een reservering voor " + TimeSlot.Movie)
                    .Result();

                var inputInformation = new TextListBuilder(Window, 1, 3)
                    .Color(ConsoleColor.DarkGray)
                    .SetItems("Voornaam:", "Achternaam:", "Zaal: " + TimeSlot.Hall.Id, $"Stoel: rij {Seat.Row} kolom {Seat.Column}")
                    .Result();

                var input = new TextListBuilder(Window, 13, 3)
                    .Color(ConsoleColor.White)
                    .SetItems("", "")
                    .AsInput(ConsoleColor.White, ConsoleColor.DarkGray)
                    .Result();

                var goBack = new TextListBuilder(Window, 1, 10)
                    .Color(ConsoleColor.Green)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.White, ConsoleColor.DarkGreen)
                    .LinkWindows(PreviousWindow)
                    .Result();
            }
        }
    }
}
