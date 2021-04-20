using System;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    class Program
    {
        static void MainScreen()
        {
            var w2 = new Window();
            var w = new Window(true);
            var list1 = new TextListBuilder(w, 1, 1).Selectable(
                    textColor: ConsoleColor.White,
                    selectionColor: new Color(ConsoleColor.White, ConsoleColor.DarkBlue),
                    useNumbers: true,
                     "Pizza", "Patat", "Pannekoek", "Soep", "Stampot"
                    )
                .Result();
            var list2 = new TextListBuilder(w, 20, 2).Selectable(
                textColor: ConsoleColor.White,
                selectionColor: new Color(ConsoleColor.Black, ConsoleColor.White),
                useNumbers: true,
                "Groente", "Fruit", "Vlees", "Vegetarisch", "Drinken"
                )
                .LinkWindows(w2, w2)
                .Result();

            var list3 = new TextListBuilder(w2, 5, 5).Selectable(
                ConsoleColor.Red,
                new Color(ConsoleColor.White, ConsoleColor.DarkRed),
                false,
                "Blijf hier!", "Ga terug"
                )
                .LinkWindows(null, w)
                .Result();
        }

        static void Main(string[] args)
        {
            //MainScreen();

            var window = new Window(true);
            var table = new TableBuilder(window, 4, 4);
            table.SetHeaders(ConsoleColor.DarkGray, "Header 1", "Header 2", "Header 3", "Header 4");
            for (int i = 0; i < 15; i++)
            {
                string[] arr = new string[4];
                for (int j = 0; j < 4; j++)
                {
                    arr[j] = $"row {i} - col {j} test!";
                }
                table.AddRow(arr);
            }
            table.Result(ConsoleColor.White);
            
            InputHandler.WaitForInput();
        }
    }
}