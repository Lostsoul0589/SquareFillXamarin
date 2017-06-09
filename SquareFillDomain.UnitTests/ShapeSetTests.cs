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

        [Test]
        public void TestAllShapesArePutIntoGridSquares()
        {
            // Arrange
            var topLeftSingleSquare = new SquareFillPoint(
                x: 0,
                y: TestConstants.SquareWidth);
            var topLeftRightHydrant = new SquareFillPoint(
                x: TestConstants.SquareWidth,
                y: 0);
            var topLeftCross = new SquareFillPoint(
                x: TestConstants.SquareWidth * 5,
                y: TestConstants.SquareWidth * 5);
            var singleSquare = new Shape(
                topLeftCorner: topLeftSingleSquare,
                squareDefinitions: _singleSquareShapeSquareList1);
            var rightHydrant = new Shape(
                topLeftCorner: topLeftRightHydrant,
                squareDefinitions: _rightHydrantSquareList);
            var cross = new Shape(
                topLeftCorner: topLeftCross,
                squareDefinitions: _crossShapeSquareList);
            var thirdShape = new Shape(
                topLeftCorner: topLeftRightHydrant,
                squareDefinitions: _rightHydrantSquareList);
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