using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class TextList : UIElement
    {
        public List<Paragraph> Items { get; set; } = new List<Paragraph>();
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
        public TextList(Window window, int x = 0, int y = 0) : base(window, x, y) { }
        public TextList(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute) : base(window, parent, x, y, positionSpace) { }

        public void Replace(TextList textList)
        {
            foreach (Paragraph P in Items)
                P.Reset();
            Items = textList.Items;
            TextColor = textList.TextColor;
            foreach (Paragraph P in Items)
                P.Init();
        }
        public void SetItems(string[] arr, string prefix = "", string suffix = "")
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Items.Add(new Paragraph(Window, Position.X, Position.Y + i));
                Items[i].TextColor = TextColor;
                Items[i].Text = arr[i];
                Items[i].Prefix = prefix;
                Items[i].Suffix = suffix;
                Items[i].Init();
            }
        }
        public void SetItems(string[] arr, bool UseNumbers, string suffix = "")
        {
            Items.Clear();
            for (int i = 0; i < arr.Length; i++)
            {
                Items.Add(new Paragraph(Window, Position.X, Position.Y + i));
                Items[i].TextColor = TextColor;
                Items[i].Text = arr[i];
                if (UseNumbers)
                    Items[i].Prefix = $"{i + 1}. ";
                Items[i].Suffix = suffix;
                Items[i].Init();
            }
        }

        public void Clear()
        {
            foreach (Paragraph p in Items)
                p.Destroy();
        }
    }
}

namespace CinemaUI.Builder
{
    public class TextListBuilder : IBuilder
    {
        private TextList _product { get; set; }
        private Tuple<Window, UIElement, int, int, Space> _params { get; set; }
        private bool useNumbers { get; set; } = false;
        private string[] Items { get; set; } = new string[] { "" };

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

        public TextListBuilder Color(ConsoleColor textColor)
        {
            _product.TextColor = textColor;
            return this;
        }

        public SelectableGroupBuilder Selectable(ConsoleColor foreground, ConsoleColor background)
        {
            _product.SetItems(Items, useNumbers);
            return new SelectableGroupBuilder(_product, new Color(foreground, background));
        }

        public TextInputListBuilder AsInput(ConsoleColor foreground, ConsoleColor background)
        {
            this._product.SetItems(Items, useNumbers);
            return new TextInputListBuilder(this._product, new Color(foreground, background));
        }

        public TextListBuilder UseNumbers(bool trueOrFalse)
        {
            useNumbers = trueOrFalse;
            return this;
        }
        public TextListBuilder UseNumbers()
        {
            useNumbers = true;
            return this;
        }

        public TextListBuilder SetItems(params string[] items)
        {
            Items = items;
            return this;
        }

        public TextList Result()
        {
            _product.SetItems(Items, useNumbers);

            TextList result = this._product;

            this.Reset();

            return result;
        }
    }
}