# TripleG3.FileParser
## Delimited Files
### Configure delimited file parser for reading / writing.
- Call the `FileParseBuilder`
- Specify the file format / parse type `AsDelimited<T>`
- Configure the parser `Configure(config =>`

```
public class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Age { get; set; }
}
```
```
var fileParser = FileParseBuilder
                .AsDelimited<Person>()
                .Configure(config =>
                {
                    config.SetDelimeter(",")
                          .SetSplitOptions(StringSplitOptions.TrimEntries)
                          .SetProperties(
                               (0, person => person.FirstName),
                               (1, person => person.LastName),
                               (2, person => person.Age));
                })
                .Build();
```
- Read the file as `IEnumerable<Person>`