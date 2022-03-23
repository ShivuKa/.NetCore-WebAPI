using ProductCatalog.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartService.Models
{
    public class Cart
    {[Key]
        public int CartId { get; set; }
        public int ProductId { get; set; }
        [Required]
        [Range(5,1000, ErrorMessage = "Please enter Quantity between 5 and 1000")]
        public int Quantity { get; set; }
        //[ForeignKey("ProductId")]
        public Product product { get; set; }

        public decimal price { get; set; }

        public string UserId { get; set; }
        // Add other properties or filters/ attributes as required.
    }
}
