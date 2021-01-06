using System;

namespace OData.Client
{
    /// <summary>
    /// Used in <c>/$ref</c> requests to re-use <see cref="IODataProperties{TEntity}"/>. 
    /// </summary>
    internal sealed class Ref : IEntity
    {
        /// <summary>
        /// This is only used internally in order to re-use stuff.
        /// </summary>
        internal static readonly EntityType<Ref> EntityType = "<<ref>>";
        
        public static readonly Required<Ref, Uri> Id = "@odata.id";
    }
}