using System;

namespace OData.Client
{
    /// <summary>
    /// A query builder for fluent chaining when constructing OData queries after sorting has been applied.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <seealso cref="IODataQuery{TEntity}"/>
    /// <example>
    /// <code>
    /// var contacts = oDataClient.Collection(Contact.EntityName);
    /// var query = contacts.Find()
    ///     .Filter(Contact.EmailAddress == "foo@bar.com")
    ///     .Select(Contact.EmailAddress, Contact.FirstName)
    ///     .Expand(Contact.ParentCustomer)
    ///     .OrderBy(Contact.CreatedAt)
    ///     .ThenByDescending(Contact.UpdatedAt);
    ///
    /// await foreach (var entity in query) {
    ///     var contactId = entity.Value(Contact.ContactId);
    ///     var emailAddress = entity.Value(Contact.EmailAddress);
    ///     var parentCustomer = entity.Value(Contact.ParentCustomer);
    /// }
    /// 
    /// </code>
    /// </example>
    public interface IODataOrderedQuery<TEntity> : IODataQuery<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Specifies another property to sort by in ascending order.
        /// </summary>
        /// <param name="property">The property to sort by.</param>
        /// <typeparam name="TValue">The type of value of the property.</typeparam>
        /// <returns>This query instance.</returns>
        IODataOrderedQuery<TEntity> ThenBy<TValue>(ISortableProperty<TEntity, TValue> property)
            where TValue : IComparable;

        /// <summary>
        /// Specifies another property to sort by in descending order.
        /// </summary>
        /// <param name="property">The property to sort by.</param>
        /// <typeparam name="TValue">The type of value of the property.</typeparam>
        /// <returns>This query instance.</returns>
        IODataOrderedQuery<TEntity> ThenByDescending<TValue>(ISortableProperty<TEntity, TValue> property)
            where TValue : IComparable;
    }
}