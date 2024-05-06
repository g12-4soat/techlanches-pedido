namespace TechLanches.Core
{
    public abstract class Entity
    {
        protected Entity()
        {

        }

        protected Entity(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
#pragma warning disable IDE0041 // Use 'is null' check
            if (ReferenceEquals(null, compareTo)) return false;
#pragma warning restore IDE0041 // Use 'is null' check

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() ^ 93) + Id.GetHashCode();
        }
    }
}