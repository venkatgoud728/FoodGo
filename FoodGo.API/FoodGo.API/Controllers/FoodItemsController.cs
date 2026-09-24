using FoodGo.API.Data;
using FoodGo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly FoodGoDbContext _context;

        public FoodItemsController(FoodGoDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodItems
        [HttpGet]
        public async Task<IActionResult> GetFoodItems()
        {
            var foodItems = await _context.FoodItems.ToListAsync();

            return Ok(foodItems);
        }

        // GET: api/FoodItems/restaurant/1
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetFoodItemsByRestaurant(int restaurantId)
        {
            var foodItems = await _context.FoodItems
                .Where(x => x.RestaurantId == restaurantId)
                .ToListAsync();

            return Ok(foodItems);
        }

        // POST: api/FoodItems
        [HttpPost]
        public async Task<IActionResult> CreateFoodItem(FoodItem foodItem)
        {
            var restaurant = await _context.Restaurants
                .FindAsync(foodItem.RestaurantId);

            if (restaurant == null)
            {
                return BadRequest("Restaurant not found");
            }

            var category = await _context.Categories
                .FindAsync(foodItem.CategoryId);

            if (category == null)
            {
                return BadRequest("Category not found");
            }

            _context.FoodItems.Add(foodItem);

            await _context.SaveChangesAsync();

            return Ok(foodItem);
        }

        // PUT: api/FoodItems/6
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodItem(
            int id,
            FoodItem foodItem)
        {
            var existingFoodItem = await _context.FoodItems
                .FindAsync(id);

            if (existingFoodItem == null)
            {
                return NotFound("Food item not found");
            }

            existingFoodItem.Name = foodItem.Name;
            existingFoodItem.Description = foodItem.Description;
            existingFoodItem.Price = foodItem.Price;
            existingFoodItem.CategoryId = foodItem.CategoryId;
            existingFoodItem.IsAvailable = foodItem.IsAvailable;

            await _context.SaveChangesAsync();

            return Ok(existingFoodItem);
        }

        // DELETE: api/FoodItems/6
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(int id)
        {
            var foodItem = await _context.FoodItems
                .FindAsync(id);

            if (foodItem == null)
            {
                return NotFound("Food item not found");
            }

            _context.FoodItems.Remove(foodItem);

            await _context.SaveChangesAsync();

            return Ok("Food item deleted successfully");
        }

        // PUT: api/FoodItems/6/availability
        [HttpPut("{id}/availability")]
        public async Task<IActionResult> UpdateAvailability(
            int id,
            bool isAvailable)
        {
            var foodItem = await _context.FoodItems
                .FindAsync(id);

            if (foodItem == null)
            {
                return NotFound("Food item not found");
            }

            foodItem.IsAvailable = isAvailable;

            await _context.SaveChangesAsync();

            return Ok(foodItem);
        }
    }
}