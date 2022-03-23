using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProductCatalog.Repository.IRepository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        T GetFirstorDefault(Expression<Func<T, bool>> filter);
        void Remove(T entity);
    }
}
