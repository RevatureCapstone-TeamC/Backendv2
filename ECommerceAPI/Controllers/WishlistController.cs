using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly CommerceContext _context;

        public WishlistController(CommerceContext context)
        {
            _context = context;
        }

        // GET: api/Wishlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlist()
        {

            return await _context.Wishlist.ToListAsync();
        }

        // GET: api/Wishlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetWishlistProducts(int? id)
        {

            var wishlist = await _context.Wishlist.Where(x => x.fk_UserId == id).ToListAsync();
            var products = await _context.Products.ToListAsync();

            List<Product> intersection = products.IntersectBy(wishlist.Select(x => x.fk_ProductId), (y) => y.ProductId).ToList();
            List<Product> updatedList= new List<Product>();
            foreach (var product in intersection)
            {
                foreach(var item in wishlist)
                {
                    if (product.ProductId == item.fk_ProductId)
                    {
                        var temp = product;
                        temp.ProductId = item.WishlistId;
                        updatedList.Add(temp);
                    }
                }
            }
           
            if (intersection is null)
            {
                return NotFound();
            }
            return updatedList;
        }

        // PUT: api/Wishlists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWishlist(int? id, Wishlist wishlist)
        {
            if (id != wishlist.WishlistId)
            {
                return BadRequest();
            }

            _context.Entry(wishlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishlistExists(id))
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

        // POST: api/Wishlists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wishlist>> PostWishlist(Wishlist wishlist)
        {
            _context.Wishlist.Add(wishlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWishlist", new { id = wishlist.WishlistId }, wishlist);
        }

        // DELETE: api/Wishlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int? id)
        {
            var wishlist = await _context.Wishlist.FindAsync(id);
            if (wishlist is null)
            {
                return NotFound();
            }

            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WishlistExists(int? id)
        {
            return (_context.Wishlist?.Any(e => e.WishlistId == id)).GetValueOrDefault();
        }
    }
}