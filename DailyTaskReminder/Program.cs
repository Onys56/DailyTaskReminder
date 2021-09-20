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
        static System.Timers.Timer remindTimer;
        static System.Timers.Timer deadlineTimer;
        static List<Task> tasks = Testing.MockTasks();

        static HttpClient client = new();

        static void Main(string[] args) 
        {
            Instances.LoadReminders("ApiKeys.json");
            Server s = new Server(tasks);
            Thread serverThread = new Thread(s.Start);
            serverThread.Start();

            EnqueueSoonestReminder();
            EnqueueSoonestDeadline();
        }

        static void EnqueueSoonestReminder()
        {
            Task soonest = tasks
                .FindAll(t => t.IsFinished == false)
                .Select(task => (task.GetRemindTime, task))
                .Min().task;
            UpdateTimer(remindTimer, soonest.GetRemindTime, soonest, HandleReminder);
        }

        static void EnqueueSoonestDeadline()
        {
            Task soonest = tasks
                .Select(task => (task.GetDeadlineTime, task))
                .Min().task;
            UpdateTimer(deadlineTimer, soonest.GetDeadlineTime, soonest, HandleDeadline);
        }

        static void UpdateTimer(System.Timers.Timer timer, DateTimeOffset time, Task task, Action<Task> handler)
        {
            if (task.GetRemindTime < DateTimeOffset.Now) handler(task);
            else
            {
                timer = new() { AutoReset = false, Interval =  (time - DateTimeOffset.Now).TotalMilliseconds };
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
                foreach (IReminder reminder in t.Reminders)
                {
                    reminder.Send(client, message);
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
