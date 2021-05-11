using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;

namespace Themes
{
    static class DefaultColors
    {
        public static Tuple<ConsoleColor, Color> NavigationMenu = new Tuple<ConsoleColor, Color>(
            ConsoleColor.Yellow, new Color(ConsoleColor.Yellow, ConsoleColor.DarkGray)
            );

        public static Tuple<ConsoleColor, Color> SelectableList = new Tuple<ConsoleColor, Color>(
            ConsoleColor.Gray, new Color(ConsoleColor.Black, ConsoleColor.White)
            );

        public static ConsoleColor InputInformation = ConsoleColor.DarkGray;

        public static ConsoleColor Title = ConsoleColor.Magenta;

        public static ConsoleColor Subtitle = ConsoleColor.DarkMagenta;

        public static Tuple<ConsoleColor, Color> Input = new Tuple<ConsoleColor, Color>(
            ConsoleColor.White,
            new Color(ConsoleColor.White)
            );
    }
}
