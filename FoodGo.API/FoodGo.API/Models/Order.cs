namespace FoodGo.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int RestaurantId { get; set; }

        public int AddressId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}