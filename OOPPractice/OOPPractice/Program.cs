using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            //IProduct fish = new Fish();
            //Console.WriteLine(fish.getPrice());
            //ICalculation cal = new Calculation();
            //IPrice price = new Fish();
            //Console.WriteLine(cal.calculation(price));
            //IDescription des = new Fish();
            //Console.WriteLine(des.getDescription());
            IDatabaseNew data = new CustomerNew();
            data.Add();
            data.Read();
        }
    }
}
