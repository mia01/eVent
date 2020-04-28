using eventapp.Domain.Dto;
using eventapp.Domain.Idenitity;
using eventapp.Domain.Models;
using eventapp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            if (userId != null)
            {
                var tasks = await _taskRepository.GetByUserId(userId);
                var taskResponses = tasks.Select(async t => new TaskResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Done = t.Done,
                    PriorityId = t.PriorityId,
                    DueDate = t.DueDate,
                    CreatedBy = t.CreatedBy,
                    CreatedByUsername = (await _userManager.FindByIdAsync(t.CreatedBy)).UserName,
                    AssignedTo = t.AssignedTo,
                    AssignedToUsername = (await _userManager.FindByIdAsync(t.AssignedTo)).UserName,
                    Reminder = t.Reminder
                });
                return await System.Threading.Tasks.Task.WhenAll(taskResponses);
            }
            
            return new List<TaskResponse>();
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

            long taskId = await _taskRepository.Add(task);
            if (taskId != 0)
            {
                task.Id = taskId;
                return Ok(task);
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
            int rowsAffected = await _taskRepository.Update(task);
            if (rowsAffected > 0)
            {
                return Ok(task);
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
