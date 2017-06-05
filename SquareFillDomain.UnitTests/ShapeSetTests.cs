using System.Collections.Generic;
using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;
using Linq = System.Collections.Generic;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeSetTests
    {
        private readonly Linq.List<Square> _singleSquareShapeSquareList1 = new Linq.List<Square>
            {
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null)
            };
        private readonly Linq.List<Square> _singleSquareShapeSquareList2 = new Linq.List<Square>
            {
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null)
            };
        readonly Linq.List<Square> _rightHydrantSquareList = new Linq.List<Square>
            {
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 1), sprite: null),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 2), sprite: null),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 1, y: 1), sprite: null)
            };

        [Test]
		public void TestWhenUserClicksInAreaOfScreenWithNoShapeThenNoShapeIsSelected() {
			// Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var singleSquareShape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _singleSquareShapeSquareList1);
			var shapeSet = new ShapeSet(shapes: new List<Shape>{singleSquareShape});
			var selectedPoint = new SquareFillPoint(x:TestConstants.SquareWidth*3 + 10, y:TestConstants.SquareWidth*3 + 10);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
			Asserter.AreEqual(null, selectedShape);
		}
		
		[Test]
		public void TestWhenUserClicksInAreaOfScreenWithSingleSquareShapeThenShapeIsSelected()
		{
			// Arrange
			var centreOfShape = new SquareFillPoint(
				x: TestConstants.SquareWidth/2, 
				y: TestConstants.SquareWidth/2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var singleSquareShape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _singleSquareShapeSquareList1);
			var shapeSet = new ShapeSet(shapes: new List<Shape>{singleSquareShape});
			var selectedPoint = centreOfShape;
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, singleSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, singleSquareShape.TopLeftCornerY);
		}
		
		[Test]
		public void TestWhenTwoShapesExistThatUserCanSelectTheCorrectShape()
		{
			// Arrange
			var centreOfSecondShape = new SquareFillPoint(
				x: TestConstants.SquareWidth + TestConstants.SquareWidth/2, 
				y: TestConstants.SquareWidth + TestConstants.SquareWidth/2);
            var topLeftFirstShape = new SquareFillPoint(x: 0, y: 0);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X + TestConstants.SquareWidth,
                y: topLeftFirstShape.Y + TestConstants.SquareWidth);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
			var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
			var shapeList = new List<Shape>{firstSquareShape, secondSquareShape};
			var shapeSet = new ShapeSet(shapes: shapeList);
			var selectedPoint = centreOfSecondShape;
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondSquareShape.TopLeftCornerY);
		}
		
		[Test]
		public void TestWhenCursorIsNotInCentreOfSingleSquareShapeThenShapeCanStillBeSelected()
		{
			// Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 0, y: 0);
            var topLeftSecondShape = new SquareFillPoint(
                x: TestConstants.SquareWidth*2,
                y: TestConstants.SquareWidth*2);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
			var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList1);
			var shapeList = new List<Shape> { firstSquareShape, secondSquareShape };
			var shapeSet = new ShapeSet(shapes: shapeList);
            var selectedPoint = new SquareFillPoint(x: topLeftSecondShape.X + 1, y: topLeftSecondShape.Y + 1);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondSquareShape.TopLeftCornerY);
		}
		
		[Test]
		public void TestWhenCursorIsInNonCentralSquareOfMultipleSquareShapeThenShapeCanStillBeSelected()
		{
			// Arrange
            var topLeftFirstShape = new SquareFillPoint(
                x: 0, 
                y: TestConstants.SquareWidth);
            var topLeftSecondShape = new SquareFillPoint(
                x: TestConstants.SquareWidth,
                y: 0);
            var firstShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
			var secondShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _rightHydrantSquareList);
			var shapeList = new List<Shape>{firstShape, secondShape};
			var shapeSet = new ShapeSet(shapes: shapeList);
            var selectedPoint = new SquareFillPoint(x: topLeftSecondShape.X + 1, y: topLeftSecondShape.Y + 1);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondShape.TopLeftCornerY);
		}

        [Test]
        public void TestWhenCursorIsInNonCornerSquareOfMultipleSquareShapeThenShapeCanStillBeSelected()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(
                x: 0,
                y: TestConstants.SquareWidth);
            var topLeftSecondShape = new SquareFillPoint(
                x: TestConstants.SquareWidth,
                y: 0);
            var firstShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _rightHydrantSquareList);
            var shapeList = new List<Shape> { firstShape, secondShape };
            var shapeSet = new ShapeSet(shapes: shapeList);
            var selectedPoint = new SquareFillPoint(
                x: topLeftSecondShape.X + TestConstants.SquareWidth + 10,
                y: topLeftSecondShape.Y + TestConstants.SquareWidth + 10);

            // Act
            var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondShape.TopLeftCornerY);
        }
	}
}