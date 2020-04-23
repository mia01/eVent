using eventapp.Areas.Identity.Data;
using eventapp.Repositories;
using eventapp.Twilio;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eventapp.Scheduler
{
    public class ReminderNotificationJob
    {
        private const string MessageTemplate =
            "Hi {0}! Just a reminder that you have an task which is due in one hour.";

        private readonly UserManager<EventAppUser> _userManager;
        private readonly TwilioClient _twilioCLient;
        private readonly TaskRepository _taskRepository;

        public ReminderNotificationJob(UserManager<EventAppUser> userManager, TwilioClient twilioClient, TaskRepository taskRepository)
        {
            _userManager = userManager;
            _twilioCLient = twilioClient;
            _taskRepository = taskRepository;
        }
        public async Task Execute()
        {
            var now = DateTime.Now;
            var oneHourMinRange = now.ToUniversalTime().AddHours(1).AddMinutes(-5);
            var oneHourMaxRange = now.ToUniversalTime().AddHours(1);
            var tasks = _taskRepository.GetTasksDueToday().ToList();
            foreach( var task in  tasks)
            {
                // check task due in the next hour
                if (task.Reminder && !task.ReminderSent && task.DueDate < oneHourMaxRange && task.DueDate > oneHourMinRange)
                {
                    //get user number
                    var assignedUser = await _userManager.FindByIdAsync(task.AssignedTo);

                    _twilioCLient.SendSmsMessage(
                    assignedUser.PhoneNumber,
                    string.Format(MessageTemplate, task.Title, task.DueDate.ToString("t")));
                    
                    task.ReminderSent = true;
                    _taskRepository.Update(task);
                }
            }
        }
    }
}
