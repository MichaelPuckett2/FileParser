using FileParser;

namespace TextFieldParser.FixedWidth.Tests;

public class TestFixedWidthPerson
{
    [Range(1, 50)]
    public string FirstName { get; set; }

    [Range(51, 50)]
    public string LastName { get; set; }

    [Range(101, 3)]
    public string Age { get; set; }
}
