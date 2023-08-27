using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthConfiguration<T> : IParseConfiguration
    {
        public Type Type => typeof(T);
        private readonly IDictionary<string, Range> propertyRanges = new Dictionary<string, Range>();
        public IReadOnlyDictionary<string, Range> PropertyRanges => new ReadOnlyDictionary<string, Range>(propertyRanges);


        public FixedWidthConfiguration<T> SetProperty(Range range, string propertyName)
        {
            if (!propertyRanges.ContainsKey(propertyName))
            {
                propertyRanges.Add(propertyName, range);
            }
            return this;
        }

        public FixedWidthConfiguration<T> SetProperty(Range range, Expression<Func<T, object>> getPropertyName)
        {
            var propertyName = getPropertyName.GetMemberName();
            return SetProperty(range, propertyName);
        }

        public FixedWidthConfiguration<T> SetProperties(params PropertyRange<T>[] propertyRanges)
        {
            foreach (PropertyRange<T> propertyRange in propertyRanges)
            {
                SetProperty(propertyRange.Range, propertyRange.PropertyName);
            }
            return this;
        }

        public static FixedWidthConfiguration<T> Empty { get; } = new FixedWidthConfiguration<T>();
    }
}
