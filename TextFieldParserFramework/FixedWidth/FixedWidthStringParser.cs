using System;
using System.ComponentModel;
using System.Linq;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthStringParser<T> : IStringParse<T>
    {
        private readonly FixedWidthConfiguration<T> configuration;

        public FixedWidthStringParser(FixedWidthConfiguration<T> configuration) => this.configuration = configuration;

        public IParseConfiguration Configuration => configuration;

        public T ConvertFromString(string str)
        {
            var implementation = Activator.CreateInstance<T>();
            foreach (var kvp in configuration.PropertyRanges)
            {
                var propertyInfo = implementation.GetType().GetProperty(kvp.Key);
                if (propertyInfo == null) continue;
                var subString = str.Substring(kvp.Value.Index - 1, kvp.Value.Length);
                var propertyValue = TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFromInvariantString(subString);
                propertyInfo.SetValue(implementation, propertyValue);
            }
            return implementation;
        }

        public string ConvertToString(T t)
        {
            int capacity;
            var (Index, Length) = configuration.PropertyRanges.Values.OrderByDescending(x => x.Index).FirstOrDefault();
            capacity = Index + Length - 1;
            string lineValue = "".PadRight(capacity);
            foreach (var kvp in configuration.PropertyRanges)
            {
                _ = t.TryGetStringFromProperty(kvp.Key, out string stringValue);
                string fieldValue;
                if (stringValue.Length > kvp.Value.Length)
                {
                    fieldValue = stringValue.Substring(0, kvp.Value.Length);
                }
                else
                {
                    fieldValue = stringValue.PadRight(kvp.Value.Length);
                }
                lineValue = lineValue.Remove(kvp.Value.Index - 1, kvp.Value.Length);
                lineValue = lineValue.Insert(kvp.Value.Index - 1, fieldValue);
            }
            return lineValue;
        }

        public string ConvertToString(object obj) => ConvertToString((T)obj);
        object IStringParse.ConvertFromString(string str) => ConvertFromString(str);
    }
}
