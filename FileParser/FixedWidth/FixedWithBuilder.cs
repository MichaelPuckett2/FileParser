using FileParser;

namespace TextFieldParser.FixedWidth;

public class FixedWithBuilder<T>
{
    private readonly FixedWidthConfiguration<T> fixedWidthConfiguration = new FixedWidthConfiguration<T>();

    public FixedWithBuilder<T> Configure(Action<FixedWidthConfiguration<T>> sendConfiguration)
    {
        sendConfiguration.Invoke(fixedWidthConfiguration);
        return this;
    }

    public FixedWidthFile<T> Build()
    {
        foreach (var property in typeof(T).GetProperties())
        {
            var rangeAttribute = (RangeAttribute?)property.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault();
            if (rangeAttribute != null)
            {
                fixedWidthConfiguration.Set(property.Name, (rangeAttribute.Index, rangeAttribute.Length));
            }
        }
        return new FixedWidthFile<T>(fixedWidthConfiguration);
    }
}
