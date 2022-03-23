using CartService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProductCatalog.Repository.IRepository;
using CartService.Repository.IRepository;

namespace OrderCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICart _cart;
        private readonly IProduct _product;
        public CartController(ILogger<CartController> logger, ICart cartDB, IProduct product)
        {
            _logger = logger;
            _cart = cartDB;
            _product = product;
        }
        // api/Cart
        [HttpGet]
        public IActionResult AddToCart(string sortOrder)
        {
            try
            {
                sortOrder = sortOrder?.ToLower();
                var cartlist = _cart.GetAll();
                IQueryable<Cart> cart = sortOrder switch
                {
                    "desc" => cartlist.OrderByDescending(p => p.ProductId).AsQueryable(),
                    "asc" => cartlist.OrderBy(p => p.ProductId).AsQueryable(),
                    _ => cartlist.AsQueryable(),
                };
                if(cart.Count() == 0)
                {
                    return Ok("No Products Added till now!!!");
                }
                else
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in GetProductList : ", ex);
                return StatusCode(500, "An error has occured.");
            }

        }

        [HttpPost]
        public IActionResult AddToCart(Cart addtocart)
        {
            try
            {
                 string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                addtocart.UserId = userId;
                var findproductid =_product.GetFirstorDefault(x=>x.ProductId==addtocart.ProductId);
                if (findproductid == null)
                {
                    return NotFound("Selected ProductId doesnot match with the existing products.");
                }
                else
                {
                    addtocart.price = findproductid.Price* addtocart.Quantity;
                    _cart.Add(addtocart);
                    _cart.Save();
                    return StatusCode(StatusCodes.Status201Created);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in adding product details : ", ex);
                return StatusCode(500, "An error has occured.");
            }
            //throw new NotImplementedException("Not implemented exception");
        }

        [HttpPut("{productId}")]
        public IActionResult Put(int productId, [FromBody] Cart Cart)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                Cart selectedProduct = _cart.GetFirstorDefault(x=>x.CartId==productId);
                var findproductid = _product.GetFirstorDefault(x => x.ProductId == Cart.ProductId);
                if (selectedProduct == null)
                    return NotFound("No Records Found for this ID - " + productId);

                if (userId != selectedProduct.UserId)
                    return BadRequest("Sorry....You don't have access to Update this record");

                else
                {
                    selectedProduct.price = findproductid.Price * Cart.Quantity;
                    selectedProduct.ProductId = Cart.ProductId;
                    selectedProduct.Quantity = Cart.Quantity;
                    _cart.edit(selectedProduct);
                    _cart.Save();
                    return Ok("Record Updated Successfully in the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in update product details : ", ex);
                return StatusCode(500, "An error has occured.");
            }
        }
        [HttpDelete("{cartid}")]
        public IActionResult Delete(int cartid)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                Cart selectedProduct = _cart.GetFirstorDefault(x=>x.CartId== cartid);
                if (selectedProduct == null)
                    return NotFound("No Records Found for this ID - " + cartid);

                if (userId != selectedProduct.UserId)
                    return BadRequest("Sorry....You don't have access to delete this record");
                else
                {
                    _cart.Remove(selectedProduct);
                    _cart.Save();
                    return Ok("Record Deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in delete product : ", ex);
                return StatusCode(500, "An error has occured while deleting product.");
            }
        }



        // add other methods and caching functionalities as required
    }
}
