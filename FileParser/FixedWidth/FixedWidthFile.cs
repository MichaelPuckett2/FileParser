namespace TextFieldParser.FixedWidth;

public class FixedWidthFile<T>
{
    private readonly FixedWidthConfiguration<T> fixedWidthConfiguration;

    internal FixedWidthFile() : this(FixedWidthConfiguration<T>.Empty) { }
    internal FixedWidthFile(FixedWidthConfiguration<T> fixedWidthConfiguration)
        => this.fixedWidthConfiguration = fixedWidthConfiguration;

    public IEnumerable<T> ReadLines(string filePath)
    {
        foreach (var line in File.ReadLines(filePath))
        {
            yield return ConvertToType(line);
        }
    }

    public void WriteLines(string filePath, IEnumerable<T> values)
    {
        File.WriteAllLines(filePath, values.Select(x => ConvertToString(x)));
    }

    public T ConvertToType(string line)
    {
        var implementation = Activator.CreateInstance<T>();
        foreach (var kvp in fixedWidthConfiguration.Ranges.OrderBy(x => x.Value.Index))
        {
            typeof(T).GetProperty(kvp.Key)?.SetValue(implementation, line.Substring(kvp.Value.Index - 1, kvp.Value.Length));
        }
        return implementation;
    }

    public string ConvertToString(T t)
    {
        int capacity;
        var (Index, Length) = fixedWidthConfiguration.Ranges.Values.OrderByDescending(x => x.Index).FirstOrDefault();
        capacity = Index + Length - 1;
        string lineValue = "".PadRight(capacity);
        foreach (var kvp in fixedWidthConfiguration.Ranges)
        {
            var propertyValue = typeof(T).GetProperty(kvp.Key)?.GetValue(t)?.ToString() ?? string.Empty;
            string fieldValue;
            if (propertyValue.Length > kvp.Value.Length)
            {
                fieldValue = propertyValue.Substring(0, kvp.Value.Length);
            }
            else
            {
                fieldValue = propertyValue.PadRight(kvp.Value.Length);
            }
            lineValue = lineValue.Remove(kvp.Value.Index - 1, kvp.Value.Length);
            lineValue = lineValue.Insert(kvp.Value.Index - 1, fieldValue);
        }
        return lineValue;
    }
}
