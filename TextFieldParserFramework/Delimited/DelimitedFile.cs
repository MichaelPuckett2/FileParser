using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedFile<T> : IFileParse<T>
    {
        private readonly DelimitedConfiguration<T> delimitedConfiguration;

        internal DelimitedFile() : this(DelimitedConfiguration<T>.Empty) { }
        internal DelimitedFile(DelimitedConfiguration<T> delimitedConfiguration)
            => this.delimitedConfiguration = delimitedConfiguration;

        public IEnumerable<T> ReadFile(string filePath)
        {
            foreach (var line in File.ReadLines(filePath))
            {
                yield return ConvertToType(line);
            }
        }

        public void WriteFile(string filePath, IEnumerable<T> values)
        {
            File.WriteAllLines(filePath, values.Select(ConvertToString));
        }

        internal T ConvertToType(string line)
        {
            var implementation = Activator.CreateInstance<T>();
            var stringValues = line.Split(delimitedConfiguration.Delimeter.ToCharArray(), delimitedConfiguration.StringSplitOptions);
            foreach (var propertyIndex in delimitedConfiguration.PropertyIndexes)
            {
                implementation.TrySetPropertyFromString(propertyIndex.PropertyName, stringValues[propertyIndex.Index]);
            }
            return implementation;
        }

        internal string ConvertToString(T t)
        {
            var stringValues = new List<string>();
            foreach (var propertyIndex in delimitedConfiguration.PropertyIndexes.OrderBy(x => x.Index))
            {
                var property = typeof(T).GetProperty(propertyIndex.PropertyName);
                if (property == null) continue;
                _ = t.TryGetStringFromProperty(propertyIndex.PropertyName, out string stringValue);
                stringValues.Add(stringValue);
            }
            var result = string.Join(delimitedConfiguration.Delimeter, stringValues);
            return result;
        }
    }
}