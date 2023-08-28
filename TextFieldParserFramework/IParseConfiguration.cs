using System;

namespace TextFieldParserFramework
{
    public interface IParseConfiguration 
    {
        Type Type { get; }
    }

    public interface IParseConfiguration<T> : IParseConfiguration { }
}
