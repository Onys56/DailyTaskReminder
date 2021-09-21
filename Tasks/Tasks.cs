using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DailyTaskReminder.Reminders;

namespace DailyTaskReminder.Tasks
{
    /// <summary>
    /// Extension methods for <c cref="DateTimeOffset">Dates</c>.
    /// </summary>
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Calculates how many days until a certain weekday is reached.
        /// </summary>
        /// <param name="d">The time from which to count the days</param>
        /// <param name="day">The day wanted</param>
        /// <returns>Amount of days</returns>
        public static int HowManyDaysUntilThisDayOfWeek(this DateTimeOffset d, DayOfWeek day)
        {
            int diff = day - d.DayOfWeek;
            if (diff < 0) diff += 7;
            return diff;
        }

        /// <summary>
        /// Calculates how many days until a certain day in a month is reached.
        /// </summary>
        /// <param name="d">The time from which to count the days</param>
        /// <param name="day">The day number wanted</param>
        /// <returns>Amount of days</returns>
        public static int HowManyDaysUntilThisDayOfMonth(this DateTimeOffset d, int day)
        {
            if (day < 1 || day > 31) throw new ArgumentException("Wrong day number", nameof(day));
            if (d.Day == day) return 0;

            int diff = 0;

            // Skip and count days of months that have less days then we need
            int skipped = 0;
            int daysInThisMonth = DateTime.DaysInMonth(d.Year, d.Month);
            if (day < d.Day)
            {
                diff += daysInThisMonth;
                skipped++;                
            }
            while (day > daysInThisMonth)
            {
                daysInThisMonth = DateTime.DaysInMonth(d.Year + (d.Month - 1 + skipped) / 12, (d.Month - 1 + skipped) % 12 + 1);
                diff += daysInThisMonth;
                skipped++;
            }

            diff += day - d.Day;
            return diff;
        }
    }

    public abstract class Task
    {
        /// <summary>
        /// Name of the task, also serves as the unique identifier of tasks.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List of reminder names
        /// </summary>
        /// <seealso cref="DailyTaskReminder.Reminders.Instances"/>
        public List<string> Reminders { get; set; } = new List<string>();
        /// <summary>
        /// Indicates if the task is finished or not.
        /// </summary>
        public bool IsFinished { get; set; }
        /// <summary>
        /// Indicates if the reminder has already been sent for this deadline.
        /// </summary>
        public bool ReminderSent { get; set; }
        /// <summary>
        /// Amount of time that determines how long before the deadline a reminder is sent.
        /// </summary>
        public TimeSpan RemindSpan { get; set; }
        /// <summary>
        /// Gets the next remind time.
        /// </summary>
        public DateTimeOffset GetRemindTime => (IsFinished || ReminderSent ? GetDeadline(true) : GetDeadline()) - RemindSpan;
        /// <summary>
        /// Get the time of the current deadline.
        /// </summary>
        public DateTimeOffset GetDeadlineTime => GetDeadline();

        /// <summary>
        /// Gets the current or next deadline.
        /// </summary>
        /// <param name="next">If true returns next deadline, otherwise returns current deadline</param>
        /// <returns>The time of deadline</returns>
        protected abstract DateTimeOffset GetDeadline(bool next = false);

        /// <summary>
        /// Serializes the task into a writable stream.
        /// </summary>
        /// <param name="sw">stream into which the task will be serialized</param>
        internal abstract void Serialize(StreamWriter sw);

        /// <summary>
        /// Deserializes the task from stream into Task object and returns the object.
        /// </summary>
        /// <param name="sr">Stream from which is read</param>
        /// <returns>The loaded task</returns>
        internal abstract Task Deserialize(StreamReader sr);

    }

    /// <summary>
    /// The base class of simple tasks.
    /// Leaving space for further extensions with more complex tasks.
    /// </summary>
    public abstract class SimpleTask : Task
    {   
        /// <summary>
        /// The time of day when the deadline is.
        /// </summary>
        public DateTimeOffset DueTime { get; set; }

        /// <summary>
        /// Task serialization of properties common to all SimpleTasks.
        /// </summary>
        /// <param name="sw">Stream to write to</param>
        protected void BaseSerialize(StreamWriter sw)
        {
            sw.WriteLine(Name);
            sw.WriteLine(DueTime);
            sw.WriteLine(RemindSpan);
            if (Reminders.Count == 0)
            {
                sw.WriteLine("-");
            }
            else
            {
                sw.WriteLine(string.Join(';', Reminders));
            }
        }

        /// <summary>
        /// Task deserialization of properties common to all SimpleTasks.
        /// </summary>
        /// <param name="sr">Stream to read from</param>
        protected void BaseDeserialize(StreamReader sr)
        {
            Name = sr.ReadLine();
            DueTime = DateTimeOffset.Parse(sr.ReadLine());
            RemindSpan = TimeSpan.Parse(sr.ReadLine());
            
            string rem = sr.ReadLine();
            if (rem == "-")
            {
                Reminders = new List<string>();
            }
            else
            {
                Reminders = rem.Split(';').ToList();
            }
        }
    }

    /// <summary>
    /// A task that happens once a day at the same time.
    /// </summary>
    public class DailyTask : SimpleTask
    {

        /// <summary>
        /// Calculates deadline.
        /// </summary>
        /// <see cref="Task.GetDeadline(bool)"/>
        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset deadline = new DateTime(now.Year, now.Month, now.Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            int i = 0;
            if (deadline < now) i++;
            if (next) i++;
            if (i > 0) deadline = deadline.AddDays(i);

            return deadline;
        }

        /// <summary>
        /// Serializes the task.
        /// </summary>
        /// <see cref="Task.Serialize(StreamWriter)"/>
        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            BaseSerialize(sw);
        }

        /// <summary>
        /// Deserializes the task.
        /// </summary>
        /// <see cref="Task.Deserialize(StreamReader)"/>
        internal override Task Deserialize(StreamReader sr)
        {
            BaseDeserialize(sr);
            return this;
        }
    }

    /// <summary>
    /// A task that happens on certain weekdays eg. Mondys and Fridays always at the same time.
    /// </summary>
    public class WeeklyTask : SimpleTask
    {
        /// <summary>
        /// List of weekdays on which this task has a deadline.
        /// </summary>
        public List<DayOfWeek> Days { get; set; } = new();

        /// <summary>
        /// Calculates deadline.
        /// </summary>
        /// <see cref="Task.GetDeadline(bool)"/>
        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset deadline = new DateTime(now.Year, now.Month, now.Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            // Order days by which one comes the soonest
            List<DayOfWeek> ordered = Days.OrderBy(day => now.HowManyDaysUntilThisDayOfWeek(day)).ToList();

            int i = 0;
            if (deadline < now) i++;
            if (next) i++;
            if (i > 0)
            {
                DayOfWeek nextDay = ordered[i % ordered.Count];
                deadline = deadline.AddDays(now.HowManyDaysUntilThisDayOfWeek(nextDay) + 7 * (i % ordered.Count));
            }

            return deadline;
        }

        /// <summary>
        /// Serializes the task.
        /// </summary>
        /// <see cref="Task.Serialize(StreamWriter)"/>
        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            sw.WriteLine(string.Join(';', Days));
            BaseSerialize(sw);
        }

        /// <summary>
        /// Deserializes the task.
        /// </summary>
        /// <see cref="Task.Deserialize(StreamReader)"/>
        internal override Task Deserialize(StreamReader sr)
        {
            string[] days = sr.ReadLine().Split(';');
            Days = days.Select(d => Enum.Parse<DayOfWeek>(d)).ToList();
            BaseDeserialize(sr);
            return this;

        }
    }

    /// <summary>
    /// Task that has deadline every month on the n-th day, always at the same time. 
    /// </summary>
    public class MonthlyTask : SimpleTask
    {
        /// <summary>
        /// The day of the month on which the task has this deadline.
        /// </summary>
        public int Day { get; set; } = 1;

        /// <summary>
        /// Calculates deadline.
        /// </summary>
        /// <see cref="Task.GetDeadline(bool)"/>
        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset deadline = new DateTime(now.Year, now.Month, now.Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            deadline = deadline.AddDays(now.HowManyDaysUntilThisDayOfMonth(Day));

            int i = 0;
            if (deadline < now) i++;
            if (next) i++;
            while (i > 0)
            {
                deadline = deadline.AddDays(1);
                deadline = deadline.AddDays(deadline.HowManyDaysUntilThisDayOfMonth(Day));
                i--;
            }

            return deadline;
        }

        /// <summary>
        /// Serializes the task.
        /// </summary>
        /// <see cref="Task.Serialize(StreamWriter)"/>
        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            sw.WriteLine(Day);
            BaseSerialize(sw);
        }

        /// <summary>
        /// Deserializes the task.
        /// </summary>
        /// <see cref="Task.Deserialize(StreamReader)"/>
        internal override Task Deserialize(StreamReader sr)
        {
            Day = int.Parse(sr.ReadLine());
            BaseDeserialize(sr);
            return this;
        }
    }

    /// <summary>
    /// Task that has deadline every year on the specified day and month.
    /// </summary>
    public class YearlyTask : SimpleTask
    {
        /// <summary>
        /// The month when the task should have deadline.
        /// </summary>
        public int Month { get; set; } = 1;
        /// <summary>
        /// The day of month when the task should have deadline.
        /// </summary>
        public int Day { get; set; } = 1;

        /// <summary>
        /// Calculates deadline.
        /// </summary>
        /// <see cref="Task.GetDeadline(bool)"/>
        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset deadline = new DateTime(now.Year, Month, Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            int i = 0;
            if (deadline < now) i++;
            if (next) i++;
            if (i > 0)
            {
                deadline = deadline.AddYears(i);
            }

            return deadline;
        }

        /// <summary>
        /// Serializes the task.
        /// </summary>
        /// <see cref="Task.Serialize(StreamWriter)"/>
        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            sw.WriteLine($"{Day};{Month}");
            BaseSerialize(sw);
        }

        /// <summary>
        /// Deserializes the task.
        /// </summary>
        /// <see cref="Task.Deserialize(StreamReader)"/>
        internal override Task Deserialize(StreamReader sr)
        {
            string[] date = sr.ReadLine().Split(';');
            Day = int.Parse(date[0]);
            Month = int.Parse(date[1]);
            BaseDeserialize(sr);
            return this;
        }
    }
}
