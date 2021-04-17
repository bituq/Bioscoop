using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI.Builder;

namespace CinemaUI
{
    public static class InputHandler
    {
        public static Window ActiveWindow { get; set; }
        public static void WaitForInput()
        {
            ActiveWindow.Init();
        }
        private static void DefaultDialog()
        {
            Window defaultWindow = new Window(true);
            Paragraph _ = new TextBuilder(defaultWindow).Result("This is the default window. No other windows have been made.");
        }
    }
}
