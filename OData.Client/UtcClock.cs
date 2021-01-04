using System;

namespace OData.Client
{
    internal sealed class UtcClock : IClock
    {
        private readonly IClock _clock;

        public UtcClock(IClock clock)
        {
            _clock = clock;
        }

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