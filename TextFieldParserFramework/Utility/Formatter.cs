using System;
using System.Collections.Generic;

namespace TextFieldParserFramework.Delimited
{
    public class Formatter<T>
    {
        private readonly IReadOnlyDictionary<Type, IStringParse> parsers;
        public Formatter(IReadOnlyDictionary<Type, IStringParse> parsers)
        {
            this.parsers = parsers;
        }

        /// <summary>
        /// Attempts to set the property on a type, if found, and then the field if the property is not found.
        /// </summary>
        /// <param name="t">The type being formatted.</param>
        /// <param name="value">The value being applied.</param>
        /// <param name="name">THe property or field name used.</param>
        /// <returns>True is succeeds and false if not.</returns>
        public bool SetValues(ref T t, string value, string name)
        {
            if (SetProperty(ref t, value, name)) return true;
            return SetField(ref t, value, name);
        }

        public bool SetProperty(ref T t, string str, string name)
        {
            var propertyInfo = t.GetType().GetProperty(name);
            if (propertyInfo == null) return false;
            var propertyValue = parsers.TryGetValue(propertyInfo.PropertyType, out IStringParse stringParse)
                              ? stringParse.ConvertFromString(str)
                              : propertyInfo.PropertyType.InitFromString(str);
            ParseHelpers.SetValue(ref t, propertyValue, (impl, value) => propertyInfo.SetValue(impl, value));
            return true;
        }

        public bool SetField(ref T t, string str, string name)
        {
            var fieldInfo = t.GetType().GetField(name);
            if (fieldInfo == null) return false;
            var fieldValue = parsers.TryGetValue(fieldInfo.FieldType, out IStringParse stringParse)
                           ? stringParse.ConvertFromString(str)
                           : fieldInfo.FieldType.InitFromString(str);
            ParseHelpers.SetValue(ref t, fieldValue, (impl, value) => fieldInfo.SetValue(impl, value));
            return true;
        }
    }
}