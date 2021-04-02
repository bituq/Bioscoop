using System;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string movies = " [ {\"name\": \"Harry Potter\", \"duration\": \"90\", \"3D\": \"Yes\", \"genre\": \"Fantasy\", \"rating\": \"4.5/5\"}, " + "{\"name\": \"Lord of the rings\", \"duration\": \"170\"} ]";

            using JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;



            var u1 = root[0];
            var u2 = root[1];
            Console.WriteLine(u1);
            Console.WriteLine(u2);

            Console.WriteLine(u1.GetProperty("name"));
            Console.WriteLine(u1.GetProperty("duration"));
            Console.WriteLine(u1.GetProperty("3D"));
            Console.WriteLine(u1.GetProperty("genre"));
            Console.WriteLine(u1.GetProperty("rating"));
            Console.WriteLine("");

            Console.WriteLine(u2.GetProperty("name"));
            Console.WriteLine(u2.GetProperty("duration"));
        }
    }
}