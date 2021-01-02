using System.Collections.Generic;
using System.Linq;

namespace OData.Client
{
    public static class ODataPropertiesExtensions
    {
        public static IODataProperties<TEntity> BindAll<TEntity, TOther>(
            this IODataProperties<TEntity> properties,
            IProperty<TEntity, IEnumerable<TOther>> property,
            params IEntityId<TOther>[] ids
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            return properties.BindAll(property, ids.AsEnumerable());
        }
    }
}