using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthStringParser<T> : IStringParse<T>
    {
        private readonly FixedWidthParseConfiguration<T> configuration;
        private readonly IReadOnlyDictionary<Type, IStringParse> parsers;

        public FixedWidthStringParser(FixedWidthParseConfiguration<T> configuration, System.Collections.Generic.IReadOnlyDictionary<Type, IStringParse> parsers)
        {
            this.configuration = configuration;
            this.parsers = parsers;
        }

        public IParseConfiguration<T> Configuration => configuration;
        IParseConfiguration IStringParse.Configuration => configuration;
        public string ConvertToString(object obj) => ConvertToString((T)obj);
        object IStringParse.ConvertFromString(string str) => ConvertFromString(str);

        public T ConvertFromString(string str)
        {
            var implementation = Activator.CreateInstance<T>();
            foreach (var kvp in configuration.PropertyRanges)
            {
                var propertyInfo = implementation.GetType().GetProperty(kvp.Key);
                if (propertyInfo == null) continue;
                var subString = str.Substring(kvp.Value.Index - 1, kvp.Value.Length);
                object propertyValue;
                if (parsers.TryGetValue(propertyInfo.PropertyType, out IStringParse stringParser))
                    propertyValue = stringParser.ConvertFromString(subString);
                else
                     propertyValue = TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFromInvariantString(subString);
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
                var propertyInfo = t.GetType().GetProperty(kvp.Key);
                var stringValue = propertyInfo.PropertyType == typeof(string)
                        ? (string)propertyInfo.GetValue(t)
                        : TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertToInvariantString(t);
                if (stringValue == null) continue;

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
    }
}
