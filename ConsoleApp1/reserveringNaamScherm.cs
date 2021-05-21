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
            

            var list = new TextListBuilder(NaamScherm, 1, 2)
                .Color(ConsoleColor.Cyan)
                .SetItems("Home/Admin/Select Search/Name Search/")
                .Result();

            var inputInformation3 = new TextListBuilder(NaamScherm, 1, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("Full name:")
                .Result();

            var inputList3 = new TextListBuilder(NaamScherm, 12, 3)
                .SetItems("")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug3 = new TextListBuilder(NaamScherm, 1, 5)
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
                    for (int k = 0; k < root3.Count; k++)
                    {
                        if ((root3[k].GetProperty("firstName").ToString() + " " + root3[k].GetProperty("lastName").ToString()) == heleNaam)
                        {
                            checker = true;
                            string code = (root3[k].GetProperty("code").ToString());
                            string voornaam = (root3[k].GetProperty("firstName").ToString());
                            string achternaam = (root3[k].GetProperty("lastName").ToString());
                            string zaal = (root3[k].GetProperty("hall").ToString());
                            string stoel = (root3[k].GetProperty("occupiedSeats").ToString()); 
                            int film = (root3[k].GetProperty("movieId").GetInt32());
                            int datum = (root3[k].GetProperty("date").GetInt32());
                            successMessage3.Replace(
                                new TextListBuilder(NaamScherm, 1, 8)
                                .Color(ConsoleColor.Green)
                                .SetItems($"The reservation code is {code}", $"{heleNaam} is going to see {FilmToText(film)} in hall {zaal} on seat {stoel}. The film plays on {UnixToDate(datum).ToString("dd/MM/yyyy")}.")
                                .Result()
                            );
                        }
                        else if (checker != true) {
                            successMessage3.Replace(
                                new TextListBuilder(NaamScherm, 1, 8)
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