namespace FoodGo.API.Models
{
    public class FoodItem
    {
        public int FoodItemId { get; set; }

        public int RestaurantId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }
    }
}