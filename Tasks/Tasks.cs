using System;

namespace Tasks
{

    public abstract class Task
    {
        public string Name { get; set; }
        public bool IsFinished { get; set; }
        public TimeSpan RemindTime { get; set; }

    }

    public class SimpleTask : Task
    {   
        DateTime dueTime { get; set; }
    }

    public class DailyTask : SimpleTask
    {

    }

    public class WeeklyTask : SimpleTask
    {
        DayOfWeek[] days { get; set; }

    }

    public class MonthlyTask : SimpleTask
    {
        int[] days { get; set; }
    }

    public class YearlyTask : SimpleTask
    {
        int month { get; set; }
        int day { get; set; }
    }


    public class CustomTask : Task
    {
        //TODO
    }
}
