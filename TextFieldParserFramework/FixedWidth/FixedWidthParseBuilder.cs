using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthParseBuilder<T> : IParseBuilder<T, FixedWidthParseConfiguration<T>>
    {
        private bool isBuilt = false;
        private readonly IDictionary<Type, IStringParse> parsers = new Dictionary<Type, IStringParse>();
        private readonly FixedWidthParseConfiguration<T> configuration = new FixedWidthParseConfiguration<T>();
        public IReadOnlyDictionary<Type, IStringParse> Parsers => new ReadOnlyDictionary<Type, IStringParse>(parsers);

        public IParseBuilder<T, FixedWidthParseConfiguration<T>> Configure(Action<FixedWidthParseConfiguration<T>> configuration)
        {
            configuration.Invoke(this.configuration);
            return this;
        }

        internal TResult PreBuild<TResult>(Func<TResult> func)
        {
            if (isBuilt) return func.Invoke();
            isBuilt = true;
            foreach (var property in typeof(T).GetProperties())
            {
                var rangeAttribute = (RangeAttribute)property.GetCustomAttributes(typeof(RangeAttribute), false).FirstOrDefault();
                if (rangeAttribute != null)
                {
                    configuration.SetProperty(rangeAttribute.Index, rangeAttribute.Length, property.Name);
                }
            }
            return func.Invoke();
        }

        public IStringParse<T> BuildStringParser() 
            => PreBuild(() => new FixedWidthStringParser<T>(configuration, Parsers));
        public IEnumerableStringParse<T> BuildEnumerableStringParser() 
            => PreBuild(() => new FixedWidthEnumerableStringParser<T>(BuildStringParser()));
        public IFileParse<T> BuildFileParser()
            => PreBuild(() => new FixedWidthFileParser<T>(BuildEnumerableStringParser()));

        public IParseBuilder<T, FixedWidthParseConfiguration<T>> AddParser<Tnew>(Func<IStringParse<Tnew>> func)
        {
            parsers.Add(typeof(Tnew), func.Invoke());
            return this;
        }

    }
}