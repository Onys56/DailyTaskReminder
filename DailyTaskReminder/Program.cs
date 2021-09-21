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
        /// <summary>
        /// List of all tasks
        /// </summary>
        static List<Task> tasks;

        /// <summary>
        /// Http client for sending reminders using Http protocol
        /// </summary>
        static HttpClient client = new();

        /// <summary>
        /// Start the program.
        /// </summary>
        /// <param name="options">Options altering the program behavior</param>
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

        /// <summary>
        /// Finds the task that should have the reminder sent soonest
        /// and creates a timer for sending the reminder (if the task has not been finished in the meantime).
        /// </summary>
        static void EnqueueSoonestReminder()
        {
            Task soonest = tasks
                .FindAll(t => t.IsFinished == false)
                .Select(task => (task.GetRemindTime, task.Name, task))
                .Min().task;
            StartTimer(soonest.GetRemindTime, soonest, HandleReminder);
        }

        /// <summary>
        /// Finds the task with the soonest deadline and creates a timer that
        /// resets its finish status.
        /// </summary>
        static void EnqueueSoonestDeadline()
        {
            Task soonest = tasks
                .Select(task => (task.GetDeadlineTime, task.Name, task))
                .Min().task;
            StartTimer(soonest.GetDeadlineTime, soonest, HandleDeadline);
        }

        /// <summary>
        /// Start the timer that will fire an action at a specific time.
        /// </summary>
        /// <param name="time">The time when the action should be fired</param>
        /// <param name="task">Parameter for the action</param>
        /// <param name="handler">Action</param>
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

        /// <summary>
        /// Possibly sends reminders and starts another timer for the next soonest reminder.
        /// </summary>
        /// <param name="t">Task that should send reminders if it has not been finished yet</param>
        static void HandleReminder(Task t)
        {
            if (!t.IsFinished && !t.ReminderSent)
            {
                string message = $"Task {t.Name} should be finished in {t.GetDeadlineTime}";
                t.ReminderSent = true;
                Console.WriteLine($"{DateTimeOffset.Now} Sending reminder(s) for {t.Name}...");
                foreach (string reminderName in t.Reminders)
                {
                    Instances.GetReminderByName[reminderName].Send(client, message);
                }
            }

            EnqueueSoonestReminder();
        }

        /// <summary>
        /// Resets the task status after its deadline has been reached and finds the next
        /// task with the soonest deadline.
        /// </summary>
        /// <param name="t"></param>
        static void HandleDeadline(Task t)
        {
            t.IsFinished = false;
            t.ReminderSent = false;
            EnqueueSoonestDeadline();
        }
    }
}
