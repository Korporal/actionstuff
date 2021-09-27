using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Nep.Scheduling
{
    /// <summary>
    /// Represents a set of schedulable objects.
    /// </summary>
    /// <remarks>
    /// The Schedule class encapsulates a set of schedulable items. This enables you to use any complex object you wish
    /// because the API relies on on the interface being implemented.
    /// This is test text.
    /// </remarks>
    [DebuggerDisplay(@"{GetScheduleString()}")]
    public class Schedule
    {
        private SortedSet<ISchedulable> items;
        private bool ignoreTime;
        /// <summary>
        /// The collection of schedulable items ordered by ascending date.
        /// </summary>
        public List<ISchedulable> Items => items.ToList();
        /// <summary>
        /// Creates a new empty instance of a Schedule
        /// </summary>
        /// <param name="IgnoreTime"></param>
        public Schedule(bool IgnoreTime)
        {
            ignoreTime = IgnoreTime;
            items = new SortedSet<ISchedulable>(new DateComparer(IgnoreTime));
        }
        /// <summary>
        /// Adds a schedulable item to the schedule, maintaining internal ordering of all items by date.
        /// </summary>
        /// <param name="Item"></param>
        public void Add(ISchedulable Item)
        {
            if (Item == null) throw new ArgumentNullException(nameof(Item));

            if (ignoreTime && items.Where(i => i.At.Date == Item.At.Date).Any())
                    throw new ArgumentException($"An item is already present that's scheduled for the date {items.Where(i => i.At.Date == Item.At.Date).First().At.Date.ToShortDateString()}");

            if (!ignoreTime && items.Where(i => i.At == Item.At).Any())
                throw new ArgumentException($"An item is already present that's scheduled for the date and time {items.Where(i => i.At == Item.At).First().At}");

            items.Add(Item);
        }

        /// <summary>
        /// Adds a sequence of schedulable item to the schedule, maintaining internal ordering of all items by date.
        /// </summary>
        /// <param name="Items"></param>
        public void AddRange(IEnumerable<ISchedulable> Items)
        {
            if (Items == null) throw new ArgumentNullException(nameof(Items));
            if (Items.Any() == false) throw new ArgumentException("The supplied set of items is empty.", nameof(Items));

            foreach (var item in Items)
                items.Add(item);
        }

        /// <summary>
        /// Creates a new schedule in which all schedulable items that are imapcted by the holiday are given new dates.
        /// </summary>
        /// <param name="Holiday">A holiday</param>
        /// <param name="Adjustment">Indicate whether to apply the holiday or retract an already applied holiday.</param>
        /// <param name="WasAdjusted">Indicates whether any changes were made to any of the schedulable items' dates.</param>
        /// <param name="RoundBy">Makes the holiday's effective duration a multiple of this value.</param>
        /// <param name="Preservation">The type of changes to make the schedule.</param>
        /// <returns></returns>
        public Schedule Adjust(Holiday Holiday, Adjustment Adjustment, out bool WasAdjusted, int RoundBy = 7, Preservation Preservation = Preservation.TimePreserving)
        {
            WasAdjusted = IsAdjustedBy(Holiday, Adjustment, RoundBy);

            if (Adjustment == Adjustment.Apply)
                return ApplyHoliday(Holiday, RoundBy, Preservation);
            else
                return RetractHoliday(Holiday, RoundBy, Preservation);
        }

        private Schedule ApplyHoliday(Holiday Holiday, int RoundBy = 7, Preservation Preservation = Preservation.TimePreserving)
        {
            IEnumerable<ISchedulable> adjusted;

            int duration = RoundUp(Holiday.Duration, RoundBy);

            if (Preservation == Preservation.TimePreserving)
            {
                // We adjust all dates that occur after the start of the holiday by the neccesary duration.
                // This means that the time available for meeting the schedule is preserved.

                if (Holiday.Exclusivity == Exclusivity.StartIsInclusive || Holiday.Exclusivity == Exclusivity.FullyInclusive)
                    adjusted = Items.Select(i => i.At >= Holiday.StartDate ? i.Clone(i.At.AddDays(duration)) : i);  
                else
                    adjusted = Items.Select(i => i.At > Holiday.StartDate ? i.Clone(i.At.AddDays(duration)) : i);
            }
            else
            {
                // We adjust all dates that occur after the start of the holiday but only if there are any within the holiday.
                // This means that if no date liese within the holiday then no adjustment are made, the dates are preserved

                throw new NotImplementedException($"{nameof(ApplyHoliday)} is not implemented for preservation mode {Preservation}");
            }

            var schedule = new Schedule(ignoreTime);

            schedule.AddRange(adjusted);

            return schedule;

        }

        private Schedule RetractHoliday(Holiday Holiday, int RoundBy = 7, Preservation Preservation = Preservation.TimePreserving)
        {
            IEnumerable<ISchedulable> adjusted;

            int duration = RoundUp(Holiday.Duration, RoundBy);

            if (Preservation == Preservation.TimePreserving)
            {

                if (Holiday.Exclusivity == Exclusivity.StartIsInclusive || Holiday.Exclusivity == Exclusivity.FullyInclusive)
                    adjusted = Items.Select(i => i.At > Holiday.StartDate ? i.Clone(i.At.AddDays(-duration)) : i);
                else
                    throw new NotImplementedException($"{nameof(RetractHoliday)} is not implemented for exclusivity {Holiday.Exclusivity}");
            }
            else
            {
                throw new NotImplementedException($"{nameof(RetractHoliday)} is not implemented for preservation mode {Preservation}");
            }

            var schedule = new Schedule(ignoreTime);

            schedule.AddRange(adjusted);

            return schedule;

        }

        public bool IsAdjustedBy(Holiday Holiday, Adjustment Adjustment, int RoundBy)
        {
            if (Adjustment == Adjustment.Apply)
            {
                if (Holiday.Exclusivity == Exclusivity.StartIsInclusive || Holiday.Exclusivity == Exclusivity.FullyInclusive)
                    return Items.Where(i => i.At >= Holiday.StartDate).Any();
                else
                    return Items.Where(i => i.At > Holiday.StartDate).Any();
            }
            else
            {
                if (Holiday.Exclusivity == Exclusivity.StartIsInclusive || Holiday.Exclusivity == Exclusivity.FullyInclusive)
                    return Items.Where(i => i.At > Holiday.StartDate).Any(); 
                else
                    throw new NotImplementedException($"{nameof(IsAdjustedBy)} is not implemented for exclusivity {Holiday.Exclusivity}");
            }
        }

        private class DateComparer : IComparer<ISchedulable>
        {
            private bool ignoreTime = false;

            public DateComparer(bool IgnoreTime)
            {
                ignoreTime = IgnoreTime;
            }
            public int Compare(ISchedulable x, ISchedulable y)
            {
                if (ignoreTime)
                    return x.At.Date.CompareTo(y.At.Date);

                return x.At.CompareTo(y.At);
            }
        }

        private int RoundUp (int Value, int Rounder)
        {
            if (Value % Rounder == 0) return Value;
            return (Rounder - Value % Rounder) + Value;
        }

        /// <summary>
        /// Returns a string representation of all of the dates of the items in the schedule.
        /// </summary>
        /// <returns></returns>
        public string GetScheduleString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var item in Items)
            {
                builder.Append(item.At.ToString("dd-MM-yyyy, "));
            }

            return builder.ToString().TrimEnd().TrimEnd(',');
        }
    }
} 