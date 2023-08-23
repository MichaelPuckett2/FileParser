using System;
using TextFieldParserFramework.Delimited;

namespace TextFieldParserFrameworkTests.Delimiter
{
    [Delimited(",", StringSplitOptions.None)]
    public class PersonWithAttributes
    {
        [Index(0)]
        public string FirstName { get; set; }

        [Index(1)]
        public string LastName { get; set; }

        [Index(2)]
        public string Age { get; set; }
    }
}