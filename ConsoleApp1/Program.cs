using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;

namespace CinemaApplication
{
	partial class Program
	{
		static void Main()
		{
			Payments();
			IDEAL();
			PayPal();
			InputHandler.WaitForInput();
		}
	}
}