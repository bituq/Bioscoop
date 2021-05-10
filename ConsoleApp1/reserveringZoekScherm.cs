using System;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        static Window zoekScherm = new Window();
        static void reserveringZoekScherm()
        {
            string filePath2 = @"C:\Users\brent\.vscode\repos\biosapp\Bioscoop\ConsoleApp1\Reserveringen.json";
            var root2 = JsonFile.FileAsList(filePath2);

            var inputInformation2 = new TextListBuilder(zoekScherm, 1, 1)
                .Color(ConsoleColor.Cyan)
                .SetItems("Reserveringscode:")
                .Result();

            var inputList2 = new TextListBuilder(zoekScherm, 19, 1)
                .SetItems("")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug2 = new TextListBuilder(zoekScherm, 1, 4)
                .Color(ConsoleColor.Green)
                .SetItems("Submit", "Ga terug")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(null, brentScherm)
                .Result();

            var successMessage2 = new TextListBuilder(zoekScherm)
                .SetItems("")
                .Result();

            var submitted2 = false;

            terug2[0].OnClick = () =>
            {
                if (!submitted2 && inputList2[0].Value != "")
                {
                    submitted2 = true;
                    string code = inputList2[0].Value;
                    for (int j = 0; j < root2.Count; j++)
                    {
                        if ((root2[j].GetProperty("reserveringNummer").ToString()) == code)
                        {
                            string voornaam = (root2[j].GetProperty("voorNaam").ToString());
                            string achternaam = (root2[j].GetProperty("achterNaam").ToString());
                            string zaal = (root2[j].GetProperty("zaal").ToString());
                            string stoel = (root2[j].GetProperty("stoelen").ToString());
                            string film = (root2[j].GetProperty("film").ToString());
                            string datum = (root2[j].GetProperty("datum").ToString());
                            successMessage2.Replace(
                                new TextListBuilder(zoekScherm, 1, 7)
                                .Color(ConsoleColor.Green)
                                .SetItems($"Uw reservering staat onder de naam {voornaam + " " + achternaam}. U gaat naar de film {film} in zaal {zaal} op stoel {stoel}. De film speelt op {datum}. Tot dan!")
                                .Result()
                            );
                        }
                    }
                }
            };
        }
    }
}