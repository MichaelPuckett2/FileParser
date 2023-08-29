using TextFieldParserFramework.Delimited;
using TextFieldParserFramework.FixedWidth;

namespace TextFieldParserFramework
{
    public class Parse
    {
        public static IParseBuilder<T, FixedWidthParseConfiguration<T>> AsFixedWidth<T>() => new FixedWidthParseBuilder<T>();
        public static IParseBuilder<T, DelimitedParseConfiguration<T>> AsDelimited<T>() => new DelimitedParseBuilder<T>();
    }
}
