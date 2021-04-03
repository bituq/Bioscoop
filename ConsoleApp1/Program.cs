using System;
using AppComponents;

namespace CinemaApplication
{
    public class Program
    {
        public static void MakeMenus(int amount, Anchor position, Tab tab)
        {
            var listArr = new Selectable[amount];
            for (int i = 0; i < listArr.Length; i++)
            {
                string[] items = new string[5];
                for (int j = 0; j < items.Length; j++)
                {
                    items[j] = $"Item {j}";
                }
                listArr[i] = new Builders.ListBuilder(
                        tab,
                        new Anchor(position.x + i * 15, position.y),
                        items,
                        ItemList.Options.Prefix.Number,
                        DefaultColor: new ItemColor(ConsoleColor.DarkGray, ConsoleColor.Black)
                    )
                    .AsSelectable(new ItemColor(ConsoleColor.White, ConsoleColor.Black), $"Menu {i}")
                    .Done();
            }
            listArr[0].Hover = true;
        }
        public static void Main()
        {
            Tab MainScreen = new Tab(false);
            Tab SecondScreen = new Tab(true);
            MakeMenus(5, new Anchor(4, 4), MainScreen);
            MakeMenus(3, new Anchor(2, 2), SecondScreen);
            MakeMenus(4, new Anchor(2, 10), SecondScreen);

            while (true)
            {
                InputHandler.WaitForInput();
            }
        }
    }
}