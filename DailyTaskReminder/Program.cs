using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using DailyTaskReminder.Tasks;

namespace DailyTaskReminder
{
    class Program 
    {
        static System.Timers.Timer remindTimer;
        static System.Timers.Timer deadlineTimer;
        static List<Task> tasks = Testing.MockTasks();

        static void Main(string[] args) 
        {
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

        static void UpdateTimer(System.Timers.Timer timer, DateTime time, Task task, Action<Task> handler)
        {
            if (task.GetRemindTime < DateTime.Now) handler(task);
            else
            {
                timer = new() { AutoReset = false, Interval =  (time - DateTime.Now).TotalMilliseconds };
                timer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) => handler(task);
                timer.Start();
            }
        }

        static void HandleReminder(Task t)
        {
            if (!t.IsFinished && !t.ReminderSent)
            {
                t.ReminderSent = true;
                Console.WriteLine($"{DateTime.Now} Sending reminder for {t.Name}...");
                // TODO: Send reminders
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

    class Server
    {
        private HttpListener listener;
        private List<Task> tasks;

        public Server(List<Task> tasks, int port = 25566)
        {
            this.tasks = tasks;
            this.listener = new HttpListener();
            listener.Prefixes.Add($"http://+:{port}/");
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                string method = context.Request.HttpMethod.ToUpper();
                switch (method)
                {
                    case "GET": 
                        HandleGet(context);
                        break;
                    case "POST":
                        HandlePost(context);
                        break;
                    default:
                        BadRequest(context, $"HttpMethod {method} is not supported.");
                        break;
                }
            }
        }

        private void HandleGet(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;
            byte[] res = JsonSerializer.SerializeToUtf8Bytes(tasks);
            response.ContentType = "application/json";
            response.ContentLength64 = res.Length;
            response.OutputStream.Write(res, 0, res.Length);
            response.StatusCode = 200;
            response.Close();
        }

        private void HandlePost(HttpListenerContext context)
        {
            Task t;
            if (context.Request.QueryString.Keys.Count == 1 &&
                context.Request.QueryString.Keys[0] == "name" &&
                (t = tasks.Find(t => t.Name == context.Request.QueryString["name"])) != null)
            {
                HttpListenerResponse response = context.Response;
                Console.WriteLine($"Setting task {t.Name} as finished...");
                t.IsFinished = true;
                response.StatusCode = 200;
                response.Close();
            }
            else
            {
                BadRequest(context, "Bad query");
            }
        }

        private void BadRequest(HttpListenerContext context, string message)
        {
            HttpListenerResponse response = context.Response;
            response.StatusCode = 400;
            response.Close();
        }
    }
}
