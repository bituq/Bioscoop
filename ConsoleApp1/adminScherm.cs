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
                .Color(Colors.breadcrumbs)
                .Text("Home/Admin/")
                .Result();

            var title2 = new TextBuilder(AdminScherm, 2, 3)
                .Color(Colors.text)
                .Text("Hello! What would you like to do?")
                .Result();
            
            var options = new TextListBuilder(AdminScherm, 2, 5)
                .Color(Colors.selection)
                .SetItems("Search reservations", "Peak hours", "Halls", "Movies", "Snacks", "Go back")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(SelecteerZoekScherm, peaksWindow, selecteerHallsScherm, adminMovieWindow, snackOptions, homeScreen)
                .Result();
        }
    }
}