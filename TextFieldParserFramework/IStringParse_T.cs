namespace TextFieldParserFramework
{
    public interface IStringParse<T> : IStringParse
    {
        new T ConvertFromString(string str);
        string ConvertToString(T t);
    }
}
