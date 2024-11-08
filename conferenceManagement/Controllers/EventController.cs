using conferenceManagement.Data;
using conferenceManagement.DTO;
using conferenceManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace conferenceManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ConferenceDbContext _conferenceDbContext;
        public EventController(ConferenceDbContext context)
        {
            _conferenceDbContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _conferenceDbContext.Events.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Event>> AddEvent(EventDto @event)
        {
            var newEvent = new Event
            {
                Title = @event.Title,
                Description = @event.Description,
                Date = @event.Date,
                Location = @event.Location
            };
            _conferenceDbContext.Events.Add(newEvent);
            _conferenceDbContext.SaveChanges();
            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, @event);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _conferenceDbContext.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, EventDto @event)
        {
            if (@event == null)
            {
                return BadRequest("Event data is invalid.");
            }
            var existingEvent = await _conferenceDbContext.Events
                                               .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEvent != null)
            {
                existingEvent.Title = @event.Title;
                existingEvent.Description = @event.Description;
                existingEvent.Date = @event.Date;
                existingEvent.Location = @event.Location;
                await _conferenceDbContext.SaveChangesAsync();
                return Ok(existingEvent);
            }
            else
            {
                var newEvent = new Event
                {
                    Title = @event.Title,
                    Description = @event.Description,
                    Date = @event.Date,
                    Location = @event.Location
                };
                _conferenceDbContext.Events.Add(newEvent);
                await _conferenceDbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, newEvent);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _conferenceDbContext.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _conferenceDbContext.Events.Remove(@event);
            await _conferenceDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
