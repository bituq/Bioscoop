using System;
using System.Text.Json;
using System.IO;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = " [ {\"name\": \"John Doe\", \"occupation\": \"gardener\"}, " +
            "{\"name\": \"Peter Novak\", \"occupation\": \"driver\"} ]";

            Console.WriteLine("Enter name of movie: ");
            var MovieName = Console.ReadLine();
            Console.WriteLine("Enter genre: ");
            var Genre = Console.ReadLine();
            Console.WriteLine("...\n" + MovieName + " is saved");
        }
    }
}