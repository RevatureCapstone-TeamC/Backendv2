using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models;

public class Deal
{
    [Key]
    public int? DealId { get; set; }
    public int? fk_Product_Id { get; set; }
    public decimal? SalePrice { get; set; }
}