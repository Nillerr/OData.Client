# OData.Client

An OData Client developed specifically for use with Microsoft Dynamics 365 CRM supporting both queries and mutations on 
entities, including some of the "advanced" query capabilities supported by Microsoft Dynamics 365 CRM such as nested 
filters and lambda expression (`any` and `all`).

```c#
var oDataClient = new ODataClient(oDataClientSettings);

var contactsCollection = oDataClient.Collection(Contact.EntityType);

// Query the "collection" of Contact entities
var contacts = await contactsCollection
    .Where(Contact.EmailAddress.EndsWith("@github.com"))
    .Select(Contact.EmailAddress, Contact.GivenName, Contact.ParentCustomer)
    .Expand(Contact.ParentCustomer)
    .ToListAsync();

foreach (var contact in contacts)
{
    // Extract properties from `IEntity<Contact>`
    IEntityId<Contact> contactId = contact.Id(Contact.ContactId);
    string emailAddress = contact.Value(Contact.EmailAddress);
    string? givenName = contact.Value(Contact.GivenName);
    
    // ...Do stuff with contact details
    
    // Update a contact
    await contactsCollection.UpdateAsync(contactId, e =>
    {
        e.Set(Contact.GivenName, "OData");
        
        // Binds a single-valued navigation property
        e.Bind(Contact.ParentCustomer, Account.EntityType.ParseId("c6ac128c-83cb-44a8-88af-f4cbb02a8887");
        
        // Adds arguments to a collection-valued navigation property
        e.BindAll(Contact.Friends, Contact.EntityType.ParseId("a401e907-cd89-4885-b824-ec20d3b6d63d");
    });

    // Removes a value from a collection-valued navigation property
    await contactsCollection.DisassociatedAsync(contactId, Contact.Friends, Contact.EntityType.ParseId("5cda42d2-84a6-457d-942b-5a74f2fcf1ee"))
}
```

## How to

This section is a WIP and only contains some minimal information. See the code in the `OData.Client.Demo` project for 
more examples of how to use the library.

### Declare an entity

The schema of entities should be declared in a class for each entity. That class must implement the `IEntity` marker 
interface. A schema for a `Contact` may look something like this:

```c#
public sealed class Contact : IEntity
{
    // Declare the entity type by specifiying the `LogicalName` of the entity in an `EntityType<TEntity>` object.
    public static readonly EntityType<Contact> EntityType = "contact";
    
    // The id of the entity is declared using the `Required<TEntity, IEntityId<TEntity>>` type.
    public static readonly Required<Contact, IEntityId<Contact>> ContactId = "contactid";
    
    // Required (non-nullable) properties are declared using the `Required<TEntity, TValue>` type. The type of `TValue` 
    // must not use nullable annotations.
    public static readonly Required<Contact, string> EmailAddress = "emailaddress1";
    
    // Optional (nullable) properties are declared using the `Optional<TEntity, TValue>` type. The type of `TValue` 
    // must not use nullable annotations.
    public static readonly Optional<Contact, string> GivenName = "givenname";
    
    // Required reference to another entity (single-valued navivation properties) are declared using the 
    // `RequiredRef<TEntity, TOther>` type. The type of `TOther` must not use nullable annotations.
    public static readonly RequiredRef<Contact, Account> ParentCustomer = "parentcustomerid";
    
    // Optional reference to another entity (single-valued navivation properties) are declared using the 
    // `OptionalRef<TEntity, TOther>` type. The type of `TOther` must not use nullable annotations.
    public static readonly OptionalRef<Contact, Contact> ReportsTo = "reportstoid";
    
    // References to other entities (collection-valued navivation properties) are declared using the 
    // `Refs<TEntity, TOther>` type. The type of `TOther` must not use nullable annotations.
    public static readonly Refs<Contact, Contact> Friends = "friends";
}
```

#### Activities

The `activity` entity has the special property `regardingobjectid`, which allows it to reference any arbitrary object 
within the system. For properties such as this, that supports referencing multiple different object types, use the 
following property declaration:

```c#
public static class Activity : IEntity
{
    public static readonly RequiredRef<Activity, IEntity> RegardingObjectId = "regardingobjectid";
} 
```

This enables the use of the following method when extracting a value:

```c#
public IEntity<Contact> Contact(IEntity<Activity> activity)
{
    // Notice how you must specify the type of entity to extract
    IEntity<Contact> contact = activity.Entity(Activity.RegardingObjectId, Contact.EntityType);
    return contact;
}
```

## @TODO

- [ ] Implement `If-Match` and `If-None-Match`
  - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/perform-conditional-operations-using-web-api
- [x] Implement __Functions__ 👨‍🔬
  - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/use-web-api-functions
  - [x] Re-use for metadata
  - [ ] Finalize API
- [ ] Implement __Query Functions__
  - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/query-data-web-api#microsoft-dataverse-web-api-query-functions
- [ ] Implement `merge`
  - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/merge-entity-using-web-api
- [ ] Implement `$expand=<entity>($select=<selection>)`
- [ ] Implement change tracking
  - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/query-data-web-api#use-change-tracking-to-synchronize-data-with-external-systems
  - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/use-change-tracking-synchronize-data-external-systems
- [ ] Consider parameter aliases
  - https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/query-data-web-api#use-parameter-aliases-with-system-query-options
  - Can be implemented by finding every `ODataConstantExpression` in a query.

### Ideas

- [ ] Create a `Cursor<TEntity>` instead of `IAsyncEnumerable<IEntity<TEntity>>`.
   - Determine why this was suggested
- [ ] Support nested lambda expression?
- [ ] Replace methods using `<TValue>` with `Set(..., int value)` and such.
   - Remove `IValueFormatter`
   - Create `ODataIntegerExpression`, `ODataFloatingPointExpression`, `ODataDateTimeExpression`, `ODataStringExpression`, `ODataGuidExpression`, etc... Keep `ODataConstantExpression`.
- [ ] Add `IAnyEntity` for use with `regardingobjectid` in activities.
   - Add `AnyRef<TEntity>`
   - Remove `IODataQuery<TEntity> Expand(IRef<TEntity, IEntity> property);`, but add a comment to it guiding consumers to use `IODataQuery<TEntity> Expand<TOther>(IRef<TEntity, IAnyEntity> property, IEntityType<TOther> other)` instead.
- [ ] Introduce a `IQuerable<TEntity>` API
- [ ] Introduce `Id<TEntity>` property.
   - Means introducing `IId<Entity>` interface
   - Means introducing `IODataProperties<TEntity> Id(IId<TEntity> property, IEntityId<TEntity> value)`
   - Copy most of `Required<TEntity, TValue>` implementation.
