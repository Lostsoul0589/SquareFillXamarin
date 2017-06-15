using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquareFillDomain.Interfaces;
//using NUnit.Framework;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;
using Linq = System.Collections.Generic;

namespace SquareFillDomain.UnitTests
{
    // class ShapeSetTests: XCTestCase
    [TestClass]
    public class ShapeSetTests
    {
        // These all have to be initialised in XCode to keep Swift happy.
        private readonly Linq.List<Square> _singleSquareShapeSquareList1;
        private readonly Linq.List<Square> _singleSquareShapeSquareList2;
        private readonly Linq.List<Square> _rightHydrantSquareList;
        private readonly Linq.List<Square> _crossShapeSquareList;

        // override func tearDown() 
        // {
        //      // This method is called after the invocation of each test method in the class.
        //      super.tearDown();
        // }

        // override func setUp() 
        // {
        //      // This method is called before the invocation of each test method in the class.
        //      super.setUp();
        //      !! The code in the constructor has to be moved in here for Swift
        // }

        public ShapeSetTests()
        {
            // !! In Swift, all this code has to go in the setUp method. 
            _singleSquareShapeSquareList1 = new List<Square>
            {
                Square(positionRelativeToParentCorner: ShapeConstants.SingleSquarePoints[0], sprite: MockSquareView())
            };
            _singleSquareShapeSquareList2 = new List<Square>
            {
                Square(positionRelativeToParentCorner: ShapeConstants.SingleSquarePoints[0], sprite: MockSquareView())
            };
            _rightHydrantSquareList = new List<Square>
            {
                Square(positionRelativeToParentCorner: ShapeConstants.RightHydrantPoints[0], sprite: MockSquareView()),
                Square(positionRelativeToParentCorner: ShapeConstants.RightHydrantPoints[1], sprite: MockSquareView()),
                Square(positionRelativeToParentCorner: ShapeConstants.RightHydrantPoints[2], sprite: MockSquareView()),
                Square(positionRelativeToParentCorner: ShapeConstants.RightHydrantPoints[3], sprite: MockSquareView())
            };
            _crossShapeSquareList = new List<Square>
            {
                Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[0], sprite: MockSquareView()),
                Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[1], sprite: MockSquareView()),
                Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[2], sprite: MockSquareView()),
                Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[3], sprite: MockSquareView()),
                Square(positionRelativeToParentCorner: ShapeConstants.CrossShapePoints[4], sprite: MockSquareView())
            };
        }

        private ISquareView MockSquareView()
        {
            return new MockSquareView();
        }

        private SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }

        private Square Square(SquareFillPoint positionRelativeToParentCorner, ISquareView sprite)
        {
            return new Square(
                positionRelativeToParentCorner: positionRelativeToParentCorner,
                sprite: sprite);
        }

        private ShapeSet ShapeSet(List<Shape> shapes)
        {
            return new ShapeSet(shapes: shapes);
        }

        private Shape Shape(
            SquareFillPoint topLeftCorner,
            List<Square> squareDefinitions,
            bool topLeftCornerIsGridRef = true)
        {
            return new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: squareDefinitions,
                topLeftCornerIsGridRef: topLeftCornerIsGridRef);
        }

        // private func ShapeList(shape: Shape) -> [Shape]
        private List<Shape> ShapeList(Shape shape)
        {
            // return [shape];
            return new List<Shape> { shape };
        }

        // private func ShapeList(shape1: Shape, shape2: Shape) -> [Shape]
        private List<Shape> ShapeList(Shape shape1, Shape shape2)
        {
            // return [shape1, shape2];
            return new List<Shape> { shape1, shape2 };
        }

        // private func ShapeList(shape1: Shape, shape2: Shape, shape3: Shape) -> [Shape]
        private List<Shape> ShapeList(Shape shape1, Shape shape2, Shape shape3)
        {
            // return [shape1, shape2, shape3];
            return new List<Shape> { shape1, shape2, shape3 };
        }

        [TestMethod]
        public void TestAllShapesArePutIntoGridSquares()
        {
            // Arrange
            var topLeftSingleSquare = SquareFillPoint(x: 0, y: 1);
            var topLeftRightHydrant = SquareFillPoint(x: 1, y: 0);
            var topLeftCross = SquareFillPoint(x: 5, y: 5);
            var singleSquare = Shape(
                topLeftCorner: topLeftSingleSquare,
                squareDefinitions: _singleSquareShapeSquareList1);
            var rightHydrant = Shape(
                topLeftCorner: topLeftRightHydrant,
                squareDefinitions: _rightHydrantSquareList);
            var cross = Shape(
                topLeftCorner: topLeftCross,
                squareDefinitions: _crossShapeSquareList);
            var squaresInShapes = new List<List<Square>>
            {
                _singleSquareShapeSquareList1,
                _rightHydrantSquareList,
                _crossShapeSquareList
            };
            var shapeList = ShapeList(shape1: singleSquare, shape2: rightHydrant, shape3: cross);
            var shapeSet = ShapeSet(shapes: shapeList);
            var shapeSetBuilder = new TestShapeSetBuilder(squareViewFactory: new MockSquareFactory());
            var occupiedGridSquares = shapeSetBuilder.MakeGridSquares();

            // Act
            shapeSet.OccupyGridSquares(occupiedGridSquares: occupiedGridSquares);

            // Assert
            var start1 = 0;
            var end1 = shapeList.Count - 1;
            for (int count1 = start1; count1 <= end1; count1++) {
                var start2 = 0;
                var end2 = squaresInShapes[count1].Count - 1;
                for (int count2 = start2; count2 <= end2; count2++) {
                    var square = squaresInShapes[count1][count2];
                    var xCoord = square.TopLeftCornerX / TestConstants.SquareWidth;
                    var yCoord = square.TopLeftCornerY / TestConstants.SquareWidth;
                    Asserter.AreEqual(actual: occupiedGridSquares.IsSquareOccupied(x: xCoord, y: yCoord), expected: true);
                }
            }
            Asserter.AreEqual(actual: occupiedGridSquares.IsSquareOccupied(x: 0, y: 0), expected: false);
        }

        [TestMethod]
		public void TestWhenUserClicksInAreaOfScreenWithNoShapeThenNoShapeIsSelected() {
			// Arrange
            var topLeftCorner = SquareFillPoint(x: 0, y: 0);
            var singleSquareShape = Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _singleSquareShapeSquareList1);
            var shapeList = ShapeList(shape: singleSquareShape);
            var shapeSet = ShapeSet(shapes: shapeList);
			var selectedPoint = SquareFillPoint(x:TestConstants.SquareWidth*3 + 10, y:TestConstants.SquareWidth*3 + 10);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
            var noShapeSelected = (selectedShape == null);
            Asserter.AreEqual(actual: noShapeSelected, expected: true);
		}
		
		[TestMethod]
		public void TestWhenUserClicksInAreaOfScreenWithSingleSquareShapeThenShapeIsSelected()
		{
			// Arrange
			var cornerOfShape = SquareFillPoint(
				x: 0, 
				y: 0);
            var topLeftCorner = SquareFillPoint(x: 0, y: 0);
            var singleSquareShape = Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _singleSquareShapeSquareList1);
            var shapeList = ShapeList(shape: singleSquareShape);
            var shapeSet = ShapeSet(shapes: shapeList);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: cornerOfShape);
			
			// Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: singleSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: singleSquareShape.TopLeftCornerY);
		}
		
		[TestMethod]
		public void TestWhenTwoShapesExistThatUserCanSelectTheCorrectShape()
		{
			// Arrange
            var topLeftFirstShape = SquareFillPoint(x: 0, y: 0);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X + 1,
                y: topLeftFirstShape.Y + 1);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
			var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList(shape1: firstSquareShape, shape2: secondSquareShape);
			var shapeSet = ShapeSet(shapes: shapeList);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: topLeftSecondShape, selectedPointIsGridRef: true);
			
			// Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenTopLeftCornerWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 1, y: 0);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X - 1,
                y: topLeftFirstShape.Y);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList(shape1: firstSquareShape, shape2: secondSquareShape);
            var shapeSet = ShapeSet(shapes: shapeList);

            // Act
            // The top left corner of the first shape is actually on the border between the two shapes.
            // But still it should be defined is inside the first shape and not inside the second shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: topLeftFirstShape, selectedPointIsGridRef: true);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: firstSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: firstSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenBottomLeftCornerWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 1, y: 0);
            var bottomLeftFirstShape = SquareFillPoint(x: ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X,
                y: topLeftFirstShape.Y + 1);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList(shape1: secondSquareShape, shape2: firstSquareShape);
            var shapeSet = ShapeSet(shapes: shapeList);

            // Act
            // The bottom left corner of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: bottomLeftFirstShape);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenLeftEdgeWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 1, y: 0);
            var leftEdgeFirstShape = SquareFillPoint(x: ShapeConstants.SquareWidth, y: ShapeConstants.SquareWidth / 2);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X - 1,
                y: topLeftFirstShape.Y);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList(shape1: firstSquareShape, shape2: secondSquareShape);
            var shapeSet = ShapeSet(shapes: shapeList);

            // Act
            // The left edge of the first shape is actually on the border between the two shapes.
            // But still it should be defined is inside the first shape and not inside the second shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: leftEdgeFirstShape);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: firstSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: firstSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenTopRightCornerWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 1, y: 0);
            var topRightFirstShape = SquareFillPoint(x: ShapeConstants.SquareWidth * 2, y: 0);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X + 1,
                y: topLeftFirstShape.Y);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList(shape1: secondSquareShape, shape2: firstSquareShape);
            var shapeSet = ShapeSet(shapes: shapeList);

            // Act
            // The top right corner of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: topRightFirstShape);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenTopEdgeWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 0, y: 1);
            var topEdgeFirstShape = SquareFillPoint(x: ShapeConstants.SquareWidth / 2, y: ShapeConstants.SquareWidth);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X,
                y: topLeftFirstShape.Y - 1);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList( shape1: firstSquareShape, shape2: secondSquareShape );
            var shapeSet = ShapeSet(shapes: shapeList);

            // Act
            // The top edge of the first shape is actually on the border between the two shapes.
            // But still it should be defined is inside the first shape and not inside the second shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: topEdgeFirstShape);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: firstSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: firstSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenBottomRightCornerWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 1, y: 0);
            var bottomRightFirstShape = SquareFillPoint(x: ShapeConstants.SquareWidth * 2, y: ShapeConstants.SquareWidth);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X + 1,
                y: topLeftFirstShape.Y + 1);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList(shape1: secondSquareShape, shape2: firstSquareShape);
            var shapeSet = ShapeSet(shapes: shapeList);

            // Act
            // The bottom right corner of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: bottomRightFirstShape);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenRightEdgeWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 1, y: 0);
            var rightEdgeFirstShape = SquareFillPoint(x: ShapeConstants.SquareWidth * 2, y: ShapeConstants.SquareWidth / 2);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X + 1,
                y: topLeftFirstShape.Y);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList( shape1: secondSquareShape, shape2: firstSquareShape );
            var shapeSet = ShapeSet(shapes: shapeList);

            // Act
            // The right edge of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: rightEdgeFirstShape);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestWhenTwoShapesAreAdjacentThenBottomEdgeWillOnlyBeConsideredToBeInsideOneOfTheShapes()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 1, y: 0);
            var bottomEdgeFirstShape = SquareFillPoint(x: ShapeConstants.SquareWidth + ShapeConstants.SquareWidth / 2, y: ShapeConstants.SquareWidth);
            var topLeftSecondShape = SquareFillPoint(
                x: topLeftFirstShape.X,
                y: topLeftFirstShape.Y + 1);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList2);
            var shapeList = ShapeList( shape1: secondSquareShape, shape2: firstSquareShape );
            var shapeSet = ShapeSet(shapes: shapeList);

            // Act
            // The bottom edge of the first shape is actually on the border between the two shapes.
            // But in fact it should be defined is inside the second shape and not inside the first shape.
            var selectedShape = shapeSet.SelectShape(selectedPoint: bottomEdgeFirstShape);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondSquareShape.TopLeftCornerY);
        }

        [TestMethod]
		public void TestWhenCursorIsNotInCentreOfSingleSquareShapeThenShapeCanStillBeSelected()
		{
			// Arrange
            var topLeftFirstShape = SquareFillPoint(x: 0, y: 0);
            var topLeftSecondShape = SquareFillPoint(x: 2, y: 2);
            var firstSquareShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
			var secondSquareShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _singleSquareShapeSquareList1);
			var shapeList = ShapeList( shape1: firstSquareShape, shape2: secondSquareShape );
			var shapeSet = ShapeSet(shapes: shapeList);
            var selectedPoint = SquareFillPoint(
                x: (topLeftSecondShape.X * ShapeConstants.SquareWidth) + 1,
                y: (topLeftSecondShape.Y * ShapeConstants.SquareWidth) + 1);

            // Act
            var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondSquareShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondSquareShape.TopLeftCornerY);
		}
		
		[TestMethod]
		public void TestWhenCursorIsInNonCentralSquareOfMultipleSquareShapeThenShapeCanStillBeSelected()
		{
			// Arrange
            var topLeftFirstShape = SquareFillPoint(x: 0, y: 1);
            var topLeftSecondShape = SquareFillPoint(x: 1, y: 0);
            var firstShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
			var secondShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _rightHydrantSquareList);
            var shapeList = ShapeList(shape1: firstShape, shape2: secondShape);
			var shapeSet = ShapeSet(shapes: shapeList);
            var selectedPoint = SquareFillPoint(
                x: (topLeftSecondShape.X * ShapeConstants.SquareWidth) + 1, 
                y: (topLeftSecondShape.Y * ShapeConstants.SquareWidth) + 1);
			
			// Act
			var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);
			
			// Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondShape.TopLeftCornerY);
		}

        [TestMethod]
        public void TestWhenCursorIsInNonCornerSquareOfMultipleSquareShapeThenShapeCanStillBeSelected()
        {
            // Arrange
            var topLeftFirstShape = SquareFillPoint(x: 0, y: 1);
            var topLeftSecondShape = SquareFillPoint(x: 1, y: 0);
            var firstShape = Shape(
                topLeftCorner: topLeftFirstShape,
                squareDefinitions: _singleSquareShapeSquareList1);
            var secondShape = Shape(
                topLeftCorner: topLeftSecondShape,
                squareDefinitions: _rightHydrantSquareList);
            var shapeList = ShapeList(shape1: firstShape, shape2: secondShape);
            var shapeSet = ShapeSet(shapes: shapeList);
            var selectedPoint = SquareFillPoint(
                x: topLeftSecondShape.X + TestConstants.SquareWidth + 10,
                y: topLeftSecondShape.Y + TestConstants.SquareWidth + 10);

            // Act
            var selectedShape = shapeSet.SelectShape(selectedPoint: selectedPoint);

            // Assert
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerX, expected: secondShape.TopLeftCornerX);
            Asserter.AreEqual(actual: selectedShape.TopLeftCornerY, expected: secondShape.TopLeftCornerY);
        }
	}
}