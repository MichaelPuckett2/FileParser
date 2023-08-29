using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using TextFieldParserFramework.Delimited;

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
            T t;
            if (typeof(T).GetConstructors().Any(x => x.GetParameters().Length == 0))
            {
                t = Activator.CreateInstance<T>();
            }
            else
            {
                t = (T)FormatterServices.GetUninitializedObject(typeof(T));
            }
            foreach (var kvp in configuration.PropertyRanges)
            {
                IStringParse stringParse;
                string subString;
                var propertyInfo = t.GetType().GetProperty(kvp.Key);
                if (propertyInfo == null)
                {
                    var fieldInfo = t.GetType().GetField(kvp.Key)
                        ?? throw new Exception($"Could not find property of field {kvp.Key} on type {t.GetType().Name}");
                    subString = str.Substring(kvp.Value.Index - 1, kvp.Value.Length);
                    var fieldValue = parsers.TryGetValue(fieldInfo.FieldType, out stringParse)
                                   ? stringParse.ConvertFromString(subString)
                                   : fieldInfo.FieldType.InitFromString(subString);
                    ParseHelpers.SetValue(ref t, fieldInfo, fieldValue);
                    continue;
                }
                subString = str.Substring(kvp.Value.Index - 1, kvp.Value.Length);
                object propertyValue;
                propertyValue = parsers.TryGetValue(propertyInfo.PropertyType, out stringParse)
                              ? stringParse.ConvertFromString(subString)
                              : propertyValue = propertyInfo.PropertyType.InitFromString(subString);
                ParseHelpers.SetValue(ref t, propertyInfo, propertyValue);
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
