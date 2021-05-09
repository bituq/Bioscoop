using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    partial class Program
    {
        static Window pm = new Window(true);
        static void PM()
        {
            var MethodsList = new string[] {"IDEAL", "PayPal", "VISA", "Meastro", "MasterCard"};

            var PMlist = new TextListBuilder(pm, 3, 2)
                .Color(ConsoleColor.DarkYellow)
                .SetItems(MethodsList)
                .Selectable(ConsoleColor.DarkYellow, ConsoleColor.Yellow)
                .LinkWindows(ideal)
                .Result();
        }
    }
}