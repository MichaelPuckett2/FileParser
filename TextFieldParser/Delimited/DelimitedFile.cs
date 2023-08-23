using System.ComponentModel;

namespace TextFieldParser.Delimited;

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
        ReadOnlySpan<string> stringValues = line.Split(delimitedConfiguration.Delimeter, delimitedConfiguration.StringSplitOptions);
        foreach (var (kvp, property) in from kvp in delimitedConfiguration.PropertyIndexes
                                        let property = typeof(T).GetProperty(kvp.Key)
                                        select (kvp, property))
        {
            if (property == null) continue;
            var propertyValue = TypeDescriptor.GetConverter(property.PropertyType).ConvertFromInvariantString(stringValues[kvp.Value]);
            property.SetValue(implementation, propertyValue);
        }
        return implementation;
    }

    internal string ConvertToString(T t)
    {
        var stringValues = new List<string>();
        foreach (var kvp in delimitedConfiguration.PropertyIndexes.OrderBy(x => x.Value))
        {
            var property = typeof(T).GetProperty(kvp.Key);
            if (property == null) continue;
            var stringValue = TypeDescriptor.GetConverter(property.PropertyType).ConvertToInvariantString(property.GetValue(t));
            stringValues.Add(stringValue ?? string.Empty);
        }
        var result = string.Join(delimitedConfiguration.Delimeter, stringValues);
        return result;
    }
}
