using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models;

public class Cart
{
    [Key]
    public int? CartId { get; set; }
    public int? fk_ProductID { get; set; }
    public int? fk_UserID { get; set; }
}