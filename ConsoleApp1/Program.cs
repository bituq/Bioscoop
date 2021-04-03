using System;
using System.Text.Json;
using System.IO;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {


            var movies = File.ReadAllText("moviesadd.json");
            JsonDocument doc = JsonDocument.Parse(movies);
            Console.WriteLine(doc);
            string input = Console.ReadLine();

            foreach (JsonElement movie in doc.RootElement.EnumerateArray())
            {
                if (movie.GetProperty("name").ToString() == input  )
                {
                    //Console.WriteLine("movie name is: " + movie.GetProperty("name"));
                    //Console.WriteLine("duration is: " + movie.GetProperty("duration"));
                    foreach (JsonElement starring in movie.GetProperty("starring").EnumerateArray())
                    {
                        Console.WriteLine(starring);
                    }
                    Console.WriteLine(movie.GetProperty("rating"));
                    Console.WriteLine(movie.GetProperty("description"));
                    Console.WriteLine("");
                }
            }
            


            /*{
            "movies" : {
                "movie1" : 
                    {
                    "name" : "",
                    "genre" : "",
                    "duration" : "",
                    "release date" : "",
                    "description" : ""
                    },
                 "movie2" : 
                    {
                    "name" : "",
                    "genre" : "",
                    "duration" : "",
                    "release date" : "",
                    "description" : ""
                    },
                "movie3" : 
                    {
                    "name" : "",
                    "genre" : "",
                    "duration" : "",
                    "release date" : "",
                    "description" : ""
                    }
            }
        }*/

            //string[] movies = new string[] {};

            //string data = " [ {\"name\": \"shrek\", \"occupation\": \"horror\"}, " +
            //"{\"name\": \"Peter Novak\", \"occupation\": \"driver\"} ]";
            //var A = data["B"];


            //Console.WriteLine("Enter name of movie: ");
            //var MovieName = Console.ReadLine();
            //Console.WriteLine("Enter genre: ");
            //var Genre = Console.ReadLine();
            //Console.WriteLine("...\n" + MovieName + " is saved");
        }
    }
}