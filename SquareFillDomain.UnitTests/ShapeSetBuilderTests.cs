//using NUnit.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquareFillDomain.Builders;
using SquareFillDomain.UnitTests.TestUtils;
using Linq = System.Collections.Generic;

namespace SquareFillDomain.UnitTests
{
    // class ShapeSetBuilderTests: XCTestCase
    [TestClass]
    public class ShapeSetBuilderTests
    {
        // override func tearDown() 
        // {
        //      // This method is called after the invocation of each test method in the class.
        //      super.tearDown();
        // }

        // override func setUp() 
        // {
        //      // This method is called before the invocation of each test method in the class.
        //      super.setUp();
        // }

        [TestMethod]
        public void TestOccupiedGridSquareMatrixIsCorrectSize()
        {
            // Arrange
            var shapeSetBuilder = new ShapeSetBuilder(squareViewFactory: new MockSquareFactory());

            // Act
            var gridSquares = shapeSetBuilder.MakeGridSquares();

            // Assert
            Asserter.AreEqual(actual: gridSquares.Width, expected: TestConstants.GridWidth);
            Asserter.AreEqual(actual: gridSquares.Height, expected: TestConstants.GridHeight);
        }
    }
}