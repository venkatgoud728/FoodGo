using FoodGo.API.Models;
using FoodGo.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly FoodGoDbContext _context;

        public CartItemsController(FoodGoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var cartItems = await _context.CartItems.ToListAsync();

            return Ok(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(CartItem cartItem)
        {
            // Check cart exists
            var cart = await _context.Cart
                .FindAsync(cartItem.CartId);

            if (cart == null)
            {
                return NotFound($"Cart with ID {cartItem.CartId} not found");
            }

            // Check food item exists
            var foodItem = await _context.FoodItems
                .FindAsync(cartItem.FoodItemId);

            if (foodItem == null)
            {
                return NotFound("Food item not found");
            }

            // Check availability
            if (!foodItem.IsAvailable)
            {
                return BadRequest("Food item is not available");
            }

            // Check quantity
            if (cartItem.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            // Check whether item already exists
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(x =>
                    x.CartId == cartItem.CartId &&
                    x.FoodItemId == cartItem.FoodItemId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return Ok(existingItem ?? cartItem);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserCart(int userId)
        {
            var cart = await _context.Cart
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var cartItems = await _context.CartItems
                .Where(x => x.CartId == cart.CartId)
                .Join(
                    _context.FoodItems,
                    cartItem => cartItem.FoodItemId,
                    foodItem => foodItem.FoodItemId,
                    (cartItem, foodItem) => new
                    {
                        cartItem.CartItemId,
                        cartItem.FoodItemId,
                        foodItem.Name,
                        foodItem.Price,
                        cartItem.Quantity,
                        SubTotal = foodItem.Price * cartItem.Quantity
                    })
                .ToListAsync();

            var total = cartItems.Sum(x => x.SubTotal);

            return Ok(new
            {
                userId,
                cartId = cart.CartId,
                items = cartItems,
                total
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, CartItem cartItem)
        {
            var existingItem = await _context.CartItems
                .FindAsync(id);

            if (existingItem == null)
            {
                return NotFound("Cart item not found");
            }

            if (cartItem.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            existingItem.Quantity = cartItem.Quantity;

            await _context.SaveChangesAsync();

            return Ok(existingItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            var cartItem = await _context.CartItems
                .FindAsync(id);

            if (cartItem == null)
            {
                return NotFound("Cart item not found");
            }

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            return Ok("Cart item removed successfully");
        }

    }
}