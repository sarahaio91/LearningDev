using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPPractice
{
    public interface IProduct
    {
        string getDescription();
    }
    class Fish : IProduct, IPrice
    {
        public string getDescription()
        {
            return "I'm a fish!";
        }
        public double getPrice()
        {
            return 5.0;
        }
    }
}
