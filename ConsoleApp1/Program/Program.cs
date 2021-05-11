using System;
using CinemaUI;
using CinemaUI.Builder;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace CinemaApplication
{
    partial class Program
    {
        static Window mainMenu = new Window(true);
        static void MainMenu()
        {
            var title = new TextBuilder(mainMenu, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Cinema Application")
                .Result();

            var subtitle = new TextBuilder(mainMenu, 2, 3)
                .Color(ConsoleColor.DarkGray)
                .Text("Project B")
                .Result();

            var menu = new TextListBuilder(mainMenu, 2, 5)
                .Color(ConsoleColor.White)
                .UseNumbers()
                .SetItems("View movies", "View snacks")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(listOfFilms, snacksWindow)
                .Result();
        }

        static void Main(string[] args)
        {
            selectieSchermBrent();
            reserveringMaakScherm();
            reserveringZoekScherm();
            MainMenu();
            ListOfFilms();
            SnacksWindow();
            Halls();

            InputHandler.WaitForInput();
        }
    }
}