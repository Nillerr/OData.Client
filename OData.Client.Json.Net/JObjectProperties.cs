using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JObjectProperties<TEntity> : IODataProperties<TEntity> where TEntity : IEntity
    {
        private readonly JObject _root = new();

        private readonly JsonSerializer _serializer;
        private readonly IEntitySetNameResolver _entitySetNameResolver;

        public JObjectProperties(JsonSerializer serializer, IEntitySetNameResolver entitySetNameResolver)
        {
            _serializer = serializer;
            _entitySetNameResolver = entitySetNameResolver;
        }

        /// <inheritdoc />
        public IODataProperties<TEntity> Set<TValue>(IProperty<TEntity, TValue> property, TValue value)
            where TValue : notnull
        {
            var token = Token(value);
            _root[property.SelectableName] = token;
            return this;
        }

        /// <inheritdoc />
        public IODataProperties<TEntity> Bind<TOther>(IRef<TEntity, TOther> property, IEntityId<TOther> id)
            where TOther : IEntity
        {
            var reference = Reference(id);
            var token = JValue.CreateString(reference);
            _root[$"{property.SelectableName}@odata.bind"] = token;
            return this;
        }

        /// <inheritdoc />
        public IODataProperties<TEntity> Bind<TOther>(IRef<TEntity, IEntity> property, IEntityId<TOther> id) where TOther : IEntity
        {
            var reference = Reference(id);
            var token = JValue.CreateString(reference);
            _root[$"{property.SelectableName}_{id.Type.Name}@odata.bind"] = token;
            return this;
        }

        /// <inheritdoc />
        public IODataProperties<TEntity> BindAll<TOther>(
            IRefs<TEntity, TOther> property,
            IEnumerable<IEntityId<TOther>> ids
        )
            where TOther : IEntity
        {
            var array = new JArray();

            foreach (var id in ids)
            {
                var reference = Reference(id);
                var token = JValue.CreateString(reference);
                array.Add(token);
            }

            _root[$"{property.SelectableName}@odata.bind"] = array;
            return this;
        }

        /// <inheritdoc />
        public void WriteTo(Stream stream)
        {
            using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
            using var jsonWriter = new JsonTextWriter(streamWriter);

            // TODO @nije: Remove formatting
            jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

            _serializer.Serialize(jsonWriter, _root);

            jsonWriter.Flush();
        }

        private string Reference<TOther>(IEntityId<TOther> id)
            where TOther : IEntity
        {
            var entitySetName = _entitySetNameResolver.EntitySetName(id.Type);
            return $"/{entitySetName}({id.Id:D})";
        }

        private JToken Token<TValue>(TValue value) => value switch
        {
            null => JValue.CreateNull(),
            IEntityId<TEntity> entityId => entityId.Id.ToString("D", CultureInfo.InvariantCulture),
            Enum enumValue => Convert.ToInt32(enumValue),
            _ => JToken.FromObject(value, _serializer)
        };
    }
}