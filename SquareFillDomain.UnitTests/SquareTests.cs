using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class SquareTests
    {        
		[Test]
		public void TestCentreOfSquareIsDefinedAsInsideSquare() {
			// Arrange
			var centreOfSquare = new SquareFillPoint(
				x: TestConstants.SquareWidth/2, 
				y: TestConstants.SquareWidth/2);
			var square = new Square();
			square.CalculateOrigin(parentShapeCentre: centreOfSquare);
			
			// Act
			var isInSquare = square.IsInSquare(point: centreOfSquare);
			
			// Assert
			Asserter.AreEqual(isInSquare, true);
		}
		
		[Test]
		public void TestAnyLocationInSquareIsDefinedAsInsideSquare() {
			// Arrange
			var centreOfSquare = new SquareFillPoint(
				x: TestConstants.SquareWidth/2, 
				y: TestConstants.SquareWidth/2);
			var square = new Square();
			square.CalculateOrigin(parentShapeCentre: centreOfSquare);
			var pointInQuestion = new SquareFillPoint(x: centreOfSquare.X + 10, y: centreOfSquare.Y - 10);
			
			// Act
			var isInSquare = square.IsInSquare(point: pointInQuestion);
			
			// Assert
			Asserter.AreEqual(isInSquare, true);
		}
		
		[Test]
		public void TestAnyLocationOutsideSquareIsNotDefinedAsInsideSquare() {
			// Arrange
			var centreOfSquare = new SquareFillPoint(
				x: TestConstants.SquareWidth/2, 
				y: TestConstants.SquareWidth/2);
			var square = new Square();
			square.CalculateOrigin(parentShapeCentre: centreOfSquare);
			var pointInQuestion = new SquareFillPoint(x: centreOfSquare.X + 50, y: centreOfSquare.Y - 10);
			
			// Act
			var isInSquare = square.IsInSquare(point: pointInQuestion);
			
			// Assert
			Asserter.AreEqual(isInSquare, false);
		}
		
		[Test]
		public void TestOriginIsCalculatedAsParentCentreAdjustedByRelativePositionAndSquareWidth() {
			// Arrange
			var parentShapeCentre = new SquareFillPoint(
				x: 4*TestConstants.SquareWidth + TestConstants.SquareWidth/2, 
				y: 4*TestConstants.SquareWidth + TestConstants.SquareWidth/2);
			var square = new Square();
			square.PositionRelativeToParent = new SquareFillPoint(x: -2, y: -3);
			
			// Act
			square.CalculateOrigin(parentShapeCentre: parentShapeCentre);
			
			// Assert
			Asserter.AreEqual(square.TopLeftCorner.X, parentShapeCentre.X
				+ (square.PositionRelativeToParent.X * TestConstants.SquareWidth) - TestConstants.SquareWidth/2);
			Asserter.AreEqual(square.TopLeftCorner.Y, parentShapeCentre.Y
				+ (square.PositionRelativeToParent.Y * TestConstants.SquareWidth) - TestConstants.SquareWidth/2);
		}

        [Test]
        public void TestTopLeftCornerIsCalculatedAsParentCornerAdjustedByRelativePosition()
        {
            // Arrange
            var parentTopLeftCorner = new SquareFillPoint(
                x: 4 * TestConstants.SquareWidth,
                y: 4 * TestConstants.SquareWidth);
            var square = new Square();
            square.PositionRelativeToParentCorner = new SquareFillPoint(x: -2, y: -3);

            // Act
            square.CalculateTopLeftCorner(parentTopLeftCorner: parentTopLeftCorner);

            // Assert
            Asserter.AreEqual(square.TopLeftCorner.X, parentTopLeftCorner.X
                + (square.PositionRelativeToParentCorner.X * TestConstants.SquareWidth));
            Asserter.AreEqual(square.TopLeftCorner.Y, parentTopLeftCorner.Y
                + (square.PositionRelativeToParentCorner.Y * TestConstants.SquareWidth));
        }

        [Test]
        public void TestPotentialTopLeftCornerIsCalculatedAsParentCornerAdjustedByRelativePosition()
        {
            // Arrange
            var parentTopLeftCorner = new SquareFillPoint(
                x: 4 * TestConstants.SquareWidth,
                y: 4 * TestConstants.SquareWidth);
            var square = new Square();
            square.PositionRelativeToParentCorner = new SquareFillPoint(x: -2, y: -3);

            // Act
            SquareFillPoint result = square.CalculatePotentialTopLeftCorner(parentTopLeftCorner: parentTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.X, parentTopLeftCorner.X + (square.PositionRelativeToParentCorner.X * TestConstants.SquareWidth));
            Asserter.AreEqual(result.Y, parentTopLeftCorner.Y + (square.PositionRelativeToParentCorner.Y * TestConstants.SquareWidth));
        }
	}
}