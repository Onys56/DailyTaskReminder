using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DailyTaskReminder.Reminders;

namespace DailyTaskReminder.Tasks
{
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
        public List<string> Reminders { get; set; } = new List<string>();
        public bool IsFinished { get; set; }
        public bool ReminderSent { get; set; }
        public TimeSpan RemindSpan { get; set; }
        public DateTimeOffset GetRemindTime => (IsFinished || ReminderSent ? GetDeadline(true) : GetDeadline()) - RemindSpan;

        public DateTimeOffset GetDeadlineTime => GetDeadline();

        protected abstract DateTimeOffset GetDeadline(bool next = false);

        internal abstract void Serialize(StreamWriter sw);
        internal abstract Task Deserialize(StreamReader sr);

    }

    public abstract class SimpleTask : Task
    {   
        public DateTimeOffset DueTime { get; set; }

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

        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            BaseSerialize(sw);
        }

        internal override Task Deserialize(StreamReader sr)
        {
            BaseDeserialize(sr);
            return this;
        }
    }

    public class WeeklyTask : SimpleTask
    {
        public List<DayOfWeek> Days { get; set; } = new();

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

        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            sw.WriteLine(string.Join(';', Days));
            BaseSerialize(sw);
        }

        internal override Task Deserialize(StreamReader sr)
        {
            string[] days = sr.ReadLine().Split(';');
            Days = days.Select(d => Enum.Parse<DayOfWeek>(d)).ToList();
            BaseDeserialize(sr);
            return this;

        }
    }

    public class MonthlyTask : SimpleTask
    {
        public int Day { get; set; } = 1;

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

        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            sw.WriteLine(Day);
            BaseSerialize(sw);
        }

        internal override Task Deserialize(StreamReader sr)
        {
            Day = int.Parse(sr.ReadLine());
            BaseDeserialize(sr);
            return this;
        }
    }

    public class YearlyTask : SimpleTask
    {
        public int Month { get; set; } = 1;
        public int Day { get; set; } = 1;

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

        internal override void Serialize(StreamWriter sw)
        {
            sw.WriteLine(GetType().Name);
            sw.WriteLine($"{Day};{Month}");
            BaseSerialize(sw);
        }

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
