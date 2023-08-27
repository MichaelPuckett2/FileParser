﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace TextFieldParserFramework.FixedWidth
{
    public class FixedWidthFileParser<T> : IFileParse<T>
    {
        private readonly IStringParse<T> stringParser;

        internal FixedWidthFileParser(IStringParse<T> stringParser, IDictionary<Type, IStringParse> parsers)
        {
            Parsers = new ReadOnlyDictionary<Type, IStringParse>(parsers);
            this.stringParser = stringParser;
        }

        public IReadOnlyDictionary<Type, IStringParse> Parsers { get; }

        public IEnumerable<T> ReadFile(string filePath)
        {
            foreach (var line in File.ReadLines(filePath))
            {
                yield return stringParser.ConvertFromString(line);
            }
        }

        public void WriteFile(string filePath, IEnumerable<T> values)
        {
            File.WriteAllLines(filePath, values.Select(stringParser.ConvertToString));
        }
    }
}