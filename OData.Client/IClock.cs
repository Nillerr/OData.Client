using System;

namespace OData.Client
{
    /// <summary>
    /// An abstraction over a clock.
    /// </summary>
    /// <seealso cref="SystemClock"/>
    public interface IClock
    {
        /// <summary>
        /// Returns the current instant in UTC.
        /// </summary>
        DateTime UtcNow { get; }
    }
}