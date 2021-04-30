using System;
using System.Collections.Generic;
using System.IO;

namespace CinemaUI
{
    /* TO DO:
     * - Update Table to consist of textlists instead.
     * - Fix consistency of builder declaration. (i.e. text string and items string[] in .Result())
     * - Fix selectable paragraph builder asking for textColor
     * - Fix bugs on textinput
     */

    public enum Space
    {
        Absolute,
        Relative
    }
    public enum Direction
    {
        Vertical,
        Horizontal
    }
    public class UIElement : Instance
    {
        private Point _position { get; set; }
        private UIElement _parent { get; set; }
        private Window _window { get; set; }

        public UIElement Parent
        {
            get => _parent;
            set
            {
                if (value == null)
                    _parent = null;
                else
                {
                    if (_parent != null)
                        _parent.RemoveChild(this);
                    if (value == this)
                        _parent = null;
                    else
                    {
                        value.AddChild(this);
                        _parent = value;
                    }
                }
            }
        }
        public Window Window
        {
            get => _window;
            set
            {
                if (value == null)
                    _window = null;
                else
                {
                    if (_window != null)
                        _window.RemoveChild(this);
                    value.AddChild(this);
                    _window = value;
                }
            }

        }
        public Space PositionSpace { get; set; } = Space.Absolute;
        public override Point Position
        {
            get => _position;
            set
            {
                if (PositionSpace == Space.Absolute)
                    _position = value;
                else if (PositionSpace == Space.Relative)
                    _position = value + Parent?.Position ?? new Point(0, 0);
            }
        }

        public UIElement(Window window, int x = 0, int y = 0)
        {
            Position = new Point(x, y);
            Window = window;
        }
        public UIElement(Window window, UIElement parent, int x = 0, int y = 0, Space positionSpace = Space.Absolute)
        {
            PositionSpace = positionSpace;
            Window = window;
            Parent = parent;
            Position = new Point(x, y);
        }

        public virtual void Init() { }
        public void Destroy()
        {
            if (Parent != null)
                Parent.RemoveChild(this);
            foreach (UIElement child in Children)
                child.Parent = null;
            Parent = null;
            ClearAllChildren();
        }
    }
}

namespace CinemaUI.Builder
{
    public interface IBuilder
    {
        public void Reset();
    }
}