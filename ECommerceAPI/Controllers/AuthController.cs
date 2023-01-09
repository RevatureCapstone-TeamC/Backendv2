using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;

namespace ECommerce.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CommerceContext _context;

        public AuthController(CommerceContext context)
        {
            this._context = context;
        }

        // * Create a user, returns either BadRequest (400) or CreatedAtAction (201) response
        [Route("auth/register")]
        [HttpPost]
        public async Task<ActionResult> Register(User newUser)
        {
            newUser.UserId = null;
            newUser.IfAdmin = false;

            try {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch {
                return BadRequest();
            }
            return Ok(newUser);
            
        }

        [Route("auth/login")]
        [HttpPost]
        public async Task<ActionResult<User>> Login(User LR)
        {
            
            var response = _context.Users.Where(u => u.UserEmail==LR.UserEmail 
                && u.UserPassword == LR.UserPassword).FirstOrDefault();
            if (response is null) {
                return BadRequest("Invalid credentials");
            }
            return response;
        }

        [Route("auth/logout")]
        [HttpPost]
        public ActionResult Logout()
        { 
            return Ok();
        }
    }
}
