using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using RaceDayBackend.Data;

using RaceDayBackend.DTOs;

using RaceDayBackend.Models;

namespace RaceDayBackend.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class EventsController : ControllerBase

    {

        private readonly RaceDayDBContext _context;

        public EventsController(RaceDayDBContext context)

        {

            _context = context;

        }

        // Helper Method: Checks Session for Authentication & Role Enforcement

        private bool IsAuthorized(string? requiredRole = null)

        {

            // 1. Are they logged in?

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null) return false;

            // 2. Do they have the right role?

            if (requiredRole != null)

            {

                var role = HttpContext.Session.GetString("Role");

                if (role != requiredRole) return false;

            }

            return true;

        }

        // GET: /api/events

        // Accessible by Any Logged In User

        [HttpGet]

        public async Task<IActionResult> GetEvents()

        {

            if (!IsAuthorized()) return Unauthorized("Please log in to view events.");

            var events = await _context.Events

                .Select(e => new

                {

                    e.EventID,

                    e.Title,

                    e.ScheduledDate,

                    e.Description,

                    Organiser = e.Organiser!.FullName

                })

                .ToListAsync();

            return Ok(events);

        }

        // POST: /api/events

        // Accessible ONLY by Organisers

        [HttpPost]

        public async Task<IActionResult> CreateEvent([FromBody] EventDTO dto)

        {

            if (!IsAuthorized("Organiser"))

                return StatusCode(403, "Access denied. Only Organisers can create events.");

            // Get the ID of the currently logged-in Organiser from the Session

            var organiserId = HttpContext.Session.GetInt32("UserId");

            var newEvent = new Event

            {

                Title = dto.Title,

                ScheduledDate = dto.ScheduledDate,

                Description = dto.Description,

                OrganiserID = organiserId.Value

            };

            _context.Events.Add(newEvent);

            await _context.SaveChangesAsync();

            return Created($"/api/events/{newEvent.EventID}", new { message = "Event generated successfully.", eventId = newEvent.EventID });

        }

        // PUT: /api/events/{id}

        // Accessible ONLY by Organisers

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDTO dto)

        {

            if (!IsAuthorized("Organiser"))

                return StatusCode(403, "Access denied. Only Organisers can update events.");

            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null) return NotFound("Event ID incorrect.");

            existingEvent.Title = dto.Title;

            existingEvent.ScheduledDate = dto.ScheduledDate;

            existingEvent.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Event updated." });

        }

    }

}
