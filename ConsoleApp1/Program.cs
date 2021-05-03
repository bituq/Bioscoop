using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
    class Program
    {
        static Window startScreen = new Window(true);
        public class House
        {
            public Window window = new Window();

            public House(int aantalBewoners, string huishouden, int aantalKamers, string dakKleur)
            {
                var information = new TextListBuilder(window, 2, 2)
                    .Color(ConsoleColor.Gray)
                    .SetItems($"Aantal bewoners: {aantalBewoners}", $"Huishouden: {huishouden}", $"Aantal kamers: {aantalKamers}", "Dak kleur: " + dakKleur)
                    .Result();

                var selectableList = new TextListBuilder(window, 2, 8)
                    .Color(ConsoleColor.Red)
                    .SetItems("Go back")
                    .Selectable(ConsoleColor.Yellow, ConsoleColor.DarkGray)
                    .LinkWindows(startScreen)
                    .Result();
            }
        }
        static void Main()
        {

            var huizen = new List<House>();

            huizen.Add(new House(3, "Peters", 8, "Rood"));
            huizen.Add(new House(4, "Anoniem", 12, "Geel"));
            huizen.Add(new House(5, "Anoniem", 6, "Groen"));
            huizen.Add(new House(5, "gdfsg", 6, "waef"));

            var huisNamen = new string[huizen.Count];
            var huisSchermen = new Window[huizen.Count];
            for (int i = 0; i < huizen.Count; i++)
            {
                huisNamen[i] = $"Huisnummer {i + 1}";
                huisSchermen[i] = huizen[i].window;
            }

            var huizenLijst = new TextListBuilder(startScreen, 2, 2)
                .SetItems(huisNamen)
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(huisSchermen)
                .Result();

            var huizenMakenLijst = new TextListBuilder(startScreen, 22, 2)
                .SetItems("Aantal bewoners", "Huishouden", "Aantal kamers", "Dak kleur")
                .Color(ConsoleColor.Gray)
                .AsInput(ConsoleColor.White, ConsoleColor.DarkRed)
                .Result();

            huizenMakenLijst.OnChange = () =>
            {
                huizen.Add(new House(5, "gdfsg", 6, "waef"));

                var huisNamen = new string[huizen.Count];
                var huisSchermen = new Window[huizen.Count];
                for (int i = 0; i < huizen.Count; i++)
                {
                    huisNamen[i] = $"Huisnummer {i + 1}";
                    huisSchermen[i] = huizen[i].window;
                }

                huizenLijst.Replace(new TextListBuilder(startScreen, 2, 2)
                .SetItems(huisNamen)
                .Selectable(ConsoleColor.Black, ConsoleColor.White)
                .LinkWindows(huisSchermen)
                .Result());
            };

            InputHandler.WaitForInput();
        }
        public void OnChange()
        {

        }
    }
}
