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
        static Window peaksWindow = new Window(true);

        public static void peaksDraw() 
        {
            var TimeSlots = File.ReadAllText("..\\..\\..\\TimeSlots.json");
            var Reserveringen = File.ReadAllText("..\\..\\..\\Reserveringen.json");
            var Movies = File.ReadAllText("..\\..\\..\\Movies.json");

            JsonDocument doc = JsonDocument.Parse(TimeSlots);

            JsonElement root = doc.RootElement;
            ;

            int yPos = 2;
            foreach (var time in getActivity(1))
            {
                var a = new TextBuilder(peaksWindow, 3, yPos++)
                    .Color(ConsoleColor.Cyan)
                    .Text(time.ToString())
                    .Result();
            }
            
        }
        public static int[] getActivity(int hours) 
        {
            var timeFile = File.ReadAllText("..\\..\\..\\TimeSlots.json");
            var reserveringFile = File.ReadAllText("..\\..\\..\\Reserveringen.json");
            var movieFile = File.ReadAllText("..\\..\\..\\Movies.json");

            JsonDocument TimeSlots = JsonDocument.Parse(timeFile);
            JsonDocument Reserveringen = JsonDocument.Parse(reserveringFile);
            JsonDocument Movies = JsonDocument.Parse(movieFile);

            int[] peaks = new int[TimeSlots.RootElement.GetArrayLength()];
            int count = 0;

            foreach (var timeslot in TimeSlots.RootElement.EnumerateArray())
            {
                int amountOfPeople = timeslot.GetProperty("occupiedSeats").GetArrayLength();


                peaks[count++] = amountOfPeople;

            }

            return peaks;
        }
    }
}