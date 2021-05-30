using System;
using System.IO;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;
using System.Text.Json;
using System.Collections.Generic;
using System.Globalization;

namespace CinemaApplication
{
    partial class Program
    {
        static Window addNewTimeSlot = new Window(true);
        static void addTimeSlots()
        {
            var list = new TextListBuilder(addNewTimeSlot, 1, 2)
                .Color(ConsoleColor.Cyan)
                .SetItems("Home/Admin/Movies/List/Timeslot/Add Timeslot/")
                .Result();

            var inputInformation3 = new TextListBuilder(addNewTimeSlot, 1, 3)
                .Color(ConsoleColor.Gray)
                .SetItems("Timeslot date:", "Timeslot hour:")
                .Result();

            var inputList3 = new TextListBuilder(addNewTimeSlot, 16, 3)
                .SetItems("","")
                .AsInput(ConsoleColor.White, ConsoleColor.Black)
                .Result();

            var terug3 = new TextListBuilder(addNewTimeSlot, 1, 6)
                .Color(ConsoleColor.Green)
                .SetItems("Submit", "Go back")
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(null, editMovieList)
                .Result();

            var successMessage3 = new TextListBuilder(addNewTimeSlot, 1, 8)
                .SetItems("")
                .Result();

            terug3[0].OnClick = () =>
            {
                string filePath3 = "..\\..\\..\\TimeSlots.json";
                var root3 = JsonFile.FileAsList(filePath3);
                successMessage3.Clear();

                string[] formats = {"d/M/yyyy h:mm tt",
                   "dd/MM/yyyy hh:mm tt", "d/M/yyyy H:mm",
                   "dd/MM/yyyy HH:mm", "d/M/yyyy H:mm", "d/M/yyyy HH:mm", "d/MM/yyyy HH:mm", "dd/M/yyyy HH:mm"};

                var listOfErrors = new List<string>();
                DateTime someFunkyDate;

                successMessage3.Clear();
                string givenDate = inputList3[0].Value + " " + inputList3[1].Value;
                double someFunkyUnixTime = 0;

                if (inputList3[0].Value == "" || inputList3[1].Value == "") 
                {
                    listOfErrors.Add("Timeslot may not be empty.");
                }
                if (!DateTime.TryParseExact((givenDate), formats, new CultureInfo("nl-NL"), DateTimeStyles.None, out someFunkyDate))
                {
                    listOfErrors.Add("Incorrect datetime format. Please follow dd/MM/yyyy.");
                }
                else
                {
                    someFunkyUnixTime = (double)(((DateTimeOffset)someFunkyDate).ToUnixTimeSeconds());
                    Int32 currently = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (someFunkyUnixTime <= (currently + 43200))
                    {
                        listOfErrors.Add("You are trying to add a timeslot that has already expired or is about to occur.");
                        listOfErrors.Add("Please try a later date.");
                    }
                }

                // string movie = FilmToText(movieid);

                static string FilmToText(int id)
                {
                    string FilePath4 = "..\\..\\..\\Movies.json";
                    var root4 = JsonFile.FileAsList(FilePath4);

                    for (int L = 0; L < root4.Count; L++)
                        if ((root4[L].GetProperty("id").GetInt32()) == id)
                            return root4[L].GetProperty("name").ToString();
                    return id.ToString();
                }
                
                if (listOfErrors.Count != 0)
                {
                    successMessage3.Replace(new TextListBuilder(addNewTimeSlot, 1, 9)
                        .Color(ConsoleColor.Red)
                        .SetItems(listOfErrors.ToArray())
                        .Result());
                }
                else
                {
                    successMessage3.Replace(new TextListBuilder(addNewTimeSlot, 1, 9)
                        .Color(ConsoleColor.Green)
                        .SetItems($"Successfully made a new timeslot for movie: ", $"at time: {someFunkyDate}")
                        .Result());
                }
            };
        }
    }
}