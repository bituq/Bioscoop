using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    partial class Program
    {
        static Window paypal = new Window(true);
        static void PayPal()
        {
            var title = new TextBuilder(paypal, 3, 2)
                .Color(ConsoleColor.Blue)
                .Text("Using PayPal...")
                .Result();
            
            var menu = new TextListBuilder(paypal, 3, 4)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(payments)
                .Result();


        }
    }
}