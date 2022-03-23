using CartService.Data;
using CartService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductCatalog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly CartDBContext _cartDBContext;
        private readonly CatalogDbContext _product;

        public OrderController(ILogger<OrderController> logger, CartDBContext cart, CatalogDbContext prodct)
        {
            _logger = logger;
            _cartDBContext = cart;
            _product = prodct;
        }

        [HttpGet]
        public IActionResult PreviousOrders(string sortOrder)
        {
            try
            {
                sortOrder = sortOrder?.ToLower();
                IQueryable<Order> Orders = sortOrder switch
                {
                    "desc" => _cartDBContext.Orders.OrderByDescending(p => p.OrderId),
                    "asc" => _cartDBContext.Orders.OrderBy(p => p.OrderId),
                    _ => _cartDBContext.Orders,
                };
                if (Orders.Count() == 0)
                {
                    return Ok("No Orders are Placed till now!!!");
                }
                else
                    return Ok(Orders);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in GetProductList : ", ex);
                return StatusCode(500, "An error has occured.");
            }

        }
        [HttpPost]
        public IActionResult OrderReview( Order order)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                order.UserId = userId;
                var findcartid = _cartDBContext.Carts.Find(order.CartId);
                var findproductid = _cartDBContext.Carts.Find(findcartid.ProductId);
                
                if (_product.ProductList.Find(findcartid.ProductId) == null)
                {
                    return NotFound("Selected ProductId doesnot match with the existing products.");
                }
                else
                {
                    decimal amount = findcartid.price;
                    int quantity = findcartid.Quantity;
                    order.TotalAmount = amount*quantity;   
                    _cartDBContext.Orders.Add(order);
                    _cartDBContext.SaveChanges();
                    return StatusCode(StatusCodes.Status201Created,"Order Confirmed!!!");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in adding product details : ", ex);
                return StatusCode(500, "An error has occured.");
            }
        }

        [HttpPut("{OrderId}")]
        public IActionResult Put(int OrderId, [FromBody] Order Order)
        {
            try
            {
                var findorderid = _cartDBContext.Orders.Find(Order.OrderId);
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var findproductid  = _cartDBContext.Orders.Find(Order.Cart.ProductId);
                if (findproductid == null)
                    return NotFound("No Records Found for this ID - " + Order.Cart.ProductId);
                
                if (userId != findproductid.UserId)
                    return BadRequest("Sorry....You don't have access to Update this record");

                else
                {
                    //var amount = _product.ProductList.Where(u => u.ProductId == Order.ProductId).Select(x => x.Price);
                    //var quantity = _cartDBContext.Carts.Where(x => x.ProductId == Order.ProductId).Select(x => x.Quantity);
                    //Order.TotalAmount = Convert.ToInt32(amount) * Convert.ToInt32(quantity);
                    findorderid.Shipping_Address = Order.Shipping_Address;
                    findorderid.Billing_address = Order.Billing_address;
                    findorderid.PostCode = Order.PostCode;
                    _cartDBContext.SaveChanges();
                    return Ok("Record Updated Successfully in the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in update product details : ", ex);
                return StatusCode(500, "An error has occured.");
            }
        }
        [HttpDelete("{orderid}")]
        public IActionResult Delete(int orderid)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                Order selectedProduct = _cartDBContext.Orders.Find(orderid);
                if (selectedProduct == null)
                    return NotFound("No Records Found for this ID - " + orderid);

                if (userId != selectedProduct.UserId)
                    return BadRequest("Sorry....You don't have access to delete this record");
                else
                {
                    _cartDBContext.Orders.Remove(selectedProduct);
                    _cartDBContext.SaveChanges();
                    return Ok("Record Deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in delete product : ", ex);
                return StatusCode(500, "An error has occured while deleting product.");
            }
        }


        // add other methods and caching functionalities as required.
    }
}
