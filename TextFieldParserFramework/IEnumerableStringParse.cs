using System.Collections.Generic;

namespace TextFieldParserFramework
{
    public interface IEnumerableStringParse<T>
    {
        IEnumerable<T> FromStrings(IEnumerable<string> strings);
        IEnumerable<string> ToStrings(IEnumerable<T> values);
    }
}
