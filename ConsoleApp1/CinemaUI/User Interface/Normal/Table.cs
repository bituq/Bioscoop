using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI.Builder;

namespace CinemaUI
{
    public class Table : UIElement
    {
        public List<Paragraph> Headers { get; set; } = new List<Paragraph>();
        public List<List<Paragraph>> Items { get; set; } = new List<List<Paragraph>>();
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;

        public Table(Window window, int x = 0, int y = 0) : base(window, x, y) { }
        public Table(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute) : base(window, parent, x, y, positionSpace) { }
    }
}

namespace CinemaUI.Builder
{
    public class TableBuilder : IBuilder
    {
        private Table _product { get; set; }
        private List<int> MaxLengthPerColumn { get; set; } = new List<int>();
        private Tuple<Window, UIElement, int, int, Space> _params { get; set; }

        public void Reset()
        {
            this._product = new Table(_params.Item1, _params.Item2, _params.Item3, _params.Item4, _params.Item5);
        }

        public TableBuilder(Window window, int x = 0, int y = 0)
        {
            this._params = new Tuple<Window, UIElement, int, int, Space>(window, null, x, y, Space.Absolute);
            this.Reset();
        }
        public TableBuilder(Window window, UIElement parent, int x, int y, Space positionSpace)
        {
            this._params = new Tuple<Window, UIElement, int, int, Space>(window, parent, x, y, positionSpace);
            this.Reset();
        }

        public void SetHeaders(ConsoleColor textColor, params string[] arr)
        {
            Paragraph par;
            for (int i = 0; i < arr.Length; i++)
            {
                MaxLengthPerColumn.Add(Int32.MinValue);
                par = new TextBuilder(_product.Window, _product.Position.X + (MaxLengthPerColumn[i] + 2) * i, _product.Position.Y).Result(arr[i]);
                par.TextColor = textColor;
                _product.Headers.Add(par);
            }
        }
        public void AddRow(params string[] arr)
        {
            var row = new List<Paragraph>();
            for (int i = 0; i < _product.Headers.Count; i++)
            {
                row.Add(new TextBuilder(_product.Window, _product.Position.X, _product.Position.Y + _product.Items.Count + 1).Result(arr[i]));
            }
            _product.Items.Add(row);
        }

        public Table Result(ConsoleColor textColor)
        {
            int spacing = 2;
            _product.TextColor = textColor;
            Table result = this._product;
            for (int colIndex = 0; colIndex < _product.Headers.Count; colIndex++)
                for (int rowIndex = 0; rowIndex < _product.Items.Count; rowIndex++)
                    MaxLengthPerColumn[colIndex] = Math.Max(
                        _product.Items[rowIndex][colIndex].Text.Length,
                        Math.Max(
                            _product.Headers[colIndex].Text.Length,
                            MaxLengthPerColumn[colIndex]
                            )
                        );
            int s;
            for (int colIndex = 0; colIndex < _product.Headers.Count; colIndex++)
            {
                s = colIndex == 0 ? 0 : spacing * colIndex;
                for (int rowIndex = 0; rowIndex < _product.Items.Count; rowIndex++)
                {
                    var adjustment = Tuple.Create(_product.Position.X + (MaxLengthPerColumn[Math.Max(colIndex - 1, 0)]) * colIndex, _product.Position.Y + rowIndex + 2);
                    result.Items[rowIndex][colIndex].Position = new Point(adjustment.Item1 + s, adjustment.Item2);
                }
                result.Headers[colIndex].Position = new Point(_product.Position.X + (MaxLengthPerColumn[Math.Max(colIndex - 1, 0)] * colIndex) + s, _product.Position.Y);
            }

            //this.Reset();

            return result;
        }
    }
}