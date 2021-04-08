using System;
using System.Collections.Generic;
using System.IO;

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
        public Color Color { get; set; }

        public Point(int xy, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            X = xy;
            Y = xy;
            Color = new Color(foreground, background);
        }
        public Point(int x, int y, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            X = x;
            Y = y;
            Color = new Color(foreground, background);
        }

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator *(Point a, Point b) => new Point(a.X * b.X, a.Y * b.Y);
        public static Point operator /(Point a, Point b) => new Point(a.X / b.X, a.Y / b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
    }
    public struct Color
    {
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

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

        public static bool operator ==(Color a, Color b) => a.Foreground == b.Foreground && a.Background == b.Background;
        public static bool operator !=(Color a, Color b) => a.Foreground != b.Foreground && a.Background != b.Background;

        public override bool Equals(object obj) => this.Equals(obj);
        public override string ToString() => $"({Foreground}, {Background})";
    }
    public interface IComponent
    {
        enum Space
        {
            Absolute,
            Relative
        }
        public static class Options
        {
            public static bool AutoDraw = true;
        }

        Space PositionSpace { get; set; }
        bool Active { get; set; }
    }
    public class Instance : IComponent
    {
        public List<IComponent> Children = new List<IComponent>();
        public Instance Parent { get; private set; }
        private Point Position { get; set; }
        public IComponent.Space PositionSpace { get; set; }
        public bool Active { get; set; } = false;

        public Point GetPosition()
        {
            if (PositionSpace == IComponent.Space.Absolute)
                return Position;
            else
                return Position + (GetParent()?.Position ?? new Point());
        }
        public virtual void SetPosition(Point value)
        {
            Position = value;
        }
        public Instance GetParent() => Parent;
        public virtual void SetParent(Instance value)
        {
            value.Children.Add(this);
            Parent = value;
        }
    }
    public class Container : Instance
    {
        public IComponent.Space ScaleSpace { get; set; } = IComponent.Space.Absolute;
        public List<Point> PointMap = new List<Point>();
        public Color Color { get; set; }
        private Point Scale;
        public Container Parent { get; private set; }

        public Point GetScale()
        {
            if (ScaleSpace == IComponent.Space.Absolute)
                return Scale;
            else
                return Scale + (GetParent()?.Scale ?? new Point());
        }
        public void SetScale(Point value)
        {
            Scale = value;
            Reset();
        }
        public Container GetParent() => Parent;
        public virtual void SetParent(Container value)
        {
            value.Children.Add(this);
            Parent = value;
        }

        public Container(Point Position, Point Scale)
        {
            this.SetPosition(Position);
            this.SetScale(Scale);
            Init();
        }
        public Container(Point Position)
        {
            this.SetPosition(Position);
            this.SetScale(new Point(1, 1));
            Init();
        }
        public Container()
        {
            this.SetPosition(new Point(0, 0));
            this.SetScale(new Point(1, 1));
            Init();
        }

        public void Draw()
        {
            foreach (Point point in PointMap)
            {
                Console.ForegroundColor = Color.Foreground;
                Console.BackgroundColor = Color.Background;
                Console.SetCursorPosition(point.X, point.Y);
                Console.Write(" ");
            }
        }
        public void Draw(Func<int, int, bool> check)
        {
            foreach (Point point in PointMap)
            {
                if (check(point.X, point.Y))
                {
                    Console.ForegroundColor = Color.Foreground;
                    Console.BackgroundColor = Color.Background;
                    Console.SetCursorPosition(point.X, point.Y);
                    Console.Write(" ");
                   
                }
            }
        }
        private void Init()
        {
            for (int row = GetPosition().X; row < GetPosition().X + GetScale().X; row++)
            {
                for (int column = GetPosition().Y; column < GetPosition().Y + GetScale().Y; column++)
                {
                    PointMap.Add(new Point(row, column, Color.Foreground, Color.Background));
                }
            }
        }
        private void Init(Func<int, int, bool> check, ConsoleColor backgroundColor)
        {
            for (int row = GetPosition().X; row < GetPosition().X + GetScale().X; row++)
            {
                for (int column = GetPosition().Y; column < GetPosition().Y + GetScale().Y; column++)
                {
                    PointMap.Add(new Point(row, column, Color.Foreground, check(column, row) ? backgroundColor : Color.Background));
                }
            }
        }
        private void Reset()
        {
            PointMap.Clear();
            Init();
        }

    }
    public class Paragraph : IComponent
    {
        private IComponent Parent;
        private Point Position;
        private Point Scale;
        public Color Color { get; set; }
        public IComponent.Space PositionSpace { get; set; } = IComponent.Space.Relative;
        public bool Active { get; set; } = false;

        public IComponent GetParent() => Parent;

        public Point GetPosition()
        {
            throw new NotImplementedException();
        }

        public void SetParent(Container value)
        {
            throw new NotImplementedException();
        }

        public void SetPosition(Point value)
        {
            throw new NotImplementedException();
        }
        public void Draw()
        {
            throw new NotImplementedException();
        }
    }
}