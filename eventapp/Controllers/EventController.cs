using eventapp.Domain.Dto;
using eventapp.Domain.Idenitity;
using eventapp.Domain.Models;
using eventapp.Domain.Repositories;
using eventapp.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly UserManager<EventAppUser> _userManager;
        private readonly UserFriendService _userFriendService;

        public EventController(EventRepository eventRepository, IHttpContextAccessor httpContextAccessor, UserManager<EventAppUser> userManager, UserFriendService userFriendService)
        {
            _eventRepository = eventRepository;
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userFriendService = userFriendService;
        }

        // GET api/events
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<IEnumerable<EventResponse>>> GetAsync()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            var responses = new List<EventResponse>();
            if (userId != null)
            {
                var userEvents = await _eventRepository.GetByUserId(userId);

                foreach (var e in userEvents)
                {
                    responses.Add(new EventResponse
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Description = e.Description,
                        CreatedBy = e.CreatedBy,
                        CreatedByUsername = (await _userManager.FindByIdAsync(e.CreatedBy)).UserName,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Reminder = e.Reminder,
                        PublicEvent = e.PublicEvent
                    });
                }

                var userFriendIds = (await _userFriendService.GetUserFriends(userId)).Select(f => f.FriendId).ToList();
                var userFriendEvents = await _eventRepository.GetByUserIds(userFriendIds);

                foreach (var e in userFriendEvents)
                {
                    responses.Add(new EventResponse
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Description = e.Description,
                        CreatedBy = e.CreatedBy,
                        CreatedByUsername = (await _userManager.FindByIdAsync(e.CreatedBy)).UserName,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Reminder = e.Reminder,
                        PublicEvent = e.PublicEvent
                    });
                }
            }
            
            return responses;
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
            eventRecord.CreatedAt = DateTime.Now;
            long eventId = await _eventRepository.Add(eventRecord);
            if (eventId != 0)
            {
                var response = new EventResponse
                {
                    Id = eventId,
                    Title = eventRecord.Title,
                    Description = eventRecord.Description,
                    CreatedBy = eventRecord.CreatedBy,
                    CreatedByUsername = (await _userManager.FindByIdAsync(eventRecord.CreatedBy)).UserName,
                    StartDate = eventRecord.StartDate,
                    EndDate = eventRecord.EndDate,
                    Reminder = eventRecord.Reminder,
                    PublicEvent = eventRecord.PublicEvent
                };
                return Ok(response);
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
            var originalEvent = await _eventRepository.GetById(eventRecord.Id.Value);
            eventRecord.CreatedBy = originalEvent.CreatedBy;
            eventRecord.CreatedAt = originalEvent.CreatedAt;
            eventRecord.UpdatedAt = DateTime.Now;
            if (originalEvent.StartDate != eventRecord.StartDate)
            {
                eventRecord.ReminderSent = false;
            }
            int rowsAffected = await _eventRepository.Update(eventRecord);
            if (rowsAffected > 0)
            {
                var response = new EventResponse
                {
                    Id = eventRecord.Id,
                    Title = eventRecord.Title,
                    Description = eventRecord.Description,
                    CreatedBy = eventRecord.CreatedBy,
                    CreatedByUsername = (await _userManager.FindByIdAsync(eventRecord.CreatedBy)).UserName,
                    StartDate = eventRecord.StartDate,
                    EndDate = eventRecord.EndDate,
                    Reminder = eventRecord.Reminder,
                    PublicEvent = eventRecord.PublicEvent
                };
                return Ok(response);
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
