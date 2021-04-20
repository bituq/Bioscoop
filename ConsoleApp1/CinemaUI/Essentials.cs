using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class Instance
    {
        private bool _active { get; set; }
        private List<UIElement> _children = new List<UIElement>();

        public virtual Point Position { get; set; }
        public virtual Point Size { get; set; }
        public string Name { get; set; }
        public string ClassName { get => this.GetType().Name; }
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                foreach (Instance child in Children)
                {
                    child.Active = value;
                }
            }
        }
        public UIElement[] Children
        {
            get
            {
                UIElement[] res = new UIElement[_children.Count];
                for (int i = 0; i < res.Length; i++)
                    res[i] = _children[i];
                return res;
            }
        }

        public void AddChild(UIElement child) => _children.Add(child);
        public void RemoveChild(UIElement child) => _children.Remove(child);
        public void ClearAllChildren() => _children.Clear();
        public bool IsA(string otherName) => ClassName == otherName;
    }

    public class Window : Instance
    {
        internal Dictionary<string, Tuple<int, int, string, Color>> Buffer { get; set; } = new Dictionary<string, Tuple<int, int, string, Color>>();
        internal List<SelectableList> SelectionOrder { get; set; } = new List<SelectableList>();
        internal SelectableList ActiveSelectable { get; set; }

        public Window(bool setAsActive = false)
        {
            Active = setAsActive;
            InputHandler.Windows.Add(this);
        }

        public void Draw()
        {
            foreach (Tuple<int, int, string, Color> cell in Buffer.Values)
            {
                    Console.SetCursorPosition(cell.Item1, cell.Item2);
                    Console.ForegroundColor = cell.Item4.Foreground;
                    Console.BackgroundColor = cell.Item4.Background;
                    Console.Write(cell.Item3);
            }
        }
        public void Init()
        {
            if (ActiveSelectable == null && SelectionOrder.Count != 0)
            {
                ActiveSelectable = SelectionOrder[0];
            }
            foreach (UIElement child in Children)
            {
                child.Init();
            }
            ActiveSelectable?.Items?.Find(item => item.Selected)?.Select();
        }
        internal void CreateCell(string key, Tuple<int, int, string, Color> value) => Buffer[key] = value;
    }
}
