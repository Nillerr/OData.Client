using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JObjectProperties<TEntity> : IODataProperties<TEntity> where TEntity : IEntity
    {
        private readonly JObject _root = new();
        private readonly JsonConverter[] _converters;

        public JObjectProperties(IList<JsonConverter> converters)
        {
            _converters = converters.ToArray();
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
            jsonWriter.Formatting = Formatting.Indented;

            _root.WriteTo(jsonWriter, _converters);
            jsonWriter.Flush();
        }

        private static string Reference<TOther>(IEntityId<TOther> id)
            where TOther : IEntity
        {
            return $"/{id.Name.Name}({id.Id:D})";
        }

        private static JToken Token<TValue>(TValue value)
        {
            if (value is null)
            {
                return JValue.CreateNull();
            }

            return JToken.FromObject(value);
        }

        public HttpContent ToHttpContent()
        {
            var json = _root.ToString(Formatting.Indented);
            Console.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            return content;
        }
    }
}