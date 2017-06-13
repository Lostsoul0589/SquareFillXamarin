using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;
using Linq = System.Collections.Generic;

namespace SquareFillDomain.UnitTests
{
    [TestClass]
    public class ShapeSetTests
    {
        private readonly Linq.List<Square> _singleSquareShapeSquareList1 = new Linq.List<Square>
        {
            new Square(positionRelativeToParentCorner: ShapeConstants.SingleSquarePoints[0], sprite: null)
        };
        private readonly Linq.List<Square> _singleSquareShapeSquareList2 = new Linq.List<Square>
        {
            new Square(positionRelativeToParentCorner: ShapeConstants.SingleSquarePoints[0], sprite: null)
        };
        readonly Linq.List<Square> _rightHydrantSquareList = new Linq.List<Square>
        {
            new Square(positionRelativeToParentCorner: ShapeConstants.RightHydrantPoints[0], sprite: null),
            new Square(positionRelativeToParentCorner: ShapeConstants.RightHydrantPoints[1], sprite: null),
            new Square(positionRelativeToParentCorner: ShapeConstants.RightHydrantPoints[2], sprite: null),
            new Square(positionRelativeToParentCorner: ShapeConstants.RightHydrantPoints[3], sprite: null)
        };
        readonly Linq.List<Square> _crossShapeSquareList = new Linq.List<Square>
        {
            new Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[0], sprite: new MockSquareView()),
            new Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[1], sprite: new MockSquareView()),
            new Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[2], sprite: new MockSquareView()),
            new Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[3], sprite: new MockSquareView()),
            new Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[4], sprite: new MockSquareView())
        };

        [TestMethod]
        public void TestAllShapesArePutIntoGridSquares()
        {
            // Arrange
            var topLeftSingleSquare = new SquareFillPoint(
                x: 0,
                y: 1);
            var topLeftRightHydrant = new SquareFillPoint(
                x: 1,
                y: 0);
            var topLeftCross = new SquareFillPoint(
                x: 5,
                y: 5);
            var singleSquare = new Shape(
                topLeftCorner: topLeftSingleSquare,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var rightHydrant = new Shape(
                topLeftCorner: topLeftRightHydrant,
                squareDefinitions: _rightHydrantSquareList,
                topLeftCornerIsInPixels: false);
            var cross = new Shape(
                topLeftCorner: topLeftCross,
                squareDefinitions: _crossShapeSquareList,
                topLeftCornerIsInPixels: false);
            var thirdShape = new Shape(
                topLeftCorner: topLeftRightHydrant,
                squareDefinitions: _rightHydrantSquareList,
                topLeftCornerIsInPixels: false);
            var squaresInShapes = new List<List<Square>>
            {
                _singleSquareShapeSquareList1,
                _rightHydrantSquareList,
                _crossShapeSquareList
            };
            var shapeList = new List<Shape> { singleSquare, rightHydrant, cross };
            var shapeSet = new ShapeSet(shapes: shapeList);
            var shapeSetBuilder = new TestShapeSetBuilder(squareViewFactory: new MockSquareFactory());
            var occupiedGridSquares = shapeSetBuilder.MakeGridSquares();

            // Act
            shapeSet.OccupyGridSquares(occupiedGridSquares: occupiedGridSquares);

            // Assert
            for (int shapeCount = 0; shapeCount < shapeList.Count; shapeCount++)
            {
                for (int squareCount = 0; squareCount < squaresInShapes[shapeCount].Count; squareCount++)
                {
                    var square = squaresInShapes[shapeCount][squareCount];
                    var xCoord = square.TopLeftCornerX / TestConstants.SquareWidth;
                    var yCoord = square.TopLeftCornerY / TestConstants.SquareWidth;
                    Asserter.AreEqual(occupiedGridSquares.IsSquareOccupied(x: xCoord, y: yCoord), true);
                }
            }
            Asserter.AreEqual(occupiedGridSquares.IsSquareOccupied(x: 0, y: 0), false);
        }

        [TestMethod]
		public void TestWhenUserClicksInAreaOfScreenWithNoShapeThenNoShapeIsSelected() {
			// Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var singleSquareShape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
			var shapeSet = new ShapeSet(shapes: new List<Shape>{singleSquareShape});
			var selectedPoint = new SquareFillPoint(x:TestConstants.SquareWidth*3 + 10, y:TestConstants.SquareWidth*3 + 10);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
			Asserter.AreEqual(null, selectedShape);
		}
		
		[TestMethod]
		public void TestWhenUserClicksInAreaOfScreenWithSingleSquareShapeThenShapeIsSelected()
		{
			// Arrange
			var cornerOfShape = new SquareFillPoint(
				x: 0, 
				y: 0);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var singleSquareShape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
			var shapeSet = new ShapeSet(shapes: new List<Shape>{singleSquareShape});
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: cornerOfShape);
			
			// Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, singleSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, singleSquareShape.TopLeftCornerY);
		}
		
		[TestMethod]
		public void TestWhenTwoShapesExistThatUserCanSelectTheCorrectShape()
		{
			// Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 0, y: 0);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X + 1,
                y: topLeftFirstShape.Y + 1);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
			var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
			var shapeList = new List<Shape>{firstSquareShape, secondSquareShape};
			var shapeSet = new ShapeSet(shapes: shapeList);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: topLeftSecondShape, selectedPointIsGridRef: true);
			
			// Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenTopLeftCornerWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 1, y: 0);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X - 1,
                y: topLeftFirstShape.Y);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
            var shapeList = new List<Shape> { firstSquareShape, secondSquareShape };
            var shapeSet = new ShapeSet(shapes: shapeList);

            // Act
            // The top left corner of the first shape is actually on the border between the two shapes.
            // But still it should be defined is inside the first shape and not inside the second shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: topLeftFirstShape, selectedPointIsGridRef: true);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, firstSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, firstSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenBottomLeftCornerWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 1, y: 0);
            var bottomLeftFirstShape = new SquareFillPoint(x: ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X,
                y: topLeftFirstShape.Y + 1);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
            var shapeList = new List<Shape> { secondSquareShape, firstSquareShape };
            var shapeSet = new ShapeSet(shapes: shapeList);

            // Act
            // The bottom left corner of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: bottomLeftFirstShape);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenLeftEdgeWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 1, y: 0);
            var leftEdgeFirstShape = new SquareFillPoint(x: ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth / 2);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X - 1,
                y: topLeftFirstShape.Y);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
            var shapeList = new List<Shape> { firstSquareShape, secondSquareShape };
            var shapeSet = new ShapeSet(shapes: shapeList);

            // Act
            // The left edge of the first shape is actually on the border between the two shapes.
            // But still it should be defined is inside the first shape and not inside the second shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: leftEdgeFirstShape);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, firstSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, firstSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenTopRightCornerWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 1, y: 0);
            var topRightFirstShape = new SquareFillPoint(x: ShapeConstants.SquareWidth * 2, y: 0);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X + 1,
                y: topLeftFirstShape.Y);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
            var shapeList = new List<Shape> { secondSquareShape, firstSquareShape };
            var shapeSet = new ShapeSet(shapes: shapeList);

            // Act
            // The top right corner of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: topRightFirstShape);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenTopEdgeWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 0, y: 1);
            var topEdgeFirstShape = new SquareFillPoint(x: ShapeConstants.SquareWidth / 2, y: ShapeConstants.SquareWidth);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X,
                y: topLeftFirstShape.Y - 1);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
            var shapeList = new List<Shape> { firstSquareShape, secondSquareShape };
            var shapeSet = new ShapeSet(shapes: shapeList);

            // Act
            // The top edge of the first shape is actually on the border between the two shapes.
            // But still it should be defined is inside the first shape and not inside the second shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: topEdgeFirstShape);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, firstSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, firstSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenBottomRightCornerWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 1, y: 0);
            var bottomRightFirstShape = new SquareFillPoint(x: ShapeConstants.SquareWidth * 2, y: ShapeConstants.SquareWidth);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X + 1,
                y: topLeftFirstShape.Y + 1);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
            var shapeList = new List<Shape> { secondSquareShape, firstSquareShape };
            var shapeSet = new ShapeSet(shapes: shapeList);

            // Act
            // The bottom right corner of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: bottomRightFirstShape);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenRightEdgeWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 1, y: 0);
            var rightEdgeFirstShape = new SquareFillPoint(x: ShapeConstants.SquareWidth * 2, y: ShapeConstants.SquareWidth / 2);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X + 1,
                y: topLeftFirstShape.Y);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
            var shapeList = new List<Shape> { secondSquareShape, firstSquareShape };
            var shapeSet = new ShapeSet(shapes: shapeList);

            // Act
            // The right edge of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: rightEdgeFirstShape);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenBottomEdgeWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = new SquareFillPoint(x: 1, y: 0);
            var bottomEdgeFirstShape = new SquareFillPoint(x: ShapeConstants.SquareWidth + ShapeConstants.SquareWidth / 2, y: ShapeConstants.SquareWidth);
            var topLeftSecondShape = new SquareFillPoint(
                x: topLeftFirstShape.X,
                y: topLeftFirstShape.Y + 1);
            var firstSquareShape = new Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1,
                topLeftCornerIsInPixels: false);
            var secondSquareShape = new Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2,
                topLeftCornerIsInPixels: false);
            var shapeList = new List<Shape> { secondSquareShape, firstSquareShape };
            var shapeSet = new ShapeSet(shapes: shapeList);

            // Act
            // The bottom edge of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: bottomEdgeFirstShape);

            // Assert
            Asserter.AreEqual(selectedShape.TopLeftCornerX, secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(selectedShape.TopLeftCornerY, secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
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
		
		[TestMethod]
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

        [TestMethod]
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