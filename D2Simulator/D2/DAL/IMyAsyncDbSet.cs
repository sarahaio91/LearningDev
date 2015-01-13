using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace D2.DAL
{
    public interface IMyAsyncDbSet<TEntity> : IDbSet<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(params Object[] keyValues);
    }
}