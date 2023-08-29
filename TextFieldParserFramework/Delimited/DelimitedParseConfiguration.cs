using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedParseConfiguration<T> : IParseConfiguration<T>
    {
        public Type Type => typeof(T);
        private readonly IList<PropertyIndex<T>> propertyIndexes = new List<PropertyIndex<T>>();
        public IReadOnlyList<PropertyIndex<T>> PropertyIndexes => new List<PropertyIndex<T>>(propertyIndexes);
        public string Delimeter { get; private set; } = string.Empty;
        public StringSplitOptions StringSplitOptions { get; private set; }

        public DelimitedParseConfiguration<T> SetDelimeter(string delimeter)
        {
            if (string.IsNullOrEmpty(delimeter))
            {
                throw new ArgumentNullException(nameof(delimeter));
            }
            Delimeter = delimeter;
            return this;
        }

        public DelimitedParseConfiguration<T> SetSplitOptions(StringSplitOptions stringSplitOptions)
        {
            StringSplitOptions = stringSplitOptions;
            return this;
        }

        public DelimitedParseConfiguration<T> SetProperty(int index, string propertyName)
        {
            if (!propertyIndexes.Any(x => x.PropertyName == propertyName))
            {
                propertyIndexes.Add(new PropertyIndex<T>(index, propertyName));
            }
            return this;
        }

        public DelimitedParseConfiguration<T> SetProperty(int index, Expression<Func<T, object>> getPropertyName)
        {
            var propertyName = getPropertyName.GetMemberName();
            return SetProperty(index, propertyName);
        }

        public static DelimitedParseConfiguration<T> Empty { get; } = new DelimitedParseConfiguration<T>();
    }
}
