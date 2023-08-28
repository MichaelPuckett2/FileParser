using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextFieldParserFramework.Delimited
{
    public class DelimitedFileParser<T> : IFileParse<T>
    {
        private readonly IStringParse<T> strParser;
        internal DelimitedFileParser(IStringParse<T> strParser) => this.strParser = strParser;
        public IEnumerable<T> ReadFile(string filePath) => File.ReadLines(filePath).Select(strParser.ConvertFromString);
        public void WriteFile(string filePath, IEnumerable<T> values) => File.WriteAllLines(filePath, values.Select(strParser.ConvertToString));
    }
}