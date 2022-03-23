using CartService.Models;
using ProductCatalog.Repository.IRepository;

namespace CartService.Repository.IRepository
{
    public interface ICart: IRepository<Cart>
    {
        void edit(Cart cart);
        void Save();
    }
}
