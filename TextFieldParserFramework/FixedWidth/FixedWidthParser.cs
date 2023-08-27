using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthParser<T>
    {
        private readonly IDictionary<Type, IStringParse> parsers = new Dictionary<Type, IStringParse>();
        public IReadOnlyDictionary<Type, IStringParse> Parsers => new ReadOnlyDictionary<Type, IStringParse>(parsers);
        private readonly FixedWidthConfiguration<T> configuration = new FixedWidthConfiguration<T>();
        public FixedWidthParser<T> Configure(Action<FixedWidthConfiguration<T>> configuration)
        {
            configuration.Invoke(this.configuration);
            return this;
        }

        public FixedWidthParser<T> AddParser<TNew>(Func<IStringParse<TNew>> func)
        {
            parsers.Add(typeof(TNew), func?.Invoke());
            return this;
        }

        public FixedWidthFileParser<T> Build()
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