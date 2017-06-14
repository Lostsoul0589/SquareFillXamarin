//using NUnit.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;

namespace SquareFillDomain.UnitTests
{
    // class SquareTests: XCTestCase
    [TestClass]
    public class SquareTests
    {
        private SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }

        private ISquareView MockSquareView()
        {
            return new MockSquareView();
        }

        private Square Square(SquareFillPoint positionRelativeToParentCorner, ISquareView sprite)
        {
            return new Square(
                positionRelativeToParentCorner: positionRelativeToParentCorner,
                sprite: sprite);
        }

        private Square Square()
        {
            return new Square();
        }

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
		public void TestCentreOfSquareIsDefinedAsInsideSquare() {
			// Arrange
			var centreOfSquare = SquareFillPoint(
				x: TestConstants.SquareWidth/2,
                y: TestConstants.SquareWidth/2);
		    var topLeftCorner = SquareFillPoint(x: 0, y: 0);
			var square = Square();
            square.MoveTopLeftCorner(newTopLeftCorner: topLeftCorner);
			
			// Act
			var isInSquare = square.IsInSquare(point: centreOfSquare);
			
			// Assert
			Asserter.AreEqual(actual: isInSquare, expected: true);
		}
		
		[TestMethod]
		public void TestAnyLocationInSquareIsDefinedAsInsideSquare() {
			// Arrange
		    var topLeftCorner = SquareFillPoint(x: 0, y: 0);
            var square = Square();
		    square.MoveTopLeftCorner(newTopLeftCorner: topLeftCorner);
            var pointInQuestion = SquareFillPoint(x: topLeftCorner.X + 10, y: topLeftCorner.Y + 11);
			
			// Act
			var isInSquare = square.IsInSquare(point: pointInQuestion);
			
			// Assert
			Asserter.AreEqual(actual: isInSquare, expected: true);
		}
		
		[TestMethod]
		public void TestAnyLocationOutsideSquareIsNotDefinedAsInsideSquare() {
			// Arrange
		    var topLeftCorner = SquareFillPoint(x: 0, y: 0);
			var square = Square();
		    square.MoveTopLeftCorner(newTopLeftCorner: topLeftCorner);
            var pointInQuestion = SquareFillPoint(x: topLeftCorner.X + 50, y: topLeftCorner.Y - 10);
			
			// Act
			var isInSquare = square.IsInSquare(point: pointInQuestion);
			
			// Assert
			Asserter.AreEqual(actual: isInSquare, expected: false);
		}

        [TestMethod]
        public void TestTopLeftCornerIsCalculatedAsParentCornerAdjustedByRelativePosition()
        {
            // Arrange
            var parentTopLeftCorner = SquareFillPoint(
                x: 4 * TestConstants.SquareWidth,
                y: 4 * TestConstants.SquareWidth);
            var square = Square(positionRelativeToParentCorner: SquareFillPoint(x: -2, y: -3), sprite: MockSquareView());

            // Act
            square.MoveTopLeftCorner(newTopLeftCorner: parentTopLeftCorner);

            // Assert
            Asserter.AreEqual(actual: square.TopLeftCornerX,
                expected: parentTopLeftCorner.X + (square.XRelativeToParentCorner * TestConstants.SquareWidth));
            Asserter.AreEqual(actual: square.TopLeftCornerY,
                expected: parentTopLeftCorner.Y + (square.YRelativeToParentCorner * TestConstants.SquareWidth));
        }

        [TestMethod]
        public void TestPotentialTopLeftCornerIsCalculatedAsParentCornerAdjustedByRelativePosition()
        {
            // Arrange
            var parentTopLeftCorner = SquareFillPoint(
                x: 4 * TestConstants.SquareWidth,
                y: 4 * TestConstants.SquareWidth);
            var square = Square(positionRelativeToParentCorner: SquareFillPoint(x: -2, y: -3), sprite: MockSquareView());

            // Act
            var result = square.CalculatePotentialTopLeftCorner(parentTopLeftCorner: parentTopLeftCorner);

            // Assert
            Asserter.AreEqual(actual: result.X, expected: parentTopLeftCorner.X + (square.XRelativeToParentCorner * TestConstants.SquareWidth));
            Asserter.AreEqual(actual: result.Y, expected: parentTopLeftCorner.Y + (square.YRelativeToParentCorner * TestConstants.SquareWidth));
        }
    }
}