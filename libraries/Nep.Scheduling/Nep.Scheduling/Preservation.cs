namespace Nep.Scheduling
{
    /// <summary>
    /// Represents the two distinct ways in which a holiday can be applied to a schedule
    /// </summary>
    public enum Preservation
    {
        /// <summary>
        /// Only adjust dates if any fall within the holiday interval.
        /// </summary>
        DatePreserving,
        /// <summary>
        /// Always adjust all dates which fall on or after the start of the holiday.
        /// </summary>
        TimePreserving
    }
}