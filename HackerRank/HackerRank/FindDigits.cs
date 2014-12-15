using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    class FindDigits
    {
        public void execute()
        {
            int numTestCases = Int32.Parse(Console.ReadLine());
            for (int i = 0; i < numTestCases; i++)
            {
                string number = Console.ReadLine();
                int total = number.Length;
                int zero = number.Count(x => x == '0');
                int eliminate = 0;
                IEnumerable<char> num = number.Distinct();
                foreach (char n in num)
                {
                    if (n != '0')
                    {
                        if (Int64.Parse(number) % Int32.Parse(n.ToString()) != 0)
                        {
                            eliminate = eliminate + number.Count(x => x == n);
                        }
                    }
                }
                Console.WriteLine(total-zero-eliminate);
            }
        }
    }
}
