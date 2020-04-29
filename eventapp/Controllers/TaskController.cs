using eventapp.Domain.Dto;
using eventapp.Domain.Idenitity;
using eventapp.Domain.Models;
using eventapp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Task = eventapp.Domain.Models.Task;

namespace eventapp.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskRepository _taskRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<EventAppUser> _userManager;

        public TaskController(TaskRepository taskRepository, IHttpContextAccessor httpContextAccessor, UserManager<EventAppUser> userManager)
        {
            _taskRepository = taskRepository;
            _contextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        // GET api/tasks
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<IEnumerable<TaskResponse>>> GetAsync()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            var taskResponses = new List<TaskResponse>();
            if (userId != null)
            {
                var tasks = await _taskRepository.GetByUserId(userId);
                foreach (var task in tasks)
                {
                    taskResponses.Add(new TaskResponse
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Done = task.Done,
                        PriorityId = task.PriorityId,
                        DueDate = task.DueDate,
                        CreatedBy = task.CreatedBy,
                        CreatedByUsername = (await _userManager.FindByIdAsync(task.CreatedBy)).UserName,
                        AssignedTo = task.AssignedTo,
                        AssignedToUsername = (await _userManager.FindByIdAsync(task.AssignedTo)).UserName,
                        Reminder = task.Reminder
                    });

                }
            }
            
            return taskResponses;
        }

        // POST api/tasks
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> PostAsync([FromBody] Task task)
        {
            if (task == null)
            {
                return BadRequest();
            }

            if (task.PriorityId == null)
            {
                task.PriorityId = Priority.DefaultPriorityId;
            }

            if (task.AssignedTo == null || task.AssignedTo == string.Empty)
            {
                task.AssignedTo = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            task.CreatedBy = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            task.CreatedAt = DateTime.Now;

            long taskId = await _taskRepository.Add(task);
            if (taskId != 0)
            {
                var response = new TaskResponse
                {
                    Id = taskId,
                    Title = task.Title,
                    Description = task.Description,
                    Done = task.Done,
                    PriorityId = task.PriorityId,
                    DueDate = task.DueDate,
                    CreatedBy = task.CreatedBy,
                    CreatedByUsername = (await _userManager.FindByIdAsync(task.CreatedBy)).UserName,
                    AssignedTo = task.AssignedTo,
                    AssignedToUsername = (await _userManager.FindByIdAsync(task.AssignedTo)).UserName,
                    Reminder = task.Reminder
                };
                return Ok(response);
            }

            return StatusCode(500);
        }

        // POST api/tasks/5
        [HttpPost("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> UpdateAsync(long id, [FromBody] Task task)
        {
            if (task == null)
            {
                return BadRequest();
            }

            if (task.AssignedTo == null || task.AssignedTo == string.Empty)
            {
                task.AssignedTo = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            var originalTask = await _taskRepository.GetById(task.Id.Value);
            task.CreatedBy = originalTask.CreatedBy;
            task.CreatedAt = originalTask.CreatedAt;
            if (originalTask.DueDate != task.DueDate)
            {
                task.ReminderSent = false;
            }
            task.UpdatedAt = DateTime.Now;
            int rowsAffected = await _taskRepository.Update(task);
            if (rowsAffected > 0)
            {
                var response = new TaskResponse
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Done = task.Done,
                    PriorityId = task.PriorityId,
                    DueDate = task.DueDate,
                    CreatedBy = task.CreatedBy,
                    CreatedByUsername = (await _userManager.FindByIdAsync(task.CreatedBy)).UserName,
                    AssignedTo = task.AssignedTo,
                    AssignedToUsername = (await _userManager.FindByIdAsync(task.AssignedTo)).UserName,
                    Reminder = task.Reminder
                };
                return Ok(response);
            }

            return StatusCode(500);
        }

        // DELETE api/tasks/5
        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> DeleteAsync(long id)
        {
            int rowsAffected = await _taskRepository.Delete(id);
            if (rowsAffected > 0)
            {
                return Ok();
            }

            return StatusCode(500);
        }
    }
}
