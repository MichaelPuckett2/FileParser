using System.Linq;

namespace TextFieldParserFramework.Delimited
{
    internal class Splitter
    {
        public string[] SplitString<T>(string str, DelimitedParseConfiguration<T> configuration)
        {
            var stringValues = str
                .Split(configuration.Delimeter.ToCharArray());

            if (configuration.StringSplitOptions == StringSplitOptions.RemoveEmptyEntries)
                stringValues = stringValues.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (configuration.StringSplitOptions == StringSplitOptions.TrimEntries)
                stringValues = stringValues.Select(x => configuration.StringSplitOptions == StringSplitOptions.TrimEntries ? x.Trim() : x)
                                           .ToArray();
            return stringValues;
        }
    }
}