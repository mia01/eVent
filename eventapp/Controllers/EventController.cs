using eventapp.Domain.Models;
using eventapp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace eventapp.Controllers
{
    [Authorize]
    [Route("api/events")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventRepository _eventRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public EventController(EventRepository eventRepository, IHttpContextAccessor httpContextAccessor)
        {
            _eventRepository = eventRepository;
            _contextAccessor = httpContextAccessor;
        }

        // GET api/events
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<IEnumerable<Event>>> GetAsync()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            var events = new List<Event>();
            if (userId != null)
            {
                events = await _eventRepository.GetByUserId(userId);
            }
            
            return events.ToList();
        }

        // POST api/events
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> PostAsync([FromBody] Event eventRecord)
        {
            if (eventRecord == null)
            {
                return BadRequest();
            }

            eventRecord.CreatedBy = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            long eventId = await _eventRepository.Add(eventRecord);
            if (eventId != 0)
            {
                eventRecord.Id = eventId;
                return Ok(eventRecord);
            }

            return StatusCode(500);
        }

        // POST api/tasks/5
        [HttpPost("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> UpdateAsync(long id, [FromBody] Event eventRecord)
        {
            if (eventRecord == null)
            {
                return BadRequest();
            }

            int rowsAffected = await _eventRepository.Update(eventRecord);
            if (rowsAffected > 0)
            {
                return Ok(eventRecord);
            }

            return StatusCode(500);
        }

        // DELETE api/event/5
        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> DeleteAsync(long id)
        {
            int rowsAffected = await _eventRepository.Delete(id);
            if (rowsAffected > 0)
            {
                return Ok();
            }

            return StatusCode(500);
        }
    }
}
