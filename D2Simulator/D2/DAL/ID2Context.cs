using D2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace D2.DAL
{
    public interface ID2Context
    {
        IMyAsyncDbSet<Hero> Heroes { set; get; }
        IDbSet<Position> Positions { set; get; }
        IMyAsyncDbSet<Map> Maps { set; get; }
    }
}