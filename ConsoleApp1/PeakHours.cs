using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        static Window peaksWindow = new Window();

        public static void peaksDraw() 
        {
            var list = new TextListBuilder(peaksWindow, 1, 2)
                .Color(ConsoleColor.Cyan)
                .SetItems("Home/Admin/Peak Hours/")
                .Result();

            var TimeSlots = File.ReadAllText("..\\..\\..\\TimeSlots.json");
            var Reserveringen = File.ReadAllText("..\\..\\..\\Reserveringen.json");
            var Movies = File.ReadAllText("..\\..\\..\\Movies.json");

            JsonDocument doc = JsonDocument.Parse(TimeSlots);
            JsonElement root = doc.RootElement;

            // output the activity with argument as interval
            getActivity(4);
        }

        public static void getActivity(int hours) 
        {
            var timeFile = File.ReadAllText("..\\..\\..\\TimeSlots.json");
            var reserveringFile = File.ReadAllText("..\\..\\..\\Reserveringen.json");
            var movieFile = File.ReadAllText("..\\..\\..\\Movies.json");

            JsonDocument TimeSlots = JsonDocument.Parse(timeFile);
            JsonDocument Reserveringen = JsonDocument.Parse(reserveringFile);
            JsonDocument Movies = JsonDocument.Parse(movieFile);

            int[] peaks = new int[TimeSlots.RootElement.GetArrayLength()];
            int count = 0;

            static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
            {
                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                return dtDateTime;
            }

            int yPos = 4;

            for (int i = 0, currTime = 0; i < 24 / hours; i++)
            {
                int people = 0;
                foreach (var timeslot in TimeSlots.RootElement.EnumerateArray())
                {
                    var currentTimeslotTime = UnixTimeStampToDateTime(timeslot.GetProperty("time").GetUInt64());
                    var currentTimeFrame = DateTime.UtcNow - DateTime.UtcNow.TimeOfDay;
                    currentTimeFrame = currentTimeFrame.AddHours(i*hours);
                    
                    //Console.WriteLine("check 1:" + currentTimeFrame.CompareTo(currentTimeslotTime));
                    if (currentTimeFrame.CompareTo(currentTimeslotTime) < 0) // timeslot time is after today
                    {
                        currentTimeslotTime = currentTimeslotTime.AddHours(-hours); // shift timeslot time to check for timeframe
                        
                        
                        if (currentTimeFrame.CompareTo(currentTimeslotTime) > 0) // timeslot time is in current timeframe
                            people += timeslot.GetProperty("occupiedSeats").GetArrayLength();
                    }
                    //Console.WriteLine("");
                }

                var a = new TextBuilder(peaksWindow, 3, yPos)
                    .Color(ConsoleColor.Gray)
                    .Text(currTime.ToString() + ":00-" + (currTime + hours) + ":00")
                    .Result();

                var b = new TextBuilder(peaksWindow, 15, yPos++)
                    .Color(ConsoleColor.Gray)
                    .Text("- " + people + " people")
                    .Result();
                currTime += hours;
            }
            var exit = new TextListBuilder(peaksWindow, 1, ++yPos)
                .Color(ConsoleColor.Green)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(AdminScherm)
                .Result();
        }
    }
}