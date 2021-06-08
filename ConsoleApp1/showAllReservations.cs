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
        static Window alleResScherm = new Window();
        static void ShowAllRes()
        {
            var list = new TextListBuilder(alleResScherm, 1, 2)
                .Color(Colors.breadcrumbs)
                .SetItems("Home/Admin/Select Search/All Reservations/")
                .Result();

            var terug3 = new TextListBuilder(alleResScherm, 1, 5)
                .Color(Colors.submit.Item1)
                .SetItems("Fetch", "Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(null, SelecteerZoekScherm)
                .Result();

            var successMessage3 = new TextListBuilder(alleResScherm, 1, 5)
                .SetItems("")
                .Result();

            terug3[0].OnClick = () =>
            {
                string filePath3 = "../../../Reserveringen.json";
                var root3 = JsonFile.FileAsList(filePath3);
                successMessage3.Clear();
                static string FilmToText(int id)
                {
                    string FilePath4 = "../../../Movies.json";
                    var root4 = JsonFile.FileAsList(FilePath4);

                    for (int L = 0; L < root4.Count; L++)
                        if ((root4[L].GetProperty("id").GetInt32()) == id)
                            return root4[L].GetProperty("name").ToString();
                    return id.ToString();
                }
                var listOfReservations = new List<string>();
                for (int k = 0; k < root3.Count; k++)
                {
                    string code = (root3[k].GetProperty("code").ToString());
                    string voornaam = (root3[k].GetProperty("firstName").ToString());
                    string achternaam = (root3[k].GetProperty("lastName").ToString());
                    string zaal = (root3[k].GetProperty("hall").ToString());
                    int film = (root3[k].GetProperty("movieId").GetInt32());
                    listOfReservations.AddRange(new string[] { $"{voornaam + " " + achternaam}", $"{FilmToText(film)} in hall {zaal}", $"code: {code}" });
                    listOfReservations.AddRange(new string[2]);
                }
                listOfReservations.Reverse();
                successMessage3.Replace(new TextListBuilder(alleResScherm, 1, 8)
                    .Color(Colors.text)
                    .SetItems(listOfReservations.ToArray())
                    .Result());
            };
        }
    }
}