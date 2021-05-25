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
            var title = new TextBuilder(AdminScherm, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/")
                .Result();

            var title2 = new TextBuilder(AdminScherm, 2, 3)
                .Color(ConsoleColor.Gray)
                .Text("Hello! What would you like to do?")
                .Result();
            
            var options = new TextListBuilder(AdminScherm, 2, 5)
                .Color(ConsoleColor.Red)
                .SetItems("Search reservations","Peak hours","Halls","Films","Snacks", "Go back")
                .UseNumbers()
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(SelecteerZoekScherm, peaksWindow, selecteerHallsScherm, adminMovieWindow, addSnack, homeScreen)
                .Result();
        }
    }
}