using DemoLibrary.Models;
using System;
using System.Collections.Generic;
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



}
