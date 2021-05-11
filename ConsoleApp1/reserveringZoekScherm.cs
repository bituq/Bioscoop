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
        static Window zoekScherm = new Window();
        static void reserveringZoekScherm() 
        {
            
            var terug2 = new TextListBuilder(zoekScherm, 40, 3)
                .SetItems("Ga terug")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(brentScherm)
                .Result();
        }
    }
}