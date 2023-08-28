using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthParseBuilder<T> : IParseBuilder<T, FixedWidthParseConfiguration<T>>
    {
        private readonly IDictionary<Type, IStringParse> parsers = new Dictionary<Type, IStringParse>();
        private readonly FixedWidthParseConfiguration<T> configuration = new FixedWidthParseConfiguration<T>();
        public IReadOnlyDictionary<Type, IStringParse> Parsers => new ReadOnlyDictionary<Type, IStringParse>(parsers);

        public IParseBuilder<T, FixedWidthParseConfiguration<T>> Configure(Action<FixedWidthParseConfiguration<T>> configuration)
        {
            configuration.Invoke(this.configuration);
            return this;
        }

        public IFileParse<T> Build()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var rangeAttribute = (RangeAttribute)property.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault();
                if (rangeAttribute != null)
                {
                    configuration.SetProperty(new Range(rangeAttribute.Index, rangeAttribute.Length), property.Name);
                }
            }
            return new FixedWidthFileParser<T>(BuildStringParser());
        }

        public IParseBuilder<T, FixedWidthParseConfiguration<T>> AddParser<Tnew>(Func<IStringParse<Tnew>> func)
        {
            parsers.Add(typeof(Tnew), func.Invoke());
            return this;
        }

        public IStringParse<T> BuildStringParser() => new FixedWidthStringParser<T>(configuration, Parsers);
    }
}