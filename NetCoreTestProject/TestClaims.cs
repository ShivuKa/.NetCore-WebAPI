
using CartService.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderCatalog.Controllers;
using ProductCatalog.Controllers;
using ProductCatalog.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NetCoreTestProject
{
    public class TestClaims
    {
        private ClaimsIdentity _identity;
        private ClaimsPrincipal _user;
        private ControllerContext _controllerContext;
        private readonly ILogger<ProductController> _logger;
        private readonly ILogger<CartController> _loggercart;
        private readonly IProduct _product;
        private readonly ICart _cart;

        public TestClaims(ILogger<ProductController> logger, IProduct catalogDbContext)
        {
            _logger = logger;   
            _product= catalogDbContext; 
            _identity = new ClaimsIdentity();
            _user = new ClaimsPrincipal(_identity);
            _controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = _user } };
        }
        public TestClaims(ILogger<CartController> logger,ICart cart, IProduct catalogDbContext)
        {
            _cart = cart;
            _loggercart = logger;
            _product = catalogDbContext;
            _identity = new ClaimsIdentity();
            _user = new ClaimsPrincipal(_identity);
            _controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = _user } };
        }

        public TestClaims WithClaims(IDictionary<string, string> claims)
        {
            _identity.AddClaims(claims.Select(c => new Claim(c.Key, c.Value)));
            return this;
        }

        public TestClaims WithIdentity(string userId, string userName)
        {
            _identity.AddClaims(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName)
            });
            return this;
        }

        public TestClaims WithDefaultIdentityClaims()
        {
            _identity.AddClaims(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testId"),
                new Claim(ClaimTypes.Name, "testName")
            });
            return this;
        }

        public ProductController Build()
        {
            return new ProductController(_logger, _product)
            {
                ControllerContext = _controllerContext
            };
        }
        public CartController Buildcart()
        {
            return new CartController(_loggercart,_cart, _product)
            {
                ControllerContext = _controllerContext
            };
        }
    }
}
