namespace FoodGo.API.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public int CartId { get; set; }

        public int FoodItemId { get; set; }

        public int Quantity { get; set; }
    }
}