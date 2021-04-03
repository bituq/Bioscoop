using System;
using AppComponents;

namespace CinemaApplication
{
    public class Program
    {
        public static void Main()
        {
            var list = new Builders.ListBuilder(
                new Anchor(4, 4),
                new string[] { "Hello", "World", "hey", "hoi", "sup" },
                ListPrefix: ItemList.Options.Prefix.Number,
                DefaultColor: new ItemColor(ConsoleColor.White, ConsoleColor.Black)
            )
            .AsSelectable(new ItemColor(ConsoleColor.Black, ConsoleColor.White), "Bioscoop Applicatie")
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