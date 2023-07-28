using DemoLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DemoLibrary.Tests
{
    public class DataAccessTests
    {
        [Fact]
        public void AddPersonToPeopleList_ShouldSuccess()
        {
            // Arrange
            List<PersonModel> persons = new List<PersonModel>();

            PersonModel person = new PersonModel() { FirstName = "Tawhidur", LastName = "Rahman" };

            // Act

            DataAccess.AddPersonToPeopleList(persons, person);

            // Assert

            Assert.True(persons.Count == 1);
            Assert.Contains(person, persons);
        }


        [Theory]
        [InlineData("", "Rahman", "FirstName")]
        [InlineData("Tawhidur", "", "LastName")]
        public void AddPersonToPeopleList_ShouldFailOnValidation(string firstName, string lastName, string param)
        {
            // Arrange
            List<PersonModel> persons = new List<PersonModel>();

            PersonModel person = new PersonModel() { FirstName = firstName, LastName = lastName };


            // Act & Assert
            Assert.Throws<ArgumentException>(param, () => DataAccess.AddPersonToPeopleList(persons, person));
            Assert.True(persons.Count == 0);
        }

        [Fact]
        public void ConvertModelsToCSV_ReturnsCorrectCSV()
        {
            // Arrange
            var people = new List<PersonModel>
            {
                new PersonModel { FirstName = "John", LastName = "Doe" },
                new PersonModel { FirstName = "Jane", LastName = "Doe" }
            };

            var expectedOutput = new List<string>
            {
                "John,Doe",
                "Jane,Doe"
            };

            // Act
            var actualOutput = DataAccess.ConvertModelsToCSV(people);

            // Assert
            Assert.Equal(expectedOutput, actualOutput);
        }

        [Fact]
        public void ConvertModelsToCSV_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var people = new List<PersonModel>();
            var expectedOutput = new List<string>();


            // Act
            var actualOutput = DataAccess.ConvertModelsToCSV(people);

            // Assert
            Assert.Equal(expectedOutput, actualOutput);
        }



    }

    public class GetAllPeopleTests : IDisposable
    {
        private readonly string _tempFile;
        private readonly List<PersonModel> _expectedPeople;

        public GetAllPeopleTests()
        {
            // Create a temporary file and write some test data into it
            _tempFile = Path.GetTempFileName();

            _expectedPeople = new List<PersonModel>
            {
                new PersonModel { FirstName = "John", LastName = "Doe" },
                new PersonModel { FirstName = "Jane", LastName = "Doe" }
            };

            var lines = new List<string>();
            foreach (var person in _expectedPeople)
            {
                lines.Add($"{person.FirstName},{person.LastName}");
            }
            File.WriteAllLines(_tempFile, lines);
        }

        [Fact]
        public void GetAllPeople_ReturnsCorrectPeople()
        {
            // Act
            var actualPeople = DataAccess.GetAllPeople(_tempFile);

            // Assert
            Assert.Equal(_expectedPeople, actualPeople, new PersonModelComparer());
        }

        public void Dispose()
        {
            // Delete the temporary file
            File.Delete(_tempFile);
        }
    }

    public class PersonModelComparer : IEqualityComparer<PersonModel>
    {
        public bool Equals(PersonModel x, PersonModel y)
        {
            if (x == null || y == null)
                return false;

            return x.FirstName == y.FirstName && x.LastName == y.LastName;
        }

        public int GetHashCode(PersonModel obj)
        {
            return obj.FirstName.GetHashCode() ^ obj.LastName.GetHashCode();
        }
    }

}
