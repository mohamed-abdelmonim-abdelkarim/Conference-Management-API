using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using conferenceManagement.Data;
using conferenceManagement.DTO;
using conferenceManagement.Model;
using Microsoft.AspNetCore.Authorization;

namespace YourNamespace.Controllers
{
    [Authorize(Roles = "Attendee")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly ConferenceDbContext _conferenceDbContext;

        public AttendeeController(ConferenceDbContext conferenceDbContext)
        {
            _conferenceDbContext = conferenceDbContext;
        }
        [HttpPost("CreateAttendee")]
        public async Task<IActionResult> CreateAttendee([FromBody] AtendeeDto @attendee)
        {
            if (@attendee == null)
            {
                return BadRequest("Attendee data is invalid.");
            }

            var newAttendee = new Attendee
            {
                FullName = @attendee.FullName,
                Email = @attendee.Email
            };

            foreach (var sessionId in @attendee.Sessions)
            {
                newAttendee.SessionAttendees.Add(new SessionAttendee
                {
                    AttendeeId = newAttendee.Id,
                    SessionId = sessionId
                });
            }
            _conferenceDbContext.Attendees.Add(newAttendee);
            await _conferenceDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttendee), new { id = newAttendee.Id }, newAttendee);
        }
        [HttpGet("GetAttendees")]
        public async Task<IActionResult> GetAttendees()
        {
            var attendees = await _conferenceDbContext.Attendees.ToListAsync();
            return Ok(attendees);
        }
        [HttpGet("GetAttendee/{id}")]
        public async Task<IActionResult> GetAttendee(int id)
        {
            var attendee = await _conferenceDbContext.Attendees.FirstOrDefaultAsync(a => a.Id == id);

            if (attendee == null)
            {
                return NotFound("Attendee not found.");
            }

            return Ok(attendee);
        }
        [HttpPut("UpdateAttendee/{id}")]
        public async Task<IActionResult> UpdateAttendee(int id, [FromBody] AtendeeDto @attendee)
        {
            if (@attendee == null)
            {
                return BadRequest("Attendee data is invalid.");
            }

            var existingAttendee = await _conferenceDbContext.Attendees
                                                   .FirstOrDefaultAsync(a => a.Id == id);

            if (existingAttendee != null)
            {
                existingAttendee.FullName = @attendee.FullName;
                existingAttendee.Email = @attendee.Email;
                existingAttendee.SessionAttendees.Clear();
                foreach (var sessionId in @attendee.Sessions)
                {
                    existingAttendee.SessionAttendees.Add(new SessionAttendee
                    {
                        AttendeeId = existingAttendee.Id,
                        SessionId = sessionId
                    });
                }

                await _conferenceDbContext.SaveChangesAsync();

                return Ok(existingAttendee);
            }
            else
            {
                return NotFound("Attendee not found.");
            }
        }
        [HttpDelete("DeleteAttendee/{id}")]
        public async Task<IActionResult> DeleteAttendee(int id)
        {
            var attendee = await _conferenceDbContext.Attendees.FirstOrDefaultAsync(a => a.Id == id);

            if (attendee == null)
            {
                return NotFound("Attendee not found.");
            }

            _conferenceDbContext.Attendees.Remove(attendee);
            await _conferenceDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
