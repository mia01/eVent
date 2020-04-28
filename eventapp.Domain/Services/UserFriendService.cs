using eventapp.Domain.Idenitity;
using eventapp.Domain.Models;
using eventapp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eventapp.Domain.Services
{
    public class UserFriendService
    {
        private readonly UserManager<EventAppUser> _userManager;
        private readonly UserFriendRepository _userFriendRepository;
        public UserFriendService(UserManager<EventAppUser> userManager, UserFriendRepository userFriendRepository)
        {
           _userManager = userManager;
           _userFriendRepository = userFriendRepository;
        }

        public async Task<List<UserFriendResponse>> GetUserFriends(string userId)
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
    }
}
