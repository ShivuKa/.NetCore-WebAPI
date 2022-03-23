using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ProductCatalog.Repository
{
    public class ProductRepository : Repository<Product>, IProduct
    {
        private readonly CatalogDbContext _catalogue;
        public ProductRepository(CatalogDbContext catalog) : base(catalog)
        {
            _catalogue= catalog;
        }

        public void edit(Product product)
        {
            _catalogue.Update(product);
        }

        public void Save()
        {
            _catalogue.SaveChanges();
        }
    }
}
