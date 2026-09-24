using FoodGo.API.Data;
using FoodGo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly FoodGoDbContext _context;

        public AddressesController(FoodGoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _context.Addresses.ToListAsync();

            return Ok(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(Address address)
        {
            var user = await _context.Users
                .FindAsync(address.UserId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (string.IsNullOrWhiteSpace(address.AddressLine) ||
                string.IsNullOrWhiteSpace(address.City) ||
                string.IsNullOrWhiteSpace(address.State) ||
                string.IsNullOrWhiteSpace(address.Pincode))
            {
                return BadRequest("All address fields are required");
            }

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            return Ok(address);
        }
    }


}