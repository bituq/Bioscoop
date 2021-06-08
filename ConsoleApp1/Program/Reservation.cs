using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;
using JsonHandler;
using System.Text.Json;
using System.IO;
using System.Globalization;

namespace CinemaApplication
{
    partial class Program
    {
        public class Reservation
        {
            private class Information
            {
                public string firstName { get; set; }
                public string lastName { get; set; }
                public string code { get; set; }
                public int hall { get; set; }
                public int movieId { get; set; }
                public int date { get; set; }
                public JsonElement[] occupiedSeats { get; set; }
                public int timeslotId { get; set; }
            }
            public Window Window { get; set; } = new Window();
            public Window PaymentsWindow { get; set; } = new Window();
            public Window FoodWindow { get; set; } = new Window();
            public Window ideal { get; set; } = new Window();
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int ReservationNumber { get; set; }
            public Hall Hall { get; set; }
            public TimeSlot TimeSlot { get; set; }
            public int DateOfReservation { get; set; }
            public List<Seat> Seats { get; set; }
            private bool submitted = false;
            double sumM = 0.00;
            public void ReservationInit()
            {
                string filePath = "..\\..\\..\\Reserveringen.json";
                var root = JsonFile.FileAsList(filePath);

                var terug = new TextListBuilder(Window, 1, 1)
                    .Color(Colors.back)
                    .SetItems("Go Back", "Submit")
                    .Selectable(Colors.backBg.Item1, Colors.backBg.Item2)
                    .LinkWindows(PaymentsWindow)
                    .Result();

                var title = new TextBuilder(Window, 11, 1)
                    .Color(Colors.title)
                    .Text("Make a reservation for " + TimeSlot.Movie.Name)
                    .Result();

                var inputInformation = new TextListBuilder(Window, 11, 3)
                    .SetItems("First Name:", "Last Name:", "E-mail adress:")
                    .Result();

                var inputList = new TextListBuilder(Window, 12 + inputInformation.Items[2].Text.Length, 3)
                    .SetItems("", "", "")
                    .AsInput(Colors.inputBg.Item1, Colors.inputBg.Item2)
                    .Result();

                string seatList = "";
                foreach (Seat seat in Seats)
                {
                    string temp = "";
                    if (!seatList.Contains($"Row {seat.Row}"))
                        temp = $"\nRow {seat.Row} - ";
                    seatList += $"{temp}stoel {seat.Column} ";
                }

                var additionalInformation = new TextListBuilder(Window, 11, 8)
                    .Color(ConsoleColor.DarkGray)
                    .SetItems($"Cost: ${Math.Round(sumM, 2)}", "Hall: " + TimeSlot.Hall.Id, "Seats: " + seatList)
                    .Result();

                var successMessage = new TextListBuilder(Window)
                    .SetItems("")
                    .Result();

                terug[1].OnClick = () =>
                {
                    if (!submitted && inputList[0].Value != "" && inputList[1].Value != "" && inputList[2].Value != "")
                    {
                        submitted = true;
                        Random rd = new Random();
                        string CreateString(int Length)
                        {
                            const string allowedChars = "0123456789";
                            char[] chars = new char[Length];
                            for (int i = 0; i < Length; i++)
                                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                            return new string(chars);
                        }
                        string randomCode = CreateString(7);
                        var existingCodes = new List<string>();

                        for (int i = 0; i < root.Count; i++)
                            existingCodes.Add(root[i].GetProperty("code").ToString());

                        while (existingCodes.Exists(existingCode => existingCode == randomCode))
                            randomCode = CreateString(7);

                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        string unixTime = unixTimestamp.ToString();

                        var info = new Information();
                        info.firstName = inputList[0].Value;
                        info.lastName = inputList[1].Value;
                        info.code = randomCode;
                        info.hall = Hall.Id;
                        info.movieId = TimeSlot.Movie.Id;
                        info.date = unixTimestamp;
                        var temp = new List<JsonElement>();
                        foreach (Seat seat in Seats)
                            temp.Add(JsonDocument.Parse(JsonSerializer.Serialize(seat, JsonFile.options)).RootElement);
                        info.occupiedSeats = temp.ToArray();
                        info.timeslotId = TimeSlot.id;
                        JsonFile.AppendToFile(info, filePath);

                        inputList.Disabled = true;

                        successMessage.Replace(
                            new TextListBuilder(Window, 1, 12)
                            .Color(ConsoleColor.Green)
                            .SetItems("Reservation has been submitted. Your reservation will be sent to you through e-mail.")
                            .Result()
                            );
                        terug.Replace(
                            new TextListBuilder(Window, 1, 1)
                            .Color(Colors.submit.Item1)
                            .SetItems("Go back", "Submit")
                            .Selectable(Colors.submit.Item2, Colors.submit.Item3)
                            .Result()
                            );
                        terug[0].Disable();
                        terug[1].Disable();

                        var finish = new TextListBuilder(Window, 1, 14)
                        .Color(Colors.submit.Item1)
                        .SetItems("Finish")
                        .Selectable(Colors.submit.Item2, Colors.submit.Item3)
                        .LinkWindows(TimeSlot.Movie.Window)
                        .Result();
                        terug.Unselect();
                        finish.Select();

                        Window.Init();

                        //VincentCooleQOLFuncties.EmailUser(inputList[2].Value, randomCode, TimeSlot, seatList, sum);
                        TimeSlot.Window.Reset();

                        string timeSlotPath = "..\\..\\..\\TimeSlots.json";
                        var thisTimeslot = JsonFile.FileAsList(timeSlotPath).Find(element => element.GetProperty("id").GetInt32() == TimeSlot.id);
                        var newTimeslot = new TimeSlot.Information();
                        newTimeslot.id = thisTimeslot.GetProperty("id").GetInt32();
                        newTimeslot.movieId = thisTimeslot.GetProperty("movieId").GetInt32();
                        newTimeslot.time = thisTimeslot.GetProperty("time").GetInt32();
                        newTimeslot.hall = thisTimeslot.GetProperty("hall").GetInt32();
                        var temp2 = new List<JsonElement>();
                        temp2.InsertRange(0, thisTimeslot.GetProperty("occupiedSeats").EnumerateArray());
                        foreach (Seat seat in Seats)
                            temp2.Add(JsonDocument.Parse(JsonSerializer.Serialize(seat, JsonFile.options)).RootElement);
                        newTimeslot.occupiedSeats = temp2.ToArray();
                        JsonFile.RemoveFromFile("id", thisTimeslot.GetProperty("id").GetInt32(), timeSlotPath);
                        JsonFile.AppendToFile(newTimeslot, timeSlotPath);

                        TimeSlot.Init();
                        finish[0].OnClick = () => {
                            NaamScherm.Reset();
                            ReserveringNaamScherm();
                            NaamScherm.Init();
                            ZoekScherm.Reset();
                            ReserveringZoekScherm();
                            ZoekScherm.Init();
                            alleResScherm.Reset();
                            ShowAllRes();
                            alleResScherm.Init();
                        };

                    }
                    else
                    {
                        successMessage.Replace(
                            new TextListBuilder(Window, 1, 12)
                            .Color(ConsoleColor.Red)
                            .SetItems("The fields may not be empty. You must fill in all the required information.")
                            .Result()
                            );
                    }
                };
            }
            public Reservation(TimeSlot TimeSlot, Window PreviousWindow, List<Seat> Seats, double Sum = 0)
            {
                this.TimeSlot = TimeSlot;
                this.Seats = Seats;
                this.Hall = TimeSlot.Hall;
                this.sumM = Sum;

                Payments();
                FoodWindows();
            }

            private void Payments()
            {
                IDEAL();

                var MethodsList = new string[] { "IDEAL", "PayPal", "VISA", "Maestro", "MasterCard" };

                var title = new TextBuilder(PaymentsWindow, 11, 1)
                    .Color(ConsoleColor.Red)
                    .Text("Select a payment method.")
                    .Result();
                var goBack = new TextListBuilder(PaymentsWindow, 1, 1)
                    .Color(Colors.back)
                    .SetItems("Go back")
                    .Selectable(Colors.backBg.Item1, Colors.backBg.Item2)
                    .LinkWindows(FoodWindow)
                    .Result();
                var menu = new TextListBuilder(PaymentsWindow, 11, 3)
                    .Color(Colors.selection)
                    .SetItems(MethodsList)
                    .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                    .LinkWindows(ideal, Window, Window, Window, Window)
                    .Result();
                
            }

            private void IDEAL()
            {
                var IDEALlist = new string[] { "ING", "ABN-AMRO", "Rabobank", "RegioBank", "SNS bank", "knab"};
                var title = new TextBuilder(ideal, 11, 1)
                    .Color(ConsoleColor.Red)
                    .Text("Choose your bank.")
                    .Result();

                var goBack = new TextListBuilder(ideal, 1, 1)
                    .Color(Colors.back)
                    .SetItems("Go back")
                    .Selectable(Colors.backBg.Item1, Colors.backBg.Item2)
                    .LinkWindows(PaymentsWindow)
                    .Result();

                var menu = new TextListBuilder(ideal, 11, 3)
                    .Color(Colors.selection)
                    .SetItems(IDEALlist)
                    .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                    .LinkWindows(Window, Window, Window, Window, Window, Window)
                    .Result();
            }
            public class Food
            {
                public Window FoodInfo { get; set; } = new Window();
                public string Name { get; set; }


                public Food(string name, string price, string vegetarian, string stock, Window previousWindow)
                {
                    Name = name;

                    var title = new TextBuilder(FoodInfo, 11, 1)
                        .Color(Colors.title)
                        .Text(name)
                        .Result();

                    var information = new TextListBuilder(FoodInfo, 11, 3)
                        .Color(Colors.description)
                        .SetItems(price, vegetarian, stock)
                        .Result();

                    var goBack = new TextListBuilder(FoodInfo, 1, 1)
                       .Color(Colors.selection)
                       .SetItems("Go back")
                       .Selectable(Colors.selectionBg.Item1, Colors.selectionBg.Item2)
                       .LinkWindows(previousWindow)
                       .Result();
                }
            }
            private void FoodWindows()
            {
                var goBack = new TextListBuilder(FoodWindow, 1, 1)
                       .Color(Colors.back)
                       .SetItems("Go back")
                       .Selectable(Colors.backBg.Item1, Colors.backBg.Item1)
                       .LinkWindows(TimeSlot.Window)
                       .Result();

                goBack[1].OnClick = () =>
                {
                    this.Window.Reset();
                    ReservationInit();
                    this.Window.Init();
                };

                var snacksAndDrinks = File.ReadAllText("..\\..\\..\\snacksAndDrinks.json");

                JsonDocument doc = JsonDocument.Parse(snacksAndDrinks);
                JsonElement root = doc.RootElement;

                Food[] snackObjects = new Food[root.GetArrayLength()];
                Window[] foodWindows = new Window[snackObjects.Length];
                string[] snackNames = new string[root.GetArrayLength()];
                string[] snackPrice = new string[root.GetArrayLength()];
                double[] snackDouble = new double[root.GetArrayLength()];
                var addbuttonarray = new string[snackNames.Length];
                var removebuttonlist = new List<string>() { };
                var cartlist = new List<string>() { };
                var cartpricelist = new List<string> { };
                var infobuttonlist = new List<string> { };
                var sumpricelist = new List<int> { };
                double sum = 0.00;
                for (int i = 0; i < snackNames.Length; i++)
                {
                    snackObjects[i] = new Food(
                        root[i].GetProperty("name").ToString(),
                        $"Price: ${root[i].GetProperty("price")}",
                        $"Vegetarian: {root[i].GetProperty("vegetarian")}",
                        $"Stock: {root[i].GetProperty("stock")}",
                        FoodWindow
                        );

                    foodWindows[i] = snackObjects[i].FoodInfo;
                    snackNames[i] = snackObjects[i].Name;
                    snackPrice[i] = $"{root[i].GetProperty("price").ToString()}";
                    snackDouble[i] = root[i].GetProperty("price").GetDouble();
                    addbuttonarray[i] = "Add to cart";
                    infobuttonlist.Add("Info");
                }

                var title1 = new TextBuilder(FoodWindow, 11, 1)
                    .Color(ConsoleColor.Red)
                    .Text("Snacks and drinks:")
                    .Result();

                var title2 = new TextBuilder(FoodWindow, 70, 1)
                    .Color(ConsoleColor.Red)
                    .Text("My cart:")
                    .Result();
                var title3 = new TextBuilder(FoodWindow, 12, snackNames.Length + 10)
                    .Color(ConsoleColor.Red)
                    .Text("Movie total :")
                    .Result();
                var title4 = new TextBuilder(FoodWindow, 11, snackNames.Length + 11)
                    .Color(ConsoleColor.Red)
                    .Text("Snacks total :")
                    .Result();
                var title5 = new TextBuilder(FoodWindow, 18, snackNames.Length + 12)
                    .Color(ConsoleColor.Red)
                    .Text("Total :")
                    .Result();

                var snackList = new TextListBuilder(FoodWindow, 11, 4)
                    .Color(ConsoleColor.White)
                    .SetItems(snackNames)
                    .Result();

                var snackPrices = new TextListBuilder(FoodWindow, 35, 4)
                    .Color(ConsoleColor.White)
                    .SetItems(snackPrice)
                    .Result();

                var addButton = new TextListBuilder(FoodWindow, 46, 4)
                    .Color(ConsoleColor.Green)
                    .SetItems(addbuttonarray)
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .Result();

                var infoButton = new TextListBuilder(FoodWindow, 61, 4)
                    .Color(ConsoleColor.Green)
                    .SetItems(infobuttonlist.ToArray())
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .LinkWindows(foodWindows)
                    .Result();

                var shopcart = new TextListBuilder(FoodWindow, 70, 4)
                    .Color(ConsoleColor.White)
                    .SetItems(cartlist.ToArray())
                    .Result();

                var shopcartprice = new TextListBuilder(FoodWindow, 94, 4)
                    .Color(ConsoleColor.White)
                    .SetItems(cartpricelist.ToArray())
                    .Result();

                var removebutton = new TextListBuilder(FoodWindow, 105, 4)
                        .Color(ConsoleColor.White)
                        .SetItems("")
                        .Selectable(ConsoleColor.Black, ConsoleColor.White)
                        .Result();

               

                var totalMovie = new TextListBuilder(FoodWindow, 28, snackNames.Length + 10)
                    .Color(ConsoleColor.White)
                    .SetItems($"${Math.Round(sumM, 2)}")
                    .Result();
                
                var totalSnack = new TextListBuilder(FoodWindow, 28, snackNames.Length + 11)
                    .Color(ConsoleColor.White)
                    .SetItems($"${sum}")
                    .Result();

                var total = new TextListBuilder(FoodWindow, 28, snackNames.Length + 12)
                    .Color(ConsoleColor.White)
                    .SetItems($"${sum + sumM}")
                    .Result();



                void onRemove()
                {
                    var removeIndex = removebutton.SelectedIndex;

                    if (cartpricelist.Count > 0)
                        sum -= Convert.ToDouble(cartpricelist[removeIndex], new CultureInfo("en-US"));
                    else
                        sum = 0;

                    cartlist.RemoveAt(removeIndex);
                    cartpricelist.RemoveAt(removeIndex);
                    removebuttonlist.RemoveAt(removeIndex);

                    bool isEmpty = false;
                    if (removebuttonlist.Count == 0)
                    {
                        isEmpty = true;
                        removebuttonlist.Add("");
                        removebutton[0].Unselect();
                    }

                    shopcart.Replace(new TextListBuilder(FoodWindow, 70, 4)
                    .Color(ConsoleColor.White)
                    .SetItems(cartlist.ToArray())
                    .Result());

                    shopcartprice.Replace(new TextListBuilder(FoodWindow, 94, 4)
                    .Color(ConsoleColor.White)
                    .SetItems(cartpricelist.ToArray())
                    .Result());

                    removebutton[removeIndex].Unselect();

                    removebutton.Replace(new TextListBuilder(FoodWindow, 105, 4)
                    .Color(ConsoleColor.Green)
                    .SetItems(removebuttonlist.ToArray())
                    .Selectable(ConsoleColor.Black, ConsoleColor.White)
                    .Result());

                    totalSnack.Replace(new TextListBuilder(FoodWindow, 28, snackNames.Length + 11)
                    .Color(ConsoleColor.White)
                    .SetItems($"${Math.Round(sum, 2)}")
                    .Result());

                    total.Replace(new TextListBuilder(FoodWindow, 28, snackNames.Length + 12)
                    .Color(ConsoleColor.White)
                    .SetItems($"${Math.Round(sum + sumM, 2)}")
                    .Result());

                    if (!isEmpty)
                    {
                        var _ = removebutton.Items.Count;
                        removebutton[0].Unselect();
                        removebutton[Math.Min(removeIndex, removebutton.Items.Count - 1)].Select();

                        foreach (SelectableText Item in removebutton.Items)
                            Item.OnClick = onRemove;
                    }
                    else
                    {
                        FoodWindow.ActiveSelectable = addButton;
                        addButton[0].Select();
                    }
                }

                for (int i = 0; i < addbuttonarray.Length; i++)
                {
                    void OnAdd()
                    {
                        var addIndex = addButton.SelectedIndex;
                        cartlist.Add(snackNames[addIndex]);
                        cartpricelist.Add(snackPrice[addIndex]);
                        if (removebuttonlist.Contains(""))
                            removebuttonlist.Remove("");

                        removebuttonlist.Add("Remove");

                        sum += Convert.ToDouble(snackDouble[addIndex]);
                        
                        shopcart.Replace(new TextListBuilder(FoodWindow, 70, 4)
                        .Color(ConsoleColor.White)
                        .SetItems(cartlist.ToArray())
                        .Result());

                        shopcartprice.Replace(new TextListBuilder(FoodWindow, 94, 4)
                        .Color(ConsoleColor.White)
                        .SetItems(cartpricelist.ToArray())
                        .Result());

                        removebutton.Replace(new TextListBuilder(FoodWindow, 105, 4)
                        .Color(ConsoleColor.Green)
                        .SetItems(removebuttonlist.ToArray())
                        .Selectable(ConsoleColor.Black, ConsoleColor.White)
                        .Result());

                        totalSnack.Replace(new TextListBuilder(FoodWindow, 28, snackNames.Length + 11)
                        .Color(ConsoleColor.White)
                        .SetItems($"${Math.Round(sum, 2)}")
                        .Result());

                        total.Replace(new TextListBuilder(FoodWindow, 28, snackNames.Length + 12)
                        .Color(ConsoleColor.White)
                        .SetItems($"${Math.Round(sum + sumM, 2)}")
                        .Result());

                        FoodWindow.ActiveSelectable = addButton;

                        foreach (SelectableText item in removebutton.Items)
                            item.OnClick = onRemove;
                    }
                    addButton[i].OnClick = OnAdd;
                }
                var goContinue = new TextListBuilder(FoodWindow, 1, 22)
                       .Color(ConsoleColor.Yellow)
                       .SetItems("Continue")
                       .Selectable(ConsoleColor.Black, ConsoleColor.White)
                       .LinkWindows(PaymentsWindow)
                       .Result();
            }
        }
    }
}
