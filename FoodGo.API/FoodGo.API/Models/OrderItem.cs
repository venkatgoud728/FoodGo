namespace FoodGo.API.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int FoodItemId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}