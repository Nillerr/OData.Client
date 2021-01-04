using System;

namespace OData.Client
{
    /// <summary>
    /// A clock that wraps another clock, and verifies the <see cref="DateTime.Kind"/> value of every value returned by
    /// the wrapped clocks <see cref="IClock.UtcNow"/> property, throwing an exception if the value is not of UTC kind. 
    /// </summary>
    internal sealed class UtcClock : IClock
    {
        private readonly IClock _clock;

        public UtcClock(IClock clock)
        {
            _clock = clock;
        }

        /// <summary>
        /// Returns the instant returned by the wrapped clocks <see cref="IClock.UtcNow"/> property, throwing an
        /// exception if the value is not of UTC kind.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value returned by the wrapped clock was not in UTC.</exception>
        public DateTime UtcNow
        {
            get
            {
                var now = _clock.UtcNow;
                if (now.Kind != DateTimeKind.Utc)
                {
                    throw new InvalidOperationException($"The value returned by {_clock.GetType()}.{nameof(_clock.UtcNow)} must be in UTC.");
                }

                return now;
            }
        }
    }
}