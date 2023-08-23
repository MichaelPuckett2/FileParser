using TextFieldParser.FixedWidth;

namespace TextFieldParserTests.FixedWidth;

[FixedWidth]
public class PersonWithAttributes
{
    [Range(1, 50)]
    public string FirstName { get; set; }

    [Range(51, 50)]
    public string LastName { get; set; }

    [Range(101, 3)]
    public string Age { get; set; }
}
