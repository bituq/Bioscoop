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
            public string Movie { get; set; } // Wordt nog een object in de toekomst.
            public int DateOfReservation { get; set; }
            public Seat Seat { get; set; }

            public Reservation(string Movie, Window PreviousWindow, Seat Seat)
            {
                this.Movie = Movie;
                this.Seat = Seat;

                var title = new TextBuilder(Window, 1, 1)
                    .Color(ConsoleColor.Magenta)
                    .Text("Maak een reservering voor " + Movie)
                    .Result();

                var goBack = new TextListBuilder(PreviousWindow, 1, 2)
                    .Color(ConsoleColor.Green)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.White, ConsoleColor.DarkGreen)
                    .Result();
            }
        }
    }
}
