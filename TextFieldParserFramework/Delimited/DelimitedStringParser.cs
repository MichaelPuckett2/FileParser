using System;
using System.Collections.Generic;
using System.Linq;
using TextFieldParserFramework.Utility;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedStringParser<T> : IStringParse<T>
    {
        private readonly DelimitedParseConfiguration<T> configuration;
        private readonly IReadOnlyDictionary<Type, IStringParse> parsers;

        public DelimitedStringParser(DelimitedParseConfiguration<T> configuration, IReadOnlyDictionary<Type, IStringParse> parsers)
        {
            this.configuration = configuration;
            this.parsers = parsers;
        }

        public IParseConfiguration<T> Configuration => configuration;
        IParseConfiguration IStringParse.Configuration => configuration;

        public T ConvertFromString(string str)
        {
            Instantiator.GetInstanceOf(out T t);
            string[] stringValues = new Splitter().SplitString(str, configuration);
            var formatter = new Formatter<T>(parsers);
            foreach (var propertyIndex in configuration.PropertyIndexes)
            {
                string value = stringValues[propertyIndex.Index];
                string name = propertyIndex.PropertyName;
                if (formatter.SetValues(ref t, value, name))
                    continue;
                throw new Exception($"Could not find property or field {name} on type {t.GetType().Name} therefore could not assign value {value}");
            }
            return t;
        }

        public string ConvertToString(T t)
        {
            var stringValues = new List<string>();
            foreach (var propertyIndex in configuration.PropertyIndexes.OrderBy(x => x.Index))
            {
                var property = typeof(T).GetProperty(propertyIndex.PropertyName);
                if (property == null) continue;
                _ = t.TryGetStringFromProperty(propertyIndex.PropertyName, out string stringValue);
                stringValues.Add(stringValue);
            }
            var result = string.Join(configuration.Delimeter, stringValues);
            return result;
        }

        public string ConvertToString(object obj) => ConvertToString((T)obj);
        object IStringParse.ConvertFromString(string str) => ConvertFromString(str);
    }
}