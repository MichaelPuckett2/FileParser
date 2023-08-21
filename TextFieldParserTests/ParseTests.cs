using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextFieldParser.FixedWidth.Tests;

namespace FileParser.Tests
{
    [TestClass()]
    public class ParseTests
    {
        [TestMethod()]
        public void AsFixedWidthReadWithAttributesTest()
        {
            //Arrange
            var actor = Parse
                .AsFixedWidth<TestFixedWidthPerson>()
                .Build();

            //Act
            var actual = actor.ReadLines("FixedWidthFile.txt");

            //Assert
            Assert.AreEqual(actual.Count(), 7);
        }

        [TestMethod()]
        public void AsFixedWidthReadWithConfigTest()
        {
            //Arrange
            var actor = Parse
                .AsFixedWidth<TestFixedWidthPerson_NoAttributes>()
                .Configure(config =>
                {
                    config
                    .Set(
                        (person => person.FirstName, (1, 50)),
                        (person => person.LastName, (51, 50)),
                        (person => person.Age, (101, 3)));
                })
                .Build();

            //Act
            var actual = actor.ReadLines("FixedWidthFile.txt");

            //Assert
            Assert.AreEqual(actual.Count(), 7);
        }

        [TestMethod()]
        public void AsFixedWidthWriteTest()
        {
            //Arrange
            var people = new List<TestFixedWidthPerson_NoAttributes>
            {
                new TestFixedWidthPerson_NoAttributes{ FirstName = "Mathew", LastName = "KJV", Age = "40" },
                new TestFixedWidthPerson_NoAttributes{ FirstName = "Mark", LastName = "KJV", Age = "40" },
                new TestFixedWidthPerson_NoAttributes{ FirstName = "Luke", LastName = "KJV", Age = "40" },
                new TestFixedWidthPerson_NoAttributes{ FirstName = "John", LastName = "KJV", Age = "40" },
                new TestFixedWidthPerson_NoAttributes{ FirstName = "Acts", LastName = "KJV", Age = "40" },
                new TestFixedWidthPerson_NoAttributes{ FirstName = "Romans", LastName = "KJV", Age = "40" },
                new TestFixedWidthPerson_NoAttributes{ FirstName = "Corinthians", LastName = "KJV", Age = "40" }
            };



            var actor = Parse
                .AsFixedWidth<TestFixedWidthPerson_NoAttributes>()
                .Configure(config =>
                {
                    config
                    .Set(
                        (person => person.FirstName, (1, 50)),
                        (person => person.LastName, (51, 50)),
                        (person => person.Age, (101, 3)));
                })
                .Build();

            //Act
            actor.WriteLines("ResultPersonWidthFixed.txt", people);

            //Assert
            Assert.IsTrue(File.Exists("ResultPersonWidthFixed.txt"));
            Assert.AreEqual(File.ReadLines("ResultPersonWidthFixed.txt").Count(), 7);
        }
    }
}