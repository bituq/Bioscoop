using System;
using System.Collections.Generic;
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
            string filePath = "..\\..\\..\\Reserveringen.json";
            var root = JsonFile.FileAsList(filePath);

            var inputInformation = new TextListBuilder(maakScherm, 1, 1)
                .Color(ConsoleColor.Cyan)
                .SetItems("Voornaam:", "Achternaam:")
                .Result();

            var inputList = new TextListBuilder(maakScherm, 13, 1)
                .SetItems("", "")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug = new TextListBuilder(maakScherm, 1, 4)
                .Color(ConsoleColor.Green)
                .SetItems("Submit", "Ga terug")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(null, brentScherm)
                .Result();

            var successMessage = new TextListBuilder(maakScherm)
                .SetItems("")
                .Result();

            bool submitted = false;

            terug[0].OnClick = () =>
            {
                if (!submitted && inputList[0].Value != "" && inputList[1].Value != "")
                {
                    submitted = true;
                    Random rd = new Random();
                    string CreateString(int Length)
                    {
                        const string allowedChars = "0123456789";
                        char[] chars = new char[Length];
                        for (int i = 0; i < Length; i++)
                            chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                        return new string(chars);
                    }
                    string randomCode = CreateString(7);
                    var existingCodes = new List<string>();

                    for (int i = 0; i < root.Count; i++)
                        existingCodes.Add(root[i].GetProperty("reserveringNummer").ToString());

                    while (existingCodes.Exists(existingCode => existingCode == randomCode))
                        randomCode = CreateString(7);

                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    string unixTime = unixTimestamp.ToString();

                    successMessage.Replace(
                        new TextListBuilder(maakScherm, 1, 7)
                        .Color(ConsoleColor.Gray)
                        .SetItems($"U gaat naar de film. Uw reserveringscode is { randomCode }. Sla de code goed op!")
                        .Result()
                        );
                }
            };
        }
    }
}