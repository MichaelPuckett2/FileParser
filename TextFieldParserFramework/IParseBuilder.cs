using System;
using System.Collections.Generic;

namespace TextFieldParserFramework
{
    public interface IParseBuilder<T, TConfig> where TConfig : IParseConfiguration<T>
    {
        IReadOnlyDictionary<Type, IStringParse> Parsers { get; }
        IFileParse<T> Build();
        IStringParse<T> BuildStringParser();
        IParseBuilder<T, TConfig> Configure(Action<TConfig> configuration);
        IParseBuilder<T, TConfig> AddParser<Tnew>(Func<IStringParse<Tnew>> func);
    }
}
