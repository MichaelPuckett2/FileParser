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

        public IParseBuilder<T, FixedWidthParseConfiguration<T>> AddParser<TNew, TStringParse>(Func<TStringParse> func) where TStringParse : IStringParse
        {
            parsers.Add(typeof(TNew), func.Invoke());
            return this;
        }

        public IFileParse<T> Build()
        {
            ConfgureFromAttributes();
            return new FixedWidthFileParser<T>(new FixedWidthStringParser<T>(configuration), parsers);
        }

        private void ConfgureFromAttributes()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var rangeAttribute = (RangeAttribute)property.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault();
                if (rangeAttribute != null)
                {
                    configuration.SetProperty(new Range(rangeAttribute.Index, rangeAttribute.Length), property.Name);
                }
            }
        }
    }
}