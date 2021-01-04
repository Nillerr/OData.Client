using System;

namespace OData.Client
{
    public sealed class Refs<TEntity, TOther> :
        IRefs<TEntity, TOther>,
        IEquatable<Refs<TEntity, TOther>>
        where TEntity : IEntity
        where TOther : IEntity
    {
        public Refs(string name) => Name = name;

        public string Name { get; }

        public Type ValueType => typeof(TOther);

        public Type EntityType => typeof(TEntity);

        public static implicit operator Refs<TEntity, TOther>(string str) => new(str);

        public bool Equals(Refs<TEntity, TOther>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object? obj) => obj is Refs<TEntity, TOther> optional && Equals(optional);

        public override int GetHashCode() => Name.GetHashCode();
        
        public static bool operator ==(Refs<TEntity, TOther>? property, Refs<TEntity, TOther>? other) => Equals(property, other);
        public static bool operator !=(Refs<TEntity, TOther>? property, Refs<TEntity, TOther>? other) => !Equals(property, other);

        public override string ToString() => $"{nameof(Name)}: {Name}";
    }
}