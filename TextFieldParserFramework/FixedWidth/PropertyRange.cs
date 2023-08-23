using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TextFieldParserFramework.FixedWidth
{
    public struct PropertyRange<T>
    {
        public Range Range;
        public Expression<Func<T, object>> GetPropertyName;

        public PropertyRange(Range range, Expression<Func<T, object>> getPropertyName)
        {
            Range = range;
            GetPropertyName = getPropertyName;
        }

        public override bool Equals(object obj)
        {
            return obj is PropertyRange<T> other &&
                   EqualityComparer<Range>.Default.Equals(Range, other.Range) &&
                   EqualityComparer<Expression<Func<T, object>>>.Default.Equals(GetPropertyName, other.GetPropertyName);
        }

        public override int GetHashCode()
        {
            int hashCode = 655372231;
            hashCode = hashCode * -1521134295 + Range.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Expression<Func<T, object>>>.Default.GetHashCode(GetPropertyName);
            return hashCode;
        }

        public void Deconstruct(out Range range, out Expression<Func<T, object>> getPropertyName)
        {
            range = Range;
            getPropertyName = GetPropertyName;
        }
    }
}
