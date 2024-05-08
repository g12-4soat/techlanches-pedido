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
    }
}