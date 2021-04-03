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
            

            
            Console.WriteLine("Enter movie name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter duration: ");
            string duration = Console.ReadLine();
            Console.WriteLine("Enter release date: ");
            string releasedate = Console.ReadLine();
            addfunc(name, duration, releasedate);

            static void addfunc(string name, string duration, string releasedate)
            {
                string filePath = "movies.json";
                StreamReader reserveringFile = new StreamReader(filePath);
                var reserveringen = reserveringFile.ReadToEnd();

                JsonDocument doc = JsonDocument.Parse(reserveringen);
                JsonElement root = doc.RootElement;
              

                foreach (JsonElement reservering in root.EnumerateArray())
                {
                    string[] lijstReserveringen = reserveringen.Split('}');
                    int len = lijstReserveringen.Length;
                    for (int i = 0; i < len - 1; i++)
                    {
                        if (i == (len - 2))
                        {
                            lijstReserveringen[i] = lijstReserveringen[i] + "},\n";
                        }
                        else
                        {
                            lijstReserveringen[i] = lijstReserveringen[i] + "}";
                        }
                    }
                    Console.WriteLine(lijstReserveringen);
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


            /*foreach (JsonElement movie in doc.RootElement.EnumerateArray())
            {
                if (movie.GetProperty("name").ToString() == input)
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
            }*/
        }
    }
}