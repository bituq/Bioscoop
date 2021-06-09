using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class SelectableText : Selectable
    {
        public ConsoleColor _defaultTextColor;
        internal Paragraph _paragraph { get; set; }
        internal Window Referral { get; set; }
        internal ConsoleColor DisabledColor { get; set; } = ConsoleColor.DarkGray;
        public Window Window { get => _paragraph.Window; set => _paragraph.Window = value; }
        public Action OnClick { get; set; } = () => { };
        public List<ConsoleKey> OnClickKeys = new List<ConsoleKey> { ConsoleKey.Enter };
        public string Text
        {
            get => _paragraph.Text;
            set => _paragraph.Text = value;
        }
        public ConsoleColor TextColor
        {
            get => _paragraph.TextColor;
            set => _paragraph.TextColor = value;
        }

        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public SelectableText(Paragraph paragraph, Color color)
        {
            this._paragraph = paragraph;
            Foreground = color.Foreground;
            Background = color.Background;
            _defaultTextColor = TextColor;
        }

        public void ActivateReferral()
        {
            Unselect();
            Console.Clear();
            Window.Active = false;
            Referral.Active = true;
            Referral.ActiveSelectable.Select();
        }

        public override void Unselect()
        {
            Selected = false;
            _paragraph.ChangeTextCells(TextColor, ConsoleColor.Black, true);
        }
        public override void Select()
        {
            Selected = true;
            _paragraph.ChangeTextCells(Foreground, Background, Background != Foreground);
        }

        public void Disable()
        {
            Disabled = true;
            _paragraph.TextColor = DisabledColor;
            _paragraph.Init();
        }

        public void Enable()
        {
            Disabled = false;
            _paragraph.TextColor = _defaultTextColor;
            _paragraph.Init();
        }
    }
}

namespace CinemaUI.Builder
{
    public class SelectableTextBuilder : IBuilder
    {
        private SelectableText _product { get; set; }
        private Tuple<Paragraph, Color> _params { get; set; }

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