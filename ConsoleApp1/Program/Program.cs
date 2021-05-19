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

        static Window homeScreen = new Window(true);
        static Window mainMenu = new Window(false);
        

        static void HomeScreen()
        { 
          var home = new TextBuilder(homeScreen, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("The Willem Theater")
                .Result();

          var screen = new TextListBuilder(homeScreen, 2, 5)
                .Color(ConsoleColor.Red)
                .UseNumbers()
                .SetItems("Visitor", "Admin")
                .Selectable(ConsoleColor.DarkBlue, ConsoleColor.Red)
                .LinkWindows(mainMenu, AdminScherm)
                .Result();
        }
        static void MainMenu()
        {
            var titelmenu = new TextListBuilder(mainMenu, 2, 2)
                .Color(ConsoleColor.Cyan)
                .SetItems("User Menu")
                .Result();

            var menu = new TextListBuilder(mainMenu, 2, 5)
                .Color(ConsoleColor.Red)
                .UseNumbers()
                .SetItems("View movies", "View snacks", "Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(listOfFilms, snacksWindow, homeScreen)
                .Result();
        }
        static void HallsScreen()
        {
            Halls();
        }

        static void Main(string[] args)
        {
            peaksDraw();
            HomeScreen();
            ReserveringZoekScherm();
            MainMenu();
            ListOfFilms();
            SnacksWindow();
            Halls();
            HallsScreen();
            SelecteerHallsScherm();
            SelectieSchermAdmin();
            ReserveringNaamScherm();
            SelectieSchermZoeken();
            
            InputHandler.WaitForInput();
        }
    }
}