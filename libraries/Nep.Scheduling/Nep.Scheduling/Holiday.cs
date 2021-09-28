using System;

namespace Nep.Scheduling
{

    /// <summary>
    /// Represents the concept of a holiday with various options.
    /// </summary>
    public class Holiday
    {
        /// <summary>
        /// The first day of the holiday.
        /// </summary>
        public DateTime StartDate { get; private set; }
        /// <summary>
        /// The last day of the holiday.
        /// </summary>
        public DateTime EndDate { get; private set; }
        /// <summary>
        /// Reflects the different ways dates within a holiday are counted.
        /// </summary>
        public Exclusivity Exclusivity { get; private set; }
        /// <summary>
        /// Creates a Holiday using an explict start and end date and an exclusivity value.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Exclusivity"></param>
        public Holiday(DateTime StartDate, DateTime EndDate, Exclusivity Exclusivity = Exclusivity.FullyInclusive)
        {
            if (!(StartDate.Date <= EndDate.Date))
                throw new ArgumentOutOfRangeException(nameof(StartDate), "The start date must be the same or before the end date.");

            switch (Exclusivity)
            {
                case Exclusivity.FullyExclusive:
                    {
                        if (StartDate == EndDate)
                            throw new ArgumentOutOfRangeException($"The start and end dates must not be the same for this exclusivity {Exclusivity}");
                        break;
                    }
            }

            this.StartDate = StartDate.Date;
            this.EndDate = EndDate.Date;
            this.Exclusivity = Exclusivity;
        }

        public Holiday(DateTime StartDate, int Duration, Exclusivity Exclusivity = Exclusivity.FullyInclusive)
        {
            if (Duration < 0)
                throw new ArgumentOutOfRangeException(nameof(Duration), "The duration must not be less than zero");

            switch (Exclusivity)
            {
                case (Exclusivity.FullyExclusive):
                    {
                        if (Duration == 0)
                            throw new ArgumentOutOfRangeException($"The duration must not be zero for this exclusivity {Exclusivity}");

                        this.StartDate = StartDate.Date;
                        this.EndDate = StartDate.AddDays(Duration + 1);
                        break;
                    }
                case (Exclusivity.FullyInclusive):
                    {
                        if (Duration == 0)
                            throw new ArgumentOutOfRangeException($"The duration must not be zero for this exclusivity {Exclusivity}");

                        this.StartDate = StartDate.Date;
                        this.EndDate = StartDate.AddDays(Duration - 1);
                        break;
                    }
                case (Exclusivity.StartIsInclusive):
                case (Exclusivity.EndIsInclusive):
                    {
                        this.StartDate = StartDate.Date;
                        this.EndDate = StartDate.AddDays(Duration);
                        break;
                    }
                default:
                    throw new InvalidOperationException("Unsupported exclusivity value was encountered.");
            }
        }
        /// <summary>
        /// The number of days considered to be part of the holiday, influenced by the exclusivity.
        /// </summary>
        public int Duration
        {
            get
            {
                switch (Exclusivity)
                {
                    case (Exclusivity.FullyExclusive):
                        {
                            return (EndDate - StartDate).Days - 1;
                        }
                    case (Exclusivity.FullyInclusive):
                        {
                            return (EndDate - StartDate).Days + 1;
                        }
                    case (Exclusivity.StartIsInclusive):
                    case (Exclusivity.EndIsInclusive):
                        {
                            return (EndDate - StartDate).Days;
                        }
                    default:
                        throw new InvalidOperationException("Unsupported exclusivity value was encountered.");
                }
            }
        }
        /// <summary>
        /// Indicates whether some date is or is not a holiday day, influenced by the exclusivity.
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public bool FallsWithinHoliday (DateTime Date)
        {
            Date = Date.Date;

            switch (Exclusivity)
            {
                case (Exclusivity.FullyExclusive):
                    {
                        if (Date > StartDate && Date < EndDate)
                            return true;
                        return false;
                    }
                case (Exclusivity.FullyInclusive):
                    {
                        if (Date >= StartDate && Date <= EndDate)
                            return true;
                        return false;

                    }
                case (Exclusivity.StartIsInclusive):
                    {
                        if (Date >= StartDate && Date < EndDate)
                            return true;
                        return false;

                    }
                case (Exclusivity.EndIsInclusive):
                    {
                        if (Date > StartDate && Date <= EndDate)
                            return true;
                        return false;
                    }
                default:
                    throw new InvalidOperationException("Unsupported exclusivity value was encountered.");
            }
        }
    }
} 