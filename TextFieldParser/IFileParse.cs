namespace TextFieldParser;

public interface IFileParse<T>
{
    IEnumerable<T> ReadFile(string filePath);
    void WriteFile(string filePath, IEnumerable<T> values);
}
