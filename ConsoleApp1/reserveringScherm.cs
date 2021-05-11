using System;
using System.IO;
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
                .Text("Hallo! Wilt U een reservering opzoeken met de reserveringscode of een reservering aanmaken op naam?")
                .Result();
            
            var options = new TextListBuilder(brentScherm, 5, 8)
                .Color(ConsoleColor.Red)
                .SetItems("Reservering opzoeken","Reservering aanmaken")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue,ConsoleColor.Red)
                .LinkWindows(zoekScherm,maakScherm)
                .Result();
        }
    }
}