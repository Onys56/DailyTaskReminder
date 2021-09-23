using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
            while (day > daysInThisMonth)
            {
                diff += daysInThisMonth;
                skipped++;
                daysInThisMonth = DateTime.DaysInMonth(d.Year + (d.Month - 1 + skipped) / 12, ((d.Month - 1 + skipped) % 12) + 1);
            }
            if (skipped == 0 && day < d.Day)
            {
                diff += daysInThisMonth;
            }
            diff += day - d.Day;
            return diff;
        }

        /// <summary>
        /// Findes the next leap year from the specified date.
        /// </summary>
        /// <param name="d">The yer from which to find the next leap year</param>
        /// <returns>Next leap year</returns>
        public static int FindNextLeapYear(this DateTimeOffset d)
        {
            int nextLeapYear = d.Year + 1;
            if (nextLeapYear % 4 != 0)
            {
                nextLeapYear += 4 - (nextLeapYear % 4);
            }
            if (nextLeapYear % 100 == 0 && nextLeapYear % 400 != 0)
            {
                nextLeapYear += 4;
            }

            return nextLeapYear;
        }
    }

    /// <summary>
    /// Abstract class for things that all tasks have in common.
    /// </summary>
    public abstract class Task
    {

        /// <summary>
        /// Name of the task, also serves as the unique identifier of tasks.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Message to send when reminding about the task.
        /// </summary>
        public string Message { get; set; } = "-";

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
        /// Always has value bigger or equal to 1 minute
        /// </summary>
        public TimeSpan RemindSpan
        {
            get => remindSpan;
            set
            {
                TimeSpan minute = new TimeSpan(0, 1, 0);
                TimeSpan maxRemindTime = GetMaxRemindSpan();

                if (value <= minute)
                {
                    remindSpan = minute;
                }
                else if (value >= maxRemindTime)
                {
                    remindSpan = maxRemindTime;
                }
                else
                {
                    remindSpan = value;
                }
            }
        }

        /// <summary>
        /// Backing field for RemindSpan
        /// </summary>
        protected TimeSpan remindSpan = new TimeSpan(0,1,0);

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
        /// Gets the maximal remind time for this task.
        /// </summary>
        /// <returns>Maximal remind time</returns>
        public abstract TimeSpan GetMaxRemindSpan();

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

        /// <summary>
        /// Checks if the task is valid.
        /// If not throws exception.
        /// </summary>
        /// <exception cref="TasksNotValidException">If the task is not valid</exception>
        internal abstract void Validate();

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
            sw.WriteLine(DueTime.TimeOfDay);
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

            string m = sr.ReadLine();
            if (m != "-" && !string.IsNullOrWhiteSpace(m))
            {
                Message = m;
            }
        }
    }

    /// <summary>
    /// A task that happens once a day at the same time.
    /// </summary>
    public class DailyTask : SimpleTask
    {
        /// <summary>
        /// Maximal remind time for daily task is 23 hours and 59 minutes
        /// </summary>
        /// <returns>Maximal remind time for this task</returns>
        public override TimeSpan GetMaxRemindSpan()
        {
            return new TimeSpan(23, 59, 0);
        }


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

        /// <summary>
        /// Daily task should need no validation.
        /// </summary>
        internal override void Validate() {}
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
        /// Maximal remind time for weekly task is minimal deadline interval - 1 minute
        /// </summary>
        /// <returns>Maximal remind time for this task</returns>
        public override TimeSpan GetMaxRemindSpan()
        {
            int minDayDifference = 7;
            foreach (DayOfWeek day1 in Days)
            {
                foreach (DayOfWeek day2 in Days)
                {
                    int diff = Math.Abs((int)day1 - (int)day2);
                    if (diff == 0) continue;
                    if (diff < minDayDifference) minDayDifference = diff;
                }
            }

            return new TimeSpan(minDayDifference * 24 - 1, 59, 0);
        }

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

        /// <summary>
        /// Checks if any weekday is selected and if no weekday repeats.
        /// </summary>
        internal override void Validate() 
        {
            if (Days.Count == 0)
            {
                throw new TasksNotValidException($"Weekly task {Name} has no weekdays selected");
            }

            foreach (DayOfWeek day1 in Days)
            {
                int count = 0;
                foreach (DayOfWeek day2 in Days)
                {
                    if (day1 == day2) count++;
                }
                if (count >= 2)
                {
                    throw new TasksNotValidException($"Weekly task {Name} has weekday {day1} specified multiple times");
                }
            }
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
        /// Maximal remind time for monthly task is 27 days 23 hours and 59 minutes
        /// </summary>
        /// <returns>Maximal remind time for this task</returns>
        public override TimeSpan GetMaxRemindSpan()
        {
            return new TimeSpan(27, 23, 59, 0);
        }

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

        /// <summary>
        /// Checks if Day number is valid
        /// </summary>
        internal override void Validate() 
        { 
            if (Day < 1 || Day > 31)
            {
                throw new TasksNotValidException($"Monthly task {Name} has invalid day: {Day}");
            }
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
        /// Maximal remind time for yearly task is 364 days 23 hours and 59 minutes
        /// </summary>
        /// <returns>Maximal remind time for this task</returns>
        public override TimeSpan GetMaxRemindSpan()
        {
            return new TimeSpan(364, 23, 59, 0);
        }

        /// <summary>
        /// Calculates deadline.
        /// </summary>
        /// <see cref="Task.GetDeadline(bool)"/>
        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset deadline = new DateTime(now.Year, Month, Day, DueTime.Hour, DueTime.Minute, DueTime.Second);
            bool isFab29 = Month == 2 && Day == 29;
            if (isFab29)
            {
                deadline = new DateTime(now.FindNextLeapYear(), Month, Day, DueTime.Hour, DueTime.Minute, DueTime.Second);
            }

            int i = 0;
            if (deadline < now) i++;
            if (next) i++;
            if (i > 0)
            {
                if (isFab29)
                {
                    while (i > 0)
                    {
                        deadline = new DateTime(deadline.FindNextLeapYear(), Month, Day, DueTime.Hour, DueTime.Minute, DueTime.Second);
                        i--;
                    }
                }
                else
                {
                    deadline = deadline.AddYears(i);
                }
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

        /// <summary>
        /// Checks if the month number is valid and if day occurs in the month.
        /// </summary>
        internal override void Validate()
        {
            if (Month < 1 || Month > 12)
            {
                throw new TasksNotValidException($"Yearly task {Name} has invalid month: {Month}");
            }

            int anyLeapYear = 2004;
            if (Day < 1 || Day > DateTime.DaysInMonth(anyLeapYear, Month))
            {
                throw new TasksNotValidException($"Yearly task {Name} has invalid date: the day {Day} does not occur in the {Month}. month");
            }
        }
    }

    /// <summary>
    /// Task that has deadline periodically with a custom period.
    /// </summary>
    public class PeriodicTask : SimpleTask
    {
        /// <summary>
        /// The time of the period.
        /// Minimum value is one minute.
        /// </summary>
        public TimeSpan Period
        {
            get => period;
            set
            {
                TimeSpan minute = new TimeSpan(0, 1, 0);
                if (value < minute)
                {
                    period = minute;
                }
                else
                {
                    period = value;
                }
            }
        }

        /// <summary>
        /// Backing field for Period.
        /// </summary>
        private TimeSpan period = new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// From which point in time should the task start repeating.
        /// </summary>
        public DateTime FirstDeadline = DateTime.Now;

        /// <summary>
        /// Maximal remind time for periodic task is the period - a minute
        /// </summary>
        /// <returns>Maximal remind time for this task</returns>
        public override TimeSpan GetMaxRemindSpan()
        {
            return period - new TimeSpan(0, 1, 0);
        }

        /// <summary>
        /// Calculates deadline.
        /// </summary>
        /// <see cref="Task.GetDeadline(bool)"/>
        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTime now = DateTime.Now;

            if (now <= FirstDeadline)
            {
                return FirstDeadline;
            }
            else
            {
                double deadlinePosition = Math.Ceiling((now - FirstDeadline) / period);
                if (next) deadlinePosition++;
                return FirstDeadline + deadlinePosition * period;
            }
        }

        /// <summary>
        /// Serializes the task.
        /// </summary>
        /// <see cref="Task.Serialize(StreamWriter)"/>
        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            sw.WriteLine(FirstDeadline);
            sw.WriteLine(period);
            sw.WriteLine(Name);
            sw.WriteLine(remindSpan);
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
        /// Deserializes the task.
        /// </summary>
        /// <see cref="Task.Deserialize(StreamReader)"/>
        internal override Task Deserialize(StreamReader sr)
        {
            FirstDeadline = DateTime.Parse(sr.ReadLine());
            Period = TimeSpan.Parse(sr.ReadLine());
            Name = sr.ReadLine();
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
            string m = sr.ReadLine();
            if (m != "-" && !string.IsNullOrWhiteSpace(m))
            {
                Message = m;
            }
            return this;
        }


        /// <summary>
        /// No verification needed for the Periodic Task.
        /// </summary>
        internal override void Validate() { }
    }
}
