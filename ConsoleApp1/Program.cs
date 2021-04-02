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

            foreach (JsonElement movie in doc.RootElement.EnumerateArray())
            {
                Console.WriteLine(movie.GetProperty("name"));
                Console.WriteLine(movie.GetProperty("duration"));
                Console.WriteLine(movie.GetProperty("genre"));
                Console.WriteLine(movie.GetProperty("rating"));
                Console.WriteLine("");
            }
        }
    }
}