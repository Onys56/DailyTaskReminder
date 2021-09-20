using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DailyTaskReminder.Reminders
{
    public static class Instances
    {
        public static Dictionary<string, IReminder> GetReminderByName;

        public static void LoadReminders(string path)
        {
            Dictionary<string, IReminder> reminders = new();

            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(path));
            foreach(JObject entry in json)
            {
                Dictionary<string, string> e = entry.ToObject<Dictionary<string, string>>();
                reminders.Add(e["Name"],
                e["Type"] switch
                {
                    "DiscordWebhook" => DiscordWebhook.Load(e),
                    "TelegramBot" => TelegramBot.Load(e),
                    _ => throw new Exception("Unknown type of reminder")
                });
            }
            GetReminderByName = reminders;
        }
    }

    public interface IReminder
    {
        public void Send(HttpClient client, string message);
    }

    public class DiscordWebhook : IReminder
    {
        public string URL;
        public void Send(HttpClient client, string message)
        {
            string json = $"{{\"content\":\"{HttpUtility.JavaScriptStringEncode(message, true)}\"}}";
            client.PostAsync(URL, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        internal static IReminder Load(Dictionary<string, string> json)
        {
            return new DiscordWebhook() { URL = json["URL"] };
        }
    }

    public class TelegramBot : IReminder
    {
        public string BotToken;
        public string ChatId;

        public void Send(HttpClient client, string message)
        {
            string url = $"https://api.telegram.org/bot{BotToken}/sendMessage?chat_id={ChatId}&text={HttpUtility.UrlEncode(message)}";
            client.GetAsync(url);
        }

        internal static IReminder Load(Dictionary<string, string> json)
        {
            return new TelegramBot() { BotToken = json["BotToken"], ChatId = json["ChatId"] };
        }
    }
}
