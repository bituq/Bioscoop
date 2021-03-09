using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 5;
            string s = "";
            int i = 0;
            while (i < num)
            {
                int j = 0;
                while (j < i)
                {
                    s += "*";
                    j++;
                }
                s += "\n";
                i++;
            }
            Console.WriteLine(s);
        }
    }
}
