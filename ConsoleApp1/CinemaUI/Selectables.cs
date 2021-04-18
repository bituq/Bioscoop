using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI.Utility;
using CinemaUI.Builder;

namespace CinemaUI.Selectables
{
    public class Selectable
    {
        public bool Selected { get; protected set; } = false;

        public virtual void Unselect() => Selected = false;
        public virtual void Select() => Selected = true;

    }

    public class SelectableText : Selectable
    {
        private Window _redirect { get; set; }
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
            Background = color.Background;
        }

        public override void Unselect()
        {
            Selected = false;
            _paragraph.ChangeTextCells(_paragraph.TextColor, ConsoleColor.Black, true);
        }
        public override void Select()
        {
            Selected = true;
            _paragraph.ChangeTextCells(Foreground, Background, Background != Foreground);
        }
    }

    public class SelectableList : Selectable
    {
        private TextList TextList { get; set; }
        internal List<SelectableText> Items { get; set; } = new List<SelectableText>();
        private int OrderIndex { get => TextList.Window.SelectionOrder.IndexOf(this); }
        protected int PriorityIndex { get; set; } = 0;

        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public SelectableList(TextList textList, Color color)
        {
            this.TextList = textList;
            Foreground = color.Foreground;
            Background = color.Background;
            textList.Window.SelectionOrder.Add(this);
        }

        public void KeyResponse(ConsoleKeyInfo keyPressed)
        {
            var selectionOrder = TextList.Window.SelectionOrder;
            var activeItem = Items.Find(item => item.Selected) ?? Items[0];
            int index = Items.IndexOf(activeItem);
            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    activeItem.Unselect();
                    Items[index == 0 ? Items.Count - 1 : index - 1].Select();
                    break;
                case ConsoleKey.DownArrow:
                    activeItem.Unselect();
                    Items[index == Items.Count - 1 ? 0 : index + 1].Select();
                    break;
                case ConsoleKey.LeftArrow:
                    TextList.Window.ActiveSelectable = selectionOrder[OrderIndex == 0 ? OrderIndex : OrderIndex - 1];
                    Unselect();
                    TextList.Window.ActiveSelectable.Select();
                    break;
                case ConsoleKey.RightArrow:
                    TextList.Window.ActiveSelectable = selectionOrder[OrderIndex == selectionOrder.Count - 1 ? OrderIndex : OrderIndex + 1];
                    Unselect();
                    TextList.Window.ActiveSelectable.Select();
                    break;
            }
        }
        public override void Unselect()
        {
            Selected = false;
            Items.Find(item => item.Selected).Unselect();
        }
        public override void Select()
        {
            Selected = true;
            TextList.Window.ActiveSelectable = this;
            Items[0].Select();
        }
    }
}

namespace CinemaUI.Selectables.Builder
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

    public class SelectableGroupBuilder : IBuilder
    {
        private SelectableList _product { get; set; }
        private Tuple<TextList, Color> _params { get; set; }

        public void Reset()
        {
            this._product = new SelectableList(_params.Item1, _params.Item2);
            foreach (Paragraph item in _params.Item1.Items)
                this._product.Items.Add(new SelectableText(item, _params.Item2));
        }

        public SelectableGroupBuilder(TextList textList, Color color)
        {
            this._params = new Tuple<TextList, Color>(textList, color);
            this.Reset();
        }
        public SelectableList Result()
        {
            SelectableList result = this._product;
            this.Reset();

            return result;
        }
    }
}