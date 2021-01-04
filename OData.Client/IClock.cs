using System;

namespace OData.Client
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}