using System;
using System.IO;
using System.Collections.Generic;


namespace DailyTaskReminder.Tasks
{
    public static class Serialization
    {
        public static void Serialize(ICollection<Task> tasks, string path)
        {
            using StreamWriter sw = new(path);
            foreach (Task task in tasks)
            {
                task.Serialize(sw);
                sw.WriteLine();
            }
        }

        public static List<Task> Deserialize(string path)
        {
            List<Task> tasks = new();
            using StreamReader sr = new(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                Task t = line switch
                {
                    "DailyTask" or "Daily" or "Day" or "D" => new DailyTask().Deserialize(sr),
                    "WeeklyTask" or "Weekly" or "Week" or "W" => new WeeklyTask().Deserialize(sr),
                    "MonthlyTask" or "Monthly" or "Month" or "M" => new MonthlyTask().Deserialize(sr),
                    "YearlyTask" or "Yearly" or "Year" or "Y" => new YearlyTask().Deserialize(sr),
                    _ => throw new Exception($"Unknown task type: {line}"),
                };
                tasks.Add(t);
            }
            return tasks;
        }
    }
}
