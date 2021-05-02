using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class Container : UIElement
    {
        private Point _size { get; set; }

        public Space ScaleSpace { get; set; } = Space.Absolute;
        public override Point Size
        {
            get => _size.X > 0 && _size.Y > 0 ? _size : new Point(1,1);
            set
            {
                if (ScaleSpace == Space.Absolute)
                    _size = value;
                else if (ScaleSpace == Space.Relative)
                    _size = value + Parent?.Size ?? new Point(0, 0);
            }
        }
        public Color Color { get; set; } = new Color(ConsoleColor.White, ConsoleColor.Black);

        public Container(Window window, int x = 0, int y = 0) : base(window, x, y) { }
        public Container(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute, Space scaleSpace = Space.Absolute) : base(window, parent, x, y, positionSpace)
        {
            ScaleSpace = scaleSpace;
        }

        public override void Init()
        {
            for (int row = Position.Y; row < Position.Y + Size.Y; row++)
            {
                for (int column = Position.X; column < Position.X + Size.X; column++)
                {
                    Window.CreateCell(new Point(column, row).ToString(), Tuple.Create(column, row, " ", Color));
                }
            }
        }
    }
}

namespace CinemaUI.Builder
{
    public class ContainerBuilder : IBuilder
    {
        private Container _product { get; set; }
        private Tuple<Window, UIElement, int, int, Space, Space> _params { get; set; }

        public void Reset()
        {
            this._product = new Container(_params.Item1, _params.Item2, _params.Item3, _params.Item4, _params.Item5, _params.Item6);
        }

        public ContainerBuilder(Window window, int x = 0, int y = 0)
        {
            this._params = new Tuple<Window, UIElement, int, int, Space, Space>(window, null, x, y, Space.Absolute, Space.Absolute);
            this.Reset();
        }
        public ContainerBuilder(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute, Space scaleSpace = Space.Absolute)
        {
            this._params = new Tuple<Window, UIElement, int, int, Space, Space>(window, parent, x, y, positionSpace, scaleSpace);
            this.Reset();
        }

        public ContainerBuilder Size(int width, int height)
        {
            _product.Size = new Point(width, height);
            return this;
        }

        public ContainerBuilder Color(ConsoleColor background)
        {
            _product.Color = new Color(background);
            return this;
        }

        public Container Result()
        {
            Container result = this._product;

            return result;
        }
    }
}