using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;

using DailyTaskReminder.Tasks;
using DailyTaskReminder.Reminders;

namespace DailyTaskReminder
{
    class Program 
    {
        static List<Task> tasks;

        static HttpClient client = new();

        public static void Start(Options options) 
        {
            Instances.LoadReminders(options.RemindersPath);
            tasks = Serialization.Deserialize(options.TasksPath, options.Hush);

            Server s = new Server(tasks, options.Port, options.Access);
            Thread serverThread = new Thread(s.Start);
            serverThread.Start();

            EnqueueSoonestReminder();
            EnqueueSoonestDeadline();
        }

        static void EnqueueSoonestReminder()
        {
            Task soonest = tasks
                .FindAll(t => t.IsFinished == false)
                .Select(task => (task.GetRemindTime, task.Name, task))
                .Min().task;
            StartTimer(soonest.GetRemindTime, soonest, HandleReminder);
        }

        static void EnqueueSoonestDeadline()
        {
            Task soonest = tasks
                .Select(task => (task.GetDeadlineTime, task.Name, task))
                .Min().task;
            StartTimer(soonest.GetDeadlineTime, soonest, HandleDeadline);
        }

        static void StartTimer(DateTimeOffset time, Task task, Action<Task> handler)
        {
            if (task.GetRemindTime < DateTimeOffset.Now) handler(task);
            else
            {
                // System.Timers.Timer won't get garbage collected as long as it is Enabled
                System.Timers.Timer timer = new() { AutoReset = false, Interval =  (time - DateTimeOffset.Now).TotalMilliseconds };
                timer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) => handler(task);
                timer.Start();
            }
        }

        static void HandleReminder(Task t)
        {
            if (!t.IsFinished && !t.ReminderSent)
            {
                string message = $"Task {t.Name} should be finished in {t.GetDeadlineTime}";
                Console.WriteLine(message);
                t.ReminderSent = true;
                Console.WriteLine($"{DateTimeOffset.Now} Sending reminder for {t.Name}...");
                foreach (string reminderName in t.Reminders)
                {
                    Instances.GetReminderByName[reminderName].Send(client, message);
                }
            }

            EnqueueSoonestReminder();
        }

        static void HandleDeadline(Task t)
        {
            t.IsFinished = false;
            t.ReminderSent = false;
            EnqueueSoonestDeadline();
        }
    }
}
