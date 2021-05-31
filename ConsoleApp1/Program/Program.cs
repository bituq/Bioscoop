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

        static Window homeScreen = new Window();
        static Window mainMenu = new Window();

        static void HomeScreen()
        { 
          var home = new TextBuilder(homeScreen, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Das Kino")
                .Result();

          var screen = new TextListBuilder(homeScreen, 2, 5)
                .Color(ConsoleColor.Red)
                .UseNumbers()
                .SetItems("Visitor", "Admin")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(mainMenu, AdminScherm)
                .Result();
        }
        static void MainMenu()
        {
            var titelmenu1 = new TextListBuilder(mainMenu, 2, 2)
                .Color(ConsoleColor.Cyan)
                .SetItems("Home/Visitor/")
                .Result();

            var titelmenu2 = new TextListBuilder(mainMenu, 2, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("What would you like to do?")
                .Result();

            var menu = new TextListBuilder(mainMenu, 2, 5)
                .Color(ConsoleColor.Red)
                .UseNumbers()
                .SetItems("View movies", "View snacks", "Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(listOfFilms, snacksWindow, homeScreen)
                .Result();
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
            SelecteerHallsScherm();
            SelectieSchermAdmin();
            ReserveringNaamScherm();
            SelectieSchermZoeken();
            AddHall();
            AddSnack();
            EditMovies();
            AdminMovieMenu();
            AddMovie();
            ShowAllRes();
            RemoveSnack();
            RemoveMovie();

            InputHandler.WaitForInput();
        }
    }
}