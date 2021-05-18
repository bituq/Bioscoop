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
        static Window AdminScherm = new Window();
        static void SelectieSchermAdmin()
        {
            var title = new TextBuilder(AdminScherm, 5, 7)
                .Color(ConsoleColor.Cyan)
                .Text("Hello! Would you like to search a reservation on name or code?")
                .Result();
            
            var options = new TextListBuilder(AdminScherm, 5, 8)
                .Color(ConsoleColor.Red)
                .SetItems("Search by code", "Search by name")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue,ConsoleColor.Red)
                .LinkWindows(ZoekScherm, NaamScherm)
                .Result();
        }
    }
}