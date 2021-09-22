using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;

using DailyTaskReminder.Tasks;

namespace DailyTaskReminder
{
    /// <summary>
    /// Wrapper class that holds the HttpListener which responds to http request from the user.
    /// Such as getting information about tasks and setting them as finished.
    /// </summary>
    class Server
    {
        /// <summary>
        /// The http listener.
        /// </summary>
        private HttpListener listener;

        /// <summary>
        /// List of all tasks.
        /// </summary>
        private List<Task> tasks;

        /// <summary>
        /// Access-Control-Allow-Origin header.
        /// </summary>
        private string access;

        public Server(List<Task> tasks, string port = "25566", string access = "*")
        {
            this.tasks = tasks;
            this.listener = new HttpListener();
            this.access = access;
            listener.Prefixes.Add($"http://+:{port}/");
        }

        /// <summary>
        /// Starts the server, which waits for requests and handles them.
        /// </summary>
        public void Start()
        {
            listener.Start();
           if (Program.Options.Verbose) Console.WriteLine("Listening...");
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

        /// <summary>
        /// Handles the GET request.
        /// Returns information about all tasks serialized into json.
        /// </summary>
        /// <param name="context">Context of the request</param>
        private void HandleGet(HttpListenerContext context)
        {
            int queryCount = context.Request.QueryString.Count;
            if (queryCount == 0)
            {
                SerializeObjectAndSend(context, tasks);
            }
            else if (queryCount == 1)
            {
                Task t;
                if (context.Request.QueryString.Keys[0] == "name" &&
                (t = tasks.Find(t => t.Name == context.Request.QueryString["name"])) != null)
                {
                    SerializeObjectAndSend(context, t);
                }
            }
            else
            {
                BadRequest(context, "Too many queries");
            }

        }

        /// <summary>
        /// Handles post requests.
        /// The request should include a query with the name of the task that is to be set as finished.
        /// </summary>
        /// <param name="context">Context of the request</param>
        private void HandlePost(HttpListenerContext context)
        {
            Task t;
            if (context.Request.QueryString.Keys.Count == 1 &&
                context.Request.QueryString.Keys[0] == "name" &&
                (t = tasks.Find(t => t.Name == context.Request.QueryString["name"])) != null)
            {
                HttpListenerResponse response = context.Response;
                if (Program.Options.Verbose) Console.WriteLine($"Setting task {t.Name} as finished...");
                t.IsFinished = true;
                response.Headers.Add("Access-Control-Allow-Origin", access);
                response.StatusCode = 200;
                response.Close();
            }
            else
            {
                BadRequest(context, "Bad query");
            }
        }

        /// <summary>
        /// Handles bad requests.
        /// </summary>
        /// <param name="context">Context of the request</param>
        /// <param name="message">Message of the error</param>
        private void BadRequest(HttpListenerContext context, string message)
        {
            HttpListenerResponse response = context.Response;
            response.StatusCode = 400;
            response.Close();
            if (Program.Options.Verbose) Console.WriteLine($"Got bad request: {message}");
        }

        /// <summary>
        /// Serializes object into json and sends it as response.
        /// </summary>
        /// <param name="context">The context from which to send the response</param>
        /// <param name="payload">The object to send</param>
        private void SerializeObjectAndSend(HttpListenerContext context, object payload)
        {
            HttpListenerResponse response = context.Response;
            response.Headers.Add("Access-Control-Allow-Origin", access);
            byte[] res = JsonSerializer.SerializeToUtf8Bytes(payload);
            response.ContentType = "application/json";
            response.ContentLength64 = res.Length;
            response.OutputStream.Write(res, 0, res.Length);
            response.StatusCode = 200;
            response.Close();
        }
    }
}
