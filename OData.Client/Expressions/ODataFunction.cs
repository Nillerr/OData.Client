namespace OData.Client.Expressions
{
    public class ODataFunction : IODataFunction
    {
        public ODataFunction(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}