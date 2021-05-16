using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CinemaUI;
using CinemaUI.Builder;
namespace CinemaApplication
{
    partial class Program
    {
        static Window brentScherm = new Window();
        static void selectieSchermBrent()
        {
            var title = new TextBuilder(brentScherm, 5, 7)
                .Color(ConsoleColor.Cyan)
                .Text("Hello! Would you like to make a reservation?")
                .Result();
            
            var options = new TextListBuilder(brentScherm, 5, 8)
                .Color(ConsoleColor.Red)
                .SetItems("Make a reservation")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue,ConsoleColor.Red)
                .LinkWindows(maakScherm)
                .Result();
        }
    }
}