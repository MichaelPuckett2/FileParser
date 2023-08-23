using System;

namespace TextFieldParserFramework.Delimited
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DelimitedAttribute : Attribute
    {
        public DelimitedAttribute(string delimiter, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        {
            Delimiter = delimiter;
            StringSplitOptions = stringSplitOptions;
        }
        public string Delimiter { get; }
        public StringSplitOptions StringSplitOptions { get; }
    }
}
