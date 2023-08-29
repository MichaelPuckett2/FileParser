using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextFieldParserFramework;
using TextFieldParserFramework.FixedWidth;
using TextFieldParserFrameworkTests.Models;

namespace TextFieldParserFrameworkTests.Delimiter
{
    [TestClass()]
    public class DelimiterTests
    {
        private const string WriteAttributeTestFile = "DelimiterWriteAttributeTest.txt";
        private const string WriteConfigurationTestFile = "DelimiterWriteConfigurationTest.txt";
        private const string ReadTestFile = "DelimitedReadTest.txt";
        private const string ReadSchemaTestFile = "PersonSchema.txt";
        private const string ReadTestFile_FixedWidth = "FixedWidthReadTest.txt";

        [TestMethod()]
        public void ReadWithAttributesTest()
        {
            //Arrange
            var actor = Parse
                .AsDelimited<PersonWithAttributes>()
                .BuildFileParser();

            //Act
            var actual = actor.ReadFile(ReadTestFile);

            //Assert
            Assert.AreEqual(actual.Count(), 7);
        }

        [TestMethod()]
        public void ReadWithConfigurationTest()
        {
            //Arrange
            var fileParser = Parse
                            .AsDelimited<Person>()
                            .Configure(config =>
                            {
                                config.SetDelimeter(",")
                                      .SetSplitOptions(StringSplitOptions.None)
                                      .SetProperty(0, person => person.FirstName)
                                      .SetProperty(1, person => person.LastName)
                                      .SetProperty(2, person => person.Age);
                            })
                            .BuildFileParser();

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
                new Person{ FirstName = "Mathew", LastName = "KJV", Age = 40 },
                new Person{ FirstName = "Mark", LastName = "KJV", Age = 40 },
                new Person{ FirstName = "Luke", LastName = "KJV", Age = 40 },
                new Person{ FirstName = "John", LastName = "KJV", Age = 40 },
                new Person{ FirstName = "Acts", LastName = "KJV", Age = 40 },
                new Person{ FirstName = "Romans", LastName = "KJV", Age = 40 },
                new Person{ FirstName = "Corinthians", LastName = "KJV", Age = 40 }
            };

            var actor = Parse
                .AsDelimited<Person>()
                .Configure(config =>
                {
                    config
                    .SetDelimeter(",")
                    .SetProperty(0, person => person.FirstName)
                    .SetProperty(1, person => person.LastName)
                    .SetProperty(2, person => person.Age);
                })
                .BuildFileParser();

            //Act
            actor.WriteFile(WriteConfigurationTestFile, people);

            //Assert
            Assert.IsTrue(File.Exists(WriteConfigurationTestFile));
            Assert.AreEqual(7, File.ReadLines(WriteConfigurationTestFile).Count());
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

            var actor = Parse
                .AsDelimited<PersonWithAttributes>()
                .BuildFileParser();

            //Act
            actor.WriteFile(WriteAttributeTestFile, people);

            //Assert
            Assert.IsTrue(File.Exists(WriteAttributeTestFile));
            Assert.AreEqual(File.ReadLines(WriteAttributeTestFile).Count(), 7);
        }

        [TestMethod()]
        public void ReadSchemaDelimitedForFixedWidthTest()
        {
            //Arrange
            var rangeParser = Parse.AsDelimited<Range>()
                                   .Configure(c => c.SetDelimeter("-")
                                   .SetProperty(0, x => x.Index)
                                   .SetProperty(1, x => x.Length))
                                   .BuildStringParser();

            var fileParser = Parse
                            .AsDelimited<PropertyRange>()
                            .Configure(config =>
                            {
                                config.SetDelimeter(",")
                                      .SetProperty(0, x => x.Range)
                                      .SetProperty(1, x => x.PropertyName);

                            })
                            .AddParser(() => rangeParser)
                            .BuildFileParser();

            //Act
            var actual = fileParser.ReadFile(ReadSchemaTestFile).ToList();

            //Assert
            Assert.AreEqual(3, actual.Count);

            Assert.AreEqual("FirstName", actual[0].PropertyName);
            Assert.AreEqual(1, actual[0].Range.Index);
            Assert.AreEqual(50, actual[0].Range.Length);

            Assert.AreEqual("LastName", actual[1].PropertyName);
            Assert.AreEqual(51, actual[1].Range.Index);
            Assert.AreEqual(50, actual[1].Range.Length);

            Assert.AreEqual("Age", actual[2].PropertyName);
            Assert.AreEqual(101, actual[2].Range.Index);
            Assert.AreEqual(3, actual[2].Range.Length);
        }

        [TestMethod()]
        public void ApplySchemaDelimitedForFixedWidthTest()
        {
            //Arrange
            var schema = Parse
                            .AsDelimited<PropertyRange>()
                            .Configure(config =>
                            {
                                config.SetDelimeter(",")
                                      .SetProperty(0, "Range")
                                      .SetProperty(1, "PropertyName");
                            })
                            .AddParser(() => Parse.AsDelimited<Range>()
                                   .Configure(c => c.SetDelimeter("-")
                                   .SetProperty(0, x => x.Index)
                                   .SetProperty(1, x => x.Length))
                                   .BuildStringParser())
                            .BuildFileParser().ReadFile(ReadSchemaTestFile).ToArray();

            var fileParser = Parse
                .AsFixedWidth<Person>()
                .Configure(config =>
                {
                    config.SetProperties(schema);
                })
                .BuildFileParser();

            //Act
            var actual = fileParser.ReadFile(ReadTestFile_FixedWidth).ToList();

            //Assert
            Assert.AreEqual(7, actual.Count);

            Assert.AreEqual("Mathew".PadRight(50), actual[0].FirstName);
            Assert.AreEqual("KJV".PadRight(50), actual[0].LastName);
            Assert.AreEqual(40, actual[0].Age);

            Assert.AreEqual("John".PadRight(50), actual[3].FirstName);
            Assert.AreEqual("KJV".PadRight(50), actual[3].LastName);
            Assert.AreEqual(40, actual[3].Age);

            Assert.AreEqual("Corinthians".PadRight(50), actual[6].FirstName);
            Assert.AreEqual("KJV".PadRight(50), actual[6].LastName);
            Assert.AreEqual(40, actual[6].Age);
        }
    }
}