using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

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
            T t;
            if (typeof(T).GetConstructors().Any(x => x.GetParameters().Length == 0))
            {
                t = Activator.CreateInstance<T>();
            }
            else
            {
                t = (T)FormatterServices.GetUninitializedObject(typeof(T));
            }
            var stringValues = str.Split(configuration.Delimeter.ToCharArray(), configuration.StringSplitOptions);
            foreach (var propertyIndex in configuration.PropertyIndexes)
            {
                IStringParse stringParse;
                var memberInfos = t.GetType().GetMembers();
                var propertyInfo = t.GetType().GetProperty(propertyIndex.PropertyName);
                if (propertyInfo == null)
                {
                    var fieldInfo = t.GetType().GetField(propertyIndex.PropertyName) 
                        ?? throw new Exception($"Could not find property of field {propertyIndex.PropertyName} on type {t.GetType().Name}");

                    var fieldValue = parsers.TryGetValue(fieldInfo.FieldType, out stringParse)
                                   ? stringParse.ConvertFromString(stringValues[propertyIndex.Index])
                                   : fieldInfo.FieldType.InitFromString(stringValues[propertyIndex.Index]);
                    ParseHelpers.SetValue(ref t, fieldInfo, fieldValue);
                    continue;
                }
                var propertyValue = parsers.TryGetValue(propertyInfo.PropertyType, out stringParse)
                                  ? stringParse.ConvertFromString(stringValues[propertyIndex.Index])
                                  : propertyInfo.PropertyType.InitFromString(stringValues[propertyIndex.Index]);
                ParseHelpers.SetValue(ref t, propertyInfo, propertyValue);
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