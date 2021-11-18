using System;
using System.Collections.Generic;
using System.Threading;

namespace SchedulerLib
{
    public class Scheduler
    {
        private static Scheduler _instance;
        private List<Timer> timers = new List<Timer>();

        private Scheduler() { }

        public static Scheduler Instance => _instance ?? (_instance = new Scheduler());
        public void ScheduleTask(int hour, int min, double interval, Action task)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                while (firstRun < now)
                    firstRun = firstRun.AddHours(interval);
            }

            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            var timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(interval));

            timers.Add(timer);
        }

        //public void ScheduleTask(int hour, int min, double interval, TIME type,  Action task)
        //{
        //    DateTime now = DateTime.Now;
        //    DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
        //    if (now > firstRun)
        //    {
        //        if (type == TIME.HR)
        //        {
        //           while(firstRun < now)
        //            firstRun = firstRun.AddHours(interval);
        //        }
        //        else if(type == TIME.DAY)
        //        {
        //            while (firstRun < now)
        //                firstRun = firstRun.AddDays(interval);
        //        }
        //        else
        //        {
        //            while (firstRun < now)
        //                firstRun = firstRun.AddSeconds(interval);
        //        }
        //    }

        //    TimeSpan timeToGo = firstRun - now;
        //    if (timeToGo <= TimeSpan.Zero)
        //    {
        //        timeToGo = TimeSpan.Zero;
        //    }

        //    var timer = new Timer(x =>
        //    {
        //        task.Invoke();
        //    }, null, timeToGo, TimeSpan.FromHours(interval));

        //    timers.Add(timer);
        //}
    }
}
