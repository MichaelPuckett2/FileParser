using System.Collections.Generic;
using System.Linq;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthEnumerableStringParser<T> : IEnumerableStringParse<T>
    {
        private readonly IStringParse<T> stringParse;
        public FixedWidthEnumerableStringParser(IStringParse<T> stringParse)
            => this.stringParse = stringParse;
        public IEnumerable<T> FromStrings(IEnumerable<string> strings)
            => strings.Select(str => stringParse.ConvertFromString(str));
        public IEnumerable<string> ToStrings(IEnumerable<T> values)
            => values.Select(value => stringParse.ConvertToString(value));
    }
}