using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI.Builder;

namespace CinemaUI
{
    public static class InputHandler
    {
        internal static Dictionary<string, Cell> _bufferCache = new Dictionary<string, Cell>();

        public static List<Window> Windows = new List<Window>();
        private static Window CurrentWindow = new Window();
        public static List<Action> OnChangeEvents = new List<Action>();

        public static bool Skip = false;

        public static void WaitForInput()
        {
            foreach (Window window in Windows)
                window.Init();
            while (true)
            {
                Window activeWindow = Windows?.Find(w => w.Active) ?? DefaultDialog();
                if (CurrentWindow != activeWindow)
                    activeWindow.Init();
                activeWindow.Draw();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Black;
                _bufferCache = activeWindow.Buffer;
                if (Skip)
                    Skip = false;
                else if (activeWindow.SelectionOrder.Count != 0)
                    activeWindow.ActiveSelectable.KeyResponse(Console.ReadKey());
                foreach (Action ev in OnChangeEvents)
                    ev();
                CurrentWindow = activeWindow;
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
