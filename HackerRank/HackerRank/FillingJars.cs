using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    class FillingJars
    {
        public void execute()
        {
            string input = Console.ReadLine();
            long numOfJars = Int64.Parse(input.Split(' ')[0]);
            long numOfOperation = Int64.Parse(input.Split(' ')[1]);
            long totalValue = 0;
            for (int i = 0; i < numOfOperation; i++)
            {
                string operation = Console.ReadLine();
                long firstJar = Int64.Parse(operation.Split(' ')[0]);
                long secondJar = Int64.Parse(operation.Split(' ')[1]);
                long value = Int64.Parse(operation.Split(' ')[2]);
                totalValue = totalValue + value * (secondJar - firstJar + 1);
                //Console.WriteLine(totalValue);
            }
            Console.WriteLine(totalValue / numOfJars);
        }
    }
}
