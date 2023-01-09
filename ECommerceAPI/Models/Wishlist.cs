using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models;
public class Wishlist 
{
    [Key]
    public int? WishlistId { get; set; }
    public int? fk_ProductId { get; set; }
    public int? fk_UserId { get; set; }

}