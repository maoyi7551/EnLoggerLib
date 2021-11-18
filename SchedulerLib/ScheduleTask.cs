using System;


namespace SchedulerLib
{
    public enum TIME { SEC, HR, DAY };
    public class ScheduleTask
    {
        public static void IntervalInSeconds(int hour, int sec, double interval, Action task)
        {
            interval = interval / 3600;
            Scheduler.Instance.ScheduleTask(hour, sec, interval, task);
        }
        public static void IntervalInMinutes(int hour, int min, double interval, Action task)
        {
            interval = interval / 60;
            Scheduler.Instance.ScheduleTask(hour, min, interval, task);
        }
        public static void IntervalInHours(int hour, int min, double interval, Action task)
        {
            Scheduler.Instance.ScheduleTask(hour, min, interval, task);
        }
        public static void IntervalInDays(int hour, int min, double interval, Action task)
        {
            interval = interval * 24;
            Scheduler.Instance.ScheduleTask(hour, min, interval, task);
        }

        //public static void IntervalInSeconds(int hour, int sec, double interval, TIME type, Action task)
        //{ 
        //    Scheduler.Instance.ScheduleTask(hour, sec, interval, type, task);
        //}

        //public static void IntervalInMinutes(int hour, int min, double interval, TIME type, Action task)
        //{ 
        //    Scheduler.Instance.ScheduleTask(hour, min, interval, type, task);
        //}

        //public static void IntervalInHours(int hour, int min, double interval, TIME type, Action task)
        //{
        //    Scheduler.Instance.ScheduleTask(hour, min, interval, type,task);
        //}

        //public static void IntervalInDays(int hour, int min, double interval, TIME type, Action task)
        //{ 
        //    Scheduler.Instance.ScheduleTask(hour, min, interval, type, task);
        //} 
    }
}
