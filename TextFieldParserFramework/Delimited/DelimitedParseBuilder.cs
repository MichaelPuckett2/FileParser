using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedParseBuilder<T> : IParseBuilder<T, DelimitedConfiguration<T>>
    {
        private readonly IDictionary<Type, IStringParse> parsers = new Dictionary<Type, IStringParse>();
        private readonly DelimitedConfiguration<T> configuration = new DelimitedConfiguration<T>();
        public IReadOnlyDictionary<Type, IStringParse> Parsers => new ReadOnlyDictionary<Type, IStringParse>(parsers);

        public IFileParse<T> Build()
        {
            if (string.IsNullOrEmpty(configuration.Delimeter))
            {
                var delimitedAttribute = typeof(T).GetCustomAttribute<DelimitedAttribute>();
                if (string.IsNullOrEmpty(delimitedAttribute?.Delimiter))
                    throw new MissingFieldException($"{nameof(DelimitedConfiguration<T>.Delimeter)} must be set in configuration or with {nameof(DelimitedAttribute)}");

                configuration.SetDelimeter(delimitedAttribute.Delimiter);
            }

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

        public IParseBuilder<T, DelimitedConfiguration<T>> Configure(Action<DelimitedConfiguration<T>> configuration)
        {
            configuration.Invoke(this.configuration);
            return this;
        }

        public IParseBuilder<T, DelimitedConfiguration<T>> AddParser<TNew, TStringParse>(Func<TStringParse> func) where TStringParse : IStringParse
        {
            parsers.Add(typeof(TNew), func.Invoke());
            return this;
        }
    }
}
