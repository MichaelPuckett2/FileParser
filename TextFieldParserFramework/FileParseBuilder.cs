using TextFieldParserFramework.Delimited;
using TextFieldParserFramework.FixedWidth;

namespace TextFieldParserFramework
{
    public class FileParseBuilder
    {
        public static FixedWidthParser<T> AsFixedWidth<T>() => new FixedWidthParser<T>();
        public static DelimitedBuilder<T> AsDelimited<T>() => new DelimitedBuilder<T>();
    }
}
