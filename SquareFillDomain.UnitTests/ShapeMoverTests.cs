using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquareFillDomain.Controllers;
// using NUnit.Framework;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    [TestClass]
    public class ShapeMoverTests
    {
        private ShapeController _shapeMover;
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
            _shapeMover = new ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);

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
            _shapeMover.EndMove(finalLocation: newCursorPosition);

            // Assert
            Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestSnapToGridIfNewCursorIsInCentreOfAGridSquareAndCursorIsInCentreOfShapeThenNewShapeCentreEqualsNewCursorPosition()    {
			// Arrange
			var cursorPositionAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth / 2,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth / 2);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewPositionIsInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenNoSnapOccurs(){
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(
                x: 10,
                y: -5);
            var cursorPositionAtStart = new SquareFillPoint(
                x: _centreOfDefaultSingleSquare.X + relativeCursorPosition.X, 
                y: _centreOfDefaultSingleSquare.Y + relativeCursorPosition.Y);
			var newCursorPosition = new SquareFillPoint(
                x: _centreOfDefaultSingleSquare.X + ShapeConstants.SquareWidth + relativeCursorPosition.X,
                y: _centreOfDefaultSingleSquare.Y + ShapeConstants.SquareWidth + relativeCursorPosition.Y);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X - relativeCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y - relativeCursorPosition.Y);
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
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewPositionIsNotInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenSnapToGridGoesToCentreOfNearestSquare(){
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(
                x: 10,
                y: 10);
            var cursorPositionAtStart = new SquareFillPoint(
				x: _centreOfDefaultSingleSquare.X + relativeCursorPosition.X,
				y: _centreOfDefaultSingleSquare.Y + relativeCursorPosition.Y);
			var centreOfNearestSquare = new SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
			var newCursorPosition = new SquareFillPoint(
				x: centreOfNearestSquare.X + relativeCursorPosition.X + 1,
				y: centreOfNearestSquare.Y + relativeCursorPosition.Y + 1);
            _shapeMover.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, centreOfNearestSquare.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, centreOfNearestSquare.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsNotInCentreOfAGridSquareThenItWillSnapToCentreOfNearestSquare() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
		    var offset = 2;
            var centreOfNearestSquare = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
                y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            var newCursorPosition = new SquareFillPoint(
                x: centreOfNearestSquare.X + offset, 
                y: centreOfNearestSquare.Y + offset);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, centreOfNearestSquare.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, centreOfNearestSquare.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsToLeftOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = new SquareFillPoint(
				x:TestConstants.ContainingRectangle.X - TestConstants.SquareWidth - TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
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
				y:TestConstants.ContainingRectangle.Y - TestConstants.SquareWidth - TestConstants.SquareWidth/2);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
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
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapePositionIsBelowContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCornerAtStart = _cornerOfDefaultSingleSquare;
			var newCursorY = TestConstants.ContainingRectangle.Y
				+ TestConstants.ContainingRectangle.Height
				+ TestConstants.SquareWidth;
			var newCursorPosition = new SquareFillPoint(x:cursorAndCornerAtStart.X, y:newCursorY);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCornerAtStart);

            // Act
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, newCursorPosition.Y);
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
            var originalOrigin = new SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY);
            _shapeMover.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeMover.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, originalOrigin.X + horizontalMovement);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, originalOrigin.Y + verticalMovement);
		}
	}
}
