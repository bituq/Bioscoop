using System;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;
using System.Text.Json;
using System.Collections.Generic;

namespace CinemaApplication
{
    partial class Program
    {
        static Window ZoekScherm = new Window();
        static void ReserveringZoekScherm()
        {
            var list = new TextListBuilder(ZoekScherm, 1, 2)
                .Color(Colors.breadcrumbs)
                .SetItems("Home/Admin/Select Search/Code Search/")
                .Result();

            var inputInformation2 = new TextListBuilder(ZoekScherm, 1, 3)
                .Color(Colors.input)
                .SetItems("Reservationcode:")
                .Result();

            var inputList2 = new TextListBuilder(ZoekScherm, 19, 3)
                .SetItems("")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug2 = new TextListBuilder(ZoekScherm, 1, 5)
                .Color(Colors.submit.Item1)
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
                    string filePath2 = "..\\..\\..\\Reserveringen.json";
                    var root2 = JsonFile.FileAsList(filePath2);
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
                    var listOfReservations = new List<string>();
                    for (int j = 0; j < root2.Count; j++)
                    {
                        if ((root2[j].GetProperty("code").ToString()) == code)
                        {
                            found = true;
                            string voornaam = (root2[j].GetProperty("firstName").ToString());
                            string achternaam = (root2[j].GetProperty("lastName").ToString());
                            string zaal = (root2[j].GetProperty("hall").ToString());
                            var stoelen = (root2[j].GetProperty("occupiedSeats"));
                            var seatList = new Dictionary<int, string>();
                            foreach (JsonElement stoel in stoelen.EnumerateArray())
                            {
                                string temp = "";
                                int row = stoel.GetProperty("row").GetInt32();
                                if (!seatList.ContainsKey(row))
                                    seatList[row] = $"Row {row} - ";
                                seatList[row] += $"{temp}seat {stoel.GetProperty("column")} ";
                            }
                            int film = (root2[j].GetProperty("movieId").GetInt32());
                            int datum = (root2[j].GetProperty("date").GetInt32());
                            listOfReservations.AddRange(new string[] { $"The reservation is on the name {voornaam + " " + achternaam}.", $"The film plays on { UnixToDate(datum).ToString("dd/MM/yyyy") }.", $"{voornaam + " " + achternaam} is going to the film {FilmToText(film)} in hall {zaal} on seat(s): " });
                            listOfReservations.AddRange(seatList.Values);
                            listOfReservations.AddRange(new string[2]);
                            successMessage2.Replace(
                                new TextListBuilder(ZoekScherm, 1, 8)
                                .Color(Colors.text)
                                .SetItems(listOfReservations.ToArray())
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