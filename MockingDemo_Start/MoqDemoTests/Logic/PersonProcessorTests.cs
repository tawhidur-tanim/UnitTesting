﻿using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using DemoLibrary.Logic;
using DemoLibrary.Models;
using DemoLibrary.Utilities;
using FluentAssertions;
using Moq;
using Xunit;

namespace MoqDemoTests.Logic
{
    public class PersonProcessorTests
    {
        [Theory]
        [InlineData("6'8\"", true, 80)]
        [InlineData("6\"8'", false, 0)]
        [InlineData("six'eight\"", false, 0)]
        public void ConvertHeightTextToInches_VariousOptions(
            string heightText, 
            bool expectedIsValid, 
            double expectedHeightInInches)
        {
            PersonProcessor processor = new PersonProcessor(null);

            var actual = processor.ConvertHeightTextToInches(heightText);

            Assert.Equal(expectedIsValid, actual.isValid);
            Assert.Equal(expectedHeightInInches, actual.heightInInches);
        }

        [Theory]
        [InlineData("Tim", "Corey", "6'8\"", 80)]
        [InlineData("Charitry", "Corey", "5'4\"", 64)]
        public void CreatePerson_Successful(string firstName, string lastName, string heightText, double expectedHeight)
        {
            PersonProcessor processor = new PersonProcessor(null);

            PersonModel expected = new PersonModel
            {
                FirstName = firstName,
                LastName = lastName,
                HeightInInches = expectedHeight,
                Id = 0
            };

            var actual = processor.CreatePerson(firstName, lastName, heightText);

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.HeightInInches, actual.HeightInInches);

        }

        [Theory]
        [InlineData("Tim#", "Corey", "6'8\"", "firstName")]
        [InlineData("Charitry", "C88ey", "5'4\"", "lastName")]
        [InlineData("Jon", "Corey", "SixTwo", "heightText")]
        [InlineData("", "Corey", "5'11\"", "firstName")]
        public void CreatePerson_ThrowsException(string firstName, string lastName, string heightText, string expectedInvalidParameter)
        {
            PersonProcessor processor = new PersonProcessor(null);

            var ex = Record.Exception(() =>processor.CreatePerson(firstName, lastName, heightText));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            if (ex is ArgumentException argEx)
            {
                Assert.Equal(expectedInvalidParameter, argEx.ParamName);
            }

        }



        [Fact]
        public void LoadPeople_LoadsSuccessfully()
        {      
            Mock<ISqliteDataAccess> _dataMock = new Mock<ISqliteDataAccess>();

            // "select * from Person"
            _dataMock.Setup(x => x.LoadData<PersonModel>(It.Is<string>(y => y == "select * from Person")))
                .Returns(GetSamplePeople());

            var cls = new PersonProcessor(_dataMock.Object);

            var expected = GetSamplePeople();

            var actual = cls.LoadPeople();

            actual.Should().NotBeNull();
            actual.Should().HaveSameCount(expected);
            actual.Should().Equal(expected,(a, e) => a.FirstName == e.FirstName && a.LastName == e.LastName);  
            

        }



        [Fact]
        public void SavePerson_SavesSuccessfully()
        {
            using (var mock = AutoMock.GetLoose())
            {
                PersonModel person = new PersonModel()
                {
                    FirstName = "Tawhidur",
                    LastName = "Rahman"
                };

                string sql = "insert into Person (FirstName, LastName, HeightInInches) values (@FirstName, @LastName, @HeightInInches)";

                mock.Mock<ISqliteDataAccess>()
                    .Setup(x => x.SaveData(person, sql));

                var cls = mock.Create<PersonProcessor>();

                cls.SavePerson(person);

                mock.Mock<ISqliteDataAccess>()
                    .Verify(x => x.SaveData(person, sql), Times.Exactly(1));
            }

        }


        private List<PersonModel> GetSamplePeople()
        {
            List<PersonModel> output = new List<PersonModel>
            {
                new PersonModel
                {
                    FirstName = "Tim",
                    LastName = "Corey"
                },
                new PersonModel
                {
                    FirstName = "Charity",
                    LastName = "Corey"
                },
                new PersonModel
                {
                    FirstName = "Jon",
                    LastName = "Corey"
                },
                new PersonModel
                {
                    FirstName = "Chris",
                    LastName = "Corey"
                }
            };

            return output;
        }
    }
}
