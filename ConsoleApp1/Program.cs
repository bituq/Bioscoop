using System;
using System.IO;
using System.Text.Json;
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

            string movies = File.ReadAllText("Movies.json");
            JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;

            string[] moviesArray = new string[root.GetArrayLength()];
            Tab[] tabs = new Tab[moviesArray.Length];

            for (int i = 0; i < moviesArray.Length; i++)
            {
                moviesArray[i] =
                    $"{root.GetProperty("name")}\t" +
                    $"{root.GetProperty("rating")}/5\t" +
                    $"{DateTime.UnixEpoch.AddSeconds(root.GetProperty("releasedate").GetInt32()):dd MMMM yyyy}";
                tabs[i] = tab;
            }
            var navMenu = Defaults.DefaultNavMenu(
                tab,
                new string[] { "Hoofdmenu", "Bekijk uw reservering", "Adminpaneel" },
                new Tab[] { tab, tab, tab }
                );
            var list = Templates.MoviesList(
                tab,
                new Anchor(20, 4),
                moviesArray,
                tabs,
                Templates.ColorPalettes.CasualMonochrome,
                "NAAM\t\t\tRATING\t\t\tRELEASE"
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