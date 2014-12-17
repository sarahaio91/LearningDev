using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrictionOperator
{
    class Program
    {
        static void Main(string[] args)
        {
        }
        public void LinQ1()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            Console.WriteLine(numbers(x => x > 5));
        }
    }
}
