using Nep.Scheduling;
using System;
using static Nep.Scheduling.TestHolidayDefinitions;

namespace Sandbox
{
    /// <summary>
    /// A sandbox console app used to aid in testing and exercising code prior to wrtitng unit tests.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            bool was;

            var schedule = new Schedule(true);

            schedule.Add(new DueDate(3, DT2021_JAN_01));
            schedule.Add(new DueDate(3, DT2021_JAN_17));
            schedule.Add(new DueDate(3, DT2021_JAN_23));
            schedule.Add(new DueDate(3, DT2021_JAN_04));
            schedule.Add(new DueDate(3, DT2021_JAN_09));
            schedule.Add(new DueDate(3, DT2021_JAN_02));
            schedule.Add(new DueDate(3, DT2021_JAN_13));
            schedule.Add(new DueDate(3, DT2021_JAN_07));

            var holiday1 = new Holiday(DT2021_JAN_04, DT2021_JAN_07);
            var holiday2 = new Holiday(DT2021_JAN_09, DT2021_JAN_14);

            var sched1 = schedule.Adjust(holiday1, Adjustment.Apply, out was).Adjust(holiday2, Adjustment.Apply, out was);

            Console.WriteLine(schedule.GetScheduleString());
            Console.WriteLine(sched1.GetScheduleString());

            var sched2 = sched1.Adjust(holiday2, Adjustment.Retract, out was).Adjust(holiday1, Adjustment.Retract, out was);

            Console.WriteLine();

            Console.WriteLine(sched1.GetScheduleString());
            Console.WriteLine(sched2.GetScheduleString());
        }
    }
}