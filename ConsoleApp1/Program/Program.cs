using System;
using CinemaUI;
using CinemaUI.Builder;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace CinemaApplication
{
    public static class Colors
    {
        public static System.ConsoleColor title = ConsoleColor.White;
        public static System.ConsoleColor undertitle = ConsoleColor.White;
        public static System.ConsoleColor breadcrumbs = ConsoleColor.Cyan;
        public static System.ConsoleColor description = ConsoleColor.White;

        public static System.ConsoleColor selection = ConsoleColor.White;
        public static Tuple<System.ConsoleColor, System.ConsoleColor> selectionBg = Tuple.Create(ConsoleColor.White, ConsoleColor.White);

        public static System.ConsoleColor text = ConsoleColor.White;

        public static System.ConsoleColor back = ConsoleColor.Green;
        public static Tuple<System.ConsoleColor, System.ConsoleColor> backBg = Tuple.Create(ConsoleColor.Black, ConsoleColor.White);

        public static Tuple<System.ConsoleColor, System.ConsoleColor, System.ConsoleColor> submit = Tuple.Create(ConsoleColor.White, ConsoleColor.White, ConsoleColor.White);

        public static System.ConsoleColor input = ConsoleColor.White;
        public static Tuple<System.ConsoleColor, System.ConsoleColor> inputBg = Tuple.Create(ConsoleColor.White, ConsoleColor.White);
    }
    partial class Program
    {

        static Window homeScreen = new Window(true);
        static Window mainMenu = new Window();

        
        static void HomeScreen()
        { 
          var home = new TextBuilder(homeScreen, 2, 2)
                .Color(Colors.title)
                .Text("Das Kino")
                .Result();

          var screen = new TextListBuilder(homeScreen, 2, 5)
                .Color(ConsoleColor.Red)
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
            ShowAllRes();
            SnackOptions();
            InputHandler.WaitForInput();
        }
    }
}