﻿using System;
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
        internal Dictionary<string, Tuple<int, int, string, Color>> Buffer { get; set; } = new Dictionary<string, Tuple<int, int, string, Color>>();

        public void Draw()
        {
            if (Children.Length > 0)
                Init();
            foreach (Tuple<int, int, string, Color> cell in Buffer.Values)
            {
                Console.SetCursorPosition(cell.Item1, cell.Item2);
                Console.ForegroundColor = cell.Item4.Foreground;
                Console.BackgroundColor = cell.Item4.Background;
                Console.Write(cell.Item3);
            }
        }
        private void Init()
        {
            foreach (UIElement child in Children)
            {
                child.Init();
            }
        }
        internal void CreateCell(string key, Tuple<int, int, string, Color> value) => Buffer[key] = value;
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
                    Window.CreateCell(new Point(column, row).ToString(), Tuple.Create(column, row, " ", Color));
                }
            }
        }
    }

    public class Paragraph : UIElement
    {
        private string _text { get; set; }

        public string Text
        {
            get => $"{Prefix}{_text}{Suffix}";
            set => _text = $@"{value}";
        }
        public string Suffix { get; set; }
        public string Prefix { get; set; }
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;

        public Paragraph(Window window, int x = 0, int y = 0) : base(window, x, y) { }
        public Paragraph(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute) : base(window, parent, x, y, positionSpace) { }

        public override void Init()
        {
            int line = Position.Y;
            int offset = Position.X;
            for (int i = 0; i < Text.Length; i++)
            {
                Point point = new Point(offset, line);
                Color color = new Color(TextColor, Window.Buffer.ContainsKey(point.ToString()) ? Window.Buffer[point.ToString()].Item4.Background : ConsoleColor.Black);
                if (Text[i] == '\n')
                {
                    Window.CreateCell(point.ToString(), Tuple.Create(offset, line, " ", color));
                    offset = 0;
                    line++;
                }
                else
                {
                    Window.CreateCell(point.ToString(), Tuple.Create(offset, line, Text[i].ToString(), color));
                    offset++;
                }
            }
        }
    }

    public class TextList : UIElement
    {
        public List<Paragraph> Items { get; set; } = new List<Paragraph>();
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
        public TextList(Window window, int x = 0, int y = 0) : base(window, x, y) { }
        public TextList(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute) : base(window, parent, x, y, positionSpace) { }

        public void SetItems(string[] arr, string prefix = "", string suffix = "")
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Items.Add(new Paragraph(Window, Position.X, Position.Y + i));
                Items[i].TextColor = TextColor;
                Items[i].Text = arr[i];
                Items[i].Prefix = prefix;
                Items[i].Suffix = suffix;
            }
        }
        public void SetItems(string[] arr, bool UseNumbers, string suffix = "")
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Items.Add(new Paragraph(Window, Position.X, Position.Y + i));
                Items[i].TextColor = TextColor;
                Items[i].Text = arr[i];
                Items[i].Prefix = $"{i+1}. ";
                Items[i].Suffix = suffix;
            }
        }
    }
}