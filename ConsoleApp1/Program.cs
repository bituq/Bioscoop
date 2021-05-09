using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI;

namespace CinemaApplication
{
	partial class Program
	{
		static void PaymentScreen()
        {
			Payments();
			IDEAL();
			PayPal();
		}
		static void HHallsScreen()
        {
			HallScreen();
        }
		static void Main()
		{
			//PaymentScreen();
			HHallsScreen();
			InputHandler.WaitForInput();
		}
	}
}