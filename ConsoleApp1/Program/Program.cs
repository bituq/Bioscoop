using System;
using CinemaUI;
using CinemaUI.Builder;
using System.IO;
using System.Text;

namespace CinemaApplication
{
    partial class Program
    {
        static Window mainMenu = new Window(true);
        static void MainMenu()
        {
            var title = new TextBuilder(mainMenu, 2, 2)
                .Color(ConsoleColor.Cyan)
                .Result("Bioscoop Applicatie");

            var subtitle = new TextBuilder(mainMenu, 2, 3)
                .Color(ConsoleColor.DarkGray)
                .Result("Project B");

            var menu = new TextListBuilder(mainMenu, 2, 5)
                .Color(ConsoleColor.White)
                .Selectable(new Color(ConsoleColor.Black, ConsoleColor.White), true, "Demonstratie", "Item 2", "Item 3")
                .LinkWindows(null, listOfFilms)
                .Result();

            var inputMenu = new TextListBuilder(mainMenu, 40, 5)
                .Color(ConsoleColor.Gray)
                .AsInput(new Color(ConsoleColor.White, ConsoleColor.DarkGray), "Insert first name", "Insert last name")
                .Result();

            var res = new TextListBuilder(mainMenu, 2, 10)
                .Result(false, "First Name:", "Last Name:");

            for (int i = 0; i < res.Items.Count; i++)
            {
                var temp = new TextBuilder(mainMenu, res.Position.X + res.Items[i].Text.Length + 1, res.Position.Y + i)
                    .Color(ConsoleColor.Gray)
                    .Result("");

                mainMenu.LinkTextInput(inputMenu.Items[i], temp);
            }
        }

        static void Main(string[] args)
        {
            MainMenu();
            ListOfFilms();
            //Demo();

            InputHandler.WaitForInput();
        }
    }
}