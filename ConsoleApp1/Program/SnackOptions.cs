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
        static Window snackOptions = new Window();
        static void SnackOptions()
        {
            var title = new TextBuilder(snackOptions, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Text("Home/Admin/Snack Options")
                .Result();

            var title2 = new TextBuilder(snackOptions, 2, 3)
                .Color(ConsoleColor.Gray)
                .Text("Hello! What would you like to do?")
                .Result();

            var options = new TextListBuilder(snackOptions, 2, 5)
                .Color(ConsoleColor.Red)
                .SetItems("Add Snack", "Remove Snack", "Go back")
                .UseNumbers()
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(addSnack, removeSnack, AdminScherm)
                .Result();
        }
    }
}