using System;
using AppComponents;

namespace CinemaApplication
{
    public class Program
    {
        public static void Main()
        {
            Tab a = new Tab(true);
            Tab b = new Tab();
            Tab c = new Tab();
            Tab d = new Tab();
            var m1 = Templates.EasyNavigationMenu(
                a,
                new Anchor(4, 4),
                new string[] {"Zoek naar films", "Bekijk uw reservering", "Ga naar adminpaneel"},
                new Tab[] { a, a, d },
                Templates.ColorPalettes.FadedMonochrome,
                "Project B Bioscoop applicatie"
            );

            var m2 = Templates.EasyNavigationMenu(
                d,
                new Anchor(4, 4),
                new string[] { "Doe coole dingen", "Doe iets minder coole dingen", "Voeg films toe", "Doe niks", "Terug naar hoofdmenu" },
                new Tab[] { d, d, d, d, a },
                Templates.ColorPalettes.FieryDragon,
                "Adminpaneel B)"
            );

            while (true)
            {
                InputHandler.WaitForInput();
            }
        }
    }
}