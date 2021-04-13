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
            var container = new Container();
            container.Position = new Point(10, 5);
            container.Size = new Point(7, 7);
            container.Window = w1;
            var container2 = new Container();
            container2.Parent = container;
            container2.PositionSpace = Space.Relative;
            container2.Position = new Point(0, 0);
            container2.Size = new Point(3, 3);
            container2.Color = new Color(ConsoleColor.White, ConsoleColor.Yellow);
            container2.Window = w1;
            w1.Draw();
            Console.SetCursorPosition(0, 20);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}