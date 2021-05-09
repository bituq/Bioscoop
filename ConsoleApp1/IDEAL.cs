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
			var IDEALlist = new string[] {"ING", "ABN-AMRO", "Rabobank", "RegioBank", "SNS bank", "knab"};
			var title = new TextBuilder(ideal, 2, 2)
				.Color(ConsoleColor.DarkGreen)
				.Text("Choose your bank")
				.Result();

			var menu = new TextListBuilder(ideal, 2, 4)
				.Color(ConsoleColor.Red)
				.SetItems(IDEALlist)
				.Selectable(ConsoleColor.Black, ConsoleColor.Yellow)
				.LinkWindows(pm)
				.Result();

        }
	}
}
