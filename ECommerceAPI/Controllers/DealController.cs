
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DealController : ControllerBase
{
    private readonly CommerceContext _context;

    public DealController(CommerceContext context) {
        this._context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Deal>>> GetDeals(){

        return await _context.Deals.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Deal>> GetDeal(int id){
        var deal = await _context.Deals.FindAsync(id);

        if (deal is null) return NotFound();

        
        return deal;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDeal(Deal deal){
        deal.DealId = null;

        //Product has to be valid
        if (_context.Products.Find(deal.fk_Product_Id) is null)
            return BadRequest("This product does not exist");
        
        //Price can't be 0 or less
        if (deal.SalePrice <= 0)
            return BadRequest("Sale price can't be $0.0 or lower");

        _context.Deals.Add(deal);
        await _context.SaveChangesAsync();

        return Ok(deal);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDeal(Deal deal){
        _context.Entry(deal).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException){
            if (!_context.Deals.Any(e => e.DealId == deal.DealId)) 
                return NotFound();
            else throw;
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDeals(){
        //Should we check that deals is non-empty here first?
        var deals = await _context.Deals.ToListAsync();

        if (!deals.Any()) 
            return BadRequest("No deals to delete...");

        try {
            _context.Deals.RemoveRange(deals);
            await _context.SaveChangesAsync();
        }
        catch (Exception e){
            return BadRequest($"Something happened while clearing deals, exception caught...\n{e.Message}");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDeal(int id){
        var deal = await _context.Deals.FindAsync(id);
        
        // ! Deal isn't in there
        if (deal is null)
            return NotFound();

        _context.Deals.Remove(deal);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}