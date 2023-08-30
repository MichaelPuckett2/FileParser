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
        public ICollection<string> Examples { get; } = new List<string>();
        public string Description { get; set; } = string.Empty;

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


    /// <summary>
    /// Taken from .NET 7 to be used here. Will be removed for the System.StringSplitOptions in .
    /// Specifies options for applicable Overload:System.String.Split method overloads,
    /// such as whether to omit empty substrings from the returned array or trim whitespace
    /// from substrings
    /// </summary>
    [Flags]
    public enum StringSplitOptions
    {
        /// <summary>
        /// Use the default options when splitting strings.
        /// </summary>
        None = 0,
        /// <summary>
        /// Omit array elements that contain an empty string from the result.
        /// If System.StringSplitOptions.RemoveEmptyEntries and System.StringSplitOptions.TrimEntries</summary>  
        RemoveEmptyEntries = 1,
        /// <summary>
        /// Trim white-space characters from each substring in the result. This field is
        /// available in .NET 5 and later versions only.
        /// </summary>
        TrimEntries = 2
    }
}
