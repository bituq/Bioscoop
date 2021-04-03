using System;
using System.Collections.Generic;

namespace AppComponents
{
	public class InputHandler
	{
		public static List<Selectable> selectables = new List<Selectable>();

		public static void Wait()
		{
			foreach (Selectable selectable in selectables)
			{
				selectable.Draw();
			}
			var info = Console.ReadKey();
			foreach (Selectable selectable in selectables)
			{
				if (selectable.hover)
				{
					if (info.Key == ConsoleKey.UpArrow)
					{
						selectable.KeyUp();
					}
					else if (info.Key == ConsoleKey.DownArrow)
					{
						selectable.KeyDown();
					}
				}
			}
		}
	}

	public class Components
	{
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

			public class ListItem
			{
				public ItemColor color = new ItemColor();
				public string value;
				public ListItem(string text)
				{
					this.value = text;
				}
			}

			public ListItem[] items;

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

			public ItemList(Anchor position, string[] Items, Options.Prefix ListPrefix = Options.Prefix.None, Options.Direction ListDirection = Options.Direction.Vertical, ItemColor DefaultColor = new ItemColor(), string CustomPrefix = "")
			{
				this.listPrefix = ListPrefix;
				this.listDirection = ListDirection;
				this.cursorPosition = position;
				this.customPrefix = CustomPrefix;
				this.defaultColor = DefaultColor;
				items = new ListItem[Items.Length];
				for (int i = 0; i < Items.Length; i++)
				{
					items[i] = new ListItem(Items[i]);
					items[i].color = defaultColor;
				}
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
			public int selectableId;

			public static int selectableCount = 0;

			private ItemList list;

			public int defaultIndex = -1;

			public int selectedIndex = -1;

			public ItemColor selectionColor;

			public bool hover = false;

			public Selectable(ItemList l, ItemColor SelectionColor = new ItemColor())
			{
				this.list = l;
				this.selectionColor = SelectionColor;
				this.selectableId = selectableCount;
				InputHandler.selectables.Add(this);
				selectableCount++;
			}

			public void KeyUp() { selectedIndex = hover ? Math.Max(selectedIndex - 1, 0) : defaultIndex; }

			public void KeyDown() { selectedIndex = hover ? Math.Min(selectedIndex + 1, list.Length - 1) : defaultIndex; }

			public void Draw()
			{
				for (int index = 0; index < list.Length; index++)
				{
					Console.SetCursorPosition(list.cursorPosition.x, list.cursorPosition.y + index);
					list.currentItemIndex = index;
					if (selectedIndex == index)
					{
						list[index].color = selectionColor;
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
				return (this.selectableId.Equals(other.selectableId));
			}
		}

		public class NavigationMenu
		{
			Selectable menu;

			public NavigationMenu(Selectable m)
			{
				this.menu = m;
			}

			public void KeyEnter()
			{
				if (menu.hover)
				{
					menu.selectedIndex = menu.defaultIndex;
				}
			}

			public void Draw() => menu.Draw();
		}
	}

	public class Builders
	{
		public class NavigationMenuBuilder
		{
			NavigationMenu menu;

			public NavigationMenuBuilder(Selectable m)
			{
				menu = new NavigationMenu(m);
			}

			public NavigationMenu Done() { return menu; }
		}

		public class SelectableBuilder
		{
			Selectable selectable;

			public SelectableBuilder(ItemList l, ItemColor SelectionColor = new ItemColor())
			{
				this.selectable = new Selectable(l, SelectionColor);
			}

			public NavigationMenuBuilder ForNavigation()
			{
				return new NavigationMenuBuilder(selectable);
			}

			public Selectable Done() { return selectable; }
		}

		public class ListBuilder
		{
			ItemList list;

			public ListBuilder(Anchor position, string[] Items, ItemList.Options.Prefix ListPrefix = 0, ItemList.Options.Direction ListDirection = 0, ItemColor DefaultColor = new ItemColor(), string CustomPrefix = "")
			{
				list = new ItemList(position, Items, ListPrefix, ListDirection, DefaultColor, CustomPrefix);
			}

			public SelectableBuilder AsSelectable(ItemColor SelectionColor)
			{
				return new SelectableBuilder(list, SelectionColor);
			}

			public ItemList Done() { return list; }
		}
	}
}
