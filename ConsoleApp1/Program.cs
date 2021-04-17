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
            var w1 = new Window();

            var testTextBuilder = new TextBuilder(w1).Selectable(ConsoleColor.White, new Color(ConsoleColor.Blue));
            var testText = testTextBuilder.Result("Hello world!");
            w1.Init();
            w1.Draw();
            w1.Init();
            testText.Select();
            w1.Draw();
            Console.SetCursorPosition(0, 10);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}