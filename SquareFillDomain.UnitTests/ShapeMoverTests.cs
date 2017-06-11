using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// using NUnit.Framework;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    [TestClass]
    public class ShapeMoverTests
    {
        private ShapeMover _shapeMover;
        private SquareFillPoint _centreOfDefaultSingleSquare;
        private SquareFillPoint _cornerOfDefaultSingleSquare;

        private TestShapeSetBuilder _shapeSetBuilder;
        private Grid _occupiedGridSquares;
        private ShapeSet _shapeSet;

        private int _outsideContainingRectangleButAtCornerOfRightHydrantX;
        private int _outsideContainingRectangleButAtCornerOfRightHydrantY;
        private SquareFillPoint _outsideContainingRectangleButInsideRightHydrant;
        private SquareFillPoint _topLeftCornerSingleSquare;
        private SquareFillPoint _topLeftCornerInsideBorder;
        private SquareFillPoint _insideBorder;
        private readonly SquareFillPoint _relativeCursorPosition = new SquareFillPoint(x: 2, y: 3);

        [TestInitialize]
        public void Setup()
        {
            _shapeSetBuilder = new TestShapeSetBuilder(squareViewFactory: new MockSquareFactory());
            _occupiedGridSquares = _shapeSetBuilder.MakeGridSquares();
            _shapeSet = _shapeSetBuilder.GetShapeSet();
            _shapeSetBuilder.OccupyBorderSquares(occupiedGridSquares: _occupiedGridSquares);
            _shapeSet.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);
            _shapeMover = new ShapeMover(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);

            _outsideContainingRectangleButAtCornerOfRightHydrantX = _shapeSetBuilder.RightHydrantShape.TopLeftCornerX;
            _outsideContainingRectangleButAtCornerOfRightHydrantY = _shapeSetBuilder.RightHydrantShape.TopLeftCornerY;

            _outsideContainingRectangleButInsideRightHydrant = new SquareFillPoint(
                x: _outsideContainingRectangleButAtCornerOfRightHydrantX + _relativeCursorPosition.X,
                y: _outsideContainingRectangleButAtCornerOfRightHydrantY + _relativeCursorPosition.Y);

            _topLeftCornerSingleSquare = new SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY);

            _topLeftCornerInsideBorder = new SquareFillPoint(
                x: _shapeSetBuilder.LeftWallBorderSquares[0].X * TestConstants.SquareWidth,
                y: _shapeSetBuilder.LeftWallBorderSquares[0].Y * TestConstants.SquareWidth);

            _insideBorder = new SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + _relativeCursorPosition.X,
                y: _topLeftCornerInsideBorder.Y + _relativeCursorPosition.Y);

            _centreOfDefaultSingleSquare = new SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX + TestConstants.SquareWidth / 2,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY + TestConstants.SquareWidth / 2);

            _cornerOfDefaultSingleSquare = new SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestCursorPositionCanBeCalculatedAccordingToTopLeftCorner()
        {
            // Arrange
            var cursorPositionAtStart = new SquareFillPoint(
                x: _topLeftCornerSingleSquare.X + 15,
                y: _topLeftCornerSingleSquare.Y + 10);
            var newCursorPosition = new SquareFillPoint(
                x: cursorPositionAtStart.X + 2,
                y: cursorPositionAtStart.Y + 3);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);
            _shapeMover.ContinueMove(newLocation: newCursorPosition);

            // Act
            SquareFillPoint calculatedCursorPosition = _shapeMover.CalculateCursorPosition();

            // Assert
            Asserter.AreEqual(calculatedCursorPosition.X, newCursorPosition.X);
            Asserter.AreEqual(calculatedCursorPosition.Y, newCursorPosition.Y);
        }
		
		[TestMethod]
		public void TestNewShapeCentreWillBeNewCursorPositionWhenCursorIsInCentreOfShape() {
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX + TestConstants.SquareWidth/2,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY + TestConstants.SquareWidth/2);
			var newCursorPosition = new SquareFillPoint(
				x: (7 * TestConstants.SquareWidth) - 10,
				y: (8 * TestConstants.SquareWidth) - 10);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.ContinueMove(newLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestNewTopLeftCornerWillBeNewCursorPositionAdjustedAccordingToPositionOfCursorWithinShape() {
			// Arrange
            var originalTopLeftCorner = _cornerOfDefaultSingleSquare;
			var cursorPositionAtStart = new SquareFillPoint(
				x: originalTopLeftCorner.X + 10,
				y: originalTopLeftCorner.Y + 15);
			int horizontalMovement = 35;
			int verticalmovement = 30;
			var newCursorPosition = new SquareFillPoint(
				x: cursorPositionAtStart.X + horizontalMovement,
				y: cursorPositionAtStart.Y + verticalmovement);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.ContinueMove(newLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, originalTopLeftCorner.X + horizontalMovement);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, originalTopLeftCorner.Y + verticalmovement);
		}
		
		[TestMethod]
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
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.ContinueMove(newLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, originalCentreOfShape.X + horizontalMovement);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, originalCentreOfShape.Y + verticalmovement);
		}
		
		[TestMethod]
		public void TestShapeCornerIsMovedCorrectlyWhenCursorMovementIsNegative() {
			// Arrange
			var originalCornerOfShape = _cornerOfDefaultSingleSquare;
			var cursorPositionAtStart = new SquareFillPoint(
				x: originalCornerOfShape.X + 5,
				y: originalCornerOfShape.Y + 25);
			int horizontalMovement = -15;
			int verticalmovement = -25;
			var newCursorPosition = new SquareFillPoint(
				x: cursorPositionAtStart.X + horizontalMovement,
				y: cursorPositionAtStart.Y + verticalmovement);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.ContinueMove(newLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, originalCornerOfShape.X + horizontalMovement);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, originalCornerOfShape.Y + verticalmovement);
		}
		
		[TestMethod]
		public void TestSnapToGridIfCursorDoesntMoveAndCursorIsInCentreOfShapeAndCursorInCentreOfAGridSquareThenNewShapeCentreEqualsOriginalCursorPosition(){
			// Arrange
            var cursorPositionAtStart = _centreOfDefaultSingleSquare;
            var newCursorPosition = cursorPositionAtStart;
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestSnapToGridIfNewCursorIsInCentreOfAGridSquareAndCursorIsInCentreOfShapeThenNewShapeCentreEqualsNewCursorPosition()    {
			// Arrange
			var cursorPositionAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth + TestConstants.SquareWidth / 2,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewPositionIsInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenNoSnapOccurs(){
			// Arrange
			var cursorPositionAtStart = new SquareFillPoint(x: 100, y: 50);
			var shapeCentreRelativeToCursorPosition = new SquareFillPoint(
				x: _centreOfDefaultSingleSquare.X - cursorPositionAtStart.X,
				y: _centreOfDefaultSingleSquare.Y - cursorPositionAtStart.Y);
			var newCursorPosition = new SquareFillPoint(
				x: _centreOfDefaultSingleSquare.X - shapeCentreRelativeToCursorPosition.X,
				y: _centreOfDefaultSingleSquare.Y - shapeCentreRelativeToCursorPosition.Y);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X + shapeCentreRelativeToCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y + shapeCentreRelativeToCursorPosition.Y);
		}

        [TestMethod]
        public void TestIfNewShapeCentreIsInCentreOfAGridSquareThenItWillNotSnap() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
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
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, centreOfNearestSquare.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, centreOfNearestSquare.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsNotInCentreOfAGridSquareThenItWillSnapToCentreOfNearestSquare() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorX = TestConstants.ContainingRectangle.X + TestConstants.SquareWidth + 2;
			var newCursorY = TestConstants.ContainingRectangle.Y + 2*TestConstants.SquareWidth + 2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:newCursorY);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, TestConstants.ContainingRectangle.X
				+ TestConstants.SquareWidth + TestConstants.SquareWidth/2);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, TestConstants.ContainingRectangle.Y
				+ 2*TestConstants.SquareWidth + TestConstants.SquareWidth/2);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsToLeftOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X - TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsAboveContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y - TestConstants.SquareWidth/2);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsToRightOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorX = TestConstants.ContainingRectangle.X
				+ TestConstants.ContainingRectangle.Width
				+ TestConstants.SquareWidth/2;
			var newCursorPosition = new SquareFillPoint(x:newCursorX, y:cursorAndCentreAtStart.Y);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsBelowContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCornerAtStart = _cornerOfDefaultSingleSquare;
			var newCursorY = TestConstants.ContainingRectangle.Y
				+ TestConstants.ContainingRectangle.Height
				+ TestConstants.SquareWidth;
			var newCursorPosition = new SquareFillPoint(x:cursorAndCornerAtStart.X, y:newCursorY);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCornerAtStart);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, newCursorPosition.Y);
		}
		
		[TestMethod]
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
            var shapeSet = new ShapeSet(new List<Shape> { shapeToMove });
            var shapeMover2 = new ShapeMover(shapeSet: shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeMover2.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            shapeMover2.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
            Asserter.AreEqual(shapeToMove.TopLeftCornerX, topLeftAtStart.X - (numSquaresLeftOfContainer * TestConstants.SquareWidth));
            Asserter.AreEqual(shapeToMove.TopLeftCornerY, topLeftAtStart.Y);
		}
		
		[TestMethod]
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
            var shapeSet = new ShapeSet(new List<Shape> { shapeToMove });
            var shapeMover2 = new ShapeMover(shapeSet: shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeMover2.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
			shapeMover2.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCornerX, topLeftAtStart.X);
            Asserter.AreEqual(shapeToMove.TopLeftCornerY, topLeftAtStart.Y - (numSquaresAboveContainer * TestConstants.SquareWidth));
		}
		
		[TestMethod]
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
            var shapeSet = new ShapeSet(new List<Shape> { shapeToMove });
            var shapeMover2 = new ShapeMover(shapeSet: shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeMover2.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
			shapeMover2.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCornerX, topLeftAtStart.X + (numSquaresRightOfContainer * TestConstants.SquareWidth));
            Asserter.AreEqual(shapeToMove.TopLeftCornerY, topLeftAtStart.Y);
		}
		
		[TestMethod]
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
            var shapeSet = new ShapeSet(new List<Shape> { shapeToMove });
            var shapeMover2 = new ShapeMover(shapeSet: shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeMover2.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
			shapeMover2.SnapToGrid(newCursorPosition: newCursorPosition);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCornerX, topLeftAtStart.X);
            Asserter.AreEqual(shapeToMove.TopLeftCornerY, topLeftAtStart.Y + (numSquaresBelowContainer * TestConstants.SquareWidth));
		}
		
		[TestMethod]
		public void TestWhenSnappingToGridShouldRecalculateSquareOrigins() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			int horizontalMovement = 2*TestConstants.SquareWidth;
			int verticalMovement = TestConstants.SquareWidth;
			var newCursorPosition = new SquareFillPoint(
				x:cursorAndCentreAtStart.X + horizontalMovement,
				y:cursorAndCentreAtStart.Y + verticalMovement);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);
            var originalOrigin = new SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY);

            // Act
            _shapeMover.SnapToGrid(newCursorPosition: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, originalOrigin.X + horizontalMovement);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, originalOrigin.Y + verticalMovement);
		}
	}
}
