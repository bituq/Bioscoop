using System;
using System.Linq;
using System.Drawing;
using AppComponents;

namespace CinemaApplication
{
    public class Program
    {
        public static void Main()
        {
            var list = new Builders.ListBuilder(
                new Anchor(5, 2),
                new string[] { "Hello", "World", "hey", "hoi", "sup" },
                ListPrefix: ItemList.Options.Prefix.Number,
                DefaultColor: new ItemColor(ConsoleColor.White, ConsoleColor.Black)
                )
                .AsSelectable(new ItemColor(ConsoleColor.Black, ConsoleColor.White))
                .ForNavigation(new ItemColor(ConsoleColor.Green, ConsoleColor.Black))
                .Done();
            list.Hover = true;

            while (true)
            {
                InputHandler.Wait();
            }
        }
    }
}