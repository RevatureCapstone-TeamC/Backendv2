using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using ECommerce.Models;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CommerceContext _context;

        public CartController(CommerceContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCart()
        {
            return await _context.Cart.ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCart(int? id)
        {

            var cart = await _context.Cart.Where(x => x.fk_UserID == id).ToListAsync();
            var products = await _context.Products.ToListAsync();
            var deals = await _context.Deals.ToListAsync();
            List<Product> updatedList = new List<Product>();

            foreach (var item in cart)
            {
                foreach (var product in products)
                {
                    if (product.ProductId == item.fk_ProductID)
                    {
                        Product temp = new Product();
                        var deal = deals.Where(d => d.fk_Product_Id == product.ProductId).ToList();
                        if (deal != null && deal.Any())
                        {
                            temp.ProductPrice = deal[0].SalePrice;
                        } else
                        {
                            temp.ProductPrice = product.ProductPrice;
                        }
                        temp.ProductId = product.ProductId;
                        temp.ProductDescription = product.ProductDescription;
                        temp.ProductQuantity = product.ProductQuantity;
                        temp.ProductName = product.ProductName;
                        temp.ProductImage = product.ProductImage;

                        temp.ProductId = item.CartId;
                        updatedList.Add(temp);
                    }
                }
            }

            if (updatedList is null)
            {
                return NotFound();
            }

            return updatedList;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int? id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            cart.CartId = null;
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.CartId }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int? id)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart is null)
            {
                return NotFound();
            }

            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int? id)
        {
            return (_context.Cart?.Any(e => e.CartId == id)).GetValueOrDefault();
        }
    }
}