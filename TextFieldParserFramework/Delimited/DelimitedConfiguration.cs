using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedConfiguration<T>
    {
        private readonly IList<PropertyIndex<T>> propertyIndexes = new List<PropertyIndex<T>>();
        public IReadOnlyList<PropertyIndex<T>> PropertyIndexes => new List<PropertyIndex<T>>(propertyIndexes);
        public string Delimeter { get; private set; } = string.Empty;
        public StringSplitOptions StringSplitOptions { get; private set; }

        public DelimitedConfiguration<T> SetDelimeter(string delimeter)
        {
            if (string.IsNullOrEmpty(delimeter))
            {
                throw new ArgumentNullException(nameof(delimeter));
            }
            Delimeter = delimeter;
            return this;
        }

        public DelimitedConfiguration<T> SetSplitOptions(StringSplitOptions stringSplitOptions)
        {
            StringSplitOptions = stringSplitOptions;
            return this;
        }

        public DelimitedConfiguration<T> SetProperty(int index, string propertyName)
        {
            if (!propertyIndexes.Any(x => x.PropertyName == propertyName))
            {
                propertyIndexes.Add(new PropertyIndex<T>(index, propertyName));
            }
            return this;
        }

        public DelimitedConfiguration<T> SetProperty(int index, Expression<Func<T, object>> getPropertyName)
        {
            var propertyName = getPropertyName.GetMemberName();
            if (!propertyIndexes.Any(x => x.PropertyName == propertyName))
            {
                propertyIndexes.Add(new PropertyIndex<T>(index, propertyName));
            }
            return this;
        }

        public DelimitedConfiguration<T> SetProperties(params PropertyIndex<T>[] propertyRanges)
        {
            foreach (var propertyRange in propertyRanges)
            {
                if (!propertyIndexes.Any(x => x.PropertyName == propertyRange.PropertyName))
                {
                    propertyIndexes.Add(new PropertyIndex<T>(propertyRange.Index, propertyRange.PropertyName));
                }
            }
            return this;
        }

        public static DelimitedConfiguration<T> Empty { get; } = new DelimitedConfiguration<T>();
    }
}
