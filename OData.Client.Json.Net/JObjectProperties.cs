using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JObjectProperties<TEntity> : IODataProperties<TEntity>
        where TEntity : IEntity
    {
        private readonly JObject _root = new();

        private readonly IODataClient _oDataClient;
        private readonly JsonSerializer _serializer;
        private readonly IEntitySetNameResolver _entitySetNameResolver;

        private readonly List<Func<Task>> _asyncBinds = new();

        public JObjectProperties(
            IODataClient oDataClient,
            JsonSerializer serializer,
            IEntitySetNameResolver entitySetNameResolver
        )
        {
            _oDataClient = oDataClient;
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
            _asyncBinds.Add(async () =>
            {
                var reference = await ReferenceAsync(id);
                var token = JValue.CreateString(reference);
                _root[$"{property.Name}@odata.bind"] = token;
            });
            
            return this;
        }

        /// <inheritdoc />
        public IODataProperties<TEntity> Bind<TOther>(IRef<TEntity, IEntity> property, IEntityId<TOther> id)
            where TOther : IEntity
        {
            _asyncBinds.Add(async () =>
            {
                var reference = await ReferenceAsync(id);
                var token = JValue.CreateString(reference);
                _root[$"{property.SelectableName}_{id.Type.Name}@odata.bind"] = token; 
            });
            
            return this;
        }

        /// <inheritdoc />
        public IODataProperties<TEntity> BindAll<TOther>(
            IRefs<TEntity, TOther> property,
            IEnumerable<IEntityId<TOther>> ids
        )
            where TOther : IEntity
        {
            _asyncBinds.Add(async () =>
            {
                var array = new JArray();

                foreach (var id in ids)
                {
                    var reference = await ReferenceAsync(id);
                    var token = JValue.CreateString(reference);
                    array.Add(token);
                }

                _root[$"{property.SelectableName}@odata.bind"] = array;
            });
            
            return this;
        }

        private async Task<string> ReferenceAsync<TOther>(IEntityId<TOther> id) where TOther : IEntity
        {
            var context = ODataMetadataContext.Create(_oDataClient, id.Type);
            var entitySetName = await _entitySetNameResolver.EntitySetNameAsync(context);
            var reference = $"/{entitySetName}({id.Id:D})";
            return reference;
        }

        /// <inheritdoc />
        public async Task WriteToAsync(Stream stream)
        {
            // Wait for the async references to be set
            foreach (var asyncBind in _asyncBinds)
            {
                await asyncBind();
            }

            // Write to stream
            var streamWriter = new StreamWriter(stream, Encoding.UTF8);
            var jsonWriter = new JsonTextWriter(streamWriter);

            // TODO @nije: Remove formatting
            jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

            _serializer.Serialize(jsonWriter, _root);

            await jsonWriter.FlushAsync();
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