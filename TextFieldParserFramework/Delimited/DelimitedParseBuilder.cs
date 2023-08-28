using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedParseBuilder<T> : IParseBuilder<T, DelimitedParseConfiguration<T>>
    {
        private readonly IDictionary<Type, IStringParse> parsers = new Dictionary<Type, IStringParse>();
        private readonly DelimitedParseConfiguration<T> configuration = new DelimitedParseConfiguration<T>();
        public IReadOnlyDictionary<Type, IStringParse> Parsers => new ReadOnlyDictionary<Type, IStringParse>(parsers);

        public IFileParse<T> Build()
        {
            if (string.IsNullOrEmpty(configuration.Delimeter))
            {
                var delimitedAttribute = typeof(T).GetCustomAttribute<DelimitedAttribute>();
                if (string.IsNullOrEmpty(delimitedAttribute?.Delimiter))
                    throw new MissingFieldException($"{nameof(DelimitedParseConfiguration<T>.Delimeter)} must be set in configuration or with {nameof(DelimitedAttribute)}");

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
            return new DelimitedFileParser<T>(BuildStringParser());
        }

        public IParseBuilder<T, DelimitedParseConfiguration<T>> Configure(Action<DelimitedParseConfiguration<T>> configuration)
        {
            configuration.Invoke(this.configuration);
            return this;
        }

        public IParseBuilder<T, DelimitedParseConfiguration<T>> AddParser<Tnew>(Func<IStringParse<Tnew>> func)
        {
            parsers.Add(typeof(Tnew), func.Invoke());
            return this;
        }

        public IStringParse<T> BuildStringParser() => new DelimitedStringParser<T>(configuration, Parsers);
    }
}
