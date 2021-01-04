using System;

namespace OData.Client
{
    public sealed class RequiredRef<TEntity, TOther> :
        IRequiredRef<TEntity, TOther>,
        IEquatable<RequiredRef<TEntity, TOther>>,
        IEquatable<OptionalRef<TEntity, TOther>>
        where TEntity : IEntity
        where TOther : IEntity
    {
        public RequiredRef(string name) => Name = name;

        public string Name { get; }

        public Type ValueType => typeof(TOther);

        public Type EntityType => typeof(TEntity);

        public static implicit operator RequiredRef<TEntity, TOther>(string str) => new(str);

        public bool Equals(RequiredRef<TEntity, TOther>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public bool Equals(OptionalRef<TEntity, TOther>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Name == other.Name;
        }

        public override bool Equals(object? obj) => obj switch
        {
            RequiredRef<TEntity, TOther> required => Equals(required),
            OptionalRef<TEntity, TOther> optional => Equals(optional),
            _ => false
        };

        public override int GetHashCode() => Name.GetHashCode();

        public IOptionalRef<TEntity, TOther> AsOptional() => new OptionalRef<TEntity, TOther>(Name);

        public static bool operator ==(RequiredRef<TEntity, TOther>? property, RequiredRef<TEntity, TOther>? other) => Equals(property, other);
        public static bool operator !=(RequiredRef<TEntity, TOther>? property, RequiredRef<TEntity, TOther>? other) => !Equals(property, other);

        // ReSharper disable once SuspiciousTypeConversion.Global
        public static bool operator ==(RequiredRef<TEntity, TOther>? property, OptionalRef<TEntity, TOther>? other) => Equals(property, other);
        // ReSharper disable once SuspiciousTypeConversion.Global
        public static bool operator !=(RequiredRef<TEntity, TOther>? property, OptionalRef<TEntity, TOther>? other) => !Equals(property, other);

        public override string ToString() => $"{nameof(Name)}: {Name}";
    }
}