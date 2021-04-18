using System;
using CinemaUI;
using CinemaUI.Builder;
using CinemaUI.Utility;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            var w1 = new Window(true);
            var testList = new TextListBuilder(w1, 5, 5)
                .Selectable(ConsoleColor.White, new Color(ConsoleColor.White, ConsoleColor.DarkBlue), true,
                "test1", "test2", "test3", "test4"
                ).Result();
            InputHandler.WaitForInput();
            Console.SetCursorPosition(0, 10);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}