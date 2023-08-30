using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TextFieldParserFramework.Delimited;
using TextFieldParserFramework.Utility;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthStringParser<T> : IStringParse<T>
    {
        private readonly FixedWidthParseConfiguration<T> configuration;
        private readonly IReadOnlyDictionary<Type, IStringParse> parsers;

        public FixedWidthStringParser(FixedWidthParseConfiguration<T> configuration, IReadOnlyDictionary<Type, IStringParse> parsers)
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
            Instantiator.GetInstanceOf(out T t);
            var formatter = new Formatter<T>(parsers);
            foreach (var kvp in configuration.PropertyRanges)
            {
                string value = str.Substring(kvp.Value.Index - 1, kvp.Value.Length);
                string name = kvp.Key;
                if (formatter.SetValues(ref t, value, name))
                    continue;
                throw new Exception($"Could not find property or field {name} on type {t.GetType().Name} therefore could not assign value {value}");
            }
            return t;
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
