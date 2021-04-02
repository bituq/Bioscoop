using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaApplication
{
    public class Options
    {
        public const bool USE_NUMBERS = true;
        public const bool USE_BULLET_POINTS = false;
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

        public MenuMaker(MenuList Menu)
        {
            this.menu = Menu;
        }

        public void DrawSolution()
        {
            for (int i = 0; i < menu.items.Length; i++)
            {
                Console.SetCursorPosition(0, i);
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

            MenuList menu = new MenuList(new string[] { "Zoek Films", "Bekijk Reservering" });
            MenuMaker menuMaker = new MenuMaker(menu);

            menu.GeneratePlaceholders(4);

            Options.Title("Bioscoop Applicatie");

            while(!selected)
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
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}