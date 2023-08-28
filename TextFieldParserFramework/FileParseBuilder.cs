using TextFieldParserFramework.Delimited;
using TextFieldParserFramework.FixedWidth;

namespace TextFieldParserFramework
{
    public class FileParseBuilder
    {
        public static IParseBuilder<T, FixedWidthParseConfiguration<T>> AsFixedWidth<T>() => new FixedWidthParseBuilder<T>();
        public static IParseBuilder<T, DelimitedConfiguration<T>> AsDelimited<T>() => new DelimitedParseBuilder<T>();
    }
}
