using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;
namespace CinemaApplication
{
    partial class Program
    {
        static Window maakScherm = new Window();
        static void reserveringMaakScherm() 
        {
            string filePath = "Reserveringen.json";
            JsonFile.FileAsList(filePath);

            Random rd = new Random();
            string CreateString(int Length)
            {
                const string allowedChars = "0123456789";
                char[] chars = new char[Length];
                for (int i = 0; i < Length; i++)
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
                return new string(chars);
            }
            string randomCode = CreateString(7);

            var inputInformation = new TextListBuilder(maakScherm, 0, 1)
                .Color(ConsoleColor.Cyan)
                .SetItems("Voornaam:", "Achternaam:")
                .Result();

            var inputList = new TextListBuilder(maakScherm, 12, 1)
                .SetItems("", "", "", "")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug = new TextListBuilder(maakScherm, 40, 3)
                .SetItems("Submit", "Ga terug")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(brentScherm)
                .Result();


        }
    }
}