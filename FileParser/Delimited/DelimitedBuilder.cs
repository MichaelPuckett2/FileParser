using System.Reflection;

namespace TextFieldParser.Delimited;

public class DelimitedBuilder<T>
{
    private readonly DelimitedConfiguration<T> configuration = new();

    public DelimitedBuilder<T> Configure(Action<DelimitedConfiguration<T>> sendConfiguration)
    {
        sendConfiguration.Invoke(configuration);
        return this;
    }

    public DelimitedFile<T> Build()
    {
        if (string.IsNullOrEmpty(configuration.Delimeter))
        {
            var delimitedAttribute = typeof(T).GetCustomAttribute<DelimitedAttribute>();
            if (string.IsNullOrEmpty(delimitedAttribute?.Delimiter))
                throw new MissingFieldException($"{nameof(DelimitedConfiguration<T>.Delimeter)} must be set in configuration or with {nameof(DelimitedAttribute)}");
            
            configuration.SetDelimeter(delimitedAttribute.Delimiter);
        }

        foreach (var property in typeof(T).GetProperties())
        {
            var indexAttribute = property.GetCustomAttribute<IndexAttribute>();
            if (indexAttribute != null)
            {
                configuration.Set(indexAttribute.Index, property.Name);
            }
        }
        return new DelimitedFile<T>(configuration);
    }
}
