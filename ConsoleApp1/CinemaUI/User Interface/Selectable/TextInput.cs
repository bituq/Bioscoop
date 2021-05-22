using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class TextInput : Selectable
    {
        private int offset => Paragraph.Text.Length - 1;
        private Paragraph Paragraph { get; set; }
        public bool IsActive { get; set; } = false;
        public string Key => Paragraph.GetHashCode().ToString();
        public Window Window { get => Paragraph.Window; set => Paragraph.Window = value; }
        public string Value
        {
            get => Paragraph.Window.Variables[Key];
            set => Paragraph.Window.Variables[Key] = value;
        }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public TextInput(Paragraph paragraph, Color color)
        {
            Paragraph = paragraph;
            Foreground = color.Foreground;
            Background = color.Background;
            Window.Variables[Key] = "";
        }

        public override void Unselect()
        {
            Selected = IsActive = false;
            Paragraph.ChangeTextCells(Paragraph.TextColor, ConsoleColor.Black, true);
            Window.ReadLine = () => { };
        }
        public override void Select()
        {
            Selected = true;
            IsActive = false;
            Console.CursorVisible = true;
            Window.FinalCursorPosition = new Point(Paragraph.Position.X + Paragraph.Text.Length, Paragraph.Position.Y);
            Paragraph.ChangeTextCells(Paragraph.TextColor, ConsoleColor.Black, true);
            Window.ReadLine = () => { };
        }

        public void Disable() => Disabled = true;

        public void Enable() => Disabled = false;

        public void Active()
        {
            Paragraph.Reset();
            IsActive = true;
        }
        public void Active(char character)
        {
            Paragraph.Text = character.ToString();
            Paragraph.Init();
            IsActive = true;
        }
        public void ReadLine(char key)
        {
            Console.SetCursorPosition(Paragraph.Position.X + offset, Paragraph.Position.Y);
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
            switch (key)
            {
                case (char)8:
                    RemoveEnd();
                    break;
                case (char)13:
                    break;
                default:
                    if (key != 0)
                        Paragraph.Text += key;
                    break;
            }
            Window.Variables[Key] = Paragraph.Text;
            Select();
        }
        private void RemoveEnd()
        {
            string newText = Paragraph.Text.Substring(0, Math.Max(0, Paragraph.Text.Length - 1));
            Paragraph.Reset();
            Paragraph.Text = newText;
        }
    }
}
