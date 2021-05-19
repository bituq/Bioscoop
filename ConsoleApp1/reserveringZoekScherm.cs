using System;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;

namespace CinemaApplication
{
    partial class Program
    {
        static Window ZoekScherm = new Window();
        static void ReserveringZoekScherm()
        {
            string filePath2 = "..\\..\\..\\Reserveringen.json";
            var root2 = JsonFile.FileAsList(filePath2);

            var list = new TextListBuilder(ZoekScherm, 1, 2)
                .Color(ConsoleColor.Cyan)
                .SetItems("Home/Admin/Select Search/Code Search/")
                .Result();

            var inputInformation2 = new TextListBuilder(ZoekScherm, 1, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("Reservationcode:")
                .Result();

            var inputList2 = new TextListBuilder(ZoekScherm, 19, 1)
                .SetItems("")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug2 = new TextListBuilder(ZoekScherm, 1, 5)
                .Color(ConsoleColor.Green)
                .SetItems("Submit", "Go back")
                .Selectable(ConsoleColor.Black,ConsoleColor.White)
                .LinkWindows(null, SelecteerZoekScherm)
                .Result();

            var successMessage2 = new TextListBuilder(ZoekScherm)
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
                                new TextListBuilder(ZoekScherm, 1, 8)
                                .Color(ConsoleColor.Green)
                                .SetItems($"The reservation is on the name {voornaam + " " + achternaam}.", $"{voornaam + " " + achternaam} is going to film {film} in room {zaal} op seat {stoel}. The film plays on {datum}.")
                                .Result()
                            );
                        }
                        else if (found != true) {
                            successMessage2.Replace(
                                new TextListBuilder(ZoekScherm, 1, 8)
                                .Color(ConsoleColor.Red)
                                .SetItems($"There is no reservation with code {code}. Please try again.")
                                .Result()
                            );
                        }
                        ZoekScherm.Init();
                    }
                }
            };
        }
    }
}