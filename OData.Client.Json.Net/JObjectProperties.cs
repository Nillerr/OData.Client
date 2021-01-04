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

        public JObjectProperties(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public IODataProperties<TEntity> Set<TValue>(IProperty<TEntity, TValue> property, TValue value)
        {
            var token = Token(value);
            _root[property.Name] = token;
            return this;
        }

        public IODataProperties<TEntity> Bind<TOther>(IRef<TEntity, TOther> property, IEntityId<TOther> id)
            where TOther : IEntity
        {
            var reference = Reference(id);
            var token = JValue.CreateString(reference);
            _root[property.Name + "@odata.bind"] = token;
            return this;
        }

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

            _root[property.Name + "@odata.bind"] = array;
            return this;
        }

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
            return $"/{id.Type.Name}({id.Id:D})";
        }

        private JToken Token<TValue>(TValue value) => value switch
        {
            null => JValue.CreateNull(),
            IEntityId<TEntity> entityId => JValue.CreateString(entityId.Id.ToString("D", CultureInfo.InvariantCulture)),
            _ => JToken.FromObject(value, _serializer)
        };
    }
}