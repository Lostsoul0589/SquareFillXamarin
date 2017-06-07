using System.Collections.Generic;
//using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;
using Linq = System.Collections.Generic;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeSetBuilderTests
    {
        [Test]
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