using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
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
            _shapeMover = new ShapeMover(screenWidth: ShapeSetBuilder.ScreenWidth, screenHeight: ShapeSetBuilder.ScreenHeight);
            _defaultSingleSquareShape = new Shape(
                    centreOfShape: new SquareFillPoint(
                        x: ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth / 2,
                        y: ShapeSetBuilder.ContainingRectangle.Y + ShapeSetBuilder.SquareWidth / 2),
                    topLeftCorner: new SquareFillPoint(
                        x: ShapeSetBuilder.ContainingRectangle.X,
                        y: ShapeSetBuilder.ContainingRectangle.Y),
                    squareDefinitions: new List<Square> { new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: new MockSquareView()) });
        }

        [Test]
		public void TestShapeMoverWillRoundScreenDimensionsDownToClosestMultiplesOfSquareWidth() {
			// Arrange & Act
			var shapeMover = new ShapeMover(
				screenWidth: (10 * ShapeSetBuilder.SquareWidth) + 10,
				screenHeight: (20 * ShapeSetBuilder.SquareWidth) + 3);
			
			// Assert
			Assert.AreEqual(shapeMover.ScreenWidth, 10 * ShapeSetBuilder.SquareWidth);
			Assert.AreEqual(shapeMover.ScreenHeight, 20 * ShapeSetBuilder.SquareWidth);
		}
		
		[Test]
		public void TestNewShapeCentreWillBeNewCursorPositionWhenCursorIsInCentreOfShape() {
			// Arrange
			var cursorPositionAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x: (3 * ShapeSetBuilder.SquareWidth) - 10,
				y: (2 * ShapeSetBuilder.SquareWidth) - 10);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.MoveToNewCursorPosition(newCursorPosition:newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
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
			Assert.AreEqual(newShapeCentre.X, originalCentreOfShape.X + horizontalMovement);
			Assert.AreEqual(newShapeCentre.Y, originalCentreOfShape.Y + verticalmovement);
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
			Assert.AreEqual(newShapeCentre.X, originalCentreOfShape.X + horizontalMovement);
			Assert.AreEqual(newShapeCentre.Y, originalCentreOfShape.Y + verticalmovement);
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
			Assert.AreEqual(newShapeCentre.X, originalCentreOfShape.X + horizontalMovement);
			Assert.AreEqual(newShapeCentre.Y, originalCentreOfShape.Y + verticalmovement);
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
			Assert.AreEqual(newShapeCentre.X, originalCentreOfShape.X + horizontalMovement);
			Assert.AreEqual(newShapeCentre.Y, originalCentreOfShape.Y + verticalmovement);
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
            Assert.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestSnapToGridIfNewCursorIsInCentreOfAGridSquareAndCursorIsInCentreOfShapeThenNewShapeCentreEqualsNewCursorPosition()    {
			// Arrange
			var cursorPositionAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x: ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2,
				y: ShapeSetBuilder.ContainingRectangle.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewPositionIsInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenNoSnapOccurs(){
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(x: 100, y: 50);
			var shapeCentreRelativeToCursorPosition = new SquareFillPoint(
				x: _defaultSingleSquareShape.CentreOfShape.X - cursorPositionAtStart.X,
				y: _defaultSingleSquareShape.CentreOfShape.Y - cursorPositionAtStart.Y);
			var newCursorPosition = new SquareFillPoint(
				x: ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth/2 - shapeCentreRelativeToCursorPosition.X,
				y: ShapeSetBuilder.ContainingRectangle.Y + ShapeSetBuilder.SquareWidth/2 - shapeCentreRelativeToCursorPosition.Y);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, newCursorPosition.X + shapeCentreRelativeToCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y + shapeCentreRelativeToCursorPosition.Y);
		}

        [Test]
        public void TestIfNewShapeCentreIsInCentreOfAGridSquareThenItWillNotSnap() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x:ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth/2,
				y:ShapeSetBuilder.ContainingRectangle.Y + ShapeSetBuilder.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewPositionIsNotInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenSnapToGridGoesToCentreOfNearestSquare(){
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(
				x: _defaultSingleSquareShape.CentreOfShape.X + 10,
				y: _defaultSingleSquareShape.CentreOfShape.Y + 10);
			var centreOfNearestSquare = new SquareFillPoint(
				x: ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth/2,
				y: ShapeSetBuilder.ContainingRectangle.Y + ShapeSetBuilder.SquareWidth/2);
			var newCursorPosition = new SquareFillPoint(
				x: centreOfNearestSquare.X + 1,
				y: centreOfNearestSquare.Y + 1);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorPositionAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, centreOfNearestSquare.X);
			Assert.AreEqual(newShapeCentre.Y, centreOfNearestSquare.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsNotInCentreOfAGridSquareThenItWillSnapToCentreOfNearestSquare() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorX = ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth + 2;
			var newCursorY = ShapeSetBuilder.ContainingRectangle.Y + 2*ShapeSetBuilder.SquareWidth + 2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:newCursorY);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, ShapeSetBuilder.ContainingRectangle.X
				+ ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
			Assert.AreEqual(newShapeCentre.Y, ShapeSetBuilder.ContainingRectangle.Y
				+ 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsToLeftOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x:ShapeSetBuilder.ContainingRectangle.X - ShapeSetBuilder.SquareWidth/2,
				y:ShapeSetBuilder.ContainingRectangle.Y + ShapeSetBuilder.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsAboveContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorPosition = new SquareFillPoint(
				x:ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth/2,
				y:ShapeSetBuilder.ContainingRectangle.Y - ShapeSetBuilder.SquareWidth/2);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsToRightOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorX = ShapeSetBuilder.ContainingRectangle.X
				+ ShapeSetBuilder.ContainingRectangle.Width
				+ ShapeSetBuilder.SquareWidth/2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:cursorAndCentreAtStart.Y);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfNewShapeCentreIsBelowContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			var newCursorY = ShapeSetBuilder.ContainingRectangle.Y
				+ ShapeSetBuilder.ContainingRectangle.Height
				+ ShapeSetBuilder.SquareWidth/2;
			var newCursorPosition = new SquareFillPoint(x:cursorAndCentreAtStart.X, y:newCursorY);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			var newShapeCentre = _shapeMover.ShapeToMove.Squares[0].Sprite.Centre();
			Assert.AreEqual(newShapeCentre.X, newCursorPosition.X);
			Assert.AreEqual(newShapeCentre.Y, newCursorPosition.Y);
		}
		
		//MARK: Shapes composed of multiple smaller squares
		
		[Test]
		public void TestIfLeftmostShapeEdgeIsLeftOfContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresLeftOfContainer = 2;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:ShapeSetBuilder.ContainingRectangle.X
					+ (numSquaresLeftOfContainer * ShapeSetBuilder.SquareWidth) + ShapeSetBuilder.SquareWidth/2,
				y:ShapeSetBuilder.ContainingRectangle.Y + ShapeSetBuilder.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingRectangle.X,
                y: ShapeSetBuilder.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X - (numSquaresLeftOfContainer * ShapeSetBuilder.SquareWidth),
				y:cursorAndCentreAtStart.Y);
			var shapeToMove = new Shape(
				centreOfShape: cursorAndCentreAtStart,
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParent: new SquareFillPoint(x:-numSquaresLeftOfContainer, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:2, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresLeftOfContainer, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresLeftOfContainer + 2, y:0), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover(screenWidth: ShapeSetBuilder.ScreenWidth, screenHeight: ShapeSetBuilder.ScreenHeight);
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Assert.AreEqual(shapeMover.ShapeToMove.CentreOfShape.X, newCursorPosition.X);
			Assert.AreEqual(shapeMover.ShapeToMove.CentreOfShape.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfTopShapeEdgeIsAboveContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresAboveContainer = 3;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth/2,
				y:ShapeSetBuilder.ContainingRectangle.Y
					+ (numSquaresAboveContainer * ShapeSetBuilder.SquareWidth) + ShapeSetBuilder.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingRectangle.X,
                y: ShapeSetBuilder.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X,
				y:cursorAndCentreAtStart.Y - (numSquaresAboveContainer * ShapeSetBuilder.SquareWidth));
			var shapeToMove = new Shape(
				centreOfShape: cursorAndCentreAtStart,
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:-numSquaresAboveContainer), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:2), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresAboveContainer), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresAboveContainer + 2), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover(screenWidth: ShapeSetBuilder.ScreenWidth, screenHeight: ShapeSetBuilder.ScreenHeight);
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Assert.AreEqual(shapeMover.ShapeToMove.CentreOfShape.X, newCursorPosition.X);
			Assert.AreEqual(shapeMover.ShapeToMove.CentreOfShape.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfRightShapeEdgeIsRightOfContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresRightOfContainer = 1;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:ShapeSetBuilder.ContainingRectangle.X
					+ ShapeSetBuilder.ContainingRectangle.Width
					- (numSquaresRightOfContainer * ShapeSetBuilder.SquareWidth)
					- ShapeSetBuilder.SquareWidth/2,
				y:ShapeSetBuilder.ContainingRectangle.Y + ShapeSetBuilder.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingRectangle.X,
                y: ShapeSetBuilder.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X + (numSquaresRightOfContainer * ShapeSetBuilder.SquareWidth),
				y:cursorAndCentreAtStart.Y);
			var shapeToMove = new Shape(
				centreOfShape: cursorAndCentreAtStart,
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParent: new SquareFillPoint(x:numSquaresRightOfContainer, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:-2, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresRightOfContainer, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:numSquaresRightOfContainer + 2, y:0), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover(screenWidth: ShapeSetBuilder.ScreenWidth, screenHeight: ShapeSetBuilder.ScreenHeight);
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Assert.AreEqual(shapeMover.ShapeToMove.CentreOfShape.X, newCursorPosition.X);
			Assert.AreEqual(shapeMover.ShapeToMove.CentreOfShape.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestIfBottomShapeEdgeIsBelowContainerThenShapeWillNotBeSnappedBackInsideContainer() {
			// Arrange
			int numSquaresBelowContainer = 2;
			var cursorAndCentreAtStart = new SquareFillPoint(
				x:ShapeSetBuilder.ContainingRectangle.X + ShapeSetBuilder.SquareWidth/2,
				y:ShapeSetBuilder.ContainingRectangle.Y
					+ ShapeSetBuilder.ContainingRectangle.Height
					- (numSquaresBelowContainer * ShapeSetBuilder.SquareWidth)
					- ShapeSetBuilder.SquareWidth/2);
            var topLeftAtStart = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingRectangle.X,
                y: ShapeSetBuilder.ContainingRectangle.Y);
            var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X,
				y:cursorAndCentreAtStart.Y + (numSquaresBelowContainer * ShapeSetBuilder.SquareWidth));
			var shapeToMove = new Shape(
				centreOfShape: cursorAndCentreAtStart,
                topLeftCorner: topLeftAtStart,
                squareDefinitions: new List<Square>{
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:numSquaresBelowContainer), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:-2), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresBelowContainer), sprite: new MockSquareView()),
					new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:numSquaresBelowContainer + 2), sprite: new MockSquareView())
				});
			var shapeMover = new ShapeMover(screenWidth: ShapeSetBuilder.ScreenWidth, screenHeight: ShapeSetBuilder.ScreenHeight);
			
			// Act
			shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: shapeToMove);
			shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Assert.AreEqual(shapeMover.ShapeToMove.CentreOfShape.X, newCursorPosition.X);
			Assert.AreEqual(shapeMover.ShapeToMove.CentreOfShape.Y, newCursorPosition.Y);
		}
		
		[Test]
		public void TestWhenSnappingToGridShouldRecalculateSquareOrigins() {
			// Arrange
			var cursorAndCentreAtStart = _defaultSingleSquareShape.CentreOfShape;
			int horizontalMovement = 2*ShapeSetBuilder.SquareWidth;
			int verticalMovement = ShapeSetBuilder.SquareWidth;
			var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X + horizontalMovement,
				y:cursorAndCentreAtStart.Y + verticalMovement);
			
			// Act
			_shapeMover.StartMove(cursorPositionAtStart:cursorAndCentreAtStart, shapeToMove: _defaultSingleSquareShape);
			var originalOrigin = _defaultSingleSquareShape.Squares[0].TopLeftCorner;
			_shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Assert.AreEqual(_shapeMover.ShapeToMove.Squares[0].TopLeftCorner.X, originalOrigin.X + horizontalMovement);
			Assert.AreEqual(_shapeMover.ShapeToMove.Squares[0].TopLeftCorner.Y, originalOrigin.Y + verticalMovement);
		}
	}
}
