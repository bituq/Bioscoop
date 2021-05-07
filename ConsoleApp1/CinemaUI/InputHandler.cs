using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI.Builder;

namespace CinemaUI
{
    public static class InputHandler
    {
        internal static Dictionary<string, Tuple<int, int, string, Color>>  _bufferCache = new Dictionary<string, Tuple<int, int, string, Color>>();

        public static List<Window> Windows = new List<Window>();

        public static void WaitForInput()
        {
            foreach (Window window in Windows)
            {
                Console.WriteLine($"Loading {(Windows.IndexOf(window) + 1) / Windows.Count * 100}%");
                window.Init();
                Console.Clear();
            }
            Console.CursorVisible = false;
            Window activeWindow = Windows?.Find(w => w.Active) ?? DefaultDialog();
            activeWindow.Draw();
            while (true)
            {
                activeWindow = Windows?.Find(w => w.Active);
                activeWindow.Draw();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Black;
                if (activeWindow.SelectionOrder.Count != 0)
                    activeWindow.ActiveSelectable.KeyResponse(Console.ReadKey());
                _bufferCache = activeWindow.Buffer;
            }
        }
        private static Window DefaultDialog()
        {
            Window defaultWindow = new Window(true);
            Paragraph temp = new TextBuilder(defaultWindow, 3, 3)
                .Color(ConsoleColor.Red)
                .Text("This is the default window. No other windows have been made.")
                .Result();

            defaultWindow.Init();
            return defaultWindow;
        }
    }
}
