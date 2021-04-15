using System;
using CinemaUI;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            var w1 = new Window();
            var container = new Container(w1, 10, 5, 7, 7);
            container.Color = new Color(ConsoleColor.Red, ConsoleColor.Red);
            var container2 = new Container(w1, container, 0, 0, 5, 3, Space.Relative);
            container2.Color = new Color(ConsoleColor.White, ConsoleColor.Yellow);
            var text = new Paragraph(w1, 0, 0);
            text.Text = "Hello world!\nHow are you on this bright and wonderful day?";
            w1.Draw();
            Console.SetCursorPosition(0, 20);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}