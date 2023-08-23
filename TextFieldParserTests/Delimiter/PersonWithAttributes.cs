using TextFieldParser.Delimited;

namespace TextFieldParserTests.Delimiter;

[Delimited(",", StringSplitOptions.TrimEntries)]
public class PersonWithAttributes
{
    [Index(0)]
    public string FirstName { get; set; }

    [Index(1)]
    public string LastName { get; set; }

    [Index(2)]
    public string Age { get; set; }
}
