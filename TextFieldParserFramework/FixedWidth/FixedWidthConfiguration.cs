using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthParseConfiguration<T> : IParseConfiguration<T>
    {
        public Type Type => typeof(T);
        private readonly IDictionary<string, Range> propertyRanges = new Dictionary<string, Range>();
        public IReadOnlyDictionary<string, Range> PropertyRanges => new ReadOnlyDictionary<string, Range>(propertyRanges);
        public ICollection<string> Examples { get; } = new List<string>();
        public string Description { get; set; } = string.Empty;

        public FixedWidthParseConfiguration<T> SetProperty(int index, int length, string propertyName)
        {
            if (!propertyRanges.ContainsKey(propertyName))
            {
                propertyRanges.Add(propertyName, new Range(index, length));
            }
            return this;
        }

        public FixedWidthParseConfiguration<T> SetProperty(int index, int length, Expression<Func<T, object>> getPropertyName)
        {
            var propertyName = getPropertyName.GetMemberName();
            return SetProperty(index, length, propertyName);
        }

        public FixedWidthParseConfiguration<T> SetProperties(params PropertyRange[] propertyRanges)
        {
            foreach (PropertyRange item in propertyRanges)
            {
                SetProperty(item.Range.Index, item.Range.Length, item.PropertyName);
            }
            return this;
        }

        public static FixedWidthParseConfiguration<T> Empty { get; } = new FixedWidthParseConfiguration<T>();
    }
}
