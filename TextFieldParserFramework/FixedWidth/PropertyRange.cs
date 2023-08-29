using System.Collections.Generic;

namespace TextFieldParserFramework.FixedWidth
{
    public struct PropertyRange
    {
        public Range Range;
        public string PropertyName;

        public PropertyRange(Range range, string propertyName)
        {
            Range = range;
            PropertyName = propertyName ?? string.Empty;
        }

        public override bool Equals(object obj)
        {
            return obj is PropertyRange other &&
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
