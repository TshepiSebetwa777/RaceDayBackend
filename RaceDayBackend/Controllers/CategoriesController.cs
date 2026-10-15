using Microsoft.AspNetCore.Mvc;

using RaceDayBackend.Data;

using RaceDayBackend.DTOs;

using RaceDayBackend.Models;

namespace RaceDayBackend.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class CategoriesController : ControllerBase

    {

        private readonly RaceDayDBContext _context;

        public CategoriesController(RaceDayDBContext context)

        {

            _context = context;

        }

        private bool IsAuthorized(string requiredRole)

        {

            var role = HttpContext.Session.GetString("Role");

            return role == requiredRole;

        }

        // POST: /api/categories

        [HttpPost]

        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)

        {

            if (!IsAuthorized("Organiser"))

                return StatusCode(403, "Insufficient permissions. Organiser role required.");

            var category = new Category

            {

                EventID = dto.EventId,

                TypeName = dto.TypeName,

                DistanceKM = dto.DistanceKm

            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return Created("", new { message = "Category generated successfully." });

        }

    }

}