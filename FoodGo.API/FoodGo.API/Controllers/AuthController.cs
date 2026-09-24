using FoodGo.API.Data;
using FoodGo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace FoodGo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FoodGoDbContext _context;

        public AuthController(FoodGoDbContext context)
        {
            _context = context;
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Name, email and password are required.");
            }

            var email = request.Email.Trim().ToLower();

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = email,
                PasswordHash = HashPassword(request.Password),
                Phone = request.Phone
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration successful.",
                userId = user.UserId,
                name = user.Name,
                email = user.Email
            });
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var email = request.Email.Trim().ToLower();

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                message = "Login successful.",
                userId = user.UserId,
                name = user.Name,
                email = user.Email,
                phone = user.Phone
            });
        }

        // =========================
        // PASSWORD HASHING
        // =========================
        private static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256,
                32
            );

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                var parts = storedHash.Split('.');

                if (parts.Length != 2)
                {
                    return false;
                }

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] expectedHash = Convert.FromBase64String(parts[1]);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    100000,
                    HashAlgorithmName.SHA256,
                    32
                );

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash
                );
            }
            catch
            {
                return false;
            }
        }

        // =========================
        // REQUEST MODELS
        // =========================
        public class RegisterRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string? Phone { get; set; }
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}