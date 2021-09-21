using System;
using System.IO;
using System.Collections.Generic;


namespace DailyTaskReminder.Tasks
{
    /// <summary>
    /// Class that provides methods for serialization and deserialization of task collections.
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        /// Serialize the task collection into a file.
        /// </summary>
        /// <param name="tasks">Collection of tasks to serialize</param>
        /// <param name="path">Path to file to which the tasks are serialized</param>
        public static void Serialize(ICollection<Task> tasks, string path)
        {
            using StreamWriter sw = new(path);
            foreach (Task task in tasks)
            {
                task.Serialize(sw);
                sw.WriteLine();
            }
        }

        /// <summary>
        /// Deserializes tasks stored in a file into a list of task instances.
        /// </summary>
        /// <param name="path">The path to file</param>
        /// <returns>List of tasks</returns>
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
