using System;
using System.Collections.Generic;
using System.Text;

namespace AppComponents
{
    public static class Templates
    {
        public static class Colors
        {
            public static ItemColor WhiteBlack = new ItemColor(ConsoleColor.White, ConsoleColor.Black);
            public static ItemColor DarkgrayBlack = new ItemColor(ConsoleColor.DarkGray, ConsoleColor.Black);
            public static ItemColor BlackWhite = new ItemColor(ConsoleColor.Black, ConsoleColor.White);
            public static ItemColor BlackBlue = new ItemColor(ConsoleColor.Black, ConsoleColor.Blue);
            public static ItemColor YellowBlack = new ItemColor(ConsoleColor.Yellow, ConsoleColor.Black);
            public static ItemColor DarkyellowBlack = new ItemColor(ConsoleColor.DarkYellow, ConsoleColor.Black);
            public static ItemColor WhiteDarkred = new ItemColor(ConsoleColor.White, ConsoleColor.DarkRed);
        }

        public static class ColorPalettes
        {
            public static ColorPalette CasualMonochrome = new ColorPalette(Colors.BlackWhite, Colors.WhiteBlack, Colors.BlackBlue);
            public static ColorPalette FadedMonochrome = new ColorPalette(Colors.DarkgrayBlack, Colors.WhiteBlack, Colors.BlackBlue);
            public static ColorPalette FieryDragon = new ColorPalette(Colors.DarkyellowBlack, Colors.WhiteDarkred, Colors.BlackBlue);
        }

        public class ColorPalette
        {
            public ItemColor Default;
            public ItemColor Selection;
            public ItemColor Active;
            public ItemColor[] Colors;

            public ColorPalette(ItemColor Default, ItemColor Selection, ItemColor Active)
            {
                this.Default = Default;
                this.Selection = Selection;
                this.Active = Active;
                this.Colors = new ItemColor[] { Default, Selection, Active };
            }

            public ItemColor this[int index]
            {
                get { return Colors[index]; }
                set { Colors[index] = value; }
            }
        }

        public static NavigationMenu EasyNavigationMenu(Tab tab, Anchor position, string[] items, Tab[] tabs, ColorPalette palette, string title = "")
        {
            var res = new Builders.ListBuilder(
                tab,
                position,
                items,
                ItemList.Options.Prefix.Dash,
                DefaultColor: palette[0]
                )
                .AsSelectable(palette[1], title)
                .ForNavigation(palette[2])
                .Done();
            res.SetTabs(tabs);
            return res;
        }
    }
}
