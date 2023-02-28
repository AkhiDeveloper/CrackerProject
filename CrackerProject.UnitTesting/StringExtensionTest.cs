using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrackerProject.Library.Extractor;

namespace CrackerProject.UnitTesting
{
    public class StringExtensionTest
    {
        [Fact]
        public void Check_StringSymbolTrim()
        {
            //Arrange
            var inputtxt = "@@%^&& rap gjjs @@@*(";
            var expectedtxt = "rap gjjs";

            //Act
            var output = inputtxt.TrimSymbol();

            //Assert
            Assert.Equal(expectedtxt, output);
        }

        [Fact]
        public void Check_StringSymbolTrimStart()
        {
            //Arrange
            var inputtxt = "@@%^&& rap gjjs @@@*(";
            var expectedtxt = "rap gjjs @@@*(";

            //Act
            var output = inputtxt.TrimSymbolStart();

            //Assert
            Assert.Equal(expectedtxt, output);
        }

        [Fact]
        public void Check_StringSymbolandNumberTrimStart()
        {
            //Arrange
            var inputtxt = "123rap gjjs @@@*(";
            var expectedtxt = "rap gjjs @@@*(";

            //Act
            var output = inputtxt.TrimNumberStart();

            //Assert
            Assert.Equal(expectedtxt, output);
        }

        [Fact]
        public void Check_GetStartNumberTrimStart()
        {
            //Arrange
            var inputtxt = "123 + rap gjjs @@@*(";
            var expectedtxt = "123";

            //Act
            var output = inputtxt.GetStartNumber();

            //Assert
            Assert.Equal(expectedtxt, output);
        }

        [Fact]
        public void Check_IsStartWithNumber()
        {
            //Arrange
            var inputtxt = "12345tyrt + rap gjjs @@@*(";
            var expectedtxt = true;

            //Act
            var output = inputtxt.IsStartWithNumber();

            //Assert
            Assert.Equal(expectedtxt, output);
        }

        [Fact]
        public void Check_IsNotStartWithNumber()
        {
            //Arrange
            var inputtxt = "tyrt + rap gjjs @@@*(";
            var expectedtxt = false;

            //Act
            var output = inputtxt.IsStartWithNumber();

            //Assert
            Assert.Equal(expectedtxt, output);
        }
    }
}
