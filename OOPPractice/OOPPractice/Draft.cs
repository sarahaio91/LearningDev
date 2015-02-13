using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPPractice
{
    public interface IDiscount
    {
        double getDiscounted(double TotalSales);
    }
    public interface IDatabase
    {
        void Add();
    }
    public class Customer
    {
        private int _CustType;

        public int CustType
        {
            get { return _CustType; }
            set { _CustType = value; }
        }

        public virtual double getDiscount(double TotalSales)
        {
            if (_CustType == 1)
            {
                return TotalSales - 100;
            }
            else
            {
                return TotalSales - 50;
            }
        }

        public virtual void Add()
        {
            try
            {
                // Database code goes here
            }
            catch (Exception ex)
            {
            }
        }
    }
    
    public class GoldCustomer : IDiscount, IDatabase
    {
        public double getDiscounted(double TotalSale)
        {
            return TotalSale * 0.1;
        }
        public void Add()
        {

        }
    }
    public class Enquiries : IDiscount
    {
        public double getDiscounted(double TotalSales)
        {
            return TotalSales * 0.2;
        }
    }
    public interface IDatabaseNew : IDatabase
    {
        void Read();
    }
    public class CustomerNew : IDatabaseNew, IDatabase
    {
        public void Add()
        {
            Console.WriteLine("Adding");
        }
        public void Read()
        {
            Console.WriteLine("Reading");
        }
    }
}
