using TextFieldParserFramework.Delimited;
using TextFieldParserFramework.FixedWidth;

namespace TextFieldParserFrameworkTests.Models
{
    [FixedWidth]
    [Delimited(",", StringSplitOptions.None)]
    public class PersonWithAttributes
    {
        [Index(0)]
        [Range(1, 50)]
        public string FirstName { get; set; }

        [Index(1)]
        [Range(51, 50)]
        public string LastName { get; set; }


        [Index(2)]
        [Range(101, 3)]
        public string Age { get; set; }
    }
}