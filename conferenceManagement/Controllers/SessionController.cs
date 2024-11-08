using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using conferenceManagement.Data;
using conferenceManagement.DTO;
using conferenceManagement.Model;
using Microsoft.AspNetCore.Authorization;

namespace YourNamespace.Controllers
{
    [Authorize(Roles = "Speaker")]
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ConferenceDbContext _conferenceDbContext;

        public SessionController(ConferenceDbContext conferenceDbContext)
        {
            _conferenceDbContext = conferenceDbContext;
        }
        [HttpPost("CreateSession")]
        public async Task<IActionResult> CreateSession([FromBody] SessionDto @session)
        {
            if (@session == null)
            {
                return BadRequest("Session data is invalid.");
            }

            var newSession = new Session
            {
                Name = @session.Name,
                StartTime = @session.StartTime,
                EndTime = @session.EndTime,
                EventId = @session.EventId
            };

            _conferenceDbContext.Sessions.Add(newSession);
            await _conferenceDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSession), new { id = newSession.Id }, newSession);
        }
        [HttpGet("GetSessions")]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _conferenceDbContext.Sessions.ToListAsync();
            return Ok(sessions);
        }
        [HttpGet("GetSession/{id}")]
        public async Task<IActionResult> GetSession(int id)
        {
            var session = await _conferenceDbContext.Sessions.FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound("Session not found.");
            }

            return Ok(session);
        }
        [HttpPut("UpdateSession/{id}")]
        public async Task<IActionResult> UpdateSession(int id, [FromBody] SessionDto @session)
        {
            if (@session == null)
            {
                return BadRequest("Session data is invalid.");
            }

            var existingSession = await _conferenceDbContext.Sessions
                                                 .FirstOrDefaultAsync(s => s.Id == id);

            if (existingSession != null)
            {
                existingSession.Name = @session.Name;
                existingSession.StartTime = @session.StartTime;
                existingSession.EndTime = @session.EndTime;
                existingSession.EventId = @session.EventId;

                await _conferenceDbContext.SaveChangesAsync();

                return Ok(existingSession);
            }
            else
            {
                return NotFound("Session not found.");
            }
        }
        [HttpDelete("DeleteSession/{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var session = await _conferenceDbContext.Sessions.FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound("Session not found.");
            }

            _conferenceDbContext.Sessions.Remove(session);
            await _conferenceDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
