using System.Collections.Generic;

namespace TextFieldParserFramework
{

    public interface IFileParse<T>
    {
        IEnumerable<T> ReadFile(string filePath);
        void WriteFile(string filePath, IEnumerable<T> values);
    }
}
