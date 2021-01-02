namespace OData.Client
{
    public class DefaultPluralizer : IPluralizer
    {
        public string ToPlural(string source)
        {
            if (source.EndsWith("s"))
            {
                return $"{source}es";
            }

            return $"{source}s";
        }
    }
}