using System;
using System.Collections.Generic;
using System.Text;

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
        internal Dictionary<string, Tuple<int, int, string, Color>> Buffer { get; set; } = new Dictionary<string, Tuple<int, int, string, Color>>();
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
            foreach (Tuple<int, int, string, Color> cell in Buffer.Values)
            {
                    Console.SetCursorPosition(cell.Item1, cell.Item2);
                    Console.ForegroundColor = cell.Item4.Foreground;
                    Console.BackgroundColor = cell.Item4.Background;
                    Console.Write(cell.Item3);
            }
            ReadLine();
            Console.SetCursorPosition(FinalCursorPosition.X, FinalCursorPosition.Y);
        }
        public void Init()
        {
            Console.CursorVisible = false;
            if (ActiveSelectable == null && SelectionOrder.Count != 0)
                SelectionOrder[0].Select();

            foreach (UIElement child in Children)
                child.Init();
        }
        internal void CreateCell(string key, Tuple<int, int, string, Color> value) => Buffer[key] = value;
    }
}
