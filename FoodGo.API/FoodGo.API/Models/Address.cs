namespace FoodGo.API.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public int UserId { get; set; }

        public string AddressLine { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Pincode { get; set; } = string.Empty;
    }
}