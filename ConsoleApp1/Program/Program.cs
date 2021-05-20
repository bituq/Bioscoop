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
                  .Color(ConsoleColor.White)
                  .UseNumbers()
                  .SetItems("Visitor", "Admin")
                  .Selectable(ConsoleColor.Black, ConsoleColor.White)
                  .LinkWindows(mainMenu, mainMenu)
                  .Result();
        }
        static void MainMenu()
        {
            var exit = new TextListBuilder(mainMenu, 1, 1)
                .Color(ConsoleColor.Yellow)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(addSnack, snacksWindow)
                .Result();

            var menu = new TextListBuilder(mainMenu, 11, 1)
                .Color(ConsoleColor.Cyan)
                .UseNumbers()
                .SetItems("View movies", "View snacks")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(listOfFilms, food)
                .Result();
        }
        static void HallsScreen()
        {
            Halls();
        }

        static void Main(string[] args)
        {
            ReserveringZoekScherm();
            MainMenu();
            AddSnack();
            //ListOfFilms();
            SnacksWindow();
            Halls();
            HallsScreen();
            SelectieSchermAdmin();
            ReserveringNaamScherm();
            SelectieSchermZoeken();
            
            InputHandler.WaitForInput();
        }
    }
}