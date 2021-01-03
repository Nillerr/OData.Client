using System;
using System.Collections.Generic;

namespace OData.Client
{
    /// <summary>
    /// A query builder for fluent chaining when constructing OData queries.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <seealso cref="ODataQueryExtensions.ToListAsync{TEntity}"/>
    /// <seealso cref="ODataQueryExtensions.ToArrayAsync{TEntity}"/>
    /// <example>
    /// <code>
    /// var contacts = oDataClient.Collection(Contact.EntityName);
    /// var query = contacts.Find()
    ///     .Filter(Contact.EmailAddress == "foo@bar.com")
    ///     .Select(Contact.EmailAddress, Contact.FirstName)
    ///     .OrderBy(Contact.CreatedAt)
    ///     .Expand(Contact.ParentCustomer);
    ///
    /// await foreach (var entity in query) {
    ///     var contactId = entity.Value(Contact.ContactId);
    ///     var emailAddress = entity.Value(Contact.EmailAddress);
    ///     var parentCustomer = entity.Value(Contact.ParentCustomer);
    /// }
    /// 
    /// </code>
    /// </example>
    public interface IODataQuery<TEntity> : IAsyncEnumerable<IEntity<TEntity>> where TEntity : IEntity
    {
        /// <summary>
        /// Specifies a filter.
        /// </summary>
        /// <remarks>
        /// Subsequent calls to filter will AND the new filter with the existing filter.
        /// </remarks>
        /// <param name="filter">The filter.</param>
        /// <returns>This query instance.</returns>
        IODataQuery<TEntity> Filter(ODataFilter<TEntity> filter);

        /// <summary>
        /// Specifies a property to select.
        /// </summary>
        /// <param name="property">The property to select.</param>
        /// <returns>This query instance.</returns>
        /// <seealso cref="Select(OData.Client.IProperty{TEntity}[])"/>
        IODataQuery<TEntity> Select(IProperty<TEntity> property);
        
        /// <summary>
        /// Specifies properties to select.
        /// </summary>
        /// <param name="properties">The properties to select.</param>
        /// <returns>This query instance.</returns>
        /// <seealso cref="Select(OData.Client.IProperty{TEntity})"/>
        IODataQuery<TEntity> Select(params IProperty<TEntity>[] properties);

        /// <summary>
        /// Specifies a single-valued navigation property to expand.
        /// </summary>
        /// <param name="property">The property to expand.</param>
        /// <typeparam name="TOther">The type of entity referenced by the navigation property.</typeparam>
        /// <returns>This query instance.</returns>
        IODataQuery<TEntity> Expand<TOther>(IEntityReference<TEntity, TOther> property) where TOther : IEntity;

        /// <summary>
        /// Specifies a collection-valued navigation property to expand.
        /// </summary>
        /// <param name="property">The property to expand.</param>
        /// <typeparam name="TOther">The type of entity referenced by the navigation property.</typeparam>
        /// <returns>This query instance.</returns>
        IODataQuery<TEntity> Expand<TOther>(IProperty<TEntity, IEnumerable<TOther>> property) where TOther : IEntity;

        // IODataQuery<TEntity> Expand<TOther>(
        //     IProperty<TEntity, TOther?> property,
        //     Action<IODataNestedQuery<TOther>> query
        // )
        //     where TOther : IEntity;

        IODataOrderedQuery<TEntity> OrderBy<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable;

        IODataOrderedQuery<TEntity> OrderByDescending<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable;

        /// <summary>
        /// Specifies the maximum page size for each page in the request, or <see langword="null"/> for the default
        /// page size specified by the API.
        /// </summary>
        /// <param name="maxPageSize">The maximum page size.</param>
        /// <returns>This query instance.</returns>
        IODataQuery<TEntity> MaxPageSize(int? maxPageSize);
    }
}