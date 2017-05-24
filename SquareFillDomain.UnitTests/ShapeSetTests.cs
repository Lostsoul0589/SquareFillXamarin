using System.Collections.Generic;
using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
using Linq = System.Collections.Generic;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeSetTests
    {
        readonly Linq.List<Square> _rightHydrantSquareList = new Linq.List<Square>
            {
                new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null),
                new Square(positionRelativeToParent: new SquareFillPoint(x: 1, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 1), sprite: null),
                new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 1), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 2), sprite: null),
                new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: -1), positionRelativeToParentCorner: new SquareFillPoint(x: 1, y: 1), sprite: null)
            };

        [Test]
		public void TestWhenUserClicksInAreaOfScreenWithNoShapeThenNoShapeIsSelected() {
			// Arrange
			var centreOfShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth/2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var singleSquareShape = new Shape(
				centreOfShape: centreOfShape,
                topLeftCorner: topLeftCorner,
                squareDefinitions: new List<Square>{new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)});
			var shapeSet = new ShapeSet(shapes: new List<Shape>{singleSquareShape});
			var selectedPoint = new SquareFillPoint(x:ShapeSetBuilder.SquareWidth*3 + 10, y:ShapeSetBuilder.SquareWidth*3 + 10);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
			Assert.AreEqual(null, selectedShape);
		}
		
		[Test]
		public void TestWhenUserClicksInAreaOfScreenWithSingleSquareShapeThenShapeIsSelected()
		{
			// Arrange
			var centreOfShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth/2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var singleSquareShape = new Shape(
				centreOfShape: centreOfShape,
                topLeftCorner: topLeftCorner,
                squareDefinitions: new List<Square>{new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)});
			var shapeSet = new ShapeSet(shapes: new List<Shape>{singleSquareShape});
			var selectedPoint = centreOfShape;
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
			Assert.AreEqual(selectedShape.CentreOfShape.X, singleSquareShape.CentreOfShape.X);
			Assert.AreEqual(selectedShape.CentreOfShape.Y, singleSquareShape.CentreOfShape.Y);
		}
		
		[Test]
		public void TestWhenTwoShapesExistThatUserCanSelectTheCorrectShape()
		{
			// Arrange
			var centreOfFirstShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth/2);
			var centreOfSecondShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var topLeftFirstShape = new SquareFillPoint(x: 0, y: 0);
            var topLeftSecondShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.SquareWidth);
            var firstSquareShape = new Shape(
				centreOfShape: centreOfFirstShape,
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: new List<Square>{new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)});
			var secondSquareShape = new Shape(
				centreOfShape: centreOfSecondShape,
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: new List<Square>{new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)});
			var shapeList = new List<Shape>{firstSquareShape, secondSquareShape};
			var shapeSet = new ShapeSet(shapes: shapeList);
			var selectedPoint = centreOfSecondShape;
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
			Assert.AreEqual(selectedShape.CentreOfShape.X, secondSquareShape.CentreOfShape.X);
			Assert.AreEqual(selectedShape.CentreOfShape.Y, secondSquareShape.CentreOfShape.Y);
		}
		
		[Test]
		public void TestWhenCursorIsNotInCentreOfSingleSquareShapeThenShapeCanStillBeSelected()
		{
			// Arrange
			var centreOfFirstShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth/2);
            var centreOfSecondShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth*2 + ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth*2 + ShapeSetBuilder.SquareWidth/2);
            var topLeftFirstShape = new SquareFillPoint(x: 0, y: 0);
            var topLeftSecondShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.SquareWidth);
            var firstSquareShape = new Shape(
				centreOfShape: centreOfFirstShape,
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: new List<Square>{new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)});
			var secondSquareShape = new Shape(
				centreOfShape: centreOfSecondShape,
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: new List<Square>{new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)});
			var shapeList = new List<Shape> { firstSquareShape, secondSquareShape };
			var shapeSet = new ShapeSet(shapes: shapeList);
			var selectedPoint = new SquareFillPoint(x: centreOfSecondShape.X + 10, y: centreOfSecondShape.Y + 10);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
			Assert.AreEqual(selectedShape.CentreOfShape.X, secondSquareShape.CentreOfShape.X);
			Assert.AreEqual(selectedShape.CentreOfShape.Y, secondSquareShape.CentreOfShape.Y);
		}
		
		[Test]
		public void TestWhenCursorIsInNonCentralSquareOfMultipleSquareShapeThenShapeCanStillBeSelected()
		{
			// Arrange
			var centreOfFirstShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
			var centreOfSecondShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var topLeftFirstShape = new SquareFillPoint(x: 0, y: 0);
            var topLeftSecondShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.SquareWidth);
            var firstSquareShape = new Shape(
				centreOfShape: centreOfFirstShape,
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: new List<Square>{new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)});
			var secondSquareShape = new Shape(
				centreOfShape: centreOfSecondShape,
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _rightHydrantSquareList);
			var shapeList = new List<Shape>{firstSquareShape, secondSquareShape};
			var shapeSet = new ShapeSet(shapes: shapeList);
			var selectedPoint = new SquareFillPoint(x: centreOfSecondShape.X + ShapeSetBuilder.SquareWidth + 10, y: centreOfSecondShape.Y + 10);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
			Assert.AreEqual(selectedShape.CentreOfShape.X, secondSquareShape.CentreOfShape.X);
			Assert.AreEqual(selectedShape.CentreOfShape.Y, secondSquareShape.CentreOfShape.Y);
		}
	}
}