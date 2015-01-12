using D2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace D2.DAL
{
    public class D2Context : DbContext, ID2Context 
    {
        public DbSet<Hero> Heroes { set; get; }
        public IDbSet<Position> Positions { set; get; }
        public IMyAsyncDbSet<Map> Maps { set; get; }
    }
}