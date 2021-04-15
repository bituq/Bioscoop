using System;
using CinemaUI;

namespace CinemaApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            var w1 = new Window();

            var leftBar = new Container(w1, 0, 0, 30, Console.WindowHeight);
            leftBar.Color = new Color(ConsoleColor.Gray, ConsoleColor.DarkGray);

            var searchBar = new Container(w1, leftBar, 5, 1, 20, 1, Space.Relative);
            searchBar.Color = new Color(ConsoleColor.DarkBlue, ConsoleColor.Black);

            var searchText = new Paragraph(w1, searchBar, 3, 0, Space.Relative);
            searchText.TextColor = ConsoleColor.Blue;
            searchText.Text = "Zoek op titel";

            var genreTitle = new Paragraph(w1, leftBar, 0, 4, Space.Relative);
            genreTitle.TextColor = ConsoleColor.Yellow;
            genreTitle.Text = "Genre";

            var genreList = new TextList(w1, genreTitle, 1, 1, Space.Relative);
            genreList.TextColor = ConsoleColor.Gray;
            genreList.SetItems(new string[] { "Actie", "Drama", "Sci-fi", "Horror", "Romantiek", "Comedy", "Thriller", "Western", "Documentaire", "Avontuur" });
            genreList.Items[0].Suffix = " <";
            genreList.Items[0].TextColor = ConsoleColor.Green;
            genreList.Items[6].Suffix = " <";
            genreList.Items[6].TextColor = ConsoleColor.Green;
            genreList.Items[9].Suffix = " <";
            genreList.Items[9].TextColor = ConsoleColor.Green;

            var dateTitle = new Paragraph(w1, leftBar, 0, 16, Space.Relative);
            dateTitle.TextColor = ConsoleColor.Yellow;
            dateTitle.Text = "Datum";

            var dateList = new TextList(w1, dateTitle, 1, 1, Space.Relative);
            dateList.TextColor = ConsoleColor.Yellow;
            dateList.SetItems(new string[] { "Dag", "Maand", "Jaar" });

            var dateOptions = new TextList(w1, dateList, 7, 0, Space.Relative);
            dateOptions.TextColor = ConsoleColor.Gray;
            dateOptions.SetItems(new string[] { "12 13 14", "5 6 7", "2001 2002 2003" }, "<- ", " ->");
            w1.Draw();
            Console.SetCursorPosition(0, 30);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}