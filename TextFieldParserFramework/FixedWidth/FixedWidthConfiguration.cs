using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthConfiguration<T> : IParserConfiguration
    {

        private readonly IDictionary<string, Range> ranges = new Dictionary<string, Range>();
        public IReadOnlyDictionary<string, Range> Ranges => new ReadOnlyDictionary<string, Range>(ranges);

        public void SetProperty(Range range, string propertyName)
        {
            if (!ranges.ContainsKey(propertyName))
            {
                ranges.Add(propertyName, range);
            }
        }

        public FixedWidthConfiguration<T> SetProperty(Range range, Expression<Func<T, object>> getPropertyName)
        {
            var propertyName = getPropertyName.GetMemberName();
            if (!ranges.ContainsKey(propertyName))
            {
                ranges.Add(propertyName, range);
            }
            return this;
        }

        public void SetProperties(params PropertyRange<T>[] propertyRanges)
        {
            foreach (PropertyRange<T> propertyRange in propertyRanges)
            {
                if (!ranges.ContainsKey(propertyRange.PropertyName))
                {
                    ranges.Add(propertyRange.PropertyName, propertyRange.Range);
                }
            }
        }

        public static FixedWidthConfiguration<T> Empty { get; } = new FixedWidthConfiguration<T>();
    }
}
