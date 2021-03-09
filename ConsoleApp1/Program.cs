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
                Console.WriteLine(IsPalindrome(131));
                Console.WriteLine(RecursiveTest(5));
            }
        }

        static void SayHello()
        {
            Console.WriteLine("Hello!");
        }
        public static string IsPalindrome(int number)
        {
            string s = "";
            int temp = number;
            while (number != 0) {
                s += (number % 10);
                number = number / 10;
            }
            if ((temp + "") == s) {
                string stringval = ($"{temp} is a palindrome!");
                return stringval;
            }
            else {
                string stringval = ($"{temp} is not a palindrome!");
                return stringval;
            }
        }
        public static int RecursiveTest(int number)
        {
            if (number == 0)
            {
                return 0;
            }
            else
            {
                int n = RecursiveTest(number-1) + number;
                return n; 
            }
        }
    }
}