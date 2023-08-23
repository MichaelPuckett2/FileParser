using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TextFieldParserFramework.FixedWidth
{
    public struct PropertyRange<T>
    {
        public Range Range;
        public string PropertyName;

        public PropertyRange(Range range, Expression<Func<T, object>> getPropertyName)
        {
            Range = range;
            PropertyName = getPropertyName.GetMemberName() ?? string.Empty;
        }

        public override bool Equals(object obj)
        {
            return obj is PropertyRange<T> other &&
                   EqualityComparer<Range>.Default.Equals(Range, other.Range) &&
                   EqualityComparer<string>.Default.Equals(PropertyName, other.PropertyName);
        }

        public override int GetHashCode()
        {
            int hashCode = 655372231;
            hashCode = hashCode * -1521134295 + Range.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            return hashCode;
        }

        public void Deconstruct(out Range range, out string propertyName)
        {
            range = Range;
            propertyName = PropertyName;
        }
    }
}
