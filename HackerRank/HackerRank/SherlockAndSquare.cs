using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    class SherlockAndSquare
    {
        public void execute()
        {
            int testCases = Int32.Parse(Console.ReadLine());
            for (int i = 0; i < testCases; i++)
            {
                string input = Console.ReadLine();
                long firstNumber = Int64.Parse(input.Split(' ')[0]);
                long secondNumber = Int64.Parse(input.Split(' ')[1]);
                double firstSquare = Math.Sqrt(firstNumber);
                double secondSquare = Math.Sqrt(secondNumber);
                Console.WriteLine(Math.Floor(secondSquare) - Math.Ceiling(firstSquare) + 1);
            }
        }
    }
}
