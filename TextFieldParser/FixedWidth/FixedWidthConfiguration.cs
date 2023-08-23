using System.Collections.ObjectModel;
using System.Linq.Expressions;
using TripleG3.ExpressionExtensions;

namespace TextFieldParser.FixedWidth;

public class FixedWidthConfiguration<T>
{    

    private readonly IDictionary<string, (int Index, int Length)> ranges = new Dictionary<string, (int Index, int Length)>();
    public IReadOnlyDictionary<string, (int Index, int Length)> Ranges => new ReadOnlyDictionary<string, (int Index, int Length)>(ranges);

    public void Set((int index, int length) range, string propertyName)
    {
        if (!ranges.ContainsKey(propertyName))
        {
            ranges.Add(propertyName, range);
        }
    }

    public FixedWidthConfiguration<T> Set((int index, int length) range, Expression<Func<T, object>> getPropertyName)
    {
        var propertyName = getPropertyName.GetMemberName();
        if (!ranges.ContainsKey(propertyName))
        {
            ranges.Add(propertyName, range);
        }
        return this;
    }

    public void Set(params ((int index, int length) range, Expression<Func<T, object>> getPropertyName)[] propertyRanges)
    {
        foreach (((int index, int length), Expression <Func<T, object>> getPropertyName) in propertyRanges)
        {
            var propertyName = getPropertyName.GetMemberName();
            if (!ranges.ContainsKey(propertyName))
            {
                ranges.Add(propertyName, (index, length));
            }
        }
    }

    public static FixedWidthConfiguration<T> Empty { get; } = new();
}
