using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextFieldParserFramework;
using TextFieldParserFramework.FixedWidth;
using TextFieldParserFrameworkTests.Models;

namespace TextFieldParserFrameworkTests.FixedWidth
{
    [TestClass()]
    public class FixWidthTests
    {
        private const string WriteAttributeTestFile = "FixedWidthWriteAttributeTest.txt";
        private const string WriteConfigurationTestFile = "FixedWidthWriteConfigurationTest.txt";
        private const string ReadTestFile = "FixedWidthReadTest.txt";

        [TestMethod()]
        public void ReadWithAttributesTest()
        {
            //Arrange
            var actor = Parse
                .AsFixedWidth<PersonWithAttributes>()
                .Build();

            //Act
            var actual = actor.ReadFile(ReadTestFile);

            //Assert
            Assert.AreEqual(actual.Count(), 7);
        }

        [TestMethod()]
        public void ReadComplexTypeTest()
        {
            //Arrange
            var actor = Parse
                .AsFixedWidth<PersonWithAttributes>()
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
            var actor = Parse
                .AsFixedWidth<Person>()
                .Configure(config =>
                {
                    config
                    .SetProperties(
                        new PropertyRange<Person>(new Range(1, 50), person => person.FirstName),
                        new PropertyRange<Person>(new Range(51, 50), person => person.LastName),
                        new PropertyRange<Person>(new Range(101, 3), person => person.Age));
                })
                .Build();

            //Act
            var actual = actor.ReadFile(ReadTestFile);

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

            var actor = Parse
                .AsFixedWidth<Person>()
                .Configure(config =>
                {
                    config
                    .SetProperties(
                        new PropertyRange<Person>(new Range(1, 50), person => person.FirstName),
                        new PropertyRange<Person>(new Range(51, 50), person => person.LastName),
                        new PropertyRange<Person>(new Range(101, 3), person => person.Age));
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

            var actor = Parse
                .AsFixedWidth<PersonWithAttributes>()
                .Build();

            //Act
            actor.WriteFile(WriteAttributeTestFile, people);

            //Assert
            Assert.IsTrue(File.Exists(WriteAttributeTestFile));
            Assert.AreEqual(File.ReadLines(WriteAttributeTestFile).Count(), 7);
        }
    }
}