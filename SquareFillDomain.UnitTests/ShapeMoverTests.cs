using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeMoverTests
    {
        private ShapeMover _shapeMover;
        private Shape _defaultSingleSquareShape;

        [SetUp]
        public void Setup()
        {
            _shapeMover = new ShapeMover();
            _defaultSingleSquareShape = new Shape(
                    centreOfShape: new SquareFillPoint(
                        x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth / 2,
                        y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth / 2),
                    topLeftCorner: new SquareFillPoint(
                        x: TestConstants.ContainingRectangle.X,
                        y: TestConstants.ContainingRectangle.Y),
                    squareDefinitions: new List<Square> { new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: new MockSquareView()) });
        }

        [Test]
        public void TestCursorPositionCanBeCalculatedAccordingToTopLeftCorner()
        {
            // Arrange
            var cursorPositionAtStart = new SquareFillPoint(
                x: _defaultSingleSquareShape.TopLeftCorner.X + 15,
                y: _defaultSingleSquareShape.TopLeftCorner.Y + 10);
            var newCursorPosition = new SquareFillPoint(
                x: cursorPositionAtStart.X + 2,
                y: cursorPositionAtStart.Y + 3);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
            _shapeMover.MoveToNewCursorPosition(newCursorPosition: newCursorPosition);

            // Act
            SquareFillPoint calculatedCursorPosition = _shapeMover.CalculateCursorPosition(topLeftCorner:_defaultSingleSquareShape.TopLeftCorner);

            // Assert
            Asserter.AreEqual(calculatedCursorPosition.X, newCursorPosition.X);
            Asserter.AreEqual(calculatedCursorPosition.Y, newCursorPosition.Y);
        }
		
		[Test]
		public void TestNewShapeCentreWillBeNewCursorPositionWhenCursorIsInCentreOfShape() {
			// Arrange
			var cursorPositionAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x: (3 * TestConstants.SquareWidth) - 10,
				y: (2 * TestConstants.SquareWidth) - 10);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.MoveToNewCursorPosition(newCursorPosition:newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestNewShapeCentreWillBeNewCursorPositionAdjustedAccordingToPositionOfCursorWithinShape() {
			// Arrange
			var originalCentreOfShape = _defaultSingleSquareShape.CentreOfShape;
			var cursorPositionAtStart = new SquareFillPoint(
				x: originalCentreOfShape.X + 10,
				y: originalCentreOfShape.Y + 15);
			int horizontalMovement = 35;
			int verticalmovement = 30;
			var newCursorPosition = new SquareFillPoint(
				x: cursorPositionAtStart.X + horizontalMovement,
				y: cursorPositionAtStart.Y + verticalmovement);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.MoveToNewCursorPosition(newCursorPosition:newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(newShapeCentre.Y, originalCentreOfShape.Y + verticalmovement);
		}
		
		[Test]
		public void TestShapeCentreIsMovedCorrectlyWhenRelativePositionIsNegative() {
			// Arrange
			var originalCentreOfShape = _defaultSingleSquareShape.CentreOfShape;
			var cursorPositionAtStart = new SquareFillPoint(
				x: originalCentreOfShape.X - 10,
				y: originalCentreOfShape.Y - 15);
			int horizontalMovement = 15;
			int verticalmovement = 25;
			var newCursorPosition = new SquareFillPoint(
				x: cursorPositionAtStart.X + horizontalMovement,
				y: cursorPositionAtStart.Y + verticalmovement);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.MoveToNewCursorPosition(newCursorPosition:newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(newShapeCentre.Y, originalCentreOfShape.Y + verticalmovement);
		}
		
		[Test]
		public void TestShapeCentreIsMovedCorrectlyWhenCursorMovementIsNegative() {
			// Arrange
			var originalCentreOfShape = _defaultSingleSquareShape.CentreOfShape;
			var cursorPositionAtStart = new SquareFillPoint(
				x: originalCentreOfShape.X + 5,
				y: originalCentreOfShape.Y + 25);
			int horizontalMovement = -15;
			int verticalmovement = -25;
			var newCursorPosition = new SquareFillPoint(
				x: cursorPositionAtStart.X + horizontalMovement,
				y: cursorPositionAtStart.Y + verticalmovement);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.MoveToNewCursorPosition(newCursorPosition:newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(newShapeCentre.Y, originalCentreOfShape.Y + verticalmovement);
		}
		
		[Test]
		public void TestShapeCentreIsMovedCorrectlyWhenRelativePositionAndCursorMovementAreNegative() {
			// Arrange
			var originalCentreOfShape = _defaultSingleSquareShape.CentreOfShape;
			var cursorPositionAtStart = new SquareFillPoint(
				x: originalCentreOfShape.X - 5,
				y: originalCentreOfShape.Y - 25);
			int horizontalMovement = -15;
			int verticalmovement = -25;
			var newCursorPosition = new SquareFillPoint(
				x: cursorPositionAtStart.X + horizontalMovement,
				y: cursorPositionAtStart.Y + verticalmovement);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.MoveToNewCursorPosition(newCursorPosition:newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(newShapeCentre.Y, originalCentreOfShape.Y + verticalmovement);
		}
		
		[Test]
		public void TestSnapToGridIfCursorDoesntMoveAndCursorIsInCentreOfShapeAndCursorInCentreOfAGridSquareThenNewShapeCentreEqualsOriginalCursorPosition(){
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(
				x: _defaultSingleSquareShape.CentreOfShape.X,
				y: _defaultSingleSquareShape.CentreOfShape.Y);
            var newCursorPosition = cursorPositionAtStart;
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
            Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestSnapToGridIfNewCursorIsInCentreOfAGridSquareAndCursorIsInCentreOfShapeThenNewShapeCentreEqualsNewCursorPosition()    {
			// Arrange
			var cursorPositionAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth + TestConstants.SquareWidth / 2,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewPositionIsInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenNoSnapOccurs(){
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(x: 100, y: 50);
			var shapeCentreRelativeToCursorPosition = new SquareFillPoint(
				x: _defaultSingleSquareShape.CentreOfShape.X - cursorPositionAtStart.X,
				y: _defaultSingleSquareShape.CentreOfShape.Y - cursorPositionAtStart.Y);
			var newCursorPosition = new SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2 - shapeCentreRelativeToCursorPosition.X,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2 - shapeCentreRelativeToCursorPosition.Y);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X + shapeCentreRelativeToCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y + shapeCentreRelativeToCursorPosition.Y);
		}

        [Test]
        public void TestIfNewShapeCentreIsInCentreOfAGridSquareThenItWillNotSnap() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewPositionIsNotInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenSnapToGridGoesToCentreOfNearestSquare(){
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(
				x: _defaultSingleSquareShape.CentreOfShape.X + 10,
				y: _defaultSingleSquareShape.CentreOfShape.Y + 10);
			var centreOfNearestSquare = new SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
			var newCursorPosition = new SquareFillPoint(
				x: centreOfNearestSquare.X + 1,
				y: centreOfNearestSquare.Y + 1);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, centreOfNearestSquare.X);
			Asserter.AreEqual(newShapeCentre.Y, centreOfNearestSquare.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsNotInCentreOfAGridSquareThenItWillSnapToCentreOfNearestSquare() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorX = TestConstants.ContainingRectangle.X + TestConstants.SquareWidth + 2;
			var newCursorY = TestConstants.ContainingRectangle.Y + 2*TestConstants.SquareWidth + 2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:newCursorY);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, TestConstants.ContainingRectangle.X
				+ TestConstants.SquareWidth + TestConstants.SquareWidth/2);
			Asserter.AreEqual(newShapeCentre.Y, TestConstants.ContainingRectangle.Y
				+ 2*TestConstants.SquareWidth + TestConstants.SquareWidth/2);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsToLeftOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X - TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsAboveContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y - TestConstants.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsToRightOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorX = TestConstants.ContainingRectangle.X
				+ TestConstants.ContainingRectangle.Width
				+ TestConstants.SquareWidth/2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:cursorAndCentreAtStart.Y);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsBelowContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorY = TestConstants.ContainingRectangle.Y
				+ TestConstants.ContainingRectangle.Height
				+ TestConstants.SquareWidth/2;
			var newCursorPosition = new SquareFillPoint(x:cursorAndCentreAtStart.X, y:newCursorY);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Asserter.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		//MARK: Shapes composed of multiple smaller squares
		
		[Test]
		public void TestIfLeftmostShapeEdgeIsLeftOfContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresLeftOfContainer = 2;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X
					+ (numSquaresLeftOfContainer * TestConstants.SquareWidth) + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X,
                y: TestConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X - (numSquaresLeftOfContainer * TestConstants.SquareWidth),
				y:cursorAndCentreAtStart.Y);
			var shapeToMove = new Shape(
				centreOfShape: cursorAndCentreAtStart,
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParent: new SquareFillPoint(x:-numSquaresLeftOfContainer, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:2, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresLeftOfContainer, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresLeftOfContainer + 2, y:0), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover();
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(shapeMover.ShapeToMove.CentreOfShape.X, newCursorPosition.X);
			Asserter.AreEqual(shapeMover.ShapeToMove.CentreOfShape.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfTopShapeEdgeIsAboveContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresAboveContainer = 3;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y
					+ (numSquaresAboveContainer * TestConstants.SquareWidth) + TestConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X,
                y: TestConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X,
				y:cursorAndCentreAtStart.Y - (numSquaresAboveContainer * TestConstants.SquareWidth));
			var shapeToMove = new Shape(
				centreOfShape: cursorAndCentreAtStart,
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:-numSquaresAboveContainer), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:2), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresAboveContainer), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresAboveContainer + 2), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover();
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(shapeMover.ShapeToMove.CentreOfShape.X, newCursorPosition.X);
			Asserter.AreEqual(shapeMover.ShapeToMove.CentreOfShape.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfRightShapeEdgeIsRightOfContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresRightOfContainer = 1;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X
					+ TestConstants.ContainingRectangle.Width
					- (numSquaresRightOfContainer * TestConstants.SquareWidth)
					- TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X,
                y: TestConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X + (numSquaresRightOfContainer * TestConstants.SquareWidth),
				y:cursorAndCentreAtStart.Y);
			var shapeToMove = new Shape(
				centreOfShape: cursorAndCentreAtStart,
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParent: new SquareFillPoint(x:numSquaresRightOfContainer, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:-2, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresRightOfContainer, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresRightOfContainer + 2, y:0), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover();
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(shapeMover.ShapeToMove.CentreOfShape.X, newCursorPosition.X);
			Asserter.AreEqual(shapeMover.ShapeToMove.CentreOfShape.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfBottomShapeEdgeIsBelowContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresBelowContainer = 2;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y
					+ TestConstants.ContainingRectangle.Height
					- (numSquaresBelowContainer * TestConstants.SquareWidth)
					- TestConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X,
                y: TestConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X,
				y:cursorAndCentreAtStart.Y + (numSquaresBelowContainer * TestConstants.SquareWidth));
			var shapeToMove = new Shape(
				centreOfShape: cursorAndCentreAtStart,
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:numSquaresBelowContainer), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:-2), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresBelowContainer), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresBelowContainer + 2), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover();
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(shapeMover.ShapeToMove.CentreOfShape.X, newCursorPosition.X);
			Asserter.AreEqual(shapeMover.ShapeToMove.CentreOfShape.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestWhenSnappingToGridShouldRecalculateSquareOrigins() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			int horizontalMovement = 2*TestConstants.SquareWidth;
			int verticalMovement = TestConstants.SquareWidth;
			var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X + horizontalMovement,
				y:cursorAndCentreAtStart.Y + verticalMovement);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			var originalOrigin = _defaultSingleSquareShape.Squares[0].TopLeftCorner;
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.Squares[0].TopLeftCorner.X, originalOrigin.X + horizontalMovement);
			Asserter.AreEqual(_shapeMover.ShapeToMove.Squares[0].TopLeftCorner.Y, originalOrigin.Y + verticalMovement);
		}
	}
}
