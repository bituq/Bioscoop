using System;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    class Program
    {

        static void Main(string[] args)
        {
            var window = new Window(true);
            var window2 = new Window(true);

            var titleContainer = new ContainerBuilder(window, 4, 2)
                .Color(ConsoleColor.DarkGray)
                .Size(21, 2)
                .Result();

            var title = new TextBuilder(window, titleContainer, 1, 0, Space.Relative)
                .Color(ConsoleColor.White)
                .Result("Project B\nBioscoop Applicatie");

            var mainMenu = new TextListBuilder(window, 4, 5)
                .Color(ConsoleColor.White)
                .Selectable(new Color(ConsoleColor.Black, ConsoleColor.White), true, "Item1", "To window 2", "Item3", "Item4")
                .LinkWindows(null, window2, null, null)
                .Result();

            var sideBar = new ContainerBuilder(window2, 22, 0)
                .Color(ConsoleColor.DarkGray)
                .Size(2, Console.WindowHeight)
                .Result();

            var title2 = new TextBuilder(window2, 1, 1)
                .Color(ConsoleColor.Yellow)
                .Result("Welcome to window 2!\nEnjoy your stay.");

            var menu2 = new TextListBuilder(window2, 1, 4)
                .Color(ConsoleColor.DarkRed)
                .Selectable(new Color(ConsoleColor.White, ConsoleColor.DarkBlue), false, "Back to window 1", "item2", "item3", "item4", "item5")
                .LinkWindows(window)
                .Result();

            Console.CursorVisible = false;
            InputHandler.WaitForInput();
        }
    }
}