namespace TextFieldParser.Delimited;

public class DelimitedFile<T> : IFileParse<T> where T : notnull
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
        foreach (var (propertyName, index) in delimitedConfiguration.PropertyIndexes)
        {
            implementation.TrySetPropertyFromString(propertyName, stringValues[index]);
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
            _ = t.TryGetStringFromProperty(kvp.Key, out string stringValue);
            stringValues.Add(stringValue);
        }
        var result = string.Join(delimitedConfiguration.Delimeter, stringValues);
        return result;
    }
}
