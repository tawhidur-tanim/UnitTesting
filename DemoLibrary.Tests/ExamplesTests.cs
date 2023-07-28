using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DemoLibrary.Tests
{
    public class ExamplesTests
    {

        [Fact]
        public void ExampleLoadTextFile_ValidFileNameGiven()
        {
            // act
            string actual = Examples.ExampleLoadTextFile("This is valid filename.");

            // assert
            Assert.True(actual.Length >  0);
        }


        [Fact]
        public void ExampleLoadTextFile_InvalidFileNameGiven()
        {
            // act & assert

            Assert.Throws<ArgumentException>("file",() => Examples.ExampleLoadTextFile(""));


        }
    }
}
