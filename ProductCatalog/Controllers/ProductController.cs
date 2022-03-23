using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductCatalog.Models;
using ProductCatalog.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ProductCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProduct _productCatalog;
        public ProductController(ILogger<ProductController> logger, IProduct catalogDbContext)
        {
            _logger = logger;
            _productCatalog = catalogDbContext;
        }

        #region Show Product
        // api/product
        [HttpGet]
        public IActionResult GetProductList(string sortOrder)
        {
            try
            {
                sortOrder = sortOrder?.ToLower();
                var productlist = _productCatalog.GetAll(); 
                IQueryable<Product> products = sortOrder switch
                {
                    "desc" => productlist.OrderByDescending(p => p.Price).AsQueryable(),
                    "asc" => productlist.OrderBy(p => p.Price).AsQueryable(),
                    _=> productlist.AsQueryable(),    
                };
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in GetProductList : "+ ex);
                return StatusCode(500, "An error has occured.");
            }
           
        }

        // api/product/#productId
        [HttpGet("{productId}")]
        public IActionResult GetProductById(int productId)
        {
            try
            {
                var selectProducts = _productCatalog.GetFirstorDefault(q => q.ProductId == productId);
               
                if (selectProducts == null )
                {
                    return NotFound("No product found against this id");
                }
                else
                    return Ok(selectProducts);
            }
            catch (Exception ex)
            {
                //_logger.LogError("Exception occured in GetProductList while getting product for product ID. " + productId +" : ", ex);
                _logger.LogError("Exception occured in GetProductList while getting product for product ID. " + productId + " : "+ ex);
                return StatusCode(500, "An error has occured while getting product for product ID. " + productId);
            }            
        }

        [HttpGet("[action]")]
        public IActionResult MyProducts(int productId)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var selectProducts = _productCatalog.GetFirstorDefault(q => q.ProductId == productId);
                if (selectProducts == null )
                {
                    return NotFound("No product found against this id");
                }
                else
                    return Ok(selectProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in GetProductList while getting product for product ID. " + productId + " : "+ ex);
                return StatusCode(500, "An error has occured while getting product for product ID. " + productId);
            }
        }

        // api/product/GetProductCategory/#productId
        [HttpGet("[action]")]
        [ResponseCache(Duration =60 )]
        public IActionResult GetProductCategory(string category)
        {
            try
            {
                var selectProducts = _productCatalog.GetFirstorDefault(q => q.Category == category);
                if (selectProducts == null )
                {
                    return NotFound("No product found against this category");
                }
                else
                {
                    return Ok(selectProducts);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in GetProductListByCategory : "+ ex);
                return StatusCode(500, "An error has occured.");
            }
        }

        #endregion

        // api/product/ProductPaging?pageNo=#pageNo&pageSize=#pageSize
        [HttpGet("[action]")]
        public IActionResult ProductPaging(int? pageNo, int? pageSize)
        {
            try
            {
                IEnumerable<Product> products = _productCatalog.GetAll();
                int currentPageSize = pageSize ?? 3;
                int currentPageNo = pageNo ?? 1;
                return Ok(products.Skip((currentPageNo - 1) * currentPageSize).Take(currentPageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in ProductPaging : "+ ex);
                return StatusCode(500, "An error has occured.");
            }           
        }

        #region Crud Operation
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                product.UserId = userId;

                _productCatalog.Add(product);
                _productCatalog.Save();
                return StatusCode(StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in adding product details : "+ ex);
                return StatusCode(500, "An error has occured.");
            }
        }

        [HttpPut("{productId}")]
        public IActionResult Put(int productId, [FromBody] Product product)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var selectedProduct = _productCatalog.GetFirstorDefault(x=>x.ProductId==productId);
                if (selectedProduct == null)
                    return NotFound("No Records Found for this ID - " + productId);

                if (userId != selectedProduct.UserId)
                    return BadRequest("Sorry....You don't have access to Update this record");

                else
                {
                    selectedProduct.Name = product.Name;
                    selectedProduct.Description = product.Description;
                    selectedProduct.Category = product.Category;
                    selectedProduct.SubCategory = product.SubCategory;
                    selectedProduct.Price = product.Price;
                    selectedProduct.DateAdded = product.DateAdded;
                    selectedProduct.ExpiryDate = product.ExpiryDate;
                    _productCatalog.Save();
                    return Ok("Record Updated Successfully in the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in update product details : ", ex);
                return StatusCode(500, "An error has occured.");
            }            
        }

        [HttpDelete("{productId}")]
        public IActionResult Delete(int productId)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                Product selectedProduct = _productCatalog.GetFirstorDefault(x=>x.ProductId==productId);
                if (selectedProduct == null)
                    return NotFound("No Records Found for this ID - " + productId);

                if (userId != selectedProduct.UserId)
                    return BadRequest("Sorry....You don't have access to delete this record");
                else
                {
                    _productCatalog.Remove(selectedProduct);
                    _productCatalog.Save();
                    return Ok("Record Deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in delete product : ", ex);
                return StatusCode(500, "An error has occured while deleting product.");
            }           
        }
        #endregion
    }
}
