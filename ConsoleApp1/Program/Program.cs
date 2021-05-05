using System;
using CinemaUI;
using CinemaUI.Builder;
using System.IO;
using System.Text;

namespace CinemaApplication
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var temp = new Hall(1, 5, 15);
            InputHandler.WaitForInput();
        }
    }
}