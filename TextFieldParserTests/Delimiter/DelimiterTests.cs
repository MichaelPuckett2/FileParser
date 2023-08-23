using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextFieldParser;

namespace TextFieldParserTests.Delimiter;

[TestClass()]
public class DelimiterTests
{
    private const string WriteAttributeTestFile = "DelimiterWriteAttributeTest.txt";
    private const string WriteConfigurationTestFile = "DelimiterWriteConfigurationTest.txt";
    private const string ReadTestFile = "DelimitedReadTest.txt";

    [TestMethod()]
    public void ReadWithAttributesTest()
    {
        //Arrange
        var actor = FileParseBuilder
            .AsDelimited<PersonWithAttributes>()
            .Build();

        //Act
        var actual = actor.ReadFile(ReadTestFile);

        //Assert
        Assert.AreEqual(actual.Count(), 7);
    }

    [TestMethod()]
    public void ReadWithConfigurationTest()
    {
        //Arrange
        var fileParser = FileParseBuilder
                        .AsDelimited<Person>()
                        .Configure(config =>
                        {
                            config.SetDelimeter(",")
                                  .SetSplitOptions(StringSplitOptions.TrimEntries)
                                  .SetProperties((0, person => person.FirstName),
                                       (1, person => person.LastName),
                                       (2, person => person.Age));
                        })
                        .Build();

        //Act
        var actual = fileParser.ReadFile(ReadTestFile);

        //Assert
        Assert.AreEqual(actual.Count(), 7);
    }

    [TestMethod()]
    public void WriteWithConfigurationTest()
    {
        //Arrange
        var people = new List<Person>
        {
            new Person{ FirstName = "Mathew", LastName = "KJV", Age = "40" },
            new Person{ FirstName = "Mark", LastName = "KJV", Age = "40" },
            new Person{ FirstName = "Luke", LastName = "KJV", Age = "40" },
            new Person{ FirstName = "John", LastName = "KJV", Age = "40" },
            new Person{ FirstName = "Acts", LastName = "KJV", Age = "40" },
            new Person{ FirstName = "Romans", LastName = "KJV", Age = "40" },
            new Person{ FirstName = "Corinthians", LastName = "KJV", Age = "40" }
        };

        var actor = FileParseBuilder
            .AsDelimited<Person>()
            .Configure(config =>
            {
                config
                .SetDelimeter(",")
                .SetSplitOptions(StringSplitOptions.TrimEntries)
                .SetProperties((0, person => person.FirstName),
                     (1, person => person.LastName),
                     (2, person => person.Age));
            })
            .Build();

        //Act
        actor.WriteFile(WriteConfigurationTestFile, people);

        //Assert
        Assert.IsTrue(File.Exists(WriteConfigurationTestFile));
        Assert.AreEqual(File.ReadLines(WriteConfigurationTestFile).Count(), 7);
    }

    [TestMethod()]
    public void WriteWithAttributeTest()
    {
        //Arrange
        var people = new List<PersonWithAttributes>
        {
            new PersonWithAttributes{ FirstName = "Mathew", LastName = "KJV", Age = "40" },
            new PersonWithAttributes{ FirstName = "Mark", LastName = "KJV", Age = "40" },
            new PersonWithAttributes{ FirstName = "Luke", LastName = "KJV", Age = "40" },
            new PersonWithAttributes{ FirstName = "John", LastName = "KJV", Age = "40" },
            new PersonWithAttributes{ FirstName = "Acts", LastName = "KJV", Age = "40" },
            new PersonWithAttributes{ FirstName = "Romans", LastName = "KJV", Age = "40" },
            new PersonWithAttributes{ FirstName = "Corinthians", LastName = "KJV", Age = "40" }
        };

        var actor = FileParseBuilder
            .AsDelimited<PersonWithAttributes>()
            .Build();

        //Act
        actor.WriteFile(WriteAttributeTestFile, people);

        //Assert
        Assert.IsTrue(File.Exists(WriteAttributeTestFile));
        Assert.AreEqual(File.ReadLines(WriteAttributeTestFile).Count(), 7);
    }
}