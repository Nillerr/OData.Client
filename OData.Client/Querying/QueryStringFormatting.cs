namespace OData.Client
{
    /// <summary>
    /// Specifies the formatting options for <see cref="ODataQuery{TEntity}"/>
    /// </summary>
    public enum QueryStringFormatting
    {
        /// <summary>
        /// No special formatting is applied.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Causes the query string to be URL escaped.
        /// </summary>
        UrlEscaped = 1,
    }
}