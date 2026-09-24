

using FoodGo.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly FoodGoDbContext _context;

        public CartsController(FoodGoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            var carts = await _context.Cart.ToListAsync();

            return Ok(carts);
        }
    }
}