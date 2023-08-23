using TextFieldParserFramework.Delimited;
using TextFieldParserFramework.FixedWidth;

namespace TextFieldParserFramework
{
    public class FileParseBuilder
    {
        public static FixedWithBuilder<T> AsFixedWidth<T>() => new FixedWithBuilder<T>();
        public static DelimitedBuilder<T> AsDelimited<T>() => new DelimitedBuilder<T>();
    }
}
