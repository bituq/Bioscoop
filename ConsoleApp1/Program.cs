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
            if(name == "")
            {                
                while (name == "")
                {
                    Console.WriteLine("Error! No input is given");
                    name = Console.ReadLine();
                }
            }
            Console.WriteLine("Enter duration: ");
            string duration = Console.ReadLine();
            if (duration == "")
            {
                while (duration == "")
                {
                    Console.WriteLine("Error! No input is given");
                    duration = Console.ReadLine();
                }
            }
            Console.WriteLine("Enter release date: ");
            string releasedate = Console.ReadLine();
            Console.WriteLine("Enter first genre: ");
            string genre1 = Console.ReadLine();
            Console.WriteLine("Enter second genre (if no second genre, leave empty): ");
            string genre2 = Console.ReadLine();
            string genre3 = "";
            if(genre2 != ""){
                Console.WriteLine("Enter third genre (if no third genre, leave empty): ");
                genre3 = Console.ReadLine();
            }
            string[] genres = ((genre2 == "") ? new string[] { genre1 } : ((genre3 == "") ? new string[] { genre1, genre2 } : new string[] { genre1, genre2, genre3}));
            Console.WriteLine("Enter language: ");
            string language = Console.ReadLine();
            Console.WriteLine("Enter company: ");
            string company = Console.ReadLine();
            addfunc(name, duration, releasedate, genres, language, company);

            static void addfunc(string name, string duration, string releasedate, string[] genres, string language, string company)
            {
                string filePath = "moviesadd.json";
                StreamReader movieFile = new StreamReader(filePath);
                var movies = movieFile.ReadToEnd();

                JsonDocument doc = JsonDocument.Parse(movies);
                JsonElement root = doc.RootElement;


                foreach (JsonElement movie in root.EnumerateArray())
                {
                    string[] listmovies = movies.Split('}');
                    int len = listmovies.Length;
                    for (int i = 0; i < len - 1; i++)
                    {
                        if (i == (len - 2))
                        {
                            listmovies[i] = listmovies[i] + "},\n";
                        }
                        else
                        {
                            listmovies[i] = listmovies[i] + "}";
                        }
                    }
                    Console.WriteLine(listmovies);

                    string toevoegen = "\t{\n" + $"\t\t\"name\" : \"{name}\",\n" + $"\t\t\"duration\" : \"{duration}\",\n" + $"\t\t\"releasedate\" : \"{releasedate}\",\n"
                        + $"\t\t\"rating\" : \"{null}\",\n" + 
                        ((genres.Length == 1) ?
                        $"\t\t\"genre\" : [\n\t\t\t\"{genres[0]}\"\n\t\t], \n" :
                        ((genres.Length == 2) ?
                        $"\t\t\"genre\" : [\n\t\t\t\"{genres[0]}\",\n\t\t\t\"{genres[1]}\"\n\t\t], \n" :
                        $"\t\t\"genre\" : [\n\t\t\t\"{genres[0]}\",\n\t\t\t\"{genres[1]}\",\n\t\t\t\"{genres[2]}\"\n\t\t], \n"))
                        + $"\t\t\"language\" : \"{language}\",\n" + $"\t\t\"company\" : \"{company}\"\n" + "\t}";

                    string newJson = "";
                    for (int j = 0; j < len - 1; j++)
                    {
                        newJson = newJson + listmovies[j];
                    }
                    newJson = newJson + toevoegen + listmovies[len - 1];
                    movieFile.Close();
                    File.WriteAllText(filePath, newJson);
                }
            }



          

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