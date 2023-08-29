using System;
using System.Collections.Generic;
using System.IO;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthFileParser<T> : IFileParse<T>
    {
        private readonly IEnumerableStringParse<T> enumerableStringParse;
        public FixedWidthFileParser(IEnumerableStringParse<T> enumerableStringParse)
            => this.enumerableStringParse = enumerableStringParse;
        public IReadOnlyDictionary<Type, IStringParse> Parsers { get; }
        public IEnumerable<T> ReadFile(string filePath)
            => enumerableStringParse.FromStrings(File.ReadLines(filePath));
        public void WriteFile(string filePath, IEnumerable<T> values)
            => File.WriteAllLines(filePath, enumerableStringParse.ToStrings(values));
    }
}
