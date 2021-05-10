using System;
using CinemaUI;
using CinemaUI.Builder;
using System.IO;
using System.Text;

namespace CinemaApplication
{
    partial class Program
    {
        static Window mainMenu = new Window(true);
        static void MainMenu()
        {
            var title = new TextBuilder(mainMenu, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Bioscoop Applicatie")
                .Result();

            var subtitle = new TextBuilder(mainMenu, 2, 3)
                .Color(ConsoleColor.DarkGray)
                .Text("Project B")
                .Result();

            var menu = new TextListBuilder(mainMenu, 2, 5)
                .Color(ConsoleColor.White)
                .UseNumbers()
                .SetItems("Lijst van films", "Lijst van snacks")
                .Selectable(new Color(ConsoleColor.Black, ConsoleColor.White))
                .LinkWindows(listOfFilms, snacksWindow)
                .Result();
        }

        static void Main(string[] args)
        {
            //MainMenu();
            ListOfFilms();
            //SnacksWindow();

            InputHandler.WaitForInput();
        }
    }
}