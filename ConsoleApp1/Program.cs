using System;
using System.Text.Json;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string movies = " [ {\"name\": \"Harry Potter and the Deathly Hallows Part 1\", \"duration\": \"   Duration: 90 minutes\", \"3D\": \"   3D: No\", \"genre\": \"   Genre: Fantasy\", \"rating\": \"   Rating: 4/5\"}" +
                ", " + "{\"name\": \"Lord of the rings\", \"duration\": \"   Duration: 178 minutes\", \"3D\": \"   3D: No\", \"genre\": \"   Genre: Fantasy\", \"rating\": \"   Rating: 4/5\"}" +
                ", " + "{\"name\": \"Lord of the rings\", \"duration\": \"   Duration: 178 minutes\", \"3D\": \"   3D: No\", \"genre\": \"   Genre: Fantasy\", \"rating\": \"   Rating: 4/5\"}" +
                ", " + "{\"name\": \"Lord of the rings\", \"duration\": \"   Duration: 178 minutes\", \"3D\": \"   3D: No\", \"genre\": \"   Genre: Fantasy\", \"rating\": \"   Rating: 4/5\"}" +
                ", " + "{\"name\": \"Lord of the rings\", \"duration\": \"   Duration: 178 minutes\", \"3D\": \"   3D: No\", \"genre\": \"   Genre: Fantasy\", \"rating\": \"   Rating: 4/5\"} ]";

            using JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;



            var u1 = root[0];
            var u2 = root[1];
            var u3 = root[2];
            var u4 = root[3];
            var u5 = root[4];


            Console.WriteLine(u1.GetProperty("name"));
            Console.WriteLine(u1.GetProperty("duration"));
            Console.WriteLine(u1.GetProperty("3D"));
            Console.WriteLine(u1.GetProperty("genre"));
            Console.WriteLine(u1.GetProperty("rating"));
            Console.WriteLine("");

            Console.WriteLine(u2.GetProperty("name"));
            Console.WriteLine(u2.GetProperty("duration"));
            Console.WriteLine(u2.GetProperty("3D"));
            Console.WriteLine(u2.GetProperty("genre"));
            Console.WriteLine(u2.GetProperty("rating"));
            Console.WriteLine("");

            Console.WriteLine(u3.GetProperty("name"));
            Console.WriteLine(u3.GetProperty("duration"));
            Console.WriteLine(u3.GetProperty("3D"));
            Console.WriteLine(u3.GetProperty("genre"));
            Console.WriteLine(u3.GetProperty("rating"));
            Console.WriteLine("");

            Console.WriteLine(u4.GetProperty("name"));
            Console.WriteLine(u4.GetProperty("duration"));
            Console.WriteLine(u4.GetProperty("3D"));
            Console.WriteLine(u4.GetProperty("genre"));
            Console.WriteLine(u4.GetProperty("rating"));
            Console.WriteLine("");

            Console.WriteLine(u5.GetProperty("name"));
            Console.WriteLine(u5.GetProperty("duration"));
            Console.WriteLine(u5.GetProperty("3D"));
            Console.WriteLine(u5.GetProperty("genre"));
            Console.WriteLine(u5.GetProperty("rating"));
            Console.WriteLine("");


        }
    }
}