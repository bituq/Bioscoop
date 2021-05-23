using System;
using System.Collections.Generic;

namespace CinemaUI
{
    public class Instance
    {
        private bool _active { get; set; } = true;
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
        internal Dictionary<string, Cell> Buffer { get; set; } = new Dictionary<string, Cell>();
        internal List<Selectable> SelectionOrder { get; set; } = new List<Selectable>();
        internal Selectable ActiveSelectable { get; set; }
        internal Point FinalCursorPosition { get; set; }
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        private Dictionary<TextInput, Paragraph> LinkedVariables { get; set; } = new Dictionary<TextInput, Paragraph>();
        public Action ReadLine { get; set; } = () => { };

        public Window(bool setAsActive = false)
        {
            Active = setAsActive;
            InputHandler.Windows.Add(this);
        }

        public void AddInFront(Selectable selectable)
        {
            var temp = new Selectable[SelectionOrder.Count + 1];
            temp[0] = selectable;
            for (int i = 0; i < SelectionOrder.Count; i++)
                temp[i + 1] = SelectionOrder[i];
            SelectionOrder = new List<Selectable>(temp);
        }
        public void ReplaceSelectable(Selectable a, Selectable b)
        {
            int index = SelectionOrder.IndexOf(a);
            SelectionOrder.Remove(a);
            SelectionOrder.Remove(b);
            SelectionOrder.Insert(index, b);
        }
        public void LinkTextInput(TextInput textInput, Paragraph paragraph) => LinkedVariables[textInput] = paragraph;
        public void Draw()
        {
            foreach (TextInput key in LinkedVariables.Keys)
            {
                LinkedVariables[key].Reset();
                LinkedVariables[key].Text = Variables[key.Key];
                LinkedVariables[key].Init();
            }
            foreach (Cell cell in Buffer.Values)
            {
                if (cell.Changed)
                {
                    cell.Changed = false;
                    Console.SetCursorPosition(Math.Min(Console.BufferWidth - 1, Math.Max(0, cell.X)), Math.Min(Console.BufferHeight - 1, Math.Max(0, cell.Y)));
                    Console.ForegroundColor = cell.Color.Foreground;
                    Console.BackgroundColor = cell.Color.Background;
                    Console.Write(cell.Content);
                }
            }
            ReadLine();
            Console.SetCursorPosition(FinalCursorPosition.X, FinalCursorPosition.Y);
        }
        public void Init()
        {
            Console.CursorVisible = false;

            foreach (UIElement child in Children)
                child.Init();

            if (Active && ActiveSelectable != null)
                ActiveSelectable.Select();
        }
        internal void CreateCell(string key, Cell value) => Buffer[key] = value;
        internal void RemoveCell(string key) => Buffer.Remove(key);

    }
}
