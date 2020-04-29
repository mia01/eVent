
using eventapp.Domain.Idenitity;
using eventapp.Domain.Models;
using eventapp.Domain.Repositories;
using eventapp.Domain.Services;
using eventapp.Domain.Twilio;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace eventapp.Domain.Jobs
{
    public class ReminderNotificationJob
    {
        private const string DefaultMessageTemplate =
            "Hi {0}! Just a reminder that you have an task which is due in one hour.";

        private const string MessageTemplate =
            "Hi {0}! {1} would like to remind you that you have an task which is due in one hour.";

        private const string EventTemplate =
            "Hi {0}! {1}'s event ({2}) is starting in one hour!";

        private readonly UserManager<EventAppUser> _userManager;
        private readonly EventappTwilioClient _twilioCLient;
        private readonly TaskRepository _taskRepository;
        private readonly EventRepository _eventRepository;
        private readonly UserFriendService _friendService;

        public ReminderNotificationJob(UserManager<EventAppUser> userManager, 
            EventappTwilioClient twilioClient, 
            TaskRepository taskRepository, 
            EventRepository eventRepository,
            UserFriendService friendService)
        {
            _userManager = userManager;
            _twilioCLient = twilioClient;
            _taskRepository = taskRepository;
            _eventRepository = eventRepository;
            _friendService = friendService;
        }
        public async Task Execute()
        {
            var now = DateTime.Now;
            var oneHourMinRange = now.ToUniversalTime().AddHours(1).AddMinutes(-5);
            var oneHourMaxRange = now.ToUniversalTime().AddHours(1);
            await SendTaskReminders(oneHourMinRange, oneHourMaxRange);
            await SendEventReminders(oneHourMinRange, oneHourMaxRange);
        }

        private async Task SendEventReminders(DateTime oneHourMinRange, DateTime oneHourMaxRange)
        {
            var events = (await _eventRepository.GetEventsStartingToday())
                .Where(e => e.Reminder && e.ReminderSent != true && e.StartDate < oneHourMaxRange && e.StartDate > oneHourMinRange);

            foreach (var eventRecord in events) {
                // get  of the event
                var eventOwner = await _userManager.FindByIdAsync(eventRecord.CreatedBy);
                
                // get his/her
                var friends = await _friendService.GetUserFriends(eventRecord.CreatedBy);
                
                // send reminder
                foreach (var friend in friends)
                {
                    var friendUser = await _userManager.FindByIdAsync(friend.FriendId);
                    var message = string.Format(EventTemplate, friendUser.UserName, eventOwner.UserName, eventRecord.Title);
                    _twilioCLient.SendSmsMessage(
                    friendUser.PhoneNumber,
                    message
                    );
                }

                eventRecord.ReminderSent = true;
                await _eventRepository.Update(eventRecord);

            }
        }

        private async Task SendTaskReminders(DateTime oneHourMinRange, DateTime oneHourMaxRange)
        {
            var tasks = await _taskRepository.GetTasksDueToday();
            foreach (var task in tasks)
            {
                // check task due in the next hour
                if (task.Reminder && !task.ReminderSent && task.DueDate < oneHourMaxRange && task.DueDate > oneHourMinRange)
                {
                    //get user number
                    var assignedUser = await _userManager.FindByIdAsync(task.AssignedTo);

                    var message = "";

                    if (task.AssignedTo == task.CreatedBy)
                    {
                        message = string.Format(DefaultMessageTemplate, assignedUser.UserName);
                    }
                    else
                    {
                        var assigner = await _userManager.FindByIdAsync(task.CreatedBy);
                        message = string.Format(MessageTemplate, assignedUser.UserName, assigner.UserName);
                    }

                    _twilioCLient.SendSmsMessage(
                    assignedUser.PhoneNumber,
                    message
                    );

                    task.ReminderSent = true;
                    await _taskRepository.Update(task);
                }
            }
        }
    }
}
