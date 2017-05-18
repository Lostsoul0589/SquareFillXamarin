using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class SquareTests
    {        
		[Test]
		public void TestCentreOfSquareIsDefinedAsInsideSquare() {
			// Arrange
			var centreOfSquare = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth/2);
			var square = new Square();
			square.CalculateOrigin(parentShapeCentre: centreOfSquare);
			
			// Act
			var isInSquare = square.IsInSquare(point: centreOfSquare);
			
			// Assert
			Assert.AreEqual(isInSquare, true);
		}
		
		[Test]
		public void TestAnyLocationInSquareIsDefinedAsInsideSquare() {
			// Arrange
			var centreOfSquare = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth/2);
			var square = new Square();
			square.CalculateOrigin(parentShapeCentre: centreOfSquare);
			var pointInQuestion = new SquareFillPoint(x: centreOfSquare.X + 10, y: centreOfSquare.Y - 10);
			
			// Act
			var isInSquare = square.IsInSquare(point: pointInQuestion);
			
			// Assert
			Assert.AreEqual(isInSquare, true);
		}
		
		[Test]
		public void TestAnyLocationOutsideSquareIsNotDefinedAsInsideSquare() {
			// Arrange
			var centreOfSquare = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth/2);
			var square = new Square();
			square.CalculateOrigin(parentShapeCentre: centreOfSquare);
			var pointInQuestion = new SquareFillPoint(x: centreOfSquare.X + 50, y: centreOfSquare.Y - 10);
			
			// Act
			var isInSquare = square.IsInSquare(point: pointInQuestion);
			
			// Assert
			Assert.AreEqual(isInSquare, false);
		}
		
		[Test]
		public void TestOriginIsCalculatedAsParentCentreAdjustedByRelativePositionAndSquareWidth() {
			// Arrange
			var parentShapeCentre = new SquareFillPoint(
				x: 4*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2, 
				y: 4*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
			var square = new Square();
			square.PositionRelativeToParent = new SquareFillPoint(x: -2, y: -3);
			
			// Act
			square.CalculateOrigin(parentShapeCentre: parentShapeCentre);
			
			// Assert
			Assert.AreEqual(square.Origin.X, parentShapeCentre.X
				+ (square.PositionRelativeToParent.X * ShapeSetBuilder.SquareWidth) - ShapeSetBuilder.SquareWidth/2);
			Assert.AreEqual(square.Origin.Y, parentShapeCentre.Y
				+ (square.PositionRelativeToParent.Y * ShapeSetBuilder.SquareWidth) - ShapeSetBuilder.SquareWidth/2);
		}
	}
}