
using eventapp.Domain.Idenitity;
using eventapp.Domain.Repositories;
using eventapp.Domain.Twilio;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace eventapp.Domain.Jobs
{
    public class ReminderNotificationJob
    {
        private const string DefaultMessageTemplate =
            "Hi {0}! Just a reminder that you have an task which is due in one hour.";
        
        private const string MessageTemplate =
            "Hi {0}! {1} would like to remind you that you have an task which is due in one hour.";

        private readonly UserManager<EventAppUser> _userManager;
        private readonly EventappTwilioClient _twilioCLient;
        private readonly TaskRepository _taskRepository;

        public ReminderNotificationJob(UserManager<EventAppUser> userManager, EventappTwilioClient twilioClient, TaskRepository taskRepository)
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
            var tasks = await _taskRepository.GetTasksDueToday();
            foreach( var task in  tasks)
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
                    } else
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
