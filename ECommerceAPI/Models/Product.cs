using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Product
    {
        [Key]
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? ProductQuantity { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }

        /* public Product() { } 

        public Product(int? id, string? name, int? quantity, decimal? price, string? description, string? image)
        {
            ProductId = id;
            ProductName = name;
            ProductQuantity = quantity;
            ProductPrice = price;
            ProductDescription = description;
            ProductImage = image;
        }
        */
    }
}