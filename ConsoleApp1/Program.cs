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

            var table = new TableBuilder(window, 5, 5);
            table.SetHeaders(ConsoleColor.DarkGray, "Rij", "Stoel");
            for (int i = 0; i < 25; i++)
            {
                table.AddRow($"{i / 7}.", $"{i + 1}");
            }
            table.Result(ConsoleColor.White);

            Console.CursorVisible = false;
            InputHandler.WaitForInput();
        }
    }
}