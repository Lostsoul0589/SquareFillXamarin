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
        private SquareFillPoint _centreOfDefaultSingleSquare;

        [SetUp]
        public void Setup()
        {
            _shapeMover = new ShapeMover();

            _defaultSingleSquareShape = new Shape(
                    topLeftCorner: new SquareFillPoint(
                        x: TestConstants.ContainingRectangle.X,
                        y: TestConstants.ContainingRectangle.Y),
                    squareDefinitions: new List<Square>
                    {
                        new Square(
                            positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), 
                            sprite: new MockSquareView())
                    });

            _centreOfDefaultSingleSquare = new SquareFillPoint(
                x: _defaultSingleSquareShape.TopLeftCorner.X + TestConstants.SquareWidth / 2,
                y: _defaultSingleSquareShape.TopLeftCorner.Y + TestConstants.SquareWidth / 2);
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
            SquareFillPoint calculatedCursorPosition = _shapeMover.CalculateCursorPosition();

            // Assert
            Asserter.AreEqual(calculatedCursorPosition.X, newCursorPosition.X);
            Asserter.AreEqual(calculatedCursorPosition.Y, newCursorPosition.Y);
        }
		
		[Test]
		public void TestNewShapeCentreWillBeNewCursorPositionWhenCursorIsInCentreOfShape() {
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(
                x: _defaultSingleSquareShape.TopLeftCorner.X + TestConstants.SquareWidth/2,
                y: _defaultSingleSquareShape.TopLeftCorner.Y + TestConstants.SquareWidth / 2);
			var newCursorPosition = new SquareFillPoint(
				x: (3 * TestConstants.SquareWidth) - 10,
				y: (2 * TestConstants.SquareWidth) - 10);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.MoveToNewCursorPosition(newCursorPosition:newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y);
		}
		
		[Test]
		public void TestNewShapeCentreWillBeNewCursorPositionAdjustedAccordingToPositionOfCursorWithinShape() {
			// Arrange
            var originalCentreOfShape = _centreOfDefaultSingleSquare;
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
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), originalCentreOfShape.Y + verticalmovement);
		}
		
		[Test]
		public void TestShapeCentreIsMovedCorrectlyWhenRelativePositionIsNegative() {
			// Arrange
		    var originalCentreOfShape = _centreOfDefaultSingleSquare;
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
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), originalCentreOfShape.Y + verticalmovement);
		}
		
		[Test]
		public void TestShapeCentreIsMovedCorrectlyWhenCursorMovementIsNegative() {
			// Arrange
			var originalCentreOfShape = _centreOfDefaultSingleSquare;
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
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), originalCentreOfShape.Y + verticalmovement);
		}
		
		[Test]
		public void TestShapeCentreIsMovedCorrectlyWhenRelativePositionAndCursorMovementAreNegative() {
			// Arrange
			var originalCentreOfShape = _centreOfDefaultSingleSquare;
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
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), originalCentreOfShape.Y + verticalmovement);
		}
		
		[Test]
		public void TestSnapToGridIfCursorDoesntMoveAndCursorIsInCentreOfShapeAndCursorInCentreOfAGridSquareThenNewShapeCentreEqualsOriginalCursorPosition(){
			// Arrange
            var cursorPositionAtStart = _centreOfDefaultSingleSquare;
            var newCursorPosition = cursorPositionAtStart;
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y);
		}
		
		[Test]
		public void TestSnapToGridIfNewCursorIsInCentreOfAGridSquareAndCursorIsInCentreOfShapeThenNewShapeCentreEqualsNewCursorPosition()    {
			// Arrange
			var cursorPositionAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth + TestConstants.SquareWidth / 2,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewPositionIsInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenNoSnapOccurs(){
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(x: 100, y: 50);
			var shapeCentreRelativeToCursorPosition = new SquareFillPoint(
				x: _centreOfDefaultSingleSquare.X - cursorPositionAtStart.X,
				y: _centreOfDefaultSingleSquare.Y - cursorPositionAtStart.Y);
			var newCursorPosition = new SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2 - shapeCentreRelativeToCursorPosition.X,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2 - shapeCentreRelativeToCursorPosition.Y);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X + shapeCentreRelativeToCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y + shapeCentreRelativeToCursorPosition.Y);
		}

        [Test]
        public void TestIfNewShapeCentreIsInCentreOfAGridSquareThenItWillNotSnap() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewPositionIsNotInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenSnapToGridGoesToCentreOfNearestSquare(){
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(
				x: _centreOfDefaultSingleSquare.X + 10,
				y: _centreOfDefaultSingleSquare.Y + 10);
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
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), centreOfNearestSquare.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), centreOfNearestSquare.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsNotInCentreOfAGridSquareThenItWillSnapToCentreOfNearestSquare() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorX = TestConstants.ContainingRectangle.X + TestConstants.SquareWidth + 2;
			var newCursorY = TestConstants.ContainingRectangle.Y + 2*TestConstants.SquareWidth + 2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:newCursorY);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), TestConstants.ContainingRectangle.X
				+ TestConstants.SquareWidth + TestConstants.SquareWidth/2);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), TestConstants.ContainingRectangle.Y
				+ 2*TestConstants.SquareWidth + TestConstants.SquareWidth/2);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsToLeftOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X - TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsAboveContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y - TestConstants.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsToRightOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorX = TestConstants.ContainingRectangle.X
				+ TestConstants.ContainingRectangle.Width
				+ TestConstants.SquareWidth/2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:cursorAndCentreAtStart.Y);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsBelowContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorY = TestConstants.ContainingRectangle.Y
				+ TestConstants.ContainingRectangle.Height
				+ TestConstants.SquareWidth/2;
			var newCursorPosition = new SquareFillPoint(x:cursorAndCentreAtStart.X, y:newCursorY);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreX(squareIndex: 0), newCursorPosition.X);
			Asserter.AreEqual(_shapeMover.ShapeToMove.SquareCentreY(squareIndex: 0), newCursorPosition.Y);
		}
		
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
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresLeftOfContainer, y:0), sprite: new MockSquareView()),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresLeftOfContainer*2, y:0), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover();
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
            Asserter.AreEqual(shapeMover.ShapeToMove.TopLeftCorner.X, topLeftAtStart.X - (numSquaresLeftOfContainer * TestConstants.SquareWidth));
            Asserter.AreEqual(shapeMover.ShapeToMove.TopLeftCorner.Y, topLeftAtStart.Y);
		}
		
		[Test]
		public void TestIfTopShapeEdgeIsAboveContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresAboveContainer = 2;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y
					+ (numSquaresAboveContainer * TestConstants.SquareWidth) 
                    + TestConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X,
                y: TestConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X,
				y:cursorAndCentreAtStart.Y - (numSquaresAboveContainer * TestConstants.SquareWidth));
			var shapeToMove = new Shape(
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresAboveContainer), sprite: new MockSquareView()),
					new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresAboveContainer*2), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover();
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            Asserter.AreEqual(shapeMover.ShapeToMove.TopLeftCorner.X, topLeftAtStart.X);
            Asserter.AreEqual(shapeMover.ShapeToMove.TopLeftCorner.Y, topLeftAtStart.Y - (numSquaresAboveContainer * TestConstants.SquareWidth));
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
                x: TestConstants.ContainingRectangle.X
                   + TestConstants.ContainingRectangle.Width
                   - (numSquaresRightOfContainer * TestConstants.SquareWidth)
                   - (numSquaresRightOfContainer * TestConstants.SquareWidth)
                   - TestConstants.SquareWidth,
                y: TestConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X + (numSquaresRightOfContainer * TestConstants.SquareWidth),
				y:cursorAndCentreAtStart.Y);
			var shapeToMove = new Shape(
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresRightOfContainer, y:0), sprite: new MockSquareView()),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresRightOfContainer*2, y:0), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover();
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            Asserter.AreEqual(shapeMover.ShapeToMove.TopLeftCorner.X, topLeftAtStart.X + (numSquaresRightOfContainer * TestConstants.SquareWidth));
            Asserter.AreEqual(shapeMover.ShapeToMove.TopLeftCorner.Y, topLeftAtStart.Y);
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
                y: TestConstants.ContainingRectangle.Y
                   + TestConstants.ContainingRectangle.Height
                   - (numSquaresBelowContainer * TestConstants.SquareWidth)
                   - (numSquaresBelowContainer * TestConstants.SquareWidth)
                   - TestConstants.SquareWidth);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X,
				y:cursorAndCentreAtStart.Y + (numSquaresBelowContainer * TestConstants.SquareWidth));
			var shapeToMove = new Shape(
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresBelowContainer), sprite: new MockSquareView()),
					new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresBelowContainer + 2), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover();
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            Asserter.AreEqual(shapeMover.ShapeToMove.TopLeftCorner.X, topLeftAtStart.X);
            Asserter.AreEqual(shapeMover.ShapeToMove.TopLeftCorner.Y, topLeftAtStart.Y + (numSquaresBelowContainer * TestConstants.SquareWidth));
		}
		
		[Test]
		public void TestWhenSnappingToGridShouldRecalculateSquareOrigins() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			int horizontalMovement = 2*TestConstants.SquareWidth;
			int verticalMovement = TestConstants.SquareWidth;
			var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X + horizontalMovement,
				y:cursorAndCentreAtStart.Y + verticalMovement);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			var originalOrigin = new SquareFillPoint(
                x: _defaultSingleSquareShape.TopLeftCorner.X,
                y: _defaultSingleSquareShape.TopLeftCorner.Y);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeMover.ShapeToMove.TopLeftCorner.X, originalOrigin.X + horizontalMovement);
			Asserter.AreEqual(_shapeMover.ShapeToMove.TopLeftCorner.Y, originalOrigin.Y + verticalMovement);
		}
	}
}
