namespace OData.Client
{
    public sealed class ODataEntityDefinition : IEntity
    {
        public static readonly EntityType<ODataEntityDefinition> EntityType = "Metadata";

        public static readonly Property<ODataEntityDefinition, IEntityId<ODataEntityDefinition>> MetadataId = "MetadataId";
        public static readonly Property<ODataEntityDefinition, string> EntitySetName = "EntitySetName";
    }
}