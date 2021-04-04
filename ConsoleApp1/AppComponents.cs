using System;
using System.Collections.Generic;

namespace AppComponents
{
	public class Tab
    {
		public bool active = true;
		public List<Selectable> selectables = new List<Selectable>();
		public List<ItemList> itemLists = new List<ItemList>();

		public Tab(bool IsMain = false)
        {
			this.active = IsMain;
			InputHandler.environments.Add(this);
        }
		public void DrawContent()
        {
			if (active)
            {
				foreach (Selectable selectable in selectables)
				{
					selectable.Draw();
				}
				foreach (ItemList list in itemLists)
				{
					list.Draw();
				}
			}
        }
		public void Stop()
        {
			for (int i = 0; i < selectables.Count; i++)
            {
				selectables[i].Hover = false;
            }
			active = false;
        }
	}
	public class InputHandler
	{
		public static List<Tab> environments = new List<Tab>();
		public static int index = 0;

		public static void WaitForInput()
		{
			foreach (Tab env in environments)
			{
				if (env.active)
				{
					env.DrawContent();
					var info = Console.ReadKey();
					foreach (Selectable selectable in env.selectables)
					{
						if (selectable.Hover)
						{
							switch (info.Key)
							{
								case ConsoleKey.UpArrow:
									selectable.KeyUp();
									break;
								case ConsoleKey.DownArrow:
									selectable.KeyDown();
									break;
								case ConsoleKey.LeftArrow:
									selectable.KeyLeft();
									break;
								case ConsoleKey.RightArrow:
									selectable.KeyRight();
									break;
								case ConsoleKey.Enter:
									selectable.KeyEnter();
									index = 0;
									break;
							}
						}
					}
					if (info.Key == ConsoleKey.LeftArrow)
					{
						env.selectables[index].Hover = false;
						index = Math.Max(index - 1, 0);
						env.selectables[index].Hover = true;
					}
					else if (info.Key == ConsoleKey.RightArrow)
					{
						env.selectables[index].Hover = false;
						index = Math.Min(index + 1, env.selectables.Count - 1);
						env.selectables[index].Hover = true;
					}
				}
			}
		}
	}
	public struct Anchor
	{
		public int x;
		public int y;

		public Anchor(int X = 0, int Y = 0)
		{
			this.x = X;
			this.y = Y;
		}

		public int this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return x;
						break;
					case 1:
						return y;
						break;
					default:
						throw new IndexOutOfRangeException("Index out of range");
						break;
				}
			}

			set
			{
				switch (index)
				{
					case 0:
						x = value;
						break;
					case 1:
						y = value;
						break;
					default:
						throw new IndexOutOfRangeException("Index out of range");
						break;
				}
			}
		}
	}

	public struct ItemColor
	{
		public ConsoleColor foreground;
		public ConsoleColor background;

		public ItemColor(ConsoleColor f, ConsoleColor b)
		{
			this.foreground = f;
			this.background = b;
		}

		public ConsoleColor this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return foreground;
						break;
					case 1:
						return background;
						break;
					default:
						throw new IndexOutOfRangeException("Index out of range");
						break;
				}
			}

			set
			{
				switch (index)
				{
					case 0:
						foreground = value;
						break;
					case 1:
						background = value;
						break;
					default:
						throw new IndexOutOfRangeException("Index out of range");
						break;
				}
			}
		}
	}

	public class ListItem
	{
		public ItemColor color = new ItemColor();
		public string value;
		public bool active = false;
		public Tab tab;
		public ListItem(string text)
		{
			this.value = text;
		}
	}

	public class ItemList
	{
		public class Options
		{
			public enum Prefix
			{
				None,
				Number,
				Dash,
				Custom
			}
			public enum Direction
			{
				Vertical,
				Horizontal
			}
		}

		public ListItem[] items;

		public Tab tab;

		public int id;

		public static int count = 0;

		public readonly Anchor cursorPosition;

		public readonly Options.Prefix listPrefix;

		public readonly Options.Direction listDirection;

		public ItemColor defaultColor;

		public string customPrefix;

		public int currentItemIndex = 0;

		public int Length => items.Length;

		public string GetPrefix()
		{
			switch (listPrefix)
			{
				case Options.Prefix.None:
					return "";
					break;
				case Options.Prefix.Number:
					return $"{currentItemIndex + 1}. ";
					break;
				case Options.Prefix.Dash:
					return $"- ";
					break;
				case Options.Prefix.Custom:
					return customPrefix;
					break;
				default:
					return "";
					break;
			}
		}

		public ItemList(Tab tab, Anchor position, string[] Items, Options.Prefix ListPrefix = Options.Prefix.None, Options.Direction ListDirection = Options.Direction.Vertical, ItemColor DefaultColor = new ItemColor(), string CustomPrefix = "")
		{
			this.tab = tab;
			this.listPrefix = ListPrefix;
			this.listDirection = ListDirection;
			this.cursorPosition = position;
			this.customPrefix = CustomPrefix;
			this.defaultColor = DefaultColor;
			items = new ListItem[Items.Length];
			for (int i = 0; i < Items.Length; i++)
			{
                items[i] = new ListItem(Items[i]) { color = defaultColor };
            }

			tab.itemLists.Add(this);
		}

		public ListItem this[int index]
		{
			get
			{
				if (index < 0 || index >= items.Length)
					throw new IndexOutOfRangeException("Index out of range");

				return items[index];
			}

			set
			{
				if (index < 0 || index >= items.Length)
					throw new IndexOutOfRangeException("Index out of range");

				items[index] = value;
			}
		}

		public void SetColor(ConsoleColor f, ConsoleColor b) { defaultColor = new ItemColor(f, b); }

		public void DrawColor(ListItem item)
		{
			Console.ForegroundColor = item.color[0];
			Console.BackgroundColor = item.color[1];
		}

		public void Draw()
		{
			for (int index = 0; index < items.Length; index++)
			{
				Console.SetCursorPosition(cursorPosition.x, cursorPosition.y + index);
				currentItemIndex = index;
				DrawColor(items[index]);
				Console.Write($"{GetPrefix()}{items[index].value}{(listDirection == Options.Direction.Vertical ? "\n" : " ")}");
			}
			Console.ResetColor();
		}
	}

	public class Selectable : IEquatable<Selectable>
	{
		public bool SavePosition = false;
		public bool InfiniteScroll = true;

		public Tab tab;

		public int id;

		public static int count = 0;

		public readonly ItemList list;

		public int defaultIndex = 0;

		public int selectedIndex = 0;

		public ItemColor selectionColor;

		public ItemColor activeColor;

		public string title;

		public Tuple<ItemColor, ItemColor> titleColor = Tuple.Create(
				new ItemColor(ConsoleColor.Yellow, ConsoleColor.Black),
				new ItemColor(ConsoleColor.DarkYellow, ConsoleColor.Black)
			);

		public bool active = false;

		public bool Hover { get; set; }

		public Selectable(Tab tab, ItemList l, string Title = "", ItemColor SelectionColor = new ItemColor())
		{
			this.tab = tab;
			this.list = l;
			this.selectionColor = SelectionColor;
			this.id = count;
			this.Hover = false;
			this.title = Title;
			tab.itemLists.Remove(l);
			tab.selectables.Add(this);
			count++;
		}

		public void KeyUp()
		{
			list[selectedIndex].active = false;
			if (!InfiniteScroll)
				selectedIndex = Hover ? Math.Max(selectedIndex - 1, 0) : defaultIndex;
            else
				selectedIndex = Hover ? (selectedIndex == 0 ? list.Length - 1 : selectedIndex - 1) : defaultIndex;
		}
		public void KeyDown()
		{
			list[selectedIndex].active = false;
			if (!InfiniteScroll)
				selectedIndex = Hover ? Math.Min(selectedIndex + 1, list.Length - 1) : defaultIndex;
			else
				selectedIndex = Hover ? (selectedIndex == list.Length - 1 ? 0 : selectedIndex + 1) : defaultIndex;
		}
		public void KeyLeft()
		{
			list[selectedIndex].active = false;
			selectedIndex = SavePosition ? selectedIndex : defaultIndex;
		}
		public void KeyRight() => KeyLeft();
		public virtual void KeyEnter() { }

		public void Draw()
		{
			Console.SetCursorPosition(list.cursorPosition.x, list.cursorPosition.y);
			if (Hover)
            {
				Console.ForegroundColor = titleColor.Item1[0];
				Console.BackgroundColor = titleColor.Item1[1];
			}
            else
            {
				Console.ForegroundColor = titleColor.Item2[0];
				Console.BackgroundColor = titleColor.Item2[1];
			}
			Console.Write(title);
			for (int index = 0; index < list.Length; index++)
			{
				Console.SetCursorPosition(list.cursorPosition.x, list.cursorPosition.y + index + (title.Length > 0 ? 1 : 0));
				list.currentItemIndex = index;
				if (selectedIndex == index && Hover)
				{
					if (list[index].active)
                    {
						//list[index].color = activeColor;
						list[index].color = selectionColor;
                    }
                    else
                    {
						list[index].color = selectionColor;
                    }
				}
				else
				{
					list[index].color = list.defaultColor;
				}
				list.DrawColor(list[index]);
				Console.Write($"{list.GetPrefix()}{list[index].value}{(list.listDirection == ItemList.Options.Direction.Vertical ? "\n" : " ")}");
			}
			Console.ResetColor();
		}

		public bool Equals(Selectable other)
		{
			if (other == null) return false;
			return (this.id.Equals(other.id));
		}
	}

    public class NavigationMenu : Selectable
    {

        public NavigationMenu(Tab tab, ItemList l, string Title, ItemColor SelectionColor, ItemColor ActiveColor) : base(tab, l, Title, SelectionColor)
        {
            this.activeColor = ActiveColor;
        }

        public override void KeyEnter()
        {
            if (Hover && list[selectedIndex].tab != null)
            {
                Console.Clear();
                list[selectedIndex].active = true;
                tab.Stop();
                list[selectedIndex].tab.active = true;
            }
        }

        public void SetTabs(Tab[] tabs)
        {
            if (tabs.Length > list.Length)
                throw new ArgumentException($"ListItem[] contains too many ListItems. ({tabs.Length}/{list.Length})");
            else
            {
                for (int i = 0; i < tabs.Length; i++)
                {
                    list[i].tab = tabs[i];
                }
            }
        }

        public void SetTab(Tab tab, string value)
        {
            foreach (ListItem item in list.items)
            {
                if (item.value == value)
                {
                    item.tab = tab;
                }
            }
        }

        public void SetTab(ListItem item, Tab tab)
        {
            item.tab = tab;
        }

    }

    public class Builders
	{
		public class NavigationMenuBuilder
		{
            private readonly NavigationMenu menu;

			public NavigationMenuBuilder(Tab tab, ItemList l, string Title, ItemColor SelectionColor, ItemColor activeColor)
			{
				this.menu = new NavigationMenu(tab, l, Title, SelectionColor, activeColor);
			}

			public NavigationMenu Done() { return menu; }
		}

		public class SelectableBuilder
		{
            private readonly ItemList list;
            private ItemColor selectionColor;
			private string title;
			private Tab tab;
			private Selectable selectable;

			public SelectableBuilder(Tab tab, ItemList l, string Title = "", ItemColor SelectionColor = new ItemColor())
			{
				this.tab = tab;
				this.list = l;
				this.selectionColor = SelectionColor;
				this.title = Title;
			}

			public NavigationMenuBuilder ForNavigation(ItemColor activeColor = new ItemColor())
			{
				return new NavigationMenuBuilder(tab, list, title, selectionColor, activeColor);
			}

			public Selectable Done()
			{
				selectable = new Selectable(tab, list, title, selectionColor);
				return selectable;
			}
		}

		public class ListBuilder
		{
            private readonly ItemList list;
			private Tab tab;

			public ListBuilder(Tab tab, Anchor position, string[] Items, ItemList.Options.Prefix ListPrefix = 0, ItemList.Options.Direction ListDirection = 0, ItemColor DefaultColor = new ItemColor(), string CustomPrefix = "")
			{
				this.tab = tab;
				list = new ItemList(tab, position, Items, ListPrefix, ListDirection, DefaultColor, CustomPrefix);
			}

			public SelectableBuilder AsSelectable(ItemColor SelectionColor, string Title = "")
			{
				return new SelectableBuilder(tab, list, Title, SelectionColor);
			}

			public ItemList Done() { return list; }
		}
	}
}
