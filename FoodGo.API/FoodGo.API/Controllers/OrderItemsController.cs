using FoodGo.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly FoodGoDbContext _context;

        public OrderItemsController(FoodGoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderItems()
        {
            var orderItems = await _context.OrderItems.ToListAsync();

            return Ok(orderItems);
        }
    }
}