using System;
using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    public sealed class EntityIdConverter<TEntity> : JsonConverter<IEntityId<TEntity>> where TEntity : IEntity
    {
        private readonly IEntityName<TEntity> _entityName;

        public EntityIdConverter(IEntityName<TEntity> entityName)
        {
            _entityName = entityName;
        }

        public override void WriteJson(JsonWriter writer, IEntityId<TEntity>? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue($"/{value.Name.Name}({value.Id:D})");
            }
        }

        public override IEntityId<TEntity> ReadJson(
            JsonReader reader,
            Type objectType,
            IEntityId<TEntity>? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            var guid = serializer.Deserialize<Guid>(reader);
            return _entityName.Id(guid);
        }
    }
}