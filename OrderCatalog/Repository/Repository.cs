using CartService.Data;
using CartService.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CartService.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CartDBContext _cart;
        internal DbSet<T> DBset;
        public Repository(CartDBContext cart)
        {
            _cart = cart;
            DBset = _cart.Set<T>();
        }
        public void Add(T entity)
        {
            DBset.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = DBset;
            return query;
        }

        public T GetFirstorDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = DBset;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            DBset.Remove(entity);
        }
    }
}
