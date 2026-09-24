using FoodGo.API.Data;
using FoodGo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FoodGoDbContext _context;

        public OrdersController(FoodGoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();

            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int userId, int addressId)
        {
            // Find user's cart
            var cart = await _context.Cart
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            // Get cart items
            var cartItems = await _context.CartItems
                .Where(x => x.CartId == cart.CartId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return BadRequest("Cart is empty");
            }

            // Check address
            var address = await _context.Addresses
                .FirstOrDefaultAsync(x =>
                    x.AddressId == addressId &&
                    x.UserId == userId);

            if (address == null)
            {
                return BadRequest("Invalid address");
            }

            // Get food items
            var foodItemIds = cartItems
                .Select(x => x.FoodItemId)
                .ToList();

            var foodItems = await _context.FoodItems
                .Where(x => foodItemIds.Contains(x.FoodItemId))
                .ToListAsync();

            // Check availability
            foreach (var cartItem in cartItems)
            {
                var foodItem = foodItems
                    .FirstOrDefault(x => x.FoodItemId == cartItem.FoodItemId);

                if (foodItem == null)
                {
                    return BadRequest("Food item not found");
                }

                if (!foodItem.IsAvailable)
                {
                    return BadRequest(
                        $"{foodItem.Name} is not available");
                }
            }

            // Calculate total
            decimal totalAmount = 0;

            foreach (var cartItem in cartItems)
            {
                var foodItem = foodItems
                    .First(x => x.FoodItemId == cartItem.FoodItemId);

                totalAmount += foodItem.Price * cartItem.Quantity;
            }

            // Get restaurant
            var firstFoodItem = foodItems.First();

            // Create order
            var order = new Order
            {
                UserId = userId,
                RestaurantId = firstFoodItem.RestaurantId,
                AddressId = addressId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Placed"
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            // Create order items
            foreach (var cartItem in cartItems)
            {
                var foodItem = foodItems
                    .First(x => x.FoodItemId == cartItem.FoodItemId);

                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    FoodItemId = cartItem.FoodItemId,
                    Quantity = cartItem.Quantity,
                    Price = foodItem.Price
                };

                _context.OrderItems.Add(orderItem);
            }

            // Save order items

            await _context.SaveChangesAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var orders = await _context.Orders
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var orderItems = await _context.OrderItems
                .Where(x => x.OrderId == orderId)
                .Join(
                    _context.FoodItems,
                    orderItem => orderItem.FoodItemId,
                    foodItem => foodItem.FoodItemId,
                    (orderItem, foodItem) => new
                    {
                        orderItem.OrderItemId,
                        orderItem.FoodItemId,
                        foodItem.Name,
                        orderItem.Price,
                        orderItem.Quantity,
                        SubTotal = orderItem.Price * orderItem.Quantity
                    })
                .ToListAsync();

            return Ok(new
            {
                order.OrderId,
                order.UserId,
                order.RestaurantId,
                order.AddressId,
                order.OrderDate,
                order.TotalAmount,
                order.Status,
                Items = orderItems
            });
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(
    int orderId,
    UpdateOrderStatusRequest request)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var allowedStatuses = new[]
            {
        "Placed",
        "Accepted",
        "Preparing",
        "Ready",
        "Delivered",
        "Cancelled"
    };

            if (!allowedStatuses.Contains(request.Status))
            {
                return BadRequest("Invalid order status");
            }

            order.Status = request.Status;

            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}