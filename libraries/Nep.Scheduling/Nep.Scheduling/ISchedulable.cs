using System;

namespace Nep.Scheduling
{
    /// <summary>
    /// Represents the interface an object must support if it is to schedulable.
    /// </summary>
    public interface ISchedulable
    {
        /// <summary>
        /// The date used to schedule the object with respect to others.
        /// </summary>
        DateTime At { get; }
        /// <summary>
        /// Creates an exact copy of the original object but with the specifid datetime.
        /// </summary>
        /// <param name="DateTime">The datetime for scheduling purpose, exposed as 'At'</param>
        /// <returns></returns>
        ISchedulable Clone(DateTime DateTime);
    }
} 