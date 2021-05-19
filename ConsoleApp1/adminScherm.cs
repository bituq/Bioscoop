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
            var title = new TextBuilder(AdminScherm, 2, 6)
                .Color(ConsoleColor.Cyan)
                .Text("Hello! What would you like to do?")
                .Result();
            
            var options = new TextListBuilder(AdminScherm, 5, 8)
                .Color(ConsoleColor.Red)
                .SetItems("Search reservations","Peak hours","Halls","Films","Snacks")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue,ConsoleColor.Red)
                .LinkWindows(SelecteerZoekScherm, null, selecteerHallsScherm, null, null)
                .Result();

            var GoBackAdminScherm = new TextListBuilder(AdminScherm, 1, 14)
                .Color(ConsoleColor.Green)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(homeScreen)
                .Result();
        }
    }
}