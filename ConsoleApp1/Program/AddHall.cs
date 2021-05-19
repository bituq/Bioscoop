using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class program
    {
        public class addHall
        {
            public string name { get; set; }
            public int rows { get; set; }
            public int columns { get; set; }

            public addHall(string Name, int Rows, int Columns)
            {
                name = Name;
                rows = Rows;
                columns = Columns;
            }
        }

        static Window addhallscreen = new Window(true);
        static void AddHall()
        {
            var newhall = new List<string>();

            var title = new TextBuilder(addhallscreen, 3, 3)
                 .Color(ConsoleColor.Red)
                 .Text("Add your hall ;)")
                 .Result();

            var title1 = new TextListBuilder(addhallscreen, 5, 3)
                .Color(ConsoleColor.DarkMagenta)
                .SetItems("")
                .Result();

            var goBack = new TextListBuilder(addhallscreen, 1, 1)
                   .Color(ConsoleColor.White)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(mainMenu)
                   .Result();

            /*var hall = new addHall();
            hall.name = "hall3";
            hall.rows = 96;
            hall.columns = 32;*/





            /*JsonFile.AppendToFile(new List<halls> { hall }, "halls.json");
            
            
            Console.WriteLine($"Successfully added \"{hall.name}\"");
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();*/

            //JsonFile.RemoveFromFile("name", "hall1", "halls.json");
            //Console.WriteLine($"Successfully removed \"{hall.name}\"");
        }
    }
}