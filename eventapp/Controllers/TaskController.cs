using eventapp.Areas.Identity.Data;
using eventapp.Models;
using eventapp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<EventAppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;


        public TaskController(TaskRepository taskRepository, UserManager<EventAppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
            _contextAccessor = httpContextAccessor;
        }

        // GET api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<Task>> Get()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            var tasks = new List<Task>();
            if (userId != null)
            {
                tasks = _taskRepository.GetByUserId(userId);
            }
            
            return tasks.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"value{id}";
        }

        // POST api/tasks
        [HttpPost]
        public IActionResult Post([FromBody] Task task)
        {
            if (task == null)
            {
                return BadRequest();
            }

            if (task.PriorityId == null)
            {
                task.PriorityId = Priority.DefaultPriorityId;
            }

            task.CreatedBy = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            task.AssignedTo = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            long taskId = _taskRepository.Add(task);
            if (taskId != 0)
            {
                task.Id = taskId;
                return Ok(task);
            }

            return StatusCode(500);
        }

        // POST api/tasks/5
        [HttpPost("{id}")]
        public IActionResult Update(long id, [FromBody] Task task)
        {
            if (task == null)
            {
                return BadRequest();
            }
            task.CreatedBy = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            task.AssignedTo = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            int rowsAffected = _taskRepository.Update(task);
            if (rowsAffected > 0)
            {
                return Ok(task);
            }

            return StatusCode(500);
        }

        // DELETE api/tasks/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            int rowsAffected = _taskRepository.Delete(id);
            if (rowsAffected > 0)
            {
                return Ok();
            }

            return StatusCode(500);
        }
    }
}
