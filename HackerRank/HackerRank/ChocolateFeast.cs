using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    class ChocolateFeast
    {
        public void execute()
        {
            int numOfTestCases = Int32.Parse(Console.ReadLine());
            for (int i = 0; i < numOfTestCases; i++ )
            {
                string input = Console.ReadLine();
                int money = Int32.Parse(input.Split(' ')[0]);
                int price = Int32.Parse(input.Split(' ')[1]);
                int wrapper = Int32.Parse(input.Split(' ')[2]);
                int directChocolate = money / price;
                int available = directChocolate;
                int finalresult = directChocolate;
                while (available >= wrapper)
                {
                    int prev = available;
                    available = available / wrapper;
                    finalresult = finalresult + available;
                    int remaining = prev - available*wrapper;
                    available = remaining + available;
                }
                Console.WriteLine(finalresult);
            }
        }
    }
}
