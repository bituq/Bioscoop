using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApplication
{
    partial class Program
    {
        public class Reservering
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int ReservationNumber { get; set; }
            public Hall Hall { get; set; }
            public string Movie { get; set; } // Wordt nog een object in de toekomst.
            public int DateOfReservation { get; set; }
            public Seat Seat { get; set; }


        }
    }
}
