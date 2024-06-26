﻿namespace TechLanches.Core
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
#pragma warning disable IDE0041 // Use 'is null' check
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
#pragma warning restore IDE0041 // Use 'is null' check
            return ReferenceEquals(left, right) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            return EqualOperator(a, b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return NotEqualOperator(a, b);
        }

        protected abstract IEnumerable<object> RetornarPropriedadesDeEquidade();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return this.RetornarPropriedadesDeEquidade().SequenceEqual(other.RetornarPropriedadesDeEquidade());
        }

        public override int GetHashCode()
        {
            return RetornarPropriedadesDeEquidade()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}