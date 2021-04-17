using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI.Utility;
using CinemaUI.Builder;

namespace CinemaUI.Selectable
{
    public abstract class Selectable
    {
        public bool Selected { get; protected set; } = false;
        protected int PriorityIndex { get; set; } = 0;

        public void Unselect() => Selected = false;
    }

    public class SelectableText : Selectable
    {
        private Paragraph _paragraph { get; set; }
        public string Text
        {
            get => _paragraph.Text;
            set => _paragraph.Text = value;
        }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public SelectableText(Paragraph paragraph, Color color)
        {
            this._paragraph = paragraph;
            Foreground = color.Foreground;
            if (color.Background != color.Foreground)
                Background = color.Background;
        }
    }
}

namespace CinemaUI.Selectable.Builder
{

    public class SelectableTextBuilder : IBuilder
    {
        private SelectableText _product { get; set; }
        private Tuple<Paragraph,Color> _params { get; set; }

        public void Reset()
        {
            this._product = new SelectableText(_params.Item1, _params.Item2);
        }

        public SelectableTextBuilder(Paragraph paragraph, Color color)
        {
            this._params = new Tuple<Paragraph, Color>(paragraph, color);
            this.Reset();
        }


        public SelectableText Result(string text)
        {
            SelectableText result = this._product;
            result.Text = text;

            this.Reset();

            return result;
        }
    }
}