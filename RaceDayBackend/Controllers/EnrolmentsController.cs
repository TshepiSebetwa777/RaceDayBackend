using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using RaceDayBackend.Data;

using RaceDayBackend.DTOs;

using RaceDayBackend.Models;

namespace RaceDayBackend.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class EnrolmentsController : ControllerBase

    {

        private readonly RaceDayDBContext _context;

        public EnrolmentsController(RaceDayDBContext context)

        {

            _context = context;

        }

        private bool IsAuthorized(string requiredRole)

        {

            var role = HttpContext.Session.GetString("Role");

            return role == requiredRole;

        }

        // POST: /api/enrolments

        [HttpPost]

        public async Task<IActionResult> Enrol([FromBody] EnrolmentDto dto)

        {

            if (!IsAuthorized("Participant"))

                return StatusCode(403, "Access restricted. Only Participants can enrol.");

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;

            // Check if already enrolled to prevent duplicates

            var existing = await _context.Enrolments

                .FirstOrDefaultAsync(e => e.UserID == userId && e.CategoryID == dto.CategoryId);

            if (existing != null) return Conflict("Already enrolled in this category.");

            var enrolment = new Enrolment

            {

                UserID = userId,

                CategoryID = dto.CategoryId,

                Status = "Confirmed"

            };

            _context.Enrolments.Add(enrolment);

            await _context.SaveChangesAsync();

            return Created("", new { message = "Enrolment confirmed." });

        }

        // GET: /api/enrolments/my-history

        [HttpGet("my-history")]

        public async Task<IActionResult> GetMyHistory()

        {

            if (!IsAuthorized("Participant")) return StatusCode(403, "Access restricted.");

            var userId = HttpContext.Session.GetInt32("UserId");

            var history = await _context.Enrolments

                .Include(e => e.Category)

                .ThenInclude(c => c!.Event)

                .Where(e => e.UserID == userId)

                .Select(e => new

                {

                    e.EnrolmentID,

                    Event = e.Category!.Event!.Title,

                    Category = e.Category.TypeName,

                    e.Status

                })

                .ToListAsync();

            return Ok(history);

        }

    }

}
