using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    class IsFibo
    {
        public void execute()
        {
            int numTestcases = Int32.Parse(Console.ReadLine());
            List<Int64> fibonaci = new List<Int64>();
            for (int i = 0; i < numTestcases; i++)
            {
                long input = Int64.Parse(Console.ReadLine());
                if (input == 0 || input == 1)
                {
                    Console.WriteLine("IsFibo");
                    continue;
                }
                if (fibonaci.Count < 2)
                {
                    fibonaci.Add(0);
                    fibonaci.Add(1);
                }
                if (input <= fibonaci.Last())
                {
                    long k = fibonaci.Find(x => x == input);
                    if (k == 0)
                    {
                        Console.WriteLine("IsNotFibo");
                    }
                    else
                    {
                        Console.WriteLine("IsFibo");
                    }
                    continue;
                }
                while (fibonaci.Last() < input)
                {
                    saveNextElement(fibonaci);
                    if (input == fibonaci.Last())
                    {
                        Console.WriteLine("IsFibo");
                    }
                    if (input < fibonaci.Last())
                    {
                        Console.WriteLine("IsNotFibo");
                    }
                }
            }
        }
        public void saveNextElement(List<Int64> fibonaci)
        {
            fibonaci.Add(fibonaci.Last() + fibonaci[(fibonaci.Count - 2)]);
        }
    }
}
