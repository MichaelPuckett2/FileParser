using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
            var t = Activator.CreateInstance<T>();
            var stringValues = str.Split(configuration.Delimeter.ToCharArray(), configuration.StringSplitOptions);
            foreach (var propertyIndex in configuration.PropertyIndexes)
            {
                var propertyInfo = t.GetType().GetProperty(propertyIndex.PropertyName);
                if (propertyInfo == null) continue;
                object propertyValue;
                if (parsers.TryGetValue(propertyInfo.PropertyType, out IStringParse stringParse))
                    propertyValue = stringParse.ConvertFromString(str);
                else
                    propertyValue = TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFromInvariantString(stringValues[propertyIndex.Index]);
                propertyInfo.SetValue(t, propertyValue);
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