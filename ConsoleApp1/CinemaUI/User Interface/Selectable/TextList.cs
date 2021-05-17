using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class SelectableList : Selectable
    {
        internal TextList TextList { get; set; }
        internal new List<SelectableText> Items { get; set; } = new List<SelectableText>();
        internal int OrderIndex { get => TextList.Window.SelectionOrder.IndexOf(this); }

        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public SelectableList(TextList textList, Color color)
        {
            this.TextList = textList;
            Foreground = color.Foreground;
            Background = color.Background;
        }

        public void Replace(SelectableList selectable)
        {
            TextList.Window.SelectionOrder.Remove(selectable);
            TextList.Replace(selectable.TextList);
            Foreground = selectable.Foreground;
            Background = selectable.Background;
            Items = selectable.Items;
        }

        public SelectableText this[int index] { get => Items[index]; }

        private void UpArrow(SelectableText activeItem, int index)
        {
            activeItem.Unselect();
            Items[index == 0 ? Items.Count - 1 : index - 1].Select();
        }
        private void DownArrow(SelectableText activeItem, int index)
        {
            activeItem.Unselect();
            Items[index == Items.Count - 1 ? 0 : index + 1].Select();
        }
        private void LeftArrow(List<Selectable> selectionOrder)
        {
            TextList.Window.ActiveSelectable = selectionOrder[OrderIndex == 0 ? OrderIndex : OrderIndex - 1];
            Unselect();
            TextList.Window.ActiveSelectable.Select();
        }
        private void RightArrow(List<Selectable> selectionOrder)
        {
            TextList.Window.ActiveSelectable = selectionOrder[OrderIndex == selectionOrder.Count - 1 ? OrderIndex : OrderIndex + 1];
            Unselect();
            TextList.Window.ActiveSelectable.Select();
        }
        private void Enter(SelectableText activeItem)
        {
            activeItem.OnClick();
            if (activeItem.Referral != null)
                activeItem.ActivateReferral();
        }

        public override void KeyResponse(ConsoleKeyInfo keyPressed)
        {
            var selectionOrder = TextList.Window.SelectionOrder;
            var activeItem = Items.Find(item => item.Selected) ?? Items[0];
            int index = Items.IndexOf(activeItem);
            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    UpArrow(activeItem, index);
                    break;
                case ConsoleKey.DownArrow:
                    DownArrow(activeItem, index);
                    break;
                case ConsoleKey.LeftArrow:
                    LeftArrow(selectionOrder);
                    break;
                case ConsoleKey.RightArrow:
                    RightArrow(selectionOrder);
                    break;
                case ConsoleKey.Enter:
                    if (!Disabled && !activeItem.Disabled)
                        Enter(activeItem);
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

        public void SetOrder(int index) => TextList.Window.SelectionOrder[index] = this;

        public void Reset()
        {
            TextList.Window.SelectionOrder.Remove(this);
        }
    }
}

namespace CinemaUI.Builder
{
    public class SelectableGroupBuilder : IBuilder
    {
        private SelectableList _product { get; set; }
        private Tuple<TextList, Color> _params { get; set; }
        private ConsoleColor disabledColor { get; set; } = ConsoleColor.DarkGray;

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
        public SelectableGroupBuilder DisabledColor(ConsoleColor textColor)
        {
            this.disabledColor = textColor;
            return this;
        }
        public SelectableList Result()
        {
            SelectableList result = this._product;
            foreach (SelectableText item in result.Items)
                item.DisabledColor = disabledColor;
            this.Reset();

            result.TextList.Window.SelectionOrder.Add(result);
            if (result.TextList.Window.SelectionOrder.IndexOf(result) == 0)
                result.Select();

            return result;
        }
    }
}