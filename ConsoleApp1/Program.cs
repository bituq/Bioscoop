using System;
using System.Linq;
using System.Drawing;

namespace CinemaApplication
{

    public class Options
    {
        public const bool USE_NUMBERS = true;
        public const bool USE_BULLET_POINTS = false;
        public const bool USE_PLACEHOLDERS = true;
        public static void Title(string name) { Console.Title = name; }
    }

    public class MenuList
    {
        public string[] items;

        public int activeItemIndex = -1;

        public string activeValue => activeItemIndex != -1 ? items[activeItemIndex] : null;

        public MenuList(string[] arr)
        {
            items = arr;
        }

        public void GeneratePlaceholders(int amount)
        {
            var temp = new string[amount + items.Length];
            for (int i = 0; i < items.Length + amount; i++)
            {
                temp[i] = i < items.Length ? items[i] : $"Option {i + 1}";
            }
            items = temp;
        }

        public void KeyUp() { activeItemIndex = Math.Max(activeItemIndex - 1, 0); }

        public void KeyDown() { activeItemIndex = Math.Min(activeItemIndex + 1, items.Length - 1); }
    }

    public class MenuMaker
    {
        MenuList menu;

        string prefix;
        string name;

        public MenuMaker(MenuList Menu, string Name)
        {
            this.menu = Menu;
            this.name = Name;
        }

        private Point Margin(int m) => new Point(m, m);

        public void DrawSolution()
        {
            Console.SetCursorPosition(1, 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);
            for (int i = 0; i < menu.items.Length; i++)
            {
                Console.SetCursorPosition(2, 3 + i);
                prefix = PrefixSelector(i);
                if (menu.activeItemIndex == i)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                Console.WriteLine($"{prefix}{menu.items[i]}");
            }
        }

        private string PrefixSelector(int index)
        {
            if (Options.USE_NUMBERS)
            {
                return $"{index + 1}. ";
            }
            else if (Options.USE_BULLET_POINTS)
            {
                return "- ";
            }
            return "";
        }
    }

    public class Program
    {
        public static void Main()
        {
            bool selected = false;
            bool reset = false;

            MenuList menu = new MenuList(new string[] { "Zoek Films", "Bekijk Reservering" });
            MenuMaker menuMaker = new MenuMaker(menu, "Project B Bioscoop Applicatie");

            menu.GeneratePlaceholders(3);

            Options.Title("Bioscoop Applicatie");

            while (!reset)
            {
                while (!selected)
                {
                    menuMaker.DrawSolution();

                    var info = Console.ReadKey();

                    if (info.Key == ConsoleKey.UpArrow)
                    {
                        menu.KeyUp();
                    }
                    else if (info.Key == ConsoleKey.DownArrow)
                    {
                        menu.KeyDown();
                    }
                    else if (info.Key == ConsoleKey.Enter)
                    {
                        selected = true;
                    }
                }


                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nVisiting page: {menu.activeValue}!");

                Console.ReadKey();
                Console.Clear();
                selected = false;
            }
        }
    }
}