using System;
using System.Reflection;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedBuilder<T>
    {
        private readonly DelimitedConfiguration<T> configuration = new DelimitedConfiguration<T>();

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

            if (typeof(T) is  a )

            foreach (var propertyinfo in typeof(T).GetProperties())
            {
                var indexAttribute = (IndexAttribute)propertyinfo.GetCustomAttribute(typeof(IndexAttribute));
                if (indexAttribute != null)
                {
                    configuration.SetProperty(indexAttribute.Index, propertyinfo.Name);
                }
            }
            return new DelimitedFile<T>(configuration);
        }
    }
}
