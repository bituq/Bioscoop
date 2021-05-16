using System;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        static Window naamScherm = new Window();
        static void reserveringNaamScherm()
        {
            string filePath3 = @"C:\Users\brent\.vscode\repos\biosapp\Bioscoop\ConsoleApp1\Reserveringen.json";
            var root3 = JsonFile.FileAsList(filePath3);

            var inputInformation3 = new TextListBuilder(naamScherm, 1, 1)
                .Color(ConsoleColor.Cyan)
                .SetItems("Volledige naam:")
                .Result();

            var inputList3 = new TextListBuilder(naamScherm, 19, 1)
                .SetItems("")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug3 = new TextListBuilder(naamScherm, 1, 4)
                .Color(ConsoleColor.Green)
                .SetItems("Submit", "Ga terug")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(null, adminScherm)
                .Result();

            var successMessage3 = new TextListBuilder(naamScherm)
                .SetItems("")
                .Result();

            terug3[0].OnClick = () =>
            {
                if (inputList3[0].Value != "")
                {
                    string heleNaam = inputList3[0].Value;
                    for (int k = 0; k < root3.Count; k++)
                    {
                        if ((root3[k].GetProperty("voorNaam").ToString() + " " + root3[k].GetProperty("achterNaam").ToString()) == heleNaam)
                        {
                            string code = (root3[k].GetProperty("reserveringNummer").ToString());
                            string voornaam = (root3[k].GetProperty("voorNaam").ToString());
                            string achternaam = (root3[k].GetProperty("achterNaam").ToString());
                            string zaal = (root3[k].GetProperty("zaal").ToString());
                            string stoel = (root3[k].GetProperty("stoelen").ToString());
                            string film = (root3[k].GetProperty("film").ToString());
                            string datum = (root3[k].GetProperty("datum").ToString());
                            successMessage3.Replace(
                                new TextListBuilder(naamScherm, 1, 7)
                                .Color(ConsoleColor.Green)
                                .SetItems($"De reservering staat onder de code {code}. {heleNaam} gaat naar de film {film} in zaal {zaal} op stoel {stoel}. De film speelt op {datum}.")
                                .Result()
                            );
                        }
                        else {
                            successMessage3.Replace(
                                new TextListBuilder(naamScherm, 1, 7)
                                .Color(ConsoleColor.Red)
                                .SetItems($"Er is geen reservering onder de naam {heleNaam}, of U heeft de naam verkeerd geschreven. Probeer het opnieuw.")
                                .Result()
                            );
                        }
                    }
                }
            };
        }
    }
}