using System;

namespace OData.Client
{
    public sealed class OptionalRef<TEntity, TOther> :
        IOptionalRef<TEntity, TOther>,
        IEquatable<OptionalRef<TEntity, TOther>>,
        IEquatable<RequiredRef<TEntity, TOther>>
        where TEntity : IEntity
        where TOther : IEntity
    {
        public OptionalRef(string name) => Name = name;

        public string Name { get; }

        public Type ValueType => typeof(TOther);

        public Type EntityType => typeof(TEntity);

        public static implicit operator OptionalRef<TEntity, TOther>(string str) => new(str);

        public bool Equals(OptionalRef<TEntity, TOther>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public bool Equals(RequiredRef<TEntity, TOther>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Name == other.Name;
        }

        public override bool Equals(object? obj) => obj switch
        {
            OptionalRef<TEntity, TOther> optional => Equals(optional),
            RequiredRef<TEntity, TOther> required => Equals(required),
            _ => false
        };

        public override int GetHashCode() => Name.GetHashCode();
        
        public static bool operator ==(OptionalRef<TEntity, TOther>? property, OptionalRef<TEntity, TOther>? other) => Equals(property, other);
        public static bool operator !=(OptionalRef<TEntity, TOther>? property, OptionalRef<TEntity, TOther>? other) => !Equals(property, other);

        // ReSharper disable once SuspiciousTypeConversion.Global
        public static bool operator ==(OptionalRef<TEntity, TOther>? property, RequiredRef<TEntity, TOther>? other) => Equals(property, other);
        // ReSharper disable once SuspiciousTypeConversion.Global
        public static bool operator !=(OptionalRef<TEntity, TOther>? property, RequiredRef<TEntity, TOther>? other) => !Equals(property, other);

        public override string ToString() => $"{nameof(Name)}: {Name}";
    }
}