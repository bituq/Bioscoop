using System;
using System.Collections.Generic;
using System.IO;
using CinemaUI.Utility;
using CinemaUI.Selectables.Builder;
using CinemaUI.Selectables;

namespace CinemaUI
{
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
        internal List<SelectableList> SelectionOrder { get; set; } = new List<SelectableList>();
        internal SelectableList ActiveSelectable { get; set; }

        public Window(bool setAsActive = false)
        {
            Active = setAsActive;
            InputHandler.Windows.Add(this);
        }

        public void Draw()
        {
            foreach (Tuple<int, int, string, Color> cell in Buffer.Values)
            {
                    Console.SetCursorPosition(cell.Item1, cell.Item2);
                    Console.ForegroundColor = cell.Item4.Foreground;
                    Console.BackgroundColor = cell.Item4.Background;
                    Console.Write(cell.Item3);
            }
        }
        public void Init()
        {
            if (ActiveSelectable == null && SelectionOrder.Count != 0)
                ActiveSelectable = SelectionOrder[0];
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
        public Window Window
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

        public override void Init() => ChangeTextCells(TextColor, ConsoleColor.Black);
        public void ChangeTextCells(ConsoleColor foreground, ConsoleColor background, bool overwrite = false)
        {
            int line = Position.Y;
            int offset = Position.X;
            for (int i = 0; i < Text.Length; i++)
            {
                Point point = new Point(offset, line);
                Color color = new Color(foreground, Window.Buffer.ContainsKey(point.ToString()) && overwrite == false ? Window.Buffer[point.ToString()].Item4.Background : background);
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

namespace CinemaUI.Builder
{
    public interface IBuilder
    {
        public void Reset();
    }

    public class TextBuilder : IBuilder
    {
        private Paragraph _product { get; set; }
        private Tuple<Window, UIElement, int, int, Space> _params { get; set; }

        public void Reset()
        {
            this._product = new Paragraph(_params.Item1, _params.Item2, _params.Item3, _params.Item4, _params.Item5);
        }

        public TextBuilder(Window window, int x = 0, int y = 0)
        {
            this._params = new Tuple<Window, UIElement, int, int, Space>(window, null, x, y, Space.Absolute);
            this.Reset();
        }
        public TextBuilder(Window window, UIElement parent, int x, int y, Space positionSpace)
        {
            this._params = new Tuple<Window, UIElement, int, int, Space>(window, parent, x, y, positionSpace);
            this.Reset();
        }

        public SelectableTextBuilder Selectable(ConsoleColor textColor, Color selectionColor)
        {
            _product.TextColor = textColor;
            return new SelectableTextBuilder(_product, selectionColor);
        }

        public Paragraph Result(string text)
        {
            Paragraph result = this._product;
            result.Text = text;

            this.Reset();

            return result;
        }
    }

    public class TextListBuilder : IBuilder
    {
        private TextList _product { get; set; }
        private Tuple<Window, UIElement, int, int, Space> _params { get; set; }

        public void Reset()
        {
            this._product = new TextList(_params.Item1, _params.Item2, _params.Item3, _params.Item4, _params.Item5);
        }

        public TextListBuilder(Window window, int x = 0, int y = 0)
        {
            this._params = new Tuple<Window, UIElement, int, int, Space>(window, null, x, y, Space.Absolute);
            this.Reset();
        }
        public TextListBuilder(Window window, UIElement parent, int x, int y, Space positionSpace)
        {
            this._params = new Tuple<Window, UIElement, int, int, Space>(window, parent, x, y, positionSpace);
            this.Reset();
        }

        public SelectableGroupBuilder Selectable(ConsoleColor textColor, Color selectionColor, bool useNumbers, params string[] items)
        {
            _product.TextColor = textColor;
            _product.SetItems(items, useNumbers);
            return new SelectableGroupBuilder(_product, selectionColor);
        }

        public TextList Result(ConsoleColor textColor, bool useNumbers, params string[] items)
        {
            _product.TextColor = textColor;
            _product.SetItems(items, useNumbers);
            TextList result = this._product;

            this.Reset();

            return result;
        }
    }
}