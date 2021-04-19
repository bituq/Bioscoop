using System;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    class Program
    {
        static void MainScreen()
        {
            var w = new Window(true);
            var list1 = new TextListBuilder(w, 2, 2).Selectable(
                    textColor: ConsoleColor.White,
                    selectionColor: new Color(ConsoleColor.White, ConsoleColor.DarkBlue),
                    useNumbers: true,
                     "Pizza", "Patat", "Pannekoek", "Soep", "Stampot"
                    ).Result();
            var list2 = new TextListBuilder(w, 20, 2).Selectable(
                textColor: ConsoleColor.White,
                selectionColor: new Color(ConsoleColor.Black, ConsoleColor.White),
                useNumbers: true,
                "Groente", "Fruit", "Vlees", "Vegetarisch", "Drinken"
                ).Result();
        }

        static void Main(string[] args)
        {
            MainScreen();
            
            InputHandler.WaitForInput();
        }
    }
}