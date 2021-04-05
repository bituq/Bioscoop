using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using AppComponents;
using JsonHandler;

namespace CinemaApplication
{
    public static class Defaults
    {
        public static NavigationMenu DefaultNavMenu(Tab tab, string[] items, Tab[] tabs)
        {
            return Templates.EasyNavigationMenu(
                    tab,
                    new Anchor(0, 1),
                    items,
                    tabs,
                    Templates.ColorPalettes.CasualMonochrome,
                    "Navigeren"
                );
        }
    }

    public class Program
    {
        public static class Screens
        {
            public static Tab mainMenu = new Tab(true);
            public static Tab movieScreen = new Tab();
            public static Tab adminScreen = new Tab();
        }
        public struct Seat
        {
            public int row;
            public int column;
            public Seat(int row, int column)
            {
                this.row = row;
                this.column = column;
            }
        }
        public class TimeSlot
        {
            public Tab tab = new Tab();
            public int time { get; set; }
            public int hall { get; set; }
            public List<Seat> occupiedSeats {get; set;}
        }
        public class Movie
        {
            public Tab tab = new Tab();
            public Tab reservationTab = new Tab();
            public string name { get; set; }
            public int duration { get; set; }
            public int releaseDate { get; set; }
            public int rating { get; set; }
            public string[] genres { get; set; }
            public string language { get; set; }
            public string company { get; set; }
            public string[] starring { get; set; }
            public string description { get; set; }
            public TimeSlot[] timeSlots { get; set; } = new TimeSlot[0];

            public Selectable saveMovieInfo() => new Builders.ListBuilder(
                    this.tab,
                    new Anchor(30, 1),
                    new string[]
                    {
                        $"Naam: {name}",
                        $"Lengte: {DateTime.UnixEpoch.AddSeconds(duration).ToString($"HH uur m")} minuten",
                        $"Publicatie: {DateTime.UnixEpoch.AddSeconds(releaseDate).ToString("dd MMMM yyyy")}",
                        $"Rating: {rating}/5",
                        $"Taal: {language}",
                        $"Bedrijf: {company}",
                        $"Beschrijving: {description}"
                    },
                    DefaultColor: Templates.Colors.GrayBlack
                )
                .AsSelectable(Templates.Colors.WhiteBlack, "Informatie over deze film")
                .Done();

            public void LoadTimeSlots()
            {
                string[] times;
                if (timeSlots.Length <= 0)
                {
                    times = new string[] { "Geen beschikbare zitplekken" };
                }
                else
                {
                    times = new string[timeSlots.Length];
                for (int i = 0; i < times.Length; i++)
                    {
                        times[i] =
                            $"{DateTime.UnixEpoch.AddSeconds(timeSlots[i].time).ToString("f")}\t\t" +
                            $"{timeSlots[i].occupiedSeats.Count}";
                    }
                }
                var list = new Builders.ListBuilder(
                    reservationTab,
                    new Anchor(30, 1),
                    times,
                    ItemList.Options.Prefix.Dash,
                    DefaultColor: Templates.ColorPalettes.CasualMonochrome[0]
                    )
                    .AsSelectable(Templates.ColorPalettes.CasualMonochrome[1], $"Datum/tijd\t\t\tBezette zitplekken")
                    .ForNavigation(Templates.ColorPalettes.CasualMonochrome[2])
                    .Done();
            }
        }
        public static void MovieScreen(Tab tab)
        {

            string movies = File.ReadAllText("Movies.json");
            JsonDocument doc = JsonDocument.Parse(movies);
            JsonElement root = doc.RootElement;

            string[] moviesArray = new string[root.GetArrayLength()];
            Tab[] tabs = new Tab[moviesArray.Length];

            for (int i = 0; i < moviesArray.Length; i++)
            {
                var movie = JsonSerializer.Deserialize<Movie>(root[i].ToString());
                var navMenuMovie = Defaults.DefaultNavMenu(
                movie.tab,
                new string[] { "Hoofdmenu", "Terug naar films", "Plaats reservering" },
                new Tab[] { Screens.mainMenu, tab, movie.reservationTab }
                );
                movie.saveMovieInfo();
                moviesArray[i] =
                    $"{root[i].GetProperty("name")}\t\t" +
                    $"{root[i].GetProperty("rating")}/5\t" +
                    $"{DateTime.UnixEpoch.AddSeconds(root[i].GetProperty("releaseDate").GetInt32()):dd MMMM yyyy}";
                tabs[i] = movie.tab;
                var navMenuReservation = Defaults.DefaultNavMenu(
                    movie.reservationTab,
                    new string[] { "Hoofdmenu", "Terug naar films", $"Terug naar {movie.name}" },
                    new Tab[] { Screens.mainMenu, tab, movie.tab }
                    );
                movie.LoadTimeSlots();

            }
            var navMenu = Defaults.DefaultNavMenu(
                tab,
                new string[] { "Hoofdmenu", "Bekijk reservering"},
                new Tab[] { Screens.mainMenu, tab}
                );
            var list = Templates.MoviesList(
                tab,
                new Anchor(30, 1),
                moviesArray,
                tabs,
                Templates.ColorPalettes.FadedMonochrome,
                "NAAM\t\t\t\tRATING\tRELEASE"
                );
        }
        public static void MainMenu(Tab tab)
        {
            var menu = Templates.EasyNavigationMenu(
                    tab,
                    new Anchor(3, 1),
                    new string[] { "Bekijk films", "Bekijk reservering", "Admin paneel" },
                    new Tab[] { Screens.movieScreen, tab, Screens.adminScreen },
                    Templates.ColorPalettes.CasualMonochrome,
                    "Project B Bioscoop Applicatie"
                );
        }
        public static void AdminPanel(Tab tab)
        {
            var navMenu = Defaults.DefaultNavMenu(
                tab,
                new string[] { "Hoofdmenu", "Voeg film toe", "Voeg tijdslot toe" },
                new Tab[] { Screens.mainMenu, tab, tab }
                );
        }
        public static void Main()
        {
            MainMenu(Screens.mainMenu);
            MovieScreen(Screens.movieScreen);
            AdminPanel(Screens.adminScreen);
            while (true)
            {
                InputHandler.WaitForInput();
            }
        }
    }
}