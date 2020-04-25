using eventapp.Models;
using eventapp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Task = eventapp.Models.Task;

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

        public TaskController(TaskRepository taskRepository, IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _contextAccessor = httpContextAccessor;
        }

        // GET api/tasks
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<IEnumerable<Task>>> GetAsync()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            var tasks = new List<Task>();
            if (userId != null)
            {
                tasks = await _taskRepository.GetByUserId(userId);
            }
            
            return tasks.ToList();
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
            task.CreatedBy = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (task.AssignedTo == null || task.AssignedTo == string.Empty)
            {
                task.AssignedTo = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
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
