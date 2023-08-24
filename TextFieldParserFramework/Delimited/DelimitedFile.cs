using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedFile<T> : IFileParse<T>
    {
        private readonly DelimitedConfiguration<T> configuration;

        internal DelimitedFile() : this(DelimitedConfiguration<T>.Empty) { }
        internal DelimitedFile(DelimitedConfiguration<T> delimitedConfiguration)
            => this.configuration = delimitedConfiguration;

        public IEnumerable<T> ReadFile(string filePath)
        {
            foreach (var line in File.ReadLines(filePath))
            {
                yield return ConvertFromLine(line);
            }
        }

        public void WriteFile(string filePath, IEnumerable<T> values)
        {
            File.WriteAllLines(filePath, values.Select(ConvertToLine));
        }

        internal T ConvertFromLine(string line)
        {
            var t = Activator.CreateInstance<T>();
            var stringValues = line.Split(configuration.Delimeter.ToCharArray(), configuration.StringSplitOptions);
            foreach (var propertyIndex in configuration.PropertyIndexes)
            {
                t.TrySetPropertyFromString(propertyIndex.PropertyName, stringValues[propertyIndex.Index]);
            }
            return t;
        }

        internal string ConvertToLine(T t)
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
    }
}