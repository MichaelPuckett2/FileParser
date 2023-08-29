using System.Collections.Generic;
using System.IO;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedFileParser<T> : IFileParse<T>
    {
        private readonly IEnumerableStringParse<T> enumerableStringParser;
        internal DelimitedFileParser(IEnumerableStringParse<T> enumerableStringParser) => this.enumerableStringParser = enumerableStringParser;
        public IEnumerable<T> ReadFile(string filePath) => enumerableStringParser.FromStrings(File.ReadLines(filePath));
        public void WriteFile(string filePath, IEnumerable<T> values) => File.WriteAllLines(filePath, enumerableStringParser.ToStrings(values));
    }
}