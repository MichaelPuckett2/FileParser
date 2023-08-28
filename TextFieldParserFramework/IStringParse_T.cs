namespace TextFieldParserFramework
{
    public interface IStringParse<T> : IStringParse
    {
        new IParseConfiguration<T> Configuration { get; }
        new T ConvertFromString(string str);
        string ConvertToString(T t);
    }
}
