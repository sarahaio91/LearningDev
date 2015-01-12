using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace D2.DAL
{
    public interface IMyAsyncDbSet<T> : IDbSet<T> where T : class
    {
        Task<T> FindAsync(params Object[] keyValues);
    }
}