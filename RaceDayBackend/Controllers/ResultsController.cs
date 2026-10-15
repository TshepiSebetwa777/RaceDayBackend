using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayBackend.Data;
using RaceDayBackend.DTOs;
using RaceDayBackend.Models;

namespace RaceDayBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDBContext _context;

        public ResultsController(RaceDayDBContext context)
        {
            _context = context;
        }

        private bool IsAuthorized(string requiredRole)
        {
            var role = HttpContext.Session.GetString("Role");
            return role == requiredRole;
        }

        // POST: /api/results
        [HttpPost]
        public async Task<IActionResult> CaptureResult([FromBody] ResultDTO dto)
        {
            if (!IsAuthorized("Organiser")) return StatusCode(403, "Only Organisers can log results.");

            if (string.IsNullOrEmpty(dto.TimeRecorded) || string.IsNullOrEmpty(dto.MedalType))
                return BadRequest("Missing required fields.");

            var result = new Result
            {
                EnrolmentID = dto.EnrolmentId,
                TimeRecorded = dto.TimeRecorded,
                MedalType = dto.MedalType
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return Created("", new { message = "Result successfully logged." });
        }

        // GET: /api/results/myachievements
        [HttpGet("myachievements")]
        public async Task<IActionResult> GetMyAchievements()
        {
            if (!IsAuthorized("Participant")) return StatusCode(403, "Access restricted.");

            var userId = HttpContext.Session.GetInt32("UserId");
            var results = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e!.Category)
                .Where(r => r.Enrolment!.UserID == userId)
                .Select(r => new
                {
                    Event = r.Enrolment!.Category!.Event!.Title,
                    Distance = r.Enrolment.Category.DistanceKM,
                    r.TimeRecorded,
                    r.MedalType
                })
                .ToListAsync();

            if (!results.Any()) return NotFound("No results published yet.");

            return Ok(results);
        }
    }
}
