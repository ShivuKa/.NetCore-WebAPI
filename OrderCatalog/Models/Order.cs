using CartService.Models;
using System;
using System.ComponentModel.DataAnnotations;
using ProductCatalog.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartService.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        public decimal TotalAmount { get; set; }
        [Required]
        public string Shipping_Address {get;set;}
        [Required]
        public string Billing_address {get;set;}  
        [Required]
        public int PostCode { get; set; }

        public string UserId { get; set; }

        // Add other properties or filters/ attributes as required.
    }
}
