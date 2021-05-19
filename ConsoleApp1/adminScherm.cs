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
        static Window AdminScherm = new Window(true);
        static void SelectieSchermAdmin()
        {
            var title = new TextBuilder(AdminScherm, 5, 7)
                .Color(ConsoleColor.Cyan)
                .Text("Hello! What would you like to do?")
                .Result();
            
            var options = new TextListBuilder(AdminScherm, 5, 8)
                .Color(ConsoleColor.Red)
                .SetItems("Search reservations")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue,ConsoleColor.Red)
                .LinkWindows(SelecteerZoekScherm)
                .Result();
        }
    }
}