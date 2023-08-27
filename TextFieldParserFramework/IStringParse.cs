namespace TextFieldParserFramework
{
    public interface IStringParse
    {
        IParseConfiguration Configuration { get; }
        object ConvertFromString(string str);
        string ConvertToString(object obj);
    }
}
