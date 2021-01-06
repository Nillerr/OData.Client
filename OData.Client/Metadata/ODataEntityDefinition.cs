namespace OData.Client
{
    public sealed class ODataEntityDefinition : IEntity
    {
        public static readonly EntityType<ODataEntityDefinition> EntityType = "Metadata";

        public static readonly Required<ODataEntityDefinition, IEntityId<ODataEntityDefinition>> MetadataId = "MetadataId";
        public static readonly Required<ODataEntityDefinition, string> EntitySetName = "EntitySetName";
    }
}