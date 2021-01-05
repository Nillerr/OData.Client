using System;
using OData.Client.Expressions;

namespace OData.Client
{
    public static class RefsOperators
    {
        #region <property>/<function>(<param>:<body>)
        
        public static ODataFilter<TEntity> Any<TEntity, TOther>(
            this IRefs<TEntity, TOther> property,
            ODataFilter<TOther> filter
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var body = CheckLambdaBody(filter, nameof(filter));
            return Lambda(property, "any", body);
        }

        public static ODataFilter<TEntity> All<TEntity, TOther>(
            this IRefs<TEntity, TOther> property,
            ODataFilter<TOther> filter
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var body = CheckLambdaBody(filter, nameof(filter));
            return Lambda(property, "all", body);
        }

        private static ODataFilter<TEntity> Lambda<TEntity, TOther>(
            this IRefs<TEntity, TOther> property,
            string function,
            IODataLambdaBody body
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var expression = new ODataLambdaExpression(property, function, body);
            return new ODataFilter<TEntity>(expression);
        }

        private static IODataLambdaBody CheckLambdaBody<TOther>(ODataFilter<TOther> filter, string paramName)
            where TOther : IEntity
        {
            if (filter.Expression is not IODataLambdaBody body)
            {
                throw new ArgumentException(
                    $"Unexpected filter of type '{filter.Expression.GetType()}': {filter.Expression}. " +
                    $"The filter type must implement '{typeof(IODataLambdaBody)}'.",
                    paramName
                );
            }

            return body;
        }

        #endregion
    }
}