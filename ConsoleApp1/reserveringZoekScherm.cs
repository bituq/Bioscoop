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
            string filePath2 = "..\\..\\..\\Reserveringen.json";
            var root2 = JsonFile.FileAsList(filePath2);

            var inputInformation2 = new TextListBuilder(zoekScherm, 1, 1)
                .Color(ConsoleColor.Cyan)
                .SetItems("Reservationcode:")
                .Result();

            var inputList2 = new TextListBuilder(zoekScherm, 19, 1)
                .SetItems("")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug2 = new TextListBuilder(zoekScherm, 1, 4)
                .Color(ConsoleColor.Green)
                .SetItems("Submit", "Go back")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(null, adminScherm)
                .Result();

            var successMessage2 = new TextListBuilder(zoekScherm)
                .SetItems("")
                .Result();

            terug2[0].OnClick = () =>
            {
                if (inputList2[0].Value != "")
                {
                    string code = inputList2[0].Value;
                    bool found = false;
                    for (int j = 0; j < root2.Count; j++)
                    {
                        if ((root2[j].GetProperty("reserveringNummer").ToString()) == code)
                        {
                            found = true;
                            string voornaam = (root2[j].GetProperty("voorNaam").ToString());
                            string achternaam = (root2[j].GetProperty("achterNaam").ToString());
                            string zaal = (root2[j].GetProperty("zaal").ToString());
                            string stoel = (root2[j].GetProperty("stoelen").ToString());
                            string film = (root2[j].GetProperty("film").ToString());
                            string datum = (root2[j].GetProperty("datum").ToString());
                            successMessage2.Replace(
                                new TextListBuilder(zoekScherm, 1, 7)
                                .Color(ConsoleColor.Green)
                                .SetItems($"The reservation is on the name {voornaam + " " + achternaam}.", $"{voornaam + " " + achternaam} is going to film {film} in room {zaal} op seat {stoel}. The film plays on {datum}.")
                                .Result()
                            );
                        }
                        else if (found != true) {
                            successMessage2.Replace(
                                new TextListBuilder(zoekScherm, 1, 7)
                                .Color(ConsoleColor.Red)
                                .SetItems($"There is no reservation with code {code}. Please try again.")
                                .Result()
                            );
                        }
                        zoekScherm.Init();
                    }
                }
            };
        }
    }
}