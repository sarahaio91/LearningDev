using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    class GameOfThrones
    {
        public void execute()
        {
            string input = Console.ReadLine();
            IEnumerable<char> distinctChar = input.Distinct();
            int globalCount = 0;
            bool flag = true;
            foreach (char var in distinctChar)
            {
                int count = input.Count(x => x == var);
                if (count % 2 != 0)
                    globalCount = globalCount + 1;
                if (globalCount >= 2)
                {
                    flag = false;
                    break;
                }
            }
            output(flag);
            
        }
        private void output(bool flag)
        {
            if (flag)
                Console.WriteLine("YES");
            else 
                Console.WriteLine("NO");
        }
    }
}
