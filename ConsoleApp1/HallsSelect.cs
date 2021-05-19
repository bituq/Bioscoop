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
        static Window selecteerHallsScherm = new Window();
        static void SelecteerHallsScherm()
        {
            var title = new TextBuilder(selecteerHallsScherm, 2, 6)
                .Color(ConsoleColor.Cyan)
                .Text("Hello! Would you like to see all halls or edit the halls?")
                .Result();

            var options = new TextListBuilder(selecteerHallsScherm, 5, 8)
                .Color(ConsoleColor.Red)
                .SetItems("See all halls", "Edit halls")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue, ConsoleColor.Red)
                .LinkWindows(hallscreen, null)
                .Result();

            var GoBackHallsScherm = new TextListBuilder(selecteerHallsScherm, 1, 12)
                .Color(ConsoleColor.Green)
                .SetItems("Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(AdminScherm)
                .Result();
        }
    }
}