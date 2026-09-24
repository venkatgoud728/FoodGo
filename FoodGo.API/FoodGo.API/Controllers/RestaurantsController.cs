using FoodGo.API.Data;
using FoodGo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly FoodGoDbContext _context;

        public RestaurantsController(FoodGoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _context.Restaurants.ToListAsync();

            return Ok(restaurants);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);

            await _context.SaveChangesAsync();

            return Ok(restaurant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, Restaurant restaurant)
        {
            var existingRestaurant = await _context.Restaurants.FindAsync(id);

            if (existingRestaurant == null)
            {
                return NotFound();
            }

            existingRestaurant.Name = restaurant.Name;
            existingRestaurant.Address = restaurant.Address;
            existingRestaurant.City = restaurant.City;
            existingRestaurant.Rating = restaurant.Rating;
            existingRestaurant.DeliveryTime = restaurant.DeliveryTime;

            await _context.SaveChangesAsync();

            return Ok(existingRestaurant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);

            await _context.SaveChangesAsync();

            return Ok("Restaurant deleted successfully");
        }
    }
}