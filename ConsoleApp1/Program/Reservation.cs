using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;
using System.Text.Json;

namespace CinemaApplication
{
    partial class Program
    {
        public class Reservation
        {
            private class Information
            {
                public string firstName { get; set; }
                public string lastName { get; set; }
                public int code { get; set; }
                public int hall { get; set; }
                public int movieId { get; set; }
                public int date { get; set; }
                public JsonElement[] occupiedSeats { get; set; }
                public int timeslotId { get; set; }
            }
            public Window Window { get; set; } = new Window();
            public Window PaymentsWindow { get; set; } = new Window();
            public Window ideal { get; set; } = new Window();
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int ReservationNumber { get; set; }
            public Hall Hall { get; set; }
            public TimeSlot TimeSlot { get; set; }
            public int DateOfReservation { get; set; }
            public List<Seat> Seats { get; set; }
            private bool submitted = false;

            public Reservation(TimeSlot TimeSlot, Window PreviousWindow, List<Seat> Seats)
            {
                this.TimeSlot = TimeSlot;
                this.Seats = Seats;
                this.Hall = TimeSlot.Hall;

                Payments();

                string filePath = "..\\..\\..\\Reserveringen.json";
                var root = JsonFile.FileAsList(filePath);

                var terug = new TextListBuilder(Window, 1, 1)
                    .Color(ConsoleColor.Yellow)
                    .SetItems("Go Back", "Submit")
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .LinkWindows(PaymentsWindow, TimeSlot.Movie.Window)
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

                string seatList = "";
                foreach (Seat seat in Seats)
                {
                    string temp = "";
                    if (!seatList.Contains($"Row {seat.Row}"))
                        temp = $"\nRow {seat.Row} - ";
                    seatList += $"{temp}stoel {seat.Column} ";
                }

                var additionalInformation = new TextListBuilder(Window, 11, 7)
                    .Color(ConsoleColor.DarkGray)
                    .SetItems("Hall: " + TimeSlot.Hall.Id, "Seats: " + seatList)
                    .Result();

                var successMessage = new TextListBuilder(Window)
                    .SetItems("")
                    .Result();

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
                            existingCodes.Add(root[i].GetProperty("code").ToString());

                        while (existingCodes.Exists(existingCode => existingCode == randomCode))
                            randomCode = CreateString(7);

                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        string unixTime = unixTimestamp.ToString();

                        var info = new Information();
                        info.firstName = inputList[0].Value;
                        info.lastName = inputList[1].Value;
                        info.code = Int32.Parse(randomCode);
                        info.hall = Hall.Id;
                        info.movieId = TimeSlot.Movie.Id;
                        info.date = unixTimestamp;
                        var temp = new List<JsonElement>();
                        foreach (Seat seat in Seats)
                            temp.Add(JsonDocument.Parse(JsonSerializer.Serialize(seat, JsonFile.options)).RootElement);
                        info.occupiedSeats = temp.ToArray();
                        info.timeslotId = TimeSlot.id;
                        JsonFile.AppendToFile(info, filePath);

                        successMessage.Replace(
                            new TextListBuilder(Window, 1, 11)
                            .Color(ConsoleColor.Green)
                            .SetItems("Reservation has been submitted. Your reservation will be sent to you through e-mail.")
                            .Result()
                            );
                        terug.Replace(
                            new TextListBuilder(Window, 1, 1)
                            .Color(ConsoleColor.Yellow)
                            .SetItems("Go back", "Submit")
                            .Selectable(ConsoleColor.Black, ConsoleColor.White)
                            .LinkWindows(TimeSlot.Window)
                            .Result()
                            );
                        terug[1].Disable();
                        Window.Init();

                        VincentCooleQOLFuncties.EmailUser(inputList[2].Value, randomCode, TimeSlot, seatList);

                        string timeSlotPath = "..\\..\\..\\TimeSlots.json";
                        var thisTimeslot = JsonFile.FileAsList(timeSlotPath).Find(element => element.GetProperty("id").GetInt32() == TimeSlot.id);
                        var newTimeslot = new TimeSlot.Information();
                        newTimeslot.id = thisTimeslot.GetProperty("id").GetInt32();
                        newTimeslot.movieId = thisTimeslot.GetProperty("movieId").GetInt32();
                        newTimeslot.time = thisTimeslot.GetProperty("time").GetInt32();
                        newTimeslot.hall = thisTimeslot.GetProperty("hall").GetInt32();
                        var temp2 = new List<JsonElement>();
                        temp2.InsertRange(0, thisTimeslot.GetProperty("occupiedSeats").EnumerateArray());
                        foreach (Seat seat in Seats)
                            temp2.Add(JsonDocument.Parse(JsonSerializer.Serialize(seat, JsonFile.options)).RootElement);
                        newTimeslot.occupiedSeats = temp2.ToArray();
                        JsonFile.RemoveFromFile("id", thisTimeslot.GetProperty("id").GetInt32(), timeSlotPath);
                        JsonFile.AppendToFile(newTimeslot, timeSlotPath);

                        TimeSlot.Window.ClearAllChildren();

                        TimeSlot.Init();
                    }
                    else
                    {
                        successMessage.Replace(
                            new TextListBuilder(Window, 1, 11)
                            .Color(ConsoleColor.Red)
                            .SetItems("The fields may not be empty. You must fill in all the required information.")
                            .Result()
                            );
                    }
                };
            }
            private void Payments()
            {
                IDEAL();

                var MethodsList = new string[] { "IDEAL", "PayPal", "VISA", "Maestro", "MasterCard" };

                var title = new TextBuilder(PaymentsWindow, 1, 1)
                    .Color(ConsoleColor.Magenta)
                    .Text("Select a payment method.")
                    .Result();
                var menu = new TextListBuilder(PaymentsWindow, 1, 3)
                    .Color(ConsoleColor.Yellow)
                    .SetItems(MethodsList)
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .LinkWindows(ideal, Window, Window, Window, Window)
                    .Result();
            }

            private void IDEAL()
            {
                var IDEALlist = new string[] { "ING", "ABN-AMRO", "Rabobank", "RegioBank", "SNS bank", "knab"};
                var title = new TextBuilder(ideal, 11, 1)
                    .Color(ConsoleColor.Magenta)
                    .Text("Choose your bank.")
                    .Result();

                var goBack = new TextListBuilder(ideal, 1, 1)
                    .Color(ConsoleColor.Yellow)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .LinkWindows(PaymentsWindow)
                    .Result();

                var menu = new TextListBuilder(ideal, 11, 3)
                    .Color(ConsoleColor.Cyan)
                    .SetItems(IDEALlist)
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .LinkWindows(Window, Window, Window, Window, Window, Window)
                    .Result();
            }
        }
    }
}
