using Hangfire;

namespace eventapp.Scheduler
{
    public class HangfireHelper
    {
        public static void InitializeJobs()
        {
            RecurringJob.AddOrUpdate<ReminderNotificationJob>(job => job.Execute(), Cron.Minutely);
        }
    }
}
