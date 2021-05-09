using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    partial class Program
    {
        static Window payments = new Window(true);
        static void Payments()
        {
            var MethodsList = new string[] {"IDEAL", "PayPal", "VISA", "Meastro", "MasterCard"};

            var title = new TextBuilder(payments, 3, 2)
                .Color(ConsoleColor.DarkGreen)
                .Text("Payment methods")
                .Result();
            var menu = new TextListBuilder(payments, 3, 4)
                .Color(ConsoleColor.DarkYellow)
                .SetItems(MethodsList)
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(ideal, paypal)
                .Result();
        }
    }
}