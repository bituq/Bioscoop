using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

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
            public TimeSlot TimeSlot { get; set; }
            public int DateOfReservation { get; set; }
            public Seat Seat { get; set; }

            public Reservation(TimeSlot TimeSlot, Window PreviousWindow, Seat Seat)
            {
                this.TimeSlot = TimeSlot;
                this.Seat = Seat;

                string filePath = "..\\..\\..\\Reserveringen.json";
                var root = JsonFile.FileAsList(filePath);

                var terug = new TextListBuilder(Window, 1, 1)
                    .Color(ConsoleColor.Yellow)
                    .SetItems("Go Back", "Submit")
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .LinkWindows(TimeSlot.Window)
                    .Result();

                var title = new TextBuilder(Window, 11, 1)
                    .Color(ConsoleColor.Magenta)
                    .Text("Make a reservation for " + TimeSlot.Movie.Name)
                    .Result();

                var inputInformation = new TextListBuilder(Window, 11, 3)
                    .SetItems("First Name:", "Last Name:", "E-mail adress:")
                    .Result();

                var inputList = new TextListBuilder(Window, 12 + inputInformation.Items[2].Text.Length, 3)
                    .SetItems("", "", "")
                    .AsInput(ConsoleColor.White, ConsoleColor.Black)
                    .Result();

                var additionalInformation = new TextListBuilder(Window, 11, 7)
                    .Color(ConsoleColor.DarkGray)
                    .SetItems("Hall: " + TimeSlot.Hall.Id, "Row: " + Seat.Row, "Seat: " + Seat.Column)
                    .Result();

                var successMessage = new TextListBuilder(Window)
                    .SetItems("")
                    .Result();

                bool submitted = false;

                terug[1].OnClick = () =>
                {
                    if (!submitted && inputList[0].Value != "" && inputList[1].Value != "" && inputList[2].Value != "")
                    {
                        submitted = true;
                        Random rd = new Random();
                        string CreateString(int Length)
                        {
                            const string allowedChars = "0123456789";
                            char[] chars = new char[Length];
                            for (int i = 0; i < Length; i++)
                                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                            return new string(chars);
                        }
                        string randomCode = CreateString(7);
                        var existingCodes = new List<string>();

                        for (int i = 0; i < root.Count; i++)
                            existingCodes.Add(root[i].GetProperty("reserveringNummer").ToString());

                        while (existingCodes.Exists(existingCode => existingCode == randomCode))
                            randomCode = CreateString(7);

                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        string unixTime = unixTimestamp.ToString();

                        successMessage.Replace(
                            new TextListBuilder(Window, 1, 11)
                            .Color(ConsoleColor.Gray)
                            .SetItems("Reservation has been submitted. Your reservation will be sent to you through e-mail.")
                            .Result()
                            );

                        VincentCooleQOLFuncties.EmailUser(inputList[2].Value, randomCode, TimeSlot, Seat);
                    }
                    else
                    {
                        successMessage.Replace(
                            new TextListBuilder(Window, 1, 11)
                            .Color(ConsoleColor.Green)
                            .SetItems("The fields may not be empty. You must fill in all the required information.")
                            .Result()
                            );
                    }
                };
            }
        }
        static Window reservationWindow = new Window();
        static void ReservationWindow()
        {
            
        }
    }
}
