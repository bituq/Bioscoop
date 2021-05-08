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
        static Window maakScherm = new Window();
        static void reserveringMaakScherm() 
        {
            var inputInformation = new TextListBuilder(maakScherm, 0, 1)
                .Color(ConsoleColor.Cyan)
                .SetItems("Voornaam:", "Achternaam:", "Film:");

            var inputList = new TextListBuilder(maakScherm, 7, 1)
                .SetItems("", "", "", "")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug = new TextListBuilder(maakScherm, 40, 3)
                .SetItems("Ga terug")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(brentScherm)
                .Result();
        }
    }
}