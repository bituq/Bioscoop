using System;
using AppComponents;

namespace CinemaApplication
{
    public class Program
    {
        public static void Main()
        {
            var listArr = new Selectable[5];
            for (int i = 0; i < listArr.Length; i++)
            {
                string[] items = new string[5];
                for (int j = 0; j < items.Length; j++)
                {
                    items[j] = $"Item {j}";
                }
                listArr[i] = new Builders.ListBuilder(
                        new Anchor(2 + i * 15, 3),
                        items,
                        ItemList.Options.Prefix.Number,
                        DefaultColor: new ItemColor(ConsoleColor.DarkGray, ConsoleColor.Black)
                    )
                    .AsSelectable(new ItemColor(ConsoleColor.White, ConsoleColor.Black), $"Menu {i}")
                    .Done();
            }
            listArr[0].Hover = true;

            while (true)
            {
                InputHandler.DrawContent();
                InputHandler.WaitForInput();
            }
        }
    }
}