using System;
using CinemaUI;
using CinemaUI.Builder;
using System.IO;

namespace CinemaApplication
{
    partial class Program
    {
        static void Main(string[] args)
        {
            SeatScreen();
            InputHandler.WaitForInput();
        }
    }
}