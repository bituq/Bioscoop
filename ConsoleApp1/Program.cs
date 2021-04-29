using System;
using CinemaUI;
using CinemaUI.Builder;
using System.IO;
using System.Text;

namespace CinemaApplication
{
    class Program
    {
        static Window mainMenu = new Window(true);
        static void MainMenu()
        {
            var title = new TextBuilder(mainMenu, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Result("Bioscoop Applicatie");

            var subtitle = new TextBuilder(mainMenu, 2, 3)
                .Color(ConsoleColor.DarkGray)
                .Result("Project B");

            var menu = new TextListBuilder(mainMenu, 2, 5)
                .Color(ConsoleColor.White)
                .Selectable(new Color(ConsoleColor.Black, ConsoleColor.White), true, "Demonstratie", "Item 2", "Item 3")
                .Result();

            var inputMenu = new TextListBuilder(mainMenu, 40, 5)
                .Color(ConsoleColor.Gray)
                .AsInput(new Color(ConsoleColor.White, ConsoleColor.DarkGray), "Test 1", "Test 2", "Test random", "test stuff")
                .Result();
        }

        static Window listOfMovies = new Window();
        static void ListOfMovies()
        {
            var title = new TextBuilder(listOfMovies, 2, 2)
                .Color(ConsoleColor.Yellow)
                .Result("List of movies");

            var menuItems = new string[] { "Hoofdmenu", "Geavanceerd zoeken" };
            var menu = new TextListBuilder(listOfMovies, 2, 4)
                .Color(ConsoleColor.Gray)
                .Selectable(new Color(ConsoleColor.Black, ConsoleColor.White), true, menuItems)
                .LinkWindows(mainMenu)
                .Result();
        }

        static void Main(string[] args)
        {
            MainMenu();

            InputHandler.WaitForInput();
        }
    }
}