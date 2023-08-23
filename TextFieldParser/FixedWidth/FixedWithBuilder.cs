namespace TextFieldParser.FixedWidth;

public class FixedWithBuilder<T>
{
    private readonly FixedWidthConfiguration<T> configuration = new();

    public FixedWithBuilder<T> Configure(Action<FixedWidthConfiguration<T>> sendConfiguration)
    {
        sendConfiguration.Invoke(configuration);
        return this;
    }

    public FixedWidthFile<T> Build()
    {
        foreach (var property in typeof(T).GetProperties())
        {
            var rangeAttribute = (RangeAttribute?)property.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault();
            if (rangeAttribute != null)
            {
                configuration.SetProperty((rangeAttribute.Index, rangeAttribute.Length), property.Name);
            }
        }
        return new FixedWidthFile<T>(configuration);
    }
}
