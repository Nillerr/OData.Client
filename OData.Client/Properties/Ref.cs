using System;

namespace OData.Client
{
    /// <summary>
    /// Used in <c>/$ref</c> requests to re-use <see cref="IODataProperties{TEntity}"/>. 
    /// </summary>
    internal sealed class Ref : IEntity
    {
        public static readonly Required<Ref, Uri> Id = "@odata.id";
    }
}