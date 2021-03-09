using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            for (int i = 0; i < 5; i++)
            {
                SayHello();
            }
        }

        static void SayHello()
        {
            Console.WriteLine("Hello!");
        }
    }
}
