//using ECommerce.Data;
using ECommerce.Models;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CommerceContext _context;

        public ProductController(CommerceContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetOne(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null) return NotFound();

            return product;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public async Task<ActionResult<Product[]>> Purchase([FromBody] IEnumerable<Product> purchaseProducts)
        {
            List<Product> products = new List<Product>();
            try
            {
                foreach (Product item in purchaseProducts)
                {
                    var tmp = _context.Products.SingleOrDefault(p =>
                        p.ProductName == item.ProductName);
                    if (tmp.ProductQuantity - item.ProductQuantity < 0)
                    {
                        //All Good - Making sure all items are in stock first
                        throw new Exception("Insuffecient inventory.");
                    }
                    await _context.SaveChangesAsync();
                }


                foreach (Product item in purchaseProducts)
                {
                    var tmp = _context.Products.SingleOrDefault(p =>
                        p.ProductName == item.ProductName);
                    if (tmp.ProductQuantity - item.ProductQuantity < 0)
                    {
                        throw new Exception("Insuffecient inventory.");
                    }
                    tmp.ProductQuantity -= item.ProductQuantity;
                    products.Add(await _context.Products.FindAsync(tmp.ProductId));
                    await _context.SaveChangesAsync();
                }
                return Ok(products);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
