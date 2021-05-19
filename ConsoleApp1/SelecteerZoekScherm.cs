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
        static Window SelecteerZoekScherm = new Window();
        static void SelectieSchermZoeken()
        {
            var title = new TextBuilder(SelecteerZoekScherm, 2, 6)
                .Color(ConsoleColor.Cyan)
                .Text("Hello! Would you like to search a reservation on name or code?")
                .Result();
            
            var options = new TextListBuilder(SelecteerZoekScherm, 5, 8)
                .Color(ConsoleColor.Red)
                .SetItems("Search by code", "Search by name")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue,ConsoleColor.Red)
                .LinkWindows(ZoekScherm, NaamScherm)
                .Result();
                
            var GoBackZoekScherm = new TextListBuilder(SelecteerZoekScherm, 1, 12)
                .Color(ConsoleColor.Green)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(AdminScherm)
                .Result();
        }
    }
}