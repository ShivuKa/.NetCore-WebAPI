using System;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Models
{
    public class Product
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "Length must be under 50")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Length must be under 200")]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }
        
        public string SubCategory { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
        public DateTime ExpiryDate { get; set; }

        // Add other properties or filters/ attributes as required.
        public string UserId { get; set; }
    }
}
