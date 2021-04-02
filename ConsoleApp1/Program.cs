using System;
using System.IO;
using System.Text.Json;

namespace CinemaApplication
{
    class Program
    {
        public static DateTime UnixToDate(int unix) 
        {
            DateTime date = DateTime.UnixEpoch;
            date = date.AddSeconds(unix);
            return date;
        }

        public static string SecondsToTime(JsonElement JsonSeconds)
        {
            int seconds = JsonSeconds.GetInt32();
            int hours = seconds / 3600;
            int minutesLeft = (seconds / 60) % 60;
            string time = $"{hours} hrs {minutesLeft} min";
            return time;
        }

        public static string[] GetTimeSlots(string name)
        {
            var slotFile = File.ReadAllText("TimeSlot.json");
            JsonDocument timeSlotsDoc = JsonDocument.Parse(slotFile);
            var timeslots = timeSlotsDoc.RootElement.EnumerateArray();

            int amountOfTimeslots = 0;

            foreach (var timeslot in timeslots) 
            {
                if (timeslot.GetProperty("movie").ToString() == name) {
                    amountOfTimeslots++;
                }
            }

            string[] availableTimeslots = new string[amountOfTimeslots];
            int count = 0;
            foreach (var timeslot in timeslots)
            {
                if (timeslot.GetProperty("movie").ToString() == name)
                {
                    availableTimeslots[count] = timeslot.GetProperty("time").ToString();
                    count++;
                }
            }
            return availableTimeslots;
        }
        public static void Draw(string name) 
        {
            var movies = File.ReadAllText("Movies.json");

            JsonDocument moviesDoc = JsonDocument.Parse(movies);

            foreach (var movie in moviesDoc.RootElement.EnumerateArray())
            {
                if (name == (movie.GetProperty("name").ToString()))
                {
                    // show movie information
                    Console.WriteLine(movie.GetProperty("name").ToString());
                    Console.WriteLine(SecondsToTime(movie.GetProperty("duration")));
                    Console.WriteLine(UnixToDate(movie.GetProperty("releaseDate").GetInt32()).ToString("d MMMM yyyy")); // is in unix
                    Console.WriteLine(movie.GetProperty("rating").ToString());
                    Console.WriteLine(movie.GetProperty("genres").ToString()); // is not an array yet
                    Console.WriteLine(movie.GetProperty("language").ToString());
                    Console.WriteLine(movie.GetProperty("company").ToString());
                    Console.WriteLine(movie.GetProperty("starring").ToString()); // is not an array yet
                    Console.WriteLine(movie.GetProperty("description").ToString());

                    // show timeslot information
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Choose your time");
                    Console.ResetColor();
                    foreach (string time in GetTimeSlots(name))
                    {
                        Console.WriteLine(time);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Draw("Placeholder (The Movie)");           
        }
    }
}