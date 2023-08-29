using System.Collections.ObjectModel;
using System.Linq.Expressions;
using TripleG3.ExpressionExtensions;

namespace TextFieldParser.Delimited;

public class DelimitedConfiguration<T> : IFileParserConfiguration
{
    private readonly IDictionary<string, int> propertyIndexes = new Dictionary<string, int>();
    public IReadOnlyDictionary<string, int> PropertyIndexes => new ReadOnlyDictionary<string, int>(propertyIndexes);
    public string Delimeter { get; private set; } = string.Empty;
    public StringSplitOptions StringSplitOptions { get; private set; }
    public Type Type { get; }
    public DelimitedConfiguration() => Type = typeof(T);

    public DelimitedConfiguration<T> SetDelimeter(string delimeter)
    {
        if (string.IsNullOrEmpty(delimeter))
        {
            throw new ArgumentNullException(nameof(delimeter));
        }
        Delimeter = delimeter;
        return this;
    }

    public DelimitedConfiguration<T> SetSplitOptions(StringSplitOptions stringSplitOptions)
    {
        StringSplitOptions = stringSplitOptions;
        return this;
    }

    public DelimitedConfiguration<T> SetProperty(int index, string propertyName)
    {
        if (!propertyIndexes.ContainsKey(propertyName))
        {
            propertyIndexes.Add(propertyName, index);
        }
        return this;
    }

    public DelimitedConfiguration<T> SetProperty(int index, Expression<Func<T, object>> getPropertyName)
    {
        var propertyName = getPropertyName.GetMemberName();
        if (!propertyIndexes.ContainsKey(propertyName))
        {
            propertyIndexes.Add(propertyName, index);
        }
        return this;
    }

    public DelimitedConfiguration<T> SetProperties(params (int index, Expression<Func<T, object>> getPropertyName)[] propertyRanges)
    {
        foreach (var (index, getPropertyName) in propertyRanges)
        {
            var propertyName = getPropertyName.GetMemberName();
            if (!propertyIndexes.ContainsKey(propertyName))
            {
                propertyIndexes.Add(propertyName, index);
            }
        }
        return this;
    }

    public static DelimitedConfiguration<T> Empty { get; } = new();
}
