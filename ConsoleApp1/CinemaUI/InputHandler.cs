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
            Console.CursorVisible = false;
            Window activeWindow = Windows?.Find(w => w.Active) ?? DefaultDialog();
            activeWindow.Init();
            activeWindow.Draw();
            while (true)
            {
                activeWindow.ActiveSelectable.KeyResponse(Console.ReadKey());
                _bufferCache = activeWindow.Buffer;
                activeWindow.Draw();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
        private static Window DefaultDialog()
        {
            Window defaultWindow = new Window(true);
            Paragraph _ = new TextBuilder(defaultWindow).Result("This is the default window. No other windows have been made.");
            return defaultWindow;
        }
    }
}
