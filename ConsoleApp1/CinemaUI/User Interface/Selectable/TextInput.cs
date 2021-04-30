using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaUI
{
    public class TextInput : Selectable
    {
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
        public void Active()
        {
            IsActive = true;
            Paragraph.Reset();
            Window.ReadLine = ReadLine;
        }
        public void ReadLine()
        {
            Console.SetCursorPosition(Paragraph.Position.X, Paragraph.Position.Y);
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
            Paragraph.Text = Console.ReadLine();
            Window.Variables[Key] = Paragraph.Text;
            Select();
        }
    }
}
