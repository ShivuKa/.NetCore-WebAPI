using CartService.Models;
using CartService.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrderCatalog.Controllers;
using ProductCatalog.Controllers;
using ProductCatalog.Models;
using ProductCatalog.Repository.IRepository;
using System;
using System.Collections.Generic;
using Xunit;

namespace NetCoreTestProject
{
    public class CartTest
    {
        public readonly Mock<IProduct> _product;
        public readonly Mock<ICart> _cart;
        private readonly Mock<ILogger<CartController>> _logger;

        public CartTest()
        {
            _cart = new Mock<ICart>();
            _product = new Mock<IProduct>();
            _logger = new Mock<ILogger<CartController>>();
        }
        [Fact]
        public void GetCart()
        {

            IEnumerable<Cart> products = GetSampleCart();
            _cart.Setup(y => y.GetAll()).Returns(GetSampleCart());
            var controller = new CartController(_logger.Object,_cart.Object, _product.Object);
            var actionresult = controller.AddToCart("");
            var result = (Microsoft.AspNetCore.Mvc.ObjectResult)actionresult as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }
        private IEnumerable<Cart> GetSampleCart()
        {
            List<Cart> output = new List<Cart>
        {
            new Cart
            {
                CartId = 2,
                ProductId = 10,
                Quantity = 1,
                price= 99,
                UserId="auth0|6228aa4a8904e60069c29639"
            },
            new Cart
            {
                CartId = 3,
                ProductId = 1,
                Quantity = 10,
                price= 9,
                UserId="auth0|6228aa4a8904e60069c29639"
            }
        };
            return output;
        }
        
        [Fact]
        public void AddCartifproductdoesnotexist()
        {
            Cart? cart = null;
            var cartdetails = new Cart()
            {
                CartId = 4,
                ProductId = 1,
                Quantity = 1,
                price= 99,
                UserId = "auth0|6228aa4a8904e60069c29639"

            };
            _cart.Setup(r => r.Add(cartdetails))
        .Callback<Cart>(x => cart = x);
            var sut = new TestClaims(_logger.Object,_cart.Object, _product.Object).WithIdentity("john.doe@mail.com", "John Doe").Buildcart();
            var controller = new CartController(_logger.Object, _cart.Object,_product.Object);
            var re = sut.AddToCart(cartdetails);
            Assert.IsType<NotFoundObjectResult>(re);    
        }
        [Fact]
        public void deleteCartnotfounddata()
        {
            var crt = new Cart() { CartId = 1234 };
            _cart.Setup(x => x.Remove(crt));
            var sut = new TestClaims(_logger.Object,_cart.Object, _product.Object).WithIdentity("john.doe@mail.com", "John Doe").Build();
            var controller = new CartController(_logger.Object, _cart.Object, _product.Object);
            var result = sut.Delete(crt.CartId);
            Assert.IsType<NotFoundObjectResult>(result);

        }
    }
}
