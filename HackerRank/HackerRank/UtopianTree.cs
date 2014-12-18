using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    class UtopianTree
    {
        public void execute()
        {
            int numOfCases = Int32.Parse(Console.ReadLine());
            int currentCycle = 0;
            List<long> tree = new List<long>();
            for (int i = 0; i< numOfCases; i++){
                int cycle = Int32.Parse(Console.ReadLine());
                if (tree.Count == 0)
                {
                    tree.Add(1);
                }
                while (currentCycle < cycle)
                {
                    if (tree.IndexOf(tree.Last()) % 2 == 0)
                        tree.Add(tree.Last() * 2);
                    else
                        tree.Add(tree.Last() + 1);
                    currentCycle = currentCycle + 1;
                }
                if (currentCycle >= cycle)
                {
                    Console.WriteLine(tree[cycle]);
                }
            }
        }
    }
}
