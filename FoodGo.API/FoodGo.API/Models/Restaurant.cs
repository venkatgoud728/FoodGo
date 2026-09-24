namespace FoodGo.API.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public decimal? Rating { get; set; }

        public string? DeliveryTime { get; set; }
    }
}