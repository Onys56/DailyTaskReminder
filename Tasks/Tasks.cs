using System;
using System.Linq;
using System.Collections.Generic;

namespace DailyTaskReminder.Tasks
{
    public static class Testing
    {
        public static List<Task> MockTasks()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(new DailyTask() { Name = "Daily16", IsFinished = false, RemindSpan = new TimeSpan(1, 0, 0), DueTime = DateTime.Today.AddHours(16) });
            return tasks;
        }
    }

    public static class DateTimeExtensions
    {
        public static int HowManyDaysUntilThisDayOfWeek(this DateTime d, DayOfWeek day)
        {
            int diff = day - d.DayOfWeek;
            if (diff < 0) diff += 7;
            return diff;
        }

        public static int HowManyDaysUntilThisDayOfMonth(this DateTime d, int day)
        {
            if (day < 1 || day > 31) throw new ArgumentException("Wrong day number", nameof(day));

            int diff = 0;

            // Skip and count days of months that have less days then we need
            int skipped = 0;
            int daysInThisMonth = DateTime.DaysInMonth(d.Year, d.Month);
            while (daysInThisMonth > day)
            {
                diff += daysInThisMonth;
                skipped++;
                daysInThisMonth = DateTime.DaysInMonth(d.Year + (d.Month + skipped) / 12, d.Month + skipped % 12);
            }

            diff += day - d.Day;
            return diff;
        }
    }

    public abstract class Task
    {
        public string Name { get; set; }
        protected bool isFinished;
        public bool IsFinished { 
            get => isFinished;
            set
            {
                isRemindCacheValid = false;
                isFinished = value;
            }
        }
        public TimeSpan RemindSpan { get; set; }
        public DateTime GetRemindTime
        {
            get
            {
                if (isRemindCacheValid) return remindTime;
                else
                {
                    remindTime = GetNextRemindTime();
                    isRemindCacheValid = true;
                    return remindTime;
                }
            }
        }
        private bool isRemindCacheValid = false;
        private DateTime remindTime;

        protected abstract DateTime GetNextRemindTime();

    }

    public abstract class SimpleTask : Task
    {   
        public DateTime DueTime { get; set; }
    }

    public class DailyTask : SimpleTask
    {
        protected override DateTime GetNextRemindTime()
        {
            DateTime now = DateTime.Now;
            DateTime deadline = new DateTime(now.Year, now.Month, now.Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            if (deadline < now || IsFinished) deadline = deadline.AddDays(1);
            DateTime remindTime = deadline - RemindSpan;

            return remindTime;
        }
    }

    public class WeeklyTask : SimpleTask
    {
        DayOfWeek[] days { get; set; }

        protected override DateTime GetNextRemindTime()
        {
            DateTime now = DateTime.Now;
            DateTime deadline = new DateTime(now.Year, now.Month, now.Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            // Order days by which one comes the soonest
            List<DayOfWeek> ordered = days.OrderBy(day => now.HowManyDaysUntilThisDayOfWeek(day)).ToList();

            if (deadline < now || IsFinished)
            {
                DayOfWeek nextDay = ordered[1 % ordered.Count];
                deadline = deadline.AddDays(now.HowManyDaysUntilThisDayOfWeek(nextDay));
            }
            DateTime remindTime = deadline - RemindSpan;

            return remindTime;
        }
    }

    public class MonthlyTask : SimpleTask
    {
        int[] days { get; set; }

        protected override DateTime GetNextRemindTime()
        {
            DateTime now = DateTime.Now;
            DateTime deadline = new DateTime(now.Year, now.Month, now.Day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            List<int> ordered = days.OrderBy(day => now.HowManyDaysUntilThisDayOfMonth(day)).ToList();

            if (deadline < now || IsFinished)
            {
                int nextDay = ordered[1 % ordered.Count];
                deadline = deadline.AddDays(now.HowManyDaysUntilThisDayOfMonth(nextDay));
            }
            DateTime remindTime = deadline - RemindSpan;

            return remindTime;
        }
    }

    public class YearlyTask : SimpleTask
    {
        int month { get; set; }
        int day { get; set; }

        protected override DateTime GetNextRemindTime()
        {
            DateTime now = DateTime.Now;
            DateTime deadline = new DateTime(now.Year, month, day, DueTime.Hour, DueTime.Minute, DueTime.Second);

            if (deadline < now || IsFinished)
            {
                deadline = deadline.AddYears(1);
            }

            return deadline - RemindSpan;
        }
    }


    public class CustomTask : Task
    {
        //TODO
        protected override DateTime GetNextRemindTime()
        {
            throw new NotImplementedException();
        }
    }
}
