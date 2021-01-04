using System;

namespace OData.Client
{
    /// <summary>
    /// A clock returning the current system time in UTC.
    /// </summary>
    public sealed class SystemClock : IClock
    {
        /// <summary>
        /// Returns the current system time in UTC.
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}