# Daily Task Reminder
Application that reminds you about finishing tasks that repeat with 
regular interval such as feeding the dog daily or watering the plants
every week.

Useful not only for forgetful people but also for the whole household, 
one member can finish the task and others will know it has already been
done.

## Getting started

1. First you need to configure your tasks. You can do that either using the [WinForms application](#configurator) or you can write your configuration [by hand](#task-configuration).

2. If you also want to get reminded you will need to configure the reminders. Again either using the [WinForms application](#configurator) or writing it by hand [by hand](#reminder-configuration).

3. Then you can finally start the application! Just run this command in the DailyTaskReminder folder (see all command line options [here](#command-line-options)): 
```console
dotnet run -- -t "PathToTaskFile.txt" -r "PathToRemindersFile.json" 
```
Make sure to run the console with elevated privileges, the program will need to start a HTTP listener. 

4. To look at the tasks and mark them as finished just open `WebClient.html` and you will see a table of the tasks. If you want to run the program on a server and access it using the WebClient just change the URL inside the `WebClient.html` from `http://localhost:25566` to the address where the server is running. For example: `http://myDomain.eu:25566`


## Task configuration

To write your own tasks configuration just follow the example. Each task has this layout in the config file:
```
TaskType (For example Daily or Monthly...)
Special line that is only some tasks have (For example weekdays of the weekly task or day of month for the monthly task)
TaskName
TaskDeadline (The time of day when the task should be finished)
RemindTime (The amount of time before deadline when the reminders are sent)
ReminderNames
```
TaskType currently supports 4 tasks:
* DailyTask / Daily / D
* WeeklyTask / Weekly / W
* MonthlyTask / Monthly / M
* YearlyTask / Yearly / Y

Special line for those types is as follows:
* Daily - skips the line
* Weekly - Weekdays separated by ';' (for example Monday;Friday)
* Monthly - Day;Month in numbers (for example 25;1)

TaskName has to be unique for all tasks.

RemindNames names of the reminders to send (separated by ';')

Concrete example:
```
Daily
Feed The Dog
18:00:00
01:00:00
DiscordPrivate;TelegramDM

M
20
Pay the rent
13:00:00
01:00:00
TelegramDM

WeeklyTask
Tuesday;Friday
Water the plants
18:00:00
01:00:00
DiscordPublic

Yearly
7;6
Grandma's Birthday
10:00:00 +01:00
23:00:00
TelegramDM
```

## Reminder configuration

Reminder configuration is just a JSON file with list of objects that each has these properties:
* `Type` - Type of the reminder (currently there are only 2 types `DiscordWebhook` and `TelegramBot`)
* `Name` - Name of the webhook - this is what should be written in task configuration on line ReminderNames
* Other fields based on type
    * `DiscordWebhook`
        * `URL` - URL of the webhook
    * `TelegramBot`
        * `BotToken` - Token of the telegram bot
        * `ChatId` - Id of the chat where the reminder should eb sent

Example: 

```json
[
  {
    "Type": "DiscordWebhook",
    "Name": "DiscordPrivate",
    "URL": "https://discord.com/api/webhooks/98798456489/okdsSaSuFZALr6zT7t3djaslkjiI7W4Ac1vGAyO1D459b6lZlUeVS1EcxwHbn5441I8rq"
  },
  {
    "Type": "DiscordWebhook",
    "Name": "DiscordPublic",
    "URL": "https://discord.com/api/webhooks/98793215989/okdsSaSuFZALr6zT7t3djaslkjiI7W4Ac1vGAyO1D459b6lZlUeVS1EcxwHbn5441I8rq" 
  },
  {
    "Type": "TelegramBot",
    "Name": "TelegramDM",
    "BotToken": "1951584223:AAG5QKaQWIsJuDkcyz4zdL1noNOOKPFR0Ps",
    "ChatId": "740123753"
  }
]
```

## Configurator

Configurator is a WinForms application for configuring the tasks and reminders.

To open it simply run this command in the Configurator folder:

```console
dotnet run
```

## Command line options

All currently supported command line options for the program.

|            Option           |  Default value |                          Description                          |
|:---------------------------:|:--------------:|:-------------------------------------------------------------:|
|     -t, --task, --tasks     |    Tasks.txt   |                Path to task configuration file                |
| -r, --reminder, --reminders | Reminders.json |              Path to reminder configuration file              |
|          -p, --port         |      25566     |                      Port of HTTP server                      |
|         -a --access         |        *       |     Value of the HTTP "Access-Control-Allow-Origin" header    |
|          -h --hush          |                | For the next hour the application will not send and reminders |
