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
        static Window adminScherm = new Window(true);
        static void selectieSchermAdmin()
        {
            var title = new TextBuilder(adminScherm, 5, 7)
                .Color(ConsoleColor.Cyan)
                .Text("Hello! Would you like to search a reservation on name or code?")
                .Result();
            
            var options = new TextListBuilder(adminScherm, 5, 8)
                .Color(ConsoleColor.Red)
                .SetItems("Search by code", "Search by name")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue,ConsoleColor.Red)
                .LinkWindows(zoekScherm, naamScherm)
                .Result();
        }
    }
}