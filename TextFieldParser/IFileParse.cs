namespace TextFieldParser;

public interface IFileParse<T> where T : notnull
{
    IEnumerable<T> ReadFile(string filePath);
    void WriteFile(string filePath, IEnumerable<T> values);
}
