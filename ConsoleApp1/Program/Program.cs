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
                .Text("Cinema Application")
                .Result();

          var subtitle = new TextBuilder(homeScreen, 2, 3)
                 .Color(ConsoleColor.DarkGray)
                 .Text("Project B")
                 .Result();

          var screen = new TextListBuilder(homeScreen, 2, 5)
                  .Color(ConsoleColor.White)
                  .UseNumbers()
                  .SetItems("Bezoeker", "Admin")
                  .Selectable(ConsoleColor.Black, ConsoleColor.White)
                  .LinkWindows(mainMenu, mainMenu)
                  .Result();
        }
        static void MainMenu()
        {
            var menu = new TextListBuilder(mainMenu, 2, 5)
                .Color(ConsoleColor.White)
                .UseNumbers()
                .SetItems("View movies", "View snacks")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(listOfFilms, snacksWindow)
                .Result();

            var exit = new TextListBuilder(mainMenu, 1, 1)
                .Color(ConsoleColor.White)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(homeScreen)
                .Result();
        }
        static void HallsScreen()
        {
            Halls();
        }

        static void Main(string[] args)
        {
            HomeScreen();
            MainMenu();
            ListOfFilms();
            SnacksWindow();
            Halls();
            HallsScreen();

            InputHandler.WaitForInput();
        }
    }
}