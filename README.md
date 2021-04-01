# OData.Client

## TODO

 - Create a `Cursor<TEntity>` instead of `IAsyncEnumerable<IEntity<TEntity>>`.
    - Determine why this was suggested
 - Implement `If-Match` and `If-None-Match`
    - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/perform-conditional-operations-using-web-api
 - Implement `Functions`
    - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/use-web-api-functions
    - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/query-data-web-api#microsoft-dataverse-web-api-query-functions
    - Can be re-used for metadata
 - Implement `merge`
    - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/merge-entity-using-web-api
 - Implement `$expand=<entity>($select=<selection>)`
 - Implement change tracking
    - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/query-data-web-api#use-change-tracking-to-synchronize-data-with-external-systems
    - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/use-change-tracking-synchronize-data-external-systems
 - Consider parameter aliases
    - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/query-data-web-api#use-parameter-aliases-with-system-query-options
    - Can be implemented by finding every `ODataConstantExpression` in a query.
 - Create a `ODataSelection` class like what `ODataQuery` is to `ODataFindRequest`.
    - Can't do that because it is input to `Retrieve`.
 - ~~Work out how to use `ISelectableProperty`~~
 - Introduce `Id<TEntity>` property.
    - Means introducing `IId<Entity>` interface
    - Means introducing `IODataProperties<TEntity> Id(IId<TEntity> property, IEntityId<TEntity> value)`
    - Copy most of `Required<TEntity, TValue>` implementation.
 - Support nested lambda expression?
 - Replace methods using `<TValue>` with `Set(..., int value)` and such.
    - Remove `IValueFormatter`
    - Create `ODataIntegerExpression`, `ODataFloatingPointExpression`, `ODataDateTimeExpression`, `ODataStringExpression`, `ODataGuidExpression`, etc... Keep `ODataConstantExpression`.
 - Add `IAnyEntity` for use with `regardingobjectid` in activities.
    - Add `AnyRef<TEntity>`
    - Remove `IODataQuery<TEntity> Expand(IRef<TEntity, IEntity> property);`, but add a comment to it guiding consumers to use `IODataQuery<TEntity> Expand<TOther>(IRef<TEntity, IAnyEntity> property, IEntityType<TOther> other)` instead.
 - Add a `IQuerable` API