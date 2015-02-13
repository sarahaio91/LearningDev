using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPPractice
{
    public interface IDescription
    {
        string getDescription();
    }
    public interface IProduct
    {
        string getDescription();
        double getPrice();
    }
    class Fish : IProduct, IPrice, IDescription
    {
        double price = 5.0;
        string description = "I'm a fish!";
        public string getDescription()
        {
            return description;
        }
        public double getPrice()
        {
            return price;
        }
    }
}
