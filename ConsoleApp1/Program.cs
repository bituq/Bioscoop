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
		static void HallsScreen()
        {
			Halls();
        }
		static void Main()
		{
			//PaymentScreen();
			HallsScreen();
			InputHandler.WaitForInput();
		}
	}
}