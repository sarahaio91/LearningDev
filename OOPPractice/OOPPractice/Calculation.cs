using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPPractice
{
    public interface ICalculation
    {
        double calculation(IPrice price);
    }
    class Calculation : ICalculation
    {
        public double calculation(IPrice price){
            return price.getPrice() + price.getPrice()*0.1;
        }
    }
}
