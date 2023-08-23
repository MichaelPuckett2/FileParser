using TextFieldParser.Delimited;
using TextFieldParser.FixedWidth;

namespace TextFieldParser;

public class Parse
{
    public static FixedWithBuilder<T> AsFixedWidth<T>() => new();
    public static DelimitedBuilder<T> AsDelimited<T>() => new();
}
