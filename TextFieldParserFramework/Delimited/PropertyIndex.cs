using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TextFieldParserFramework.Delimited
{
    public struct PropertyIndex<T>
    {
        public int Index;
        public string PropertyName;

        public PropertyIndex(int index, Expression<Func<T, object>> getPropertyName)
        {
            Index = index;
            PropertyName = getPropertyName.GetMemberName() ?? string.Empty;
        }

        public PropertyIndex(int index, string propertyName)
        {
            Index = index;
            PropertyName = propertyName;
        }

        public override bool Equals(object obj)
        {
            return obj is PropertyIndex<T> other 
                && Index == other.Index 
                && PropertyName == other.PropertyName;
        }

        public override int GetHashCode()
        {
            int hashCode = -676832376;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            return hashCode;
        }

        public void Deconstruct(out int index, out string propertyName)
        {
            index = Index;
            propertyName = PropertyName;
        }
    }
}
