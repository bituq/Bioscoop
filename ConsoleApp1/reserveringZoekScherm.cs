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

            var inputList2 = new TextListBuilder(ZoekScherm, 19, 3)
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
                    static DateTime UnixToDate(int unix)
                    {
                        DateTime date = DateTime.UnixEpoch;
                        date = date.AddSeconds(unix);
                        return date;
                    }
                    static string FilmToText(int id)
                    {
                        string FilePath4 = "..\\..\\..\\Movies.json";
                        var root4 = JsonFile.FileAsList(FilePath4);

                        for (int L = 0; L < root4.Count; L++)
                        {
                            if ((root4[L].GetProperty("id").GetInt32()) == id)
                            {
                                return root4[L].GetProperty("name").ToString();
                            }
                        }
                        return id.ToString();
                    }
                    string code = inputList2[0].Value;
                    bool found = false;
                    for (int j = 0; j < root2.Count; j++)
                    {
                        if ((root2[j].GetProperty("code").ToString()) == code)
                        {
                            found = true;
                            string voornaam = (root2[j].GetProperty("firstName").ToString());
                            string achternaam = (root2[j].GetProperty("lastName").ToString());
                            string zaal = (root2[j].GetProperty("hall").ToString());
                            string stoel = (root2[j].GetProperty("occupiedSeats").ToString());
                            int film = (root2[j].GetProperty("movieId").GetInt32());
                            int datum = (root2[j].GetProperty("date").GetInt32());
                            successMessage2.Replace(
                                new TextListBuilder(ZoekScherm, 1, 8)
                                .Color(ConsoleColor.Green)
                                .SetItems($"The reservation is on the name {voornaam + " " + achternaam}.", $"{voornaam + " " + achternaam} is going to film {FilmToText(film)} in hall {zaal} on seat {stoel}. The film plays on {UnixToDate(datum).ToString("dd/MM/yyyy")}.")
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