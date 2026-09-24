using FoodGo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodGo.API.Data
{
    public class FoodGoDbContext : DbContext
    {
        public FoodGoDbContext(DbContextOptions<FoodGoDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<FoodItem> FoodItems { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}