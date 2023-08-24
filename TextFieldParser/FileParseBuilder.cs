using TextFieldParser.Delimited;
using TextFieldParser.FixedWidth;

namespace TextFieldParser;

public class FileParseBuilder
{
    public static FixedWithBuilder<T> AsFixedWidth<T>() where T : notnull => new();
    public static DelimitedBuilder<T> AsDelimited<T>() where T : notnull => new();
}
