using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    partial class Program
    {
        static Window payments = new Window();
        static void Payments()
        {
            var MethodsList = new string[] {"IDEAL", "PayPal", "VISA", "Maestro", "MasterCard"};

            var title = new TextBuilder(payments, 3, 2)
                .Color(ConsoleColor.DarkGreen)
                .Text("Payment methods")
                .Result();
            var menu = new TextListBuilder(payments, 3, 4)
                .Color(ConsoleColor.Red)
                .SetItems(MethodsList)
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(ideal, paypal, visa, maestro, mastercard)
                .Result();
        }


        static Window visa = new Window();
        static void VISA()
        {
            var title = new TextBuilder(visa, 3, 2)
                .Color(ConsoleColor.Blue)
                .Text("Using VISA...")
                .Result();

            var menu = new TextListBuilder(visa, 3, 4)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(payments)
                .Result();
        }
        

        static Window maestro = new Window();
        static void Maestro()
        {
            var title = new TextBuilder(maestro, 3, 2)
                .Color(ConsoleColor.Red)
                .Text("Using Maestro...")
                .Result();

            var menu = new TextListBuilder(maestro, 3, 4)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(payments)
                .Result();
        }
        

        static Window mastercard = new Window();
        static void MasterCard()
        {
            var title = new TextBuilder(mastercard, 3, 2)
                .Color(ConsoleColor.DarkYellow)
                .Text("Using MasterCard...")
                .Result();

            var menu = new TextListBuilder(mastercard, 3, 4)
                .Color(ConsoleColor.Red)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(payments)
                .Result();
        }
    }
}