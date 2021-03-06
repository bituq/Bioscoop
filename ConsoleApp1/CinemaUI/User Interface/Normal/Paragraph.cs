using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
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
        public ConsoleColor Background { get; set; } = ConsoleColor.Black;

        public Paragraph(Window window, int x = 0, int y = 0) : base(window, x, y) { }
        public Paragraph(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute) : base(window, parent, x, y, positionSpace) { }

        public override void Init() => ChangeTextCells(TextColor, Background, true);
        public void Reset()
        {
            Destroy();
            int len = Text.Length;
            Text = "";
            for (int i = 0; i < len; i++)
                Text += " ";
            ChangeTextCells(TextColor, ConsoleColor.Black);
            Text = "";
        }
        public void Destroy()
        {
            int line = Position.Y;
            int offset = Position.X;
            for (int i = 0; i < Text.Length; i++)
            {
                Point point = new Point(offset, line);
                if (Text[i] == '\n')
                {
                    Window.RemoveCell(point.ToString());
                    offset = Position.X;
                    line++;
                }
                else
                {
                    Window.RemoveCell(point.ToString());
                    offset++;
                }
            }
        }
        public void ChangeTextCells(ConsoleColor foreground, ConsoleColor background, bool overwrite = false)
        {
            int line = Position.Y;
            int offset = Position.X;
            for (int i = 0; i < Text.Length; i++)
            {
                Point point = new Point(offset, line);
                Color color = new Color(foreground, Window.Buffer.ContainsKey(point.ToString()) && !overwrite ? Window.Buffer[point.ToString()].Color.Background : background);
                if (Text[i] == '\n')
                {
                    Window.CreateCell(point.ToString(), new Cell(offset, line, " ", color));
                    offset = Position.X;
                    line++;
                }
                else
                {
                    Window.CreateCell(point.ToString(),new Cell(offset, line, Text[i].ToString(), color));
                    offset++;
                }
            }
        }
    }
}

namespace CinemaUI.Builder
{
    public class TextBuilder : IBuilder
    {
        private Paragraph _product { get; set; }
        private Tuple<Window, UIElement, int, int, Space> _params { get; set; }
        private string text { get; set; } = "";
        private ConsoleColor textColor { get; set; } = ConsoleColor.White;

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

        public SelectableTextBuilder Selectable(ConsoleColor foreground, ConsoleColor background)
        {
            return new SelectableTextBuilder(_product, new Color(foreground, background));
        }

        public TextBuilder Color(ConsoleColor textColor)
        {
            this.textColor = textColor;
            return this;
        }

        public TextBuilder Text(string text)
        {
            this.text = text;
            return this;
        }

        public Paragraph Result()
        {
            Paragraph result = this._product;
            result.TextColor = textColor;
            result.Text = text;
            result.Init();

            this.Reset();

            return result;
        }
    }
}
