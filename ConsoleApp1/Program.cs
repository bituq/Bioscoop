using System;
using AppComponents;

namespace CinemaApplication
{
    public static class Defaults
    {
        public static NavigationMenu DefaultNavMenu(Tab tab, string[] items, Tab[] tabs)
        {
            return Templates.EasyNavigationMenu(
                    tab,
                    new Anchor(0, 0),
                    items,
                    tabs,
                    Templates.ColorPalettes.FadedMonochrome,
                    "Navigeren"
                );
        }
    }

    public class Program
    {
        public static void MovieScreen()
        {
            Tab tab = new Tab(true);
            var navMenu = Defaults.DefaultNavMenu(
                tab,
                new string[] { "Hoofdmenu", "Bekijk uw reservering", "Adminpaneel" },
                new Tab[] { tab, tab, tab }
                );
        }
        public static void Main()
        {
            MovieScreen();
            while (true)
            {
                InputHandler.WaitForInput();
            }
        }
    }
}