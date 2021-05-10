﻿using System;
using System.IO;
using System.Collections.Generic;
using JsonHandler;

namespace CinemaApplication
{
    public class AddHalls
    {
        public class halls
        {
            public string name { get; set; }
            public int rows { get; set; }
            public int columns { get; set; }
        }
        public static void Wain()
        {
            var hall = new halls();
            hall.name = "hall3";
            hall.rows = 96;
            hall.columns = 32;





            JsonFile.AppendToFile(new List<halls> { hall }, "halls.json");
            Console.WriteLine($"Successfully added \"{hall.name}\"");
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();

            //JsonFile.RemoveFromFile("name", "hall1", "halls.json");
            //Console.WriteLine($"Successfully removed \"{hall.name}\"");
        }
    }
}