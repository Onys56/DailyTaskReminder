using System;
using System.Collections.Generic;

namespace DailyTaskReminder.Tasks
{
    /// <summary>
    /// Exception to throw when the tasks are not valid.
    /// </summary>
    public class TasksNotValidException : Exception
    {
        public TasksNotValidException() : base() { }
        public TasksNotValidException(string message) : base(message) { }
    }

    /// <summary>
    /// Holds method for validating a list of tasks
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Checks if all names are unique and validates each task.
        /// </summary>
        /// <param name="tasks">List of tasks</param>
        /// <exception cref="TasksNotValidException">If any of the tasks is not valid</exception>

        public static void ValidateTasks(List<Task> tasks, bool testThatReminderNamesExist = false)
        {
            AllNamesAreUnique(tasks);

            foreach (Task task in tasks)
            {
                task.Validate();
            }

            if (testThatReminderNamesExist) RemindersExist(tasks);
        }

        /// <summary>
        /// Checks all names of task and if any of them occurs twice or more times throws an exceptio.
        /// </summary>
        /// <param name="tasks">List of tasks</param>
        /// <exception cref="TasksNotValidException">If any tasks share the same name</exception>
        private static void AllNamesAreUnique(List<Task> tasks)
        {
            foreach (Task task1 in tasks)
            {
                int count = 0;
                foreach (Task task2 in tasks)
                {
                    if (task1.Name == task2.Name)
                    {
                        count++;
                    }
                }
                if (count >= 2)
                {
                    throw new TasksNotValidException($"Name {task1.Name} is not unique.");
                }
            }
        }

        /// <summary>
        /// Tests that reminders with specified names in the tasks all exist.
        /// If any reminder does not exist exception is thrown.
        /// </summary>
        /// <param name="tasks">List of tasks</param>
        /// <exception cref="TasksNotValidException">If any reminder name does not exist</exception>
        private static void RemindersExist(List<Task> tasks)
        {
            foreach (Task task in tasks)
            {
                foreach (string name in task.Reminders)
                {
                    if (!Reminders.Instances.GetReminderByName.ContainsKey(name))
                    {
                        throw new TasksNotValidException($"Task {task.Name} specifies a reminder that is not written in reminders config file: {name}");
                    }
                }
            }
        }
    }
}
