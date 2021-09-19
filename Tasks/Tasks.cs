using System;
using System.Linq;
using System.Collections.Generic;

namespace Tasks
{
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
        public bool IsFinished { get; set; }
        public TimeSpan RemindSpan { get; set; }

        public abstract DateTime GetNextRemindTime();

    }

    public abstract class SimpleTask : Task
    {   
        protected DateTime dueTime { get; set; }
    }

    public class DailyTask : SimpleTask
    {
        public override DateTime GetNextRemindTime()
        {
            DateTime now = DateTime.Now;
            DateTime deadline = new DateTime(now.Year, now.Month, now.Day, dueTime.Hour, dueTime.Minute, dueTime.Second);

            if (deadline < now || IsFinished) deadline = deadline.AddDays(1);
            DateTime remindTime = deadline - RemindSpan;

            return remindTime;
        }
    }

    public class WeeklyTask : SimpleTask
    {
        DayOfWeek[] days { get; set; }

        public override DateTime GetNextRemindTime()
        {
            DateTime now = DateTime.Now;
            DateTime deadline = new DateTime(now.Year, now.Month, now.Day, dueTime.Hour, dueTime.Minute, dueTime.Second);

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

        public override DateTime GetNextRemindTime()
        {
            DateTime now = DateTime.Now;
            DateTime deadline = new DateTime(now.Year, now.Month, now.Day, dueTime.Hour, dueTime.Minute, dueTime.Second);

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

        public override DateTime GetNextRemindTime()
        {
            DateTime now = DateTime.Now;
            DateTime deadline = new DateTime(now.Year, month, day, dueTime.Hour, dueTime.Minute, dueTime.Second);

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
        public override DateTime GetNextRemindTime()
        {
            throw new NotImplementedException();
        }
    }
}
