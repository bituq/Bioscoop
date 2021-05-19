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
            var title = new TextBuilder(selecteerHallsScherm, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/Hall Select/")
                .Result();

            var title2 = new TextBuilder(selecteerHallsScherm, 2, 3)
                .Color(ConsoleColor.Gray)
                .Text("Hello! Would you like to see all halls or edit the halls?")
                .Result();

            var options = new TextListBuilder(selecteerHallsScherm, 2, 5)
                .Color(ConsoleColor.Red)
                .SetItems("See all halls", "Edit halls", "Go back")
                .UseNumbers()
                .Selectable(ConsoleColor.DarkBlue, ConsoleColor.Red)
                .LinkWindows(hallscreen, null, AdminScherm)
                .Result();
        }
    }
}