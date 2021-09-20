using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;

using DailyTaskReminder.Tasks;

namespace DailyTaskReminder
{
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
            response.Headers.Add("Access-Control-Allow-Origin", "*");
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
                response.Headers.Add("Access-Control-Allow-Origin", "*");
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
