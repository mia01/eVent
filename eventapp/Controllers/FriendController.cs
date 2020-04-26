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
using System.Threading.Tasks;

namespace eventapp.Controllers
{
    [Authorize]
    [Route("api/friends")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class FriendController : Controller
    {
        private readonly UserManager<EventAppUser> _userManager;
        private readonly UserFriendRepository _userFriendRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public FriendController(UserManager<EventAppUser> userManager, UserFriendRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userFriendRepository = userRepository;
            _contextAccessor = httpContextAccessor;
        }

        [HttpGet("FindByUsername")]
        public async Task<ActionResult<UserFriendRequest>> FindByUsername([FromQuery]string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                return new UserFriendRequest { Username = user.UserName, UserId = user.Id };
            }

            return null;
        }

        [HttpGet("GetUserFriends")]
        public async Task<ActionResult<IEnumerable<UserFriendResponse>>> GetUserFriends()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId != null)
            {
                var userFriends = await _userFriendRepository.GetUserFriends(userId);
                var userAcceptedInvites = userFriends.Where(f => f.UserFriendId != userId).Select(async f => new UserFriendResponse
                {
                    Id = f.Id,
                    FriendId = f.UserFriendId,
                    FriendUsername = (await _userManager.FindByIdAsync(f.UserFriendId)).UserName,
                    Accepted = f.Accepted
                }).ToList();

                var userAcceptedRequests = userFriends.Where(f => f.UserId != userId).Select(async f => new UserFriendResponse
                {
                    Id = f.Id,
                    FriendId = f.UserId,
                    FriendUsername = (await _userManager.FindByIdAsync(f.UserId)).UserName,
                    Accepted = f.Accepted
                }).ToList();

                var userAcceptedInvitesList = await System.Threading.Tasks.Task.WhenAll(userAcceptedInvites);
                var userAcceptedRequestsList = await System.Threading.Tasks.Task.WhenAll(userAcceptedRequests);

                return userAcceptedRequestsList.Union(userAcceptedInvitesList).ToList();
            }

            return new List<UserFriendResponse>();
        }

        [HttpGet("GetUserFriendRequests")]
        public async Task<ActionResult<IEnumerable<UserFriendResponse>>> GetUserFriendRequests()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId != null)
            {
                var userFriends = await _userFriendRepository.GetUserFriendRequests(userId);
                var userFriendDetails = userFriends.Select(async f => new UserFriendResponse { 
                    Id = f.Id,
                    FriendId = f.UserFriendId,
                    FriendUsername = (await _userManager.FindByIdAsync(f.UserFriendId)).UserName,
                    Accepted = f.Accepted}).ToList();
                return await System.Threading.Tasks.Task.WhenAll(userFriendDetails);
            }

            return new List<UserFriendResponse>();
        }

        [HttpGet("GetUserFriendInvites")]
        public async Task<ActionResult<IEnumerable<UserFriendResponse>>> GetUserFriendInvites()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId != null)
            {
                var userFriends = await _userFriendRepository.GetUserFriendInvites(userId);
                var userFriendDetails = userFriends.Select(async f => new UserFriendResponse
                {
                    Id = f.Id,
                    FriendId = f.UserFriendId,
                    FriendUsername = (await _userManager.FindByIdAsync(f.UserFriendId)).UserName,
                    Accepted = f.Accepted
                });
                return await System.Threading.Tasks.Task.WhenAll(userFriendDetails);
            }

            return new List<UserFriendResponse>();
        }

        [HttpPost("CreateFriendRequest")]
        public async Task<IActionResult> CreateFriendRequest([FromBody] UserFriendRequest request)
        {
            var userFriend = new UserFriend
            {
                UserId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value,
                UserFriendId = request.UserId,
                Accepted = false
            };

            var userFriendRecordId = await _userFriendRepository.Add(userFriend);
            if (userFriendRecordId != 0)
            {
                userFriend.Id = userFriendRecordId;
                var response = new UserFriendResponse
                {
                    Id = userFriend.Id,
                    FriendId = userFriend.UserFriendId,
                    FriendUsername = (await _userManager.FindByIdAsync(userFriend.UserFriendId)).UserName,
                    Accepted = userFriend.Accepted
                };
                return Ok(response);
            }

            return StatusCode(500);
        }

        [HttpPost("AcceptFriendRequest")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] long id)
        {
            var userFriendrequest = await _userFriendRepository.GetById(id);
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == userFriendrequest.UserFriendId)
            {
                userFriendrequest.Accepted = true;
                var updatedRequest = await _userFriendRepository.Update(userFriendrequest);
                if (updatedRequest > 0)
                {
                    return Ok(userFriendrequest);
                }
                return StatusCode(500);
            }

            return Unauthorized();
        }
    }
}