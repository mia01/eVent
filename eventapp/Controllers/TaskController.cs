using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eventapp.Models;
using eventapp.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Task = eventapp.Models.Task;

namespace eventapp.Controllers
{
    [Route("api/tasks")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskRepository _taskRepository;

        public TaskController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<Task>> Get()
        {
            IEnumerable<Task> tasks = _taskRepository.GetAll();
            
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

            int rowsAffected = _taskRepository.Add(task);
            if (rowsAffected > 0)
            {
                return Ok(task);
            }

            return StatusCode(500);
        }

        // POST api/tasks/5
        [HttpPost("{id}")]
        public IActionResult Update(int id, [FromBody] Task task)
        {
            if (task == null)
            {
                return BadRequest();
            }

            int rowsAffected = _taskRepository.Update(task);
            if (rowsAffected > 0)
            {
                return Ok(task);
            }

            return StatusCode(500);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
