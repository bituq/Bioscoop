using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class SelectableList : Selectable
    {
        internal TextList TextList { get; set; }
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
                case ConsoleKey.Enter:
                    if (activeItem.Referral != null)
                    {
                        Unselect();
                        Console.Clear();
                        TextList.Window.Active = false;
                        activeItem.Referral.Active = true;
                    }
                    break;
            }
        }

        public override void Unselect()
        {
            var activeItem = Items.Find(item => item.Selected);
            Selected = false;
            if (activeItem != null)
                activeItem.Unselect();
        }
        public override void Select()
        {
            Selected = true;
            TextList.Window.ActiveSelectable = this;
            Items[0].Select();
        }
    }
}

namespace CinemaUI.Builder
{
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

        public SelectableGroupBuilder LinkWindows(params Window[] windows)
        {
            for (int i = 0; i < Math.Min(_product.Items.Count, windows.Length); i++)
                _product.Items[i].Referral = windows[i];
            return this;
        }
        public SelectableList Result()
        {
            SelectableList result = this._product;
            this.Reset();

            result.TextList.Window.SelectionOrder.Add(result);
            if (result.TextList.Window.SelectionOrder.IndexOf(result) == 0)
                result.Select();

            return result;
        }
    }
}