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
    /// <summary>
    /// Provides method for loading reminders and retriving their instances by their names.
    /// </summary>
    public static class Instances
    {
        /// <summary>
        /// Dictionary of Reminders indexed by their name.
        /// </summary>
        public static Dictionary<string, IReminder> GetReminderByName;

        /// <summary>
        /// Loads the reminder keys and names from a file and stores them in the dictionary
        /// where they can be retrivied by <c cref="GetReminderByName">a public property</c>.
        /// </summary>
        /// <param name="path">Path to file where the remiders are stored</param>
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

    /// <summary>
    /// Interface for reminders.
    /// </summary>
    public interface IReminder
    {
        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="client">HttpClient that sends the reminder</param>
        /// <param name="message">Message to be sent</param>
        public void Send(HttpClient client, string message);
    }

    /// <summary>
    /// A discord webhook that is able to send message to a channel.
    /// </summary>
    /// <seealso href="https://discord.com/"/>
    /// <seealso href="https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks"/>
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

    /// <summary>
    /// Telegram bot that can send messages to chats.
    /// </summary>
    /// <seealso href="https://telegram.org/"/>
    /// <seealso href="https://core.telegram.org/bots"/>
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
