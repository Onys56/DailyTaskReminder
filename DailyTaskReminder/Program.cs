using System;
using System.Threading;
using System.Net;

namespace DailyTaskReminder 
{
    class Program 
    {
        static void Main(string[] args) 
        {
            Server s = new Server(25566);
            Thread serverThread = new Thread(s.Start);
            serverThread.Start();
        }
    }

    class Server
    {
        private HttpListener listener;

        public Server(int port)
        {
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
                Console.WriteLine(context.Request);
                HttpListenerResponse response = context.Response;
                response.StatusCode = 200;
                response.Close();
            }
        }
    }
}
