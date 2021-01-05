using System;
using OData.Client.Expressions;

namespace OData.Client
{
    public static class RefOperators
    {
        #region <property> <operator> <reference>

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> references the
        /// <paramref name="other"/> id.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> References<TEntity, TOther>(
            this IRef<TEntity, TOther> property,
            IEntityId<TOther> other
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            return Binary(property, "eq", other);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> does not reference the
        /// <paramref name="other"/> id.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> DoesNotReference<TEntity, TOther>(
            this IRef<TEntity, TOther> property,
            IEntityId<TOther> other
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            return Binary(property, "ne", other);
        }

        private static ODataFilter<TEntity> Binary<TEntity, TOther>(
            IRef<TEntity, TOther> property,
            string @operator,
            IEntityId<TOther> other
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var refValueProperty = new RefValue<TEntity, TOther>(property);
            var left = new ODataPropertyExpression(refValueProperty);
            var right = new ODataConstantExpression(other, typeof(IEntityId<TOther>));
            var expression = new ODataBinaryExpression(left, @operator, right);
            return new ODataFilter<TEntity>(expression);
        }

        #endregion

        private sealed class RefValue<TEntity, TValue> : IRef<TEntity, TValue>
            where TEntity : IEntity
            where TValue : IEntity
        {
            private readonly IRef<TEntity, TValue> _reference;

            public RefValue(IRef<TEntity, TValue> reference)
            {
                _reference = reference;
            }

            public string Name => _reference.ValueName();
            public Type ValueType => _reference.ValueType;
            public Type EntityType => _reference.EntityType;
        }
    }
}