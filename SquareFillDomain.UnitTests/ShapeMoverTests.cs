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
                        x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth / 2,
                        y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth / 2),
                    topLeftCorner: new SquareFillPoint(
                        x: ShapeConstants.ContainingRectangle.X,
                        y: ShapeConstants.ContainingRectangle.Y),
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
				x: (3 * ShapeConstants.SquareWidth) - 10,
				y: (2 * ShapeConstants.SquareWidth) - 10);
			
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
				x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth / 2,
				y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth / 2);
			
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
				x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2 - shapeCentreRelativeToCursorPosition.X,
				y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth/2 - shapeCentreRelativeToCursorPosition.Y);
			
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
				x:ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
				y:ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth/2);
			
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
				x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
				y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth/2);
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
			var newCursorX = ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth + 2;
			var newCursorY = ShapeConstants.ContainingRectangle.Y + 2*ShapeConstants.SquareWidth + 2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:newCursorY);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Asserter.AreEqual(newShapeCentre.X, ShapeConstants.ContainingRectangle.X
				+ ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
			Asserter.AreEqual(newShapeCentre.Y, ShapeConstants.ContainingRectangle.Y
				+ 2*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsToLeftOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x:ShapeConstants.ContainingRectangle.X - ShapeConstants.SquareWidth/2,
				y:ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth/2);
			
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
				x:ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
				y:ShapeConstants.ContainingRectangle.Y - ShapeConstants.SquareWidth/2);
			
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
			var newCursorX = ShapeConstants.ContainingRectangle.X
				+ ShapeConstants.ContainingRectangle.Width
				+ ShapeConstants.SquareWidth/2;
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
			var newCursorY = ShapeConstants.ContainingRectangle.Y
				+ ShapeConstants.ContainingRectangle.Height
				+ ShapeConstants.SquareWidth/2;
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
				x:ShapeConstants.ContainingRectangle.X
					+ (numSquaresLeftOfContainer * ShapeConstants.SquareWidth) + ShapeConstants.SquareWidth/2,
				y:ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X,
                y: ShapeConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X - (numSquaresLeftOfContainer * ShapeConstants.SquareWidth),
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
				x:ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
				y:ShapeConstants.ContainingRectangle.Y
					+ (numSquaresAboveContainer * ShapeConstants.SquareWidth) + ShapeConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X,
                y: ShapeConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X,
				y:cursorAndCentreAtStart.Y - (numSquaresAboveContainer * ShapeConstants.SquareWidth));
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
				x:ShapeConstants.ContainingRectangle.X
					+ ShapeConstants.ContainingRectangle.Width
					- (numSquaresRightOfContainer * ShapeConstants.SquareWidth)
					- ShapeConstants.SquareWidth/2,
				y:ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X,
                y: ShapeConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X + (numSquaresRightOfContainer * ShapeConstants.SquareWidth),
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
				x:ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
				y:ShapeConstants.ContainingRectangle.Y
					+ ShapeConstants.ContainingRectangle.Height
					- (numSquaresBelowContainer * ShapeConstants.SquareWidth)
					- ShapeConstants.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X,
                y: ShapeConstants.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X,
				y:cursorAndCentreAtStart.Y + (numSquaresBelowContainer * ShapeConstants.SquareWidth));
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
			int horizontalMovement = 2*ShapeConstants.SquareWidth;
			int verticalMovement = ShapeConstants.SquareWidth;
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
