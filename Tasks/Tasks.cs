using System;
using System.Linq;
using System.Collections.Generic;
using DailyTaskReminder.Reminders;

namespace DailyTaskReminder.Tasks
{
    public static class Testing
    {
        public static List<Task> MockTasks()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(new DailyTask() { Name = "Daily16", IsFinished = false, RemindSpan = new TimeSpan(1, 0, 0), DueTime = DateTime.Today.AddHours(12) });
            tasks.Add(new DailyTask() { Name = "Daily18", IsFinished = false, RemindSpan = new TimeSpan(1, 0, 0), DueTime = DateTime.Today.AddHours(21).AddMinutes(32) });
            tasks.Add(new WeeklyTask() { Name = "WeeklyMo", IsFinished = false, RemindSpan = new TimeSpan(2, 0, 0), DueTime = DateTime.Today.AddHours(3), days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday} });
            tasks.Add(new MonthlyTask() { Name = "Monthly19", IsFinished = false, RemindSpan = new TimeSpan(1, 0, 0), DueTime = DateTime.Today.AddHours(19).AddMinutes(52), days = new int[] { 19} });
            tasks.Add(new MonthlyTask() { Name = "Monthly1920", IsFinished = false, RemindSpan = new TimeSpan(1, 0, 0), DueTime = DateTime.Today.AddHours(19).AddMinutes(52), days = new int[] { 19, 20 } });
            return tasks;
        }
    }

    public static class DateTimeOffsetExtensions
    {
        public static int HowManyDaysUntilThisDayOfWeek(this DateTimeOffset d, DayOfWeek day)
        {
            int diff = day - d.DayOfWeek;
            if (diff < 0) diff += 7;
            return diff;
        }

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
        public string Name { get; set; }
        public List<IReminder> Reminders { get; set; } = new List<IReminder>();
        public bool IsFinished { get; set; }
        public bool ReminderSent { get; set; }
        public TimeSpan RemindSpan { get; set; }
        public DateTimeOffset GetRemindTime => (IsFinished || ReminderSent ? GetDeadline(true) : GetDeadline()) - RemindSpan;

        public DateTimeOffset GetDeadlineTime => GetDeadline();

        protected abstract DateTimeOffset GetDeadline(bool next = false);

    }

    public abstract class SimpleTask : Task
    {   
        public DateTimeOffset DueTime { get; set; }
    }

    public class DailyTask : SimpleTask
    {
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
    }

    public class WeeklyTask : SimpleTask
    {
        public DayOfWeek[] days { get; internal set; }

        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset deadline = new DateTime(now.Year, now.Month, now.Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            // Order days by which one comes the soonest
            List<DayOfWeek> ordered = days.OrderBy(day => now.HowManyDaysUntilThisDayOfWeek(day)).ToList();

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
    }

    public class MonthlyTask : SimpleTask
    {
        public int[] days { get; internal set; }

        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset deadline = new DateTime(now.Year, now.Month, now.Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            List<int> ordered = days.OrderBy(day => now.HowManyDaysUntilThisDayOfMonth(day)).ToList();
            deadline = deadline.AddDays(now.HowManyDaysUntilThisDayOfMonth(ordered[0]));

            int i = 0;
            if (deadline < now) i++;
            if (next) i++;
            while (i > 0)
            {
                ordered = days.OrderBy(day => deadline.HowManyDaysUntilThisDayOfMonth(day)).ToList();
                int nextDay = ordered[1 % ordered.Count];
                deadline = deadline.AddDays(1);
                deadline = deadline.AddDays(deadline.HowManyDaysUntilThisDayOfMonth(nextDay));
                i--;
            }

            return deadline;
        }
    }

    public class YearlyTask : SimpleTask
    {
        public int month { get; internal set; }
        public int day { get; internal set; }

        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset deadline = new DateTime(now.Year, month, day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            int i = 0;
            if (deadline < now) i++;
            if (next) i++;
            if (i > 0)
            {
                deadline = deadline.AddYears(i);
            }

            return deadline;
        }
    }


    public class CustomTask : Task
    {
        //TODO
        protected override DateTimeOffset GetDeadline(bool next = false)
        {
            throw new NotImplementedException();
        }
    }
}
