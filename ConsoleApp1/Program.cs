using System;
using System.IO;
using System.Text.Json;
using System.Net;
using System.Net.Mail;

namespace CinemaApplication
{
    class Program
    {
        public static void EmailUser(string recipient, string code) 
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("kappenmetspammennu@gmail.com");
            msg.To.Add(recipient);
            msg.Subject = "your reservation code";
            msg.Body = "This email containts your reservation code.\n\n code: " + code;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("kappenmetspammennu@gmail.com", "lol nah");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }
        }

        public static Tuple<string, string>[] GetAllMovies(string category)
        {
            // returns a list of tuples that contain the movie ID and the specified category
            var slotFile = File.ReadAllText("Movies.json");
            JsonDocument timeSlotsDoc = JsonDocument.Parse(slotFile);
            var movies = timeSlotsDoc.RootElement.EnumerateArray();

            int amountOfMovies = 0;

            foreach (var movie in movies)
            {
                amountOfMovies++;
            }
            Tuple<string, string>[] movieArr = new Tuple<string, string>[amountOfMovies];
            int index = 0;

            foreach (var movie in movies)
            {
                movieArr[index++] = new Tuple<string, string>(movie.GetProperty("name").ToString(), movie.GetProperty(category).ToString());
            }
            return movieArr;
        }

        public static string[] FilterByCategory(string category, string part) 
        {
            // returns the list of movies where the specified category matches the given part
            int size = 0;
            Tuple<string, string>[] movieList = GetAllMovies(category);

            for (int i = 0; i < movieList.Length; i++)
			{
                if (movieList[i].Item2.ToLower().Contains(part.ToLower()))
                {
                    size++;
                }
			}
            string[] newArr = new string[size];

            for (int i = 0, index = 0; i < movieList.Length; i++)
			{
                if (movieList[i].Item2.ToLower().Contains(part.ToLower()))
                {
                    newArr[index++] = movieList[i].Item1;
                }
			}
            return newArr;
        }

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

        public static JsonElement[] GetTimeSlots(string name)
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

            JsonElement[] availableTimeslots = new JsonElement[amountOfTimeslots];
            int count = 0;
            foreach (var timeslot in timeslots)
            {
                if (timeslot.GetProperty("movie").ToString() == name)
                {
                    availableTimeslots[count] = timeslot;
                    count++;
                }
            }
            return availableTimeslots;
        }
        static int selected = 0;
        public static void Draw(string name, ConsoleKey key) 
        {
            var movies = File.ReadAllText("Movies.json");

            JsonDocument moviesDoc = JsonDocument.Parse(movies);

            foreach (var movie in moviesDoc.RootElement.EnumerateArray())
            {
                if (name == (movie.GetProperty("name").ToString()))
                {
                    // show timeslot information
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("At what time would you like to see the movie?");
                    Console.ResetColor();

                    JsonElement[] timeslotArray = GetTimeSlots(name);

                    if (key == ConsoleKey.UpArrow && selected > 0)
                    {
                        selected--;
                    }
                    else if (key == ConsoleKey.DownArrow && selected < timeslotArray.Length - 1)
                    {
                        selected++;
                    }
                    for (int i = 0; i < timeslotArray.Length; i++)
                    {
                        JsonElement timeslot = timeslotArray[i];
                        // color
                        if (selected == i)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        // text
                        Console.WriteLine("- " + timeslot.GetProperty("time"));
                        Console.ResetColor();
                    }
                    Console.WriteLine("Movies: ");
                    string[] list = FilterByCategory("genres", "comedy");
                    for (int i = 0; i < list.Length; i++)
                    {
                        Console.WriteLine(list[i]);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            // EmailUser("alumite10@gmail.com", "1111111");
            bool firstRun = true;
            string movieName = "Placeholder (The Movie)";
            while (true)
            {
                if (firstRun) 
                {
                    ConsoleKey key = ConsoleKey.UpArrow;
                    Draw(movieName, key);
                    firstRun = false;
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.Clear();
                Draw(movieName, keyInfo.Key);
            }
        }
    }
}