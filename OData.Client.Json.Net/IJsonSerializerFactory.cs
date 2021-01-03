using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    public interface IJsonSerializerFactory
    {
        JsonSerializer CreateSerializer<TEntity>(IEntityName<TEntity> name) where TEntity : IEntity;
    }
}