using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class TextInputList : Selectable
    {
        internal TextList TextList { get; set; }
        internal List<TextInput> Items { get; set; } = new List<TextInput>();
        public Window Window { get => TextList.Window; set => TextList.Window = value; }
        internal int OrderIndex { get => TextList.Window.SelectionOrder.IndexOf(this); }
        private Color ActiveColor { get; set; }
        public TextInputList(TextList textList, Color activeColor)
        {
            TextList = textList;
            ActiveColor = activeColor;
        }
        public Action OnChange { get; set; } = () => { };
        private bool AfterSelection { get; set; } = false;

        public TextInput this[int index] { get => Items[index]; }

        private void UpArrow(TextInput activeItem, int index)
        {
            activeItem.Unselect();
            Items[index == 0 ? Items.Count - 1 : index - 1].Select();
        }
        private void DownArrow(TextInput activeItem, int index)
        {
            activeItem.Unselect();
            Items[index == Items.Count - 1 ? 0 : index + 1].Select();
        }
        private void LeftArrow(List<Selectable> selectionOrder)
        {
            Window.ActiveSelectable = selectionOrder[OrderIndex == 0 ? OrderIndex : OrderIndex - 1];
            Unselect();
            Window.ActiveSelectable.Select();
        }
        private void RightArrow(List<Selectable> selectionOrder)
        {
            Window.ActiveSelectable = selectionOrder[OrderIndex == selectionOrder.Count - 1 ? OrderIndex : OrderIndex + 1];
            Unselect();
            Window.ActiveSelectable.Select();
        }
        private void Enter(TextInput activeItem)
        {
            activeItem.Active();
        }

        public override void KeyResponse(ConsoleKeyInfo keyPressed)
        {
            var selectionOrder = Window.SelectionOrder;
            var selectedItem = Items.Find(item => item.Selected) ?? Items[0];
            int selectedIndex = Items.IndexOf(selectedItem);
            Console.CursorVisible = false;
            if (AfterSelection)
            {
                OnChange();
                Window.Init();
                AfterSelection = false;
            }
            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    UpArrow(selectedItem, selectedIndex);
                    break;
                case ConsoleKey.DownArrow:
                    DownArrow(selectedItem, selectedIndex);
                    break;
                case ConsoleKey.LeftArrow:
                    LeftArrow(selectionOrder);
                    break;
                case ConsoleKey.RightArrow:
                    RightArrow(selectionOrder);
                    break;
                default:
                    Enter(selectedItem);
                    if (!selectedItem.IsActive)
                    {
                        AfterSelection = false;
                    }
                    else
                        AfterSelection = true;
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
    public class TextInputListBuilder : IBuilder
    {
        private TextInputList _product { get; set; }
        private Tuple<TextList, Color> _params { get; set; }

        public void Reset()
        {
            this._product = new TextInputList(_params.Item1, _params.Item2);
            foreach (Paragraph item in _params.Item1.Items)
            {
                _product.Items.Add(new TextInput(item, _params.Item2));
            }
        }

        public TextInputListBuilder(TextList textList, Color color)
        {
            this._params = new Tuple<TextList, Color>(textList, color);
            this.Reset();
        }

        public TextInputList Result()
        {
            TextInputList result = this._product;
            this.Reset();

            result.Window.SelectionOrder.Add(result);
            if (result.Window.SelectionOrder.IndexOf(result) == 0)
                result.Select();

            return result;
        }
    }
}
