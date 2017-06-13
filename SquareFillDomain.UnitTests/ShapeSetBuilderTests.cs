//using NUnit.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquareFillDomain.Builders;
using SquareFillDomain.UnitTests.TestUtils;
using Linq = System.Collections.Generic;

namespace SquareFillDomain.UnitTests
{
    [TestClass]
    public class ShapeSetBuilderTests
    {
        [TestMethod]
        public void TestOccupiedGridSquareMatrixIsCorrectSize()
        {
            // Arrange
            var shapeSetBuilder = new ShapeSetBuilder(squareViewFactory: new MockSquareFactory());

            // Act
            var gridSquares = shapeSetBuilder.MakeGridSquares();

            // Assert
            Asserter.AreEqual(gridSquares.Width, TestConstants.GridWidth);
            Asserter.AreEqual(gridSquares.Height, TestConstants.GridHeight);
        }
    }
}