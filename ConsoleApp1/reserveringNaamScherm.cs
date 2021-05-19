using System;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        static Window NaamScherm = new Window();
        static void ReserveringNaamScherm()
        {
            string filePath3 = "..\\..\\..\\Reserveringen.json";
            var root3 = JsonFile.FileAsList(filePath3);

            var inputInformation3 = new TextListBuilder(NaamScherm, 1, 1)
                .Color(ConsoleColor.Cyan)
                .SetItems("Full name:")
                .Result();

            var inputList3 = new TextListBuilder(NaamScherm, 12, 1)
                .SetItems("")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug3 = new TextListBuilder(NaamScherm, 1, 4)
                .Color(ConsoleColor.Green)
                .SetItems("Submit", "Go back")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(null, SelecteerZoekScherm)
                .Result();

            var successMessage3 = new TextListBuilder(NaamScherm)
                .SetItems("")
                .Result();

            terug3[0].OnClick = () =>
            {
                if (inputList3[0].Value != "")
                {
                    string heleNaam = inputList3[0].Value;
                    bool checker = false;
                    for (int k = 0; k < root3.Count; k++)
                    {
                        if ((root3[k].GetProperty("voorNaam").ToString() + " " + root3[k].GetProperty("achterNaam").ToString()) == heleNaam)
                        {
                            checker = true;
                            string code = (root3[k].GetProperty("reserveringNummer").ToString());
                            string voornaam = (root3[k].GetProperty("voorNaam").ToString());
                            string achternaam = (root3[k].GetProperty("achterNaam").ToString());
                            string zaal = (root3[k].GetProperty("zaal").ToString());
                            string stoel = (root3[k].GetProperty("stoelen").ToString());
                            string film = (root3[k].GetProperty("film").ToString());
                            string datum = (root3[k].GetProperty("datum").ToString());
                            successMessage3.Replace(
                                new TextListBuilder(NaamScherm, 1, 7)
                                .Color(ConsoleColor.Green)
                                .SetItems($"The reservation code is {code}", $"{heleNaam} is going to see {film} in room {zaal} on seat {stoel}. The film plays on {datum}.")
                                .Result()
                            );
                        }
                        else if (checker != true) {
                            successMessage3.Replace(
                                new TextListBuilder(NaamScherm, 1, 7)
                                .Color(ConsoleColor.Red)
                                .SetItems($"There is no reservation on name {heleNaam}, or you misspelled it. Please try again.")
                                .Result()
                            );
                        }
                    }
                }
            };
        }
    }
}