using System.Collections.Generic;
using System.Linq;
using eventapp.Models;
using eventapp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace eventapp.Controllers
{
    [Authorize]
    [Route("api/priorities")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class PriorityController : ControllerBase
    {
        private readonly PriorityRepository _priorityRepository;

        public PriorityController(PriorityRepository priorityRepository)
        {
            _priorityRepository = priorityRepository;
        }

        // GET api/priorities
        [HttpGet]
        public ActionResult<IEnumerable<Priority>> Get()
        {
            IEnumerable<Priority> priorities = _priorityRepository.GetAll();
            return priorities.ToList();
        }
    }
}
