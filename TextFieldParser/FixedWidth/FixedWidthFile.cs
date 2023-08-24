namespace TextFieldParser.FixedWidth;

public class FixedWidthFile<T> : IFileParser<T> where T : notnull
{
    private readonly FixedWidthConfiguration<T> fixedWidthConfiguration;

    internal FixedWidthFile() : this(FixedWidthConfiguration<T>.Empty) { }
    internal FixedWidthFile(FixedWidthConfiguration<T> fixedWidthConfiguration)
        => this.fixedWidthConfiguration = fixedWidthConfiguration;

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
        foreach (var kvp in fixedWidthConfiguration.Ranges.OrderBy(x => x.Value.Index))
        {
            implementation.TrySetPropertyFromString(kvp.Key, line.Substring(kvp.Value.Index - 1, kvp.Value.Length));
        }
        return implementation;
    }

    internal string ConvertToString(T t)
    {
        int capacity;
        var (Index, Length) = fixedWidthConfiguration.Ranges.Values.OrderByDescending(x => x.Index).FirstOrDefault();
        capacity = Index + Length - 1;
        string lineValue = "".PadRight(capacity);
        foreach (var kvp in fixedWidthConfiguration.Ranges)
        {
            _ = t.TryGetStringFromProperty(kvp.Key, out string stringValue);
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
