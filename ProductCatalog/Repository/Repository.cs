﻿using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ProductCatalog.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CatalogDbContext _catalogue;
        public DbSet<T> DBset;
        public Repository(CatalogDbContext catalog)
        {
            _catalogue = catalog;
            DBset = _catalogue.Set<T>();
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
