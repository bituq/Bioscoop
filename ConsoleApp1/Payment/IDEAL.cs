using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;
using CinemaUI.Builder;

namespace CinemaApplication
{
	partial class Program
	{
		static Window ideal = new Window();
		static void IDEAL()
		{
			var IDEALlist = new string[] { "ING", "ABN-AMRO", "Rabobank", "RegioBank", "SNS bank", "knab", "Go back" };
			var title = new TextBuilder(ideal, 3, 2)
				.Color(ConsoleColor.DarkGreen)
				.Text("Choose your bank")
				.Result();

			var menu = new TextListBuilder(ideal, 3, 4)
				.Color(ConsoleColor.Red)
				.SetItems(IDEALlist)
				.Selectable(ConsoleColor.Black, ConsoleColor.White)
				.LinkWindows(ing, abn, rabo, regio, sns, knab, payments)
				.Result();
		}
		static Window ing = new Window();
		static void ING()
		{
			var title = new TextBuilder(ing, 3, 2)
				.Color(ConsoleColor.Blue)
				.Text("Using ING...")
				.Result();

			var menu = new TextListBuilder(ing, 3, 4)
				.Color(ConsoleColor.Yellow)
				.SetItems("Go back")
				.Selectable(ConsoleColor.Black, ConsoleColor.White)
				.LinkWindows(ideal)
				.Result();
		}


		static Window abn = new Window();
			static void ABN()
			{
				var title = new TextBuilder(abn, 3, 2)
					.Color(ConsoleColor.Blue)
					.Text("Using ABN-AMRO...")
					.Result();

				var menu = new TextListBuilder(abn, 3, 4)
					.Color(ConsoleColor.Yellow)
					.SetItems("Go back")
					.Selectable(ConsoleColor.Black, ConsoleColor.White)
					.LinkWindows(ideal)
					.Result();
			}
		static Window rabo = new Window();
		static void RABO()
		{
			var title = new TextBuilder(rabo, 3, 2)
				.Color(ConsoleColor.Blue)
				.Text("Using Rabobank...")
				.Result();

			var menu = new TextListBuilder(rabo, 3, 4)
				.Color(ConsoleColor.Red)
				.SetItems("Go back")
				.Selectable(ConsoleColor.Black, ConsoleColor.White)
				.LinkWindows(ideal)
				.Result();
		}
		static Window regio = new Window();
		static void REGIO()
		{
			var title = new TextBuilder(regio, 3, 2)
				.Color(ConsoleColor.Blue)
				.Text("Using RegioBank...")
				.Result();

			var menu = new TextListBuilder(regio, 3, 4)
				.Color(ConsoleColor.Red)
				.SetItems("Go back")
				.Selectable(ConsoleColor.Black, ConsoleColor.White)
				.LinkWindows(ideal)
				.Result();
		}
		static Window sns = new Window();
		static void SNS()
		{
			var title = new TextBuilder(sns, 3, 2)
				.Color(ConsoleColor.Blue)
				.Text("Using SNS bank...")
				.Result();

			var menu = new TextListBuilder(sns, 3, 4)
				.Color(ConsoleColor.Red)
				.SetItems("Go back")
				.Selectable(ConsoleColor.Black, ConsoleColor.White)
				.LinkWindows(ideal)
				.Result();
		}
		static Window knab = new Window();
		static void KNAB()
		{
			var title = new TextBuilder(knab, 3, 2)
				.Color(ConsoleColor.Blue)
				.Text("Using knab...")
				.Result();

			var menu = new TextListBuilder(knab, 3, 4)
				.Color(ConsoleColor.Red)
				.SetItems("Go back")
				.Selectable(ConsoleColor.Black, ConsoleColor.White)
				.LinkWindows(ideal)
				.Result();
		}
	}
}
