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
}

namespace CinemaUI.Builder
{
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
}
