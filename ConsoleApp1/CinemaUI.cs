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
        #region Constructors
        public Point(int xy)
        {
            X = Y = xy;
            Color = new Color(ConsoleColor.White, ConsoleColor.Black);
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
            Color = new Color(ConsoleColor.White, ConsoleColor.Black);
        }
        public Point(int xy, Color color)
        {
            X = Y = xy;
            Color = color;
        }
        public Point(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
        #endregion
        #region Overloaders
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator *(Point a, Point b) => new Point(a.X * b.X, a.Y * b.Y);
        public static Point operator /(Point a, Point b) => new Point(a.X / b.X, a.Y / b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
        #endregion
        public override string ToString() => $"({X}, {Y})";
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

        public override string ToString() => $"({Foreground}, {Background})";
    }

    public enum Space
    {
        Absolute,
        Relative
    }

    public class Instance
    {
        private bool _active { get; set; }
        private List<UIElement> _children = new List<UIElement>();

        public virtual Point Position { get; set; }
        public virtual Point Size { get; set; }
        public string Name { get; set; }
        public string ClassName { get => this.GetType().Name; }
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                foreach (Instance child in Children)
                {
                    child.Active = value;
                }
            }
        }
        public UIElement[] Children
        {
            get
            {
                UIElement[] res = new UIElement[_children.Count];
                for (int i = 0; i < res.Length; i++)
                    res[i] = _children[i];
                return res;
            }
        }

        public void AddChild(UIElement child) => _children.Add(child);
        public void RemoveChild(UIElement child) => _children.Remove(child);
        public void ClearAllChildren() => _children.Clear();
        public bool IsA(string otherName) => ClassName == otherName;
    }

    public class Window : Instance
    {
        public List<Tuple<Point, string>> PointMap { get; set; } = new List<Tuple<Point, string>>();

        public void Draw()
        {
            if (Children.Length > 0)
                Init();
            foreach (Tuple<Point, string> buffer in PointMap)
            {
                Console.SetCursorPosition(buffer.Item1.X, buffer.Item1.Y);
                Console.ForegroundColor = buffer.Item1.Color.Foreground;
                Console.BackgroundColor = buffer.Item1.Color.Background;
                Console.Write(buffer.Item2);
            }
        }
        private void Init()
        {
            foreach (UIElement child in Children)
            {
                child.Init();
            }
        }
    }

    public class UIElement : Instance
    {
        private Point _position { get; set; }
        private UIElement _parent { get; set; }
        private Window _window { get; set; }

        protected UIElement Parent
        {
            get => _parent;
            set
            {
                if (value == null)
                    _parent = null;
                else
                {
                    if (_parent != null)
                        _parent.RemoveChild(this);
                    if (value == this)
                        _parent = null;
                    else
                    {
                        value.AddChild(this);
                        _parent = value;
                    }
                }
            }
        }
        protected Window Window
        {
            get => _window;
            set
            {
                if (value == null)
                    _window = null;
                else
                {
                    if (_window != null)
                        _window.RemoveChild(this);
                    value.AddChild(this);
                    _window = value;
                }
            }

        }
        public Space PositionSpace { get; set; } = Space.Absolute;
        public override Point Position
        {
            get => _position;
            set
            {
                if (PositionSpace == Space.Absolute)
                    _position = value;
                else if (PositionSpace == Space.Relative)
                    _position = value + Parent?.Position ?? new Point(0, 0);
            }
        }

        public UIElement(Window window, int x = 0, int y = 0)
        {
            Position = new Point(x, y);
            Window = window;
        }
        public UIElement(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute)
        {
            PositionSpace = positionSpace;
            Window = window;
            Parent = parent;
            Position = new Point(x, y);
        }

        public virtual void Init() { }
        public void Destroy()
        {
            if (Parent != null)
                Parent.RemoveChild(this);
            foreach (UIElement child in Children)
                child.Parent = null;
            Parent = null;
            ClearAllChildren();
        }
    }

    public class Container : UIElement
    {
        private Point _size { get; set; }

        public Space ScaleSpace { get; set; } = Space.Absolute;
        public override Point Size
        {
            get => _size.X > 0 && _size.Y > 0 ? _size : new Point(1,1);
            set
            {
                if (ScaleSpace == Space.Absolute)
                    _size = value;
                else if (ScaleSpace == Space.Relative)
                    _size = value + Parent?.Size ?? new Point(0, 0);
            }
        }
        public Color Color { get; set; } = new Color(ConsoleColor.White, ConsoleColor.Black);

        public Container(Window window, int x = 0, int y = 0, int width = 1, int height = 1) : base(window, x, y)
        {
            Size = new Point(width, height);
        }
        public Container(Window window, UIElement parent, int x = 0, int y = 0, int width = 1, int height = 1, Space positionSpace = Space.Absolute, Space scaleSpace = Space.Absolute) : base(window, parent, x, y, positionSpace)
        {
            ScaleSpace = scaleSpace;
            Size = new Point(width, height);
        }

        public override void Init()
        {
            for (int row = Position.Y; row < Position.Y + Size.Y; row++)
            {
                for (int column = Position.X; column < Position.X + Size.X; column++)
                {
                    Window.PointMap.Add(Tuple.Create(new Point(column, row, Color), " "));
                }
            }
        }
    }

    public class Paragraph : UIElement
    {
        private string _text { get; set; }

        public string Text
        {
            get => _text;
            set
            {
                _text = @value;
            }
        }

        public Paragraph(Window window, int x = 0, int y = 0) : base(window, x, y) { }
        public Paragraph(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute) : base(window, parent, x, y, positionSpace) { }

        public override void Init()
        {
            int line = 0;
            int offset = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] == '\n')
                {
                    Window.PointMap.Add(Tuple.Create(new Point(offset, ++line), " "));
                    offset = 0;
                }
                else
                {
                    Window.PointMap.Add(Tuple.Create(new Point(offset, line), Text[offset].ToString()));
                    offset++;
                }
            }
        }
    }
}