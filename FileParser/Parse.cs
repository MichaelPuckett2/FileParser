using TextFieldParser.FixedWidth;

namespace FileParser;

public class Parse
{
    public static FixedWithBuilder<T> AsFixedWidth<T>() => new();
}
