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
            var title = new TextBuilder(SelecteerZoekScherm, 2, 2)
                .Color(Colors.breadcrumbs)
                .Text("Home/Admin/Select Search/")
                .Result();

            var title2 = new TextBuilder(SelecteerZoekScherm, 2, 3)
                .Color(Colors.text)
                .Text("Hello! Would you like to search a reservation on name or code?")
                .Result();
            
            var options = new TextListBuilder(SelecteerZoekScherm, 2, 5)
                .Color(Colors.selection)
                .SetItems("Search by code", "Search by name", "Show all reservations", "Go back")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(ZoekScherm, NaamScherm, alleResScherm, AdminScherm)
                .Result();
        }
    }
}