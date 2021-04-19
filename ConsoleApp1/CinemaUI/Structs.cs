using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsEmpty
        {
            get => X == 0 && Y == 0;
        }
        #region Constructors
        public Point(int xy)
        {
            X = Y = xy;
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        #endregion
        #region Overloaders
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator *(Point a, Point b) => new Point(a.X * b.X, a.Y * b.Y);
        public static Point operator /(Point a, Point b) => new Point(a.X / b.X, a.Y / b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
        public override string ToString() => $"({X}, {Y})";
        #endregion
    }
    public struct Color
    {
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        #region Constructors
        public Color(ConsoleColor color)
        {
            Foreground = color;
            Background = color;
        }
        public Color(ConsoleColor foreground, ConsoleColor background)
        {
            Foreground = foreground;
            Background = background;
        }
        #endregion
        #region Overloaders
        public static bool operator ==(Color a, Color b) => a.Foreground == b.Foreground && a.Background == b.Background;
        public static bool operator !=(Color a, Color b) => a.Foreground != b.Foreground && a.Background != b.Background;

        public override string ToString() => $"({Foreground}, {Background})";
        #endregion
    }
}
