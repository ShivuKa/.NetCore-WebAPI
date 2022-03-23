using ProductCatalog.Models;

namespace ProductCatalog.Repository.IRepository
{
    public interface IProduct : IRepository<Product>
    {
        void edit(Product product);
        void Save();
    }
}
