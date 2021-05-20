using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;
using System.Text.Json;
using System.IO;

namespace CinemaApplication
{
    partial class Program
    {
        public class SnacksAdd
        {
           
            public string name { get; set; }


            
        }

        public static Window addSnack = new Window();
        static void AddSnack()
        {
            var _ = new TextListBuilder(snacksWindow, 1, 1)
                   .Color(ConsoleColor.White)
                   .SetItems("Go back")
                   .Selectable(ConsoleColor.Black, ConsoleColor.White)
                   .LinkWindows(mainMenu)
                   .Result();

            var snacksAndDrinks = File.ReadAllText("..\\..\\..\\snacksAndDrinks.json");

            JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
            JsonElement root = doc.RootElement;

       
        }

    }
}
