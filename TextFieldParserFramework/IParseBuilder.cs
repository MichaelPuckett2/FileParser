using System;
using System.Collections.Generic;

namespace TextFieldParserFramework
{
    public interface IParseBuilder<TItem, TConfig> where TConfig : IParseConfiguration<TItem>
    {
        IReadOnlyDictionary<Type, IStringParse> Parsers { get; }
        IFileParse<TItem> Build();
        IParseBuilder<TItem, TConfig> Configure(Action<TConfig> configuration);
        IParseBuilder<TItem, TConfig> AddParser<TNew, TStringParse>(Func<TStringParse> func)
            where TStringParse : IStringParse;
    }
}
