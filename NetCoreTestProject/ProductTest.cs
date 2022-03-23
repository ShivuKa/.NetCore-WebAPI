using ProductCatalog.Repository.IRepository;
using Xunit;
using Moq;
using ProductCatalog.Models;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using ProductCatalog.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreTestProject
{
    public class ProductTest
    {
        public readonly Mock<IProduct> _product;
        private readonly Mock<ILogger<ProductController>> _logger;
        private readonly ProductController controller; 

        public ProductTest()
        {
            _product = new Mock<IProduct>();
            _logger = new Mock<ILogger<ProductController>>();
            controller = new ProductController(_logger.Object,_product.Object);
        }
        [Fact]
        public void GetListofProducts()
        {

            IEnumerable<Product> products = GetSampleProduct();
            _product.Setup(y => y.GetAll()).Returns(GetSampleProduct());
            var actionresult = controller.GetProductList("");
            var result = (Microsoft.AspNetCore.Mvc.ObjectResult)actionresult as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }
        private IEnumerable<Product> GetSampleProduct()
        {
            List<Product> output = new List<Product>
        {
            new Product
            {
                ProductId=1,
                Name="Shivi",
                Description="Mobile Orders",
                Category="Electronic",
                SubCategory="Mobile Phones",
                Price=999,
                DateAdded=Convert.ToDateTime( "2022-03-09"),
                ExpiryDate=Convert.ToDateTime( "2022-03-09"),
                UserId="auth0|6228aa4a8904e60069c29639"
            },
            new Product
            {
                ProductId=2,
                Name="Shivi",
                Description="Laptops",
                Category="Electronic",
                SubCategory="HP",
                Price=999,
                DateAdded=Convert.ToDateTime( "2022-03-09"),
                ExpiryDate=Convert.ToDateTime( "2022-09-09"),
                UserId="auth0|6228aa4a8904e60069c29639"
            }
        };
            return output;
        }
        [Fact]
        public void GetListofProductsByID()
        {
            var product = new Product();
            var Irepo = new Mock<IRepository<Product>>();
            IEnumerable<Product> products = GetSampleProduct();
            var i = Irepo.Setup(x => x.GetFirstorDefault(u => u.ProductId == 1));
            _product.Setup(x => x.GetFirstorDefault(u=>u.ProductId==1)).Returns(product);
            var actionresult = controller.GetProductById(1);
            var result = (Microsoft.AspNetCore.Mvc.ObjectResult)actionresult as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void AddProducts()
        {
            Product? pro = null;
            var productdetails = new Product()
            {
                ProductId = 10,
                Name = "Shivi",
                Description = "Mobile Orders",
                Category = "Electronic",
                SubCategory = "Mobile Phones",
                Price = 999,
                DateAdded = Convert.ToDateTime("2022-03-09"),
                ExpiryDate = Convert.ToDateTime("2022-03-09"),
                UserId = "auth0|6228aa4a8904e60069c29639"
            };
            _product.Setup(r => r.Add(productdetails)) 
        .Callback<Product>(x => pro = x);
            var sut = new TestClaims(_logger.Object, _product.Object).WithIdentity("john.doe@mail.com", "John Doe").Build();
            var re = sut.Post(productdetails);
            _product.Verify(x => x.Add(productdetails));
            Assert.Equal(pro.Name, productdetails.Name);
            Assert.Equal(pro.Price, productdetails.Price);  
            Assert.Equal(pro.Description, productdetails.Description);  
            Assert.Equal(pro.Category, productdetails.Category);    
            Assert.Equal(pro.SubCategory, productdetails.SubCategory);
          Assert.Equal(201,((Microsoft.AspNetCore.Mvc.StatusCodeResult)re).StatusCode);
        }
        [Fact]
        public void deleteproductnotfounddata()
        {
            List<Product> getproducts = GetProduct();
            var product = Mock.Of<Product>(x=>x.ProductId==1 );
            _product.Setup(x => x.GetAll()).Returns(getproducts);
            var actual = controller.GetProductList("");
            Assert.IsType<OkObjectResult>(actual);
            var sut = new TestClaims(_logger.Object, _product.Object).WithIdentity("john.doe@mail.com", "John Doe").Build();
            var mod = Mock.Of<Product>(x => x.ProductId == product.ProductId);
            var result = sut.Delete(product.ProductId);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        private List<Product> GetProduct()
        {
            List<Product> output = new List<Product>
        {
            new Product
            {
                ProductId=1,
                Name="Shivi",
                Description="Mobile Orders",
                Category="Electronic",
                SubCategory="Mobile Phones",
                Price=999,
                DateAdded=Convert.ToDateTime( "2022-03-09"),
                ExpiryDate=Convert.ToDateTime( "2022-03-09"),
                UserId="auth0|6228aa4a8904e60069c29639"
            },
            new Product
            {
                ProductId=2,
                Name="Shivi",
                Description="Laptops",
                Category="Electronic",
                SubCategory="HP",
                Price=999,
                DateAdded=Convert.ToDateTime( "2022-03-09"),
                ExpiryDate=Convert.ToDateTime( "2022-09-09"),
                UserId="auth0|6228aa4a8904e60069c29639"
            }
        };
            return output;
        }
        [Fact]
        public void getall()
        {
            _product.Setup(r => r.GetFirstorDefault(u=>u.Category== "Electronic"))
        .Returns( new Product());
            var result = controller.GetProductCategory("Electronic");
             Assert.IsType<OkObjectResult>(result);
        }
    }
}
