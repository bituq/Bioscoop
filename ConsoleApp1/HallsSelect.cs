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
                .Color(Colors.breadcrumbs)
                .Text("Home/Admin/Hall Select/")
                .Result();

            var title2 = new TextBuilder(selecteerHallsScherm, 2, 3)
                .Color(Colors.text)
                .Text("Would you like to view or add halls?")
                .Result();

            var options = new TextListBuilder(selecteerHallsScherm, 2, 5)
                .Color(Colors.selection)
                .SetItems("View / remove halls", "Add halls", "Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(hallscreen, addhallscreen, AdminScherm)
                .Result();
        }
    }
}