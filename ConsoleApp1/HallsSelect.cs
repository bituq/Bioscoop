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
                .Text("Would you like to view or add halls?")
                .Result();

            var options = new TextListBuilder(selecteerHallsScherm, 2, 5)
                .Color(ConsoleColor.Red)
                .SetItems("View halls", "Add halls", "Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(hallscreen, addhallscreen, AdminScherm)
                .Result();
        }
    }
}