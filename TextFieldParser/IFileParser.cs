namespace TextFieldParser;

public interface IFileParser<T> where T : notnull
{
    IEnumerable<T> ReadFile(string filePath);
    void WriteFile(string filePath, IEnumerable<T> values);
}
