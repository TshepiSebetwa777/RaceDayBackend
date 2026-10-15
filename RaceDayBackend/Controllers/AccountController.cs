using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using RaceDayBackend.Data;

using RaceDayBackend.DTOs;

using RaceDayBackend.Models;

namespace RaceDayBackend.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class AccountController : ControllerBase

    {

        private readonly RaceDayDBContext _context;

        public AccountController(RaceDayDBContext context)

        {

            _context = context;

        }

        // POST: /api/account/register

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)

        {

            // Check if email already exists

            if (await _context.Users.AnyAsync(u => u.EmailAddress == dto.EmailAddress))

            {

                return BadRequest("Email is already registered.");

            }

            // Verify the Role exists

            var role = await _context.Roles.FindAsync(dto.RoleId);

            if (role == null) return BadRequest("Invalid Role ID.");

            // Hash the password

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User

            {

                FullName = dto.FullName,

                EmailAddress = dto.EmailAddress,

                PasswordHash = hashedPassword,

                RoleID = dto.RoleId

            };

            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();

            return Created("", new { message = "Registration success." });

        }

        // POST: /api/account/login

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginDto dto)

        {

            var user = await _context.Users

                .Include(u => u.Role)

                .FirstOrDefaultAsync(u => u.EmailAddress == dto.EmailAddress);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))

            {

                return NotFound("User does not exist or invalid credentials.");

            }

            // Set Server-Side Session Variables

            HttpContext.Session.SetInt32("UserId", user.UserID);

            HttpContext.Session.SetString("Role", user.Role!.RoleName);

            return Ok(new

            {

                message = "Login successful",

                role = user.Role.RoleName

            });

        }

        // GET: /api/account/me

        [HttpGet("me")]

        public async Task<IActionResult> GetMe()

        {

            // Check if user is logged in via Session

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)

            {

                return Unauthorized("No valid session provided.");

            }

            var user = await _context.Users

                .Select(u => new { u.UserID, u.FullName, u.EmailAddress, u.RoleID })

                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) return NotFound();

            return Ok(user);

        }

    }

}

