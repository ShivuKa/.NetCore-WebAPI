using CartService.Data;
using CartService.Models;
using CartService.Repository.IRepository;
using CartService.Repository;

namespace CartService.Repository
{
    public class CartRepository : Repository<Cart>, ICart
    {
        private readonly CartDBContext _cart;
        public CartRepository(CartDBContext cart) : base(cart)
        {
            _cart = cart;
        }
        public void edit(Cart cart)
        {
            _cart.Update(cart);
        }

        public void Save()
        {
            _cart.SaveChanges();
        }
    }
}
