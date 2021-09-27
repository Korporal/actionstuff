namespace Nep.Scheduling
{
    /// <summary>
    /// The different ways that the start and end date of a holiday can be counted.
    /// </summary>
    public enum Exclusivity
    {
        FullyExclusive,
        FullyInclusive,
        StartIsInclusive,
        EndIsInclusive
    }
} 