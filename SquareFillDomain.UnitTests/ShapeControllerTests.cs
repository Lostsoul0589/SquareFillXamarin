using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquareFillDomain.Controllers;
// using NUnit.Framework;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    // class ShapeControllerTests: XCTestCase
    [TestClass]
    public class ShapeControllerTests
    {
        private ShapeController _shapeController;
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
        private SquareFillPoint _relativeCursorPosition;

        private SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }

        private ShapeController ShapeController(ShapeSet shapeSet, Grid occupiedGridSquares)
        {
            return new ShapeController(shapeSet: shapeSet, occupiedGridSquares: occupiedGridSquares);
        }

        // override func tearDown() 
        // {
        //      // This method is called after the invocation of each test method in the class.
        //      super.tearDown();
        // }

        // override func setUp() 
        // {
        //      // This method is called before the invocation of each test method in the class.
        //      super.setUp();
        [TestInitialize]
        public void Setup()
        {
            _relativeCursorPosition = SquareFillPoint(x: 2, y: 3);
            _shapeSetBuilder = new TestShapeSetBuilder(squareViewFactory: new MockSquareFactory());
            _occupiedGridSquares = _shapeSetBuilder.MakeGridSquares();
            _shapeSet = _shapeSetBuilder.GetShapeSet();
            _shapeSetBuilder.OccupyBorderSquares(occupiedGridSquares: _occupiedGridSquares);
            _shapeSet.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);
            _shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);

            _outsideContainingRectangleButAtCornerOfRightHydrantX = _shapeSetBuilder.RightHydrantShape.TopLeftCornerX;
            _outsideContainingRectangleButAtCornerOfRightHydrantY = _shapeSetBuilder.RightHydrantShape.TopLeftCornerY;

            _outsideContainingRectangleButInsideRightHydrant = SquareFillPoint(
                x: _outsideContainingRectangleButAtCornerOfRightHydrantX + _relativeCursorPosition.X,
                y: _outsideContainingRectangleButAtCornerOfRightHydrantY + _relativeCursorPosition.Y);

            _topLeftCornerSingleSquare = SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY);

            _topLeftCornerInsideBorder = SquareFillPoint(
                x: _shapeSetBuilder.LeftWallBorderSquares[0].X * TestConstants.SquareWidth,
                y: _shapeSetBuilder.LeftWallBorderSquares[0].Y * TestConstants.SquareWidth);

            _insideBorder = SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + _relativeCursorPosition.X,
                y: _topLeftCornerInsideBorder.Y + _relativeCursorPosition.Y);

            _centreOfDefaultSingleSquare = SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX + TestConstants.SquareWidth / 2,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY + TestConstants.SquareWidth / 2);

            _cornerOfDefaultSingleSquare = SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY);
        }

        [TestMethod]
        public void TestPerformanceOfStartMove()
        {
            // Arrange
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);

            // Assert
            /*self.measure()	{
                // Act
                shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            }*/
        }

        [TestMethod]
        public void TestPerformanceOfContinueMove()
        {
            // Arrange
            var startingX = TestConstants.SquareWidth + TestConstants.SquareWidth / 2;
            var insideContainingRectangle = SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth / 2,
                y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Assert
            // self.measure()	{
            //    var start = 1;
            //    var end = 100;
            //    for(int count = start; count <= end; count++) {
            //        shapeController.ContinueMove(newLocation: insideContainingRectangle);
            //        insideContainingRectangle.X = startingX + count;
            //    }
            //}
        }

        [TestMethod]
        public void TestPerformanceOfEndMove()
        {
            // Arrange
            var insideContainingRectangle = SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth / 2,
                y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Assert
            // self.measure()	{
            //      shapeController.EndMove(finalLocation: insideContainingRectangle);
            // }
        }

        [TestMethod]
        public void TestWhenShapeStaysOutsideGameGridThenSquaresAreStillOccupied()
        {
            // Arrange
            var stillOutsideContainingRectangle = SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X - 2 * TestConstants.SquareWidth,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var topLeftVacatedGridX = _shapeSetBuilder.RightHydrantShape.TopLeftCornerX / TestConstants.SquareWidth;
            var topLeftVacatedGridY = _shapeSetBuilder.RightHydrantShape.TopLeftCornerY / TestConstants.SquareWidth;
            var topLeftOccupiedGridX = (_shapeSetBuilder.RightHydrantShape.TopLeftCornerX / TestConstants.SquareWidth) - 2;
            var topLeftOccupiedGridY = topLeftVacatedGridY;
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: stillOutsideContainingRectangle);

            // Assert
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: topLeftVacatedGridX, y: topLeftVacatedGridY), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: topLeftVacatedGridX, y: topLeftVacatedGridY + 1), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: topLeftVacatedGridX, y: topLeftVacatedGridY + 2), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: topLeftVacatedGridX + 1, y: topLeftVacatedGridY + 1), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: topLeftOccupiedGridX, y: topLeftOccupiedGridY), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: topLeftOccupiedGridX, y: topLeftOccupiedGridY + 1), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: topLeftOccupiedGridX, y: topLeftOccupiedGridY + 2), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: topLeftOccupiedGridX + 1, y: topLeftOccupiedGridY + 1), true);
        }

        [TestMethod]
        public void TestShapeSquaresAreVacatedAfterStartMove()
        {
            // Arrange
            var insideContainingRectangle = SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + _relativeCursorPosition.X,
                y: TestConstants.ContainingRectangle.Y + _relativeCursorPosition.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            shapeController.EndMove(finalLocation: insideContainingRectangle);

            // Act
            shapeController.StartMove(cursorPositionAtStart: insideContainingRectangle);

            // Assert
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 0 + containingY), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 1 + containingY), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 2 + containingY), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 1 + containingX, y: 1 + containingY), false);
        }

        [TestMethod]
        public void TestSquaresAreStillVacatedIfShapeStartsOutsideGameGrid()
        {
            // Arrange
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            _occupiedGridSquares.OccupyAllSquares();
            var topLeftGridX = _shapeSetBuilder.RightHydrantShape.TopLeftCornerX / TestConstants.SquareWidth;
            var topLeftGridY = _shapeSetBuilder.RightHydrantShape.TopLeftCornerY / TestConstants.SquareWidth;

            // Act
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Assert
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + topLeftGridX, y: 0 + topLeftGridY), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + topLeftGridX, y: 0 + topLeftGridY + 1), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + topLeftGridX, y: 0 + topLeftGridY + 2), false);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + topLeftGridX + 1, y: 0 + topLeftGridY + 1), false);
        }

        [TestMethod]
        public void TestShapeSquaresAreOccupiedAfterEndMoveWhenShapeIsPerfectlyAlignedWithGrid()
        {
            // Arrange
            var insideContainingRectangle = SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + _relativeCursorPosition.X,
                y: TestConstants.ContainingRectangle.Y + _relativeCursorPosition.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: insideContainingRectangle);

            // Assert
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 0 + containingY), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 1 + containingY), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 2 + containingY), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 1 + containingX, y: 1 + containingY), true);
        }

        [TestMethod]
        public void TestShapeSquaresAreOccupiedToSnappedLocationsAfterEndMove()
        {
            // Arrange
            var insideContainingRectangleButNotAlignedWithGrid = SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + _relativeCursorPosition.X + 5,
                y: TestConstants.ContainingRectangle.Y + _relativeCursorPosition.Y + 6);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: insideContainingRectangleButNotAlignedWithGrid);

            // Assert
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 0 + containingY), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 1 + containingY), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 2 + containingY), true);
            Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 1 + containingX, y: 1 + containingY), true);
        }

        [TestMethod]
        public void TestShapeDoesNotMoveIfAnotherShapeIsInTheWay()
        {
            // Arrange
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            var shapeToMove = shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var originalX = shapeController.CurrentShapeCornerX;
            var originalY = shapeController.CurrentShapeCornerY;

            // Act
            shapeController.EndMove(finalLocation: _insideBorder);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCornerX, originalX);
            Asserter.AreEqual(shapeToMove.TopLeftCornerY, originalY);
        }

        [TestMethod]
        public void TestWhenAnObstacleIsDetectedThenAnyFurtherMovesOutsideTheShapeAreIgnored()
        {
            // Arrange
            var cursorOutsideLatestShapePosition = SquareFillPoint(
                x: _insideBorder.X + 3 * TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var originalX = shapeController.CurrentShapeCornerX;
            var originalY = shapeController.CurrentShapeCornerY;

            // Act
            shapeController.ContinueMove(newLocation: _insideBorder);
            shapeController.ContinueMove(newLocation: cursorOutsideLatestShapePosition);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, originalX);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, originalY);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedAndCursorLeavesShapeItWillStartMovingAgainWhenCursorReturns()
        {
            // Arrange
            var xOffset = 3;
            var yOffset = 4;
            var immediatelyToTheRightOfObstacleWithOffset = SquareFillPoint(
                x: _insideBorder.X + TestConstants.SquareWidth + xOffset,
                y: _insideBorder.Y + yOffset);
            var immediatelyToTheRightOfObstacleWithoutOffset = SquareFillPoint(
                x: _insideBorder.X + 2 * TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var topLeftCornerImmediatelyToTheRightOfObstacle = SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.ContinueMove(newLocation: immediatelyToTheRightOfObstacleWithOffset);
            shapeController.ContinueMove(newLocation: _insideBorder);
            shapeController.ContinueMove(newLocation: immediatelyToTheRightOfObstacleWithoutOffset);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, topLeftCornerImmediatelyToTheRightOfObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, topLeftCornerImmediatelyToTheRightOfObstacle.Y);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedOnTheRightThenShapeWillSnapToRightHandBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xOffset = 12;
            var yOffset = 13;
            var insideFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.FourSquareShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.FourSquareShape.TopLeftCornerY + relativeCursorPosition.Y);
            var directlyToLeftOfObstacle = SquareFillPoint(
                x: (_shapeSetBuilder.RightWallBorderSquares[2].X * TestConstants.SquareWidth) - 2 * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.RightWallBorderSquares[2].Y * TestConstants.SquareWidth));
            var insideObstacleToRightOfFourSquare = SquareFillPoint(
                x: directlyToLeftOfObstacle.X + TestConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToLeftOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var toLeftOfObstacle = SquareFillPoint(
                x: directlyToLeftOfObstacle.X + relativeCursorPosition.X - xOffset,
                y: directlyToLeftOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var snappedToRightHandBorderWithObstacle = SquareFillPoint(
                x: directlyToLeftOfObstacle.X,
                y: directlyToLeftOfObstacle.Y + yOffset);

            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: toLeftOfObstacle);
            shapeController.ContinueMove(newLocation: insideObstacleToRightOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, snappedToRightHandBorderWithObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, snappedToRightHandBorderWithObstacle.Y);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedOnTheLeftThenShapeWillSnapToLeftHandBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xOffset = 12;
            var yOffset = 13;
            var directlyToRightOfObstacle = SquareFillPoint(
                x: _shapeSetBuilder.RightHydrantShape.TopLeftCornerX + 2 * TestConstants.SquareWidth,
                y: _shapeSetBuilder.RightHydrantShape.TopLeftCornerY + TestConstants.SquareWidth);
            var insideFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.FourSquareShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.FourSquareShape.TopLeftCornerY + relativeCursorPosition.Y);
            var insideObstacleToLeftOfFourSquare = SquareFillPoint(
                x: directlyToRightOfObstacle.X - TestConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var nearObstacleToLeftOfFourSquare = SquareFillPoint(
                x: directlyToRightOfObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var snappedToLeftHandBorderWithObstacle = SquareFillPoint(
                x: directlyToRightOfObstacle.X,
                y: directlyToRightOfObstacle.Y + yOffset);

            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToLeftOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, snappedToLeftHandBorderWithObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, snappedToLeftHandBorderWithObstacle.Y);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedAboveThenShapeWillSnapToTopOfBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xOffset = 10;
            var yOffset = 11;
            var directlyBelowObstacle = SquareFillPoint(
                x: _shapeSetBuilder.TopRowBorderSquares[3].X * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.TopRowBorderSquares[3].Y + 1) * TestConstants.SquareWidth);
            var insideFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.FourSquareShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.FourSquareShape.TopLeftCornerY + relativeCursorPosition.Y);
            var nearObstacleAboveFourSquare = SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyBelowObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleAboveFourSquare = SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyBelowObstacle.Y - TestConstants.SquareWidth + relativeCursorPosition.Y + yOffset);
            var snappedToTopOfBorderWithObstacle = SquareFillPoint(
                x: directlyBelowObstacle.X + xOffset,
                y: directlyBelowObstacle.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleAboveFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, snappedToTopOfBorderWithObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, snappedToTopOfBorderWithObstacle.Y);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedBelowThenShapeWillSnapToBottomOfBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xOffset = 12;
            var yOffset = 13;

            var insideFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.FourSquareShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.FourSquareShape.TopLeftCornerY + relativeCursorPosition.Y);
            var insideObstacleBelowFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.LeftCornerShape.TopLeftCornerX + relativeCursorPosition.X + xOffset,
                y: _shapeSetBuilder.LeftCornerShape.TopLeftCornerY + relativeCursorPosition.Y + yOffset);
            var directlyAboveObstacle = SquareFillPoint(
                x: _shapeSetBuilder.LeftCornerShape.TopLeftCornerX,
                y: _shapeSetBuilder.LeftCornerShape.TopLeftCornerY - 2 * TestConstants.SquareWidth);
            var nearObstacleBelowFourSquare = SquareFillPoint(
                x: directlyAboveObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyAboveObstacle.Y + relativeCursorPosition.Y - yOffset);
            var snappedToBottomOfBorderWithObstacle = SquareFillPoint(
                x: directlyAboveObstacle.X + xOffset,
                y: directlyAboveObstacle.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleBelowFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleBelowFourSquare);

            // Assert
            Asserter.AreEqual(snappedToBottomOfBorderWithObstacle.X, shapeController.CurrentShapeCornerX);
            Asserter.AreEqual(snappedToBottomOfBorderWithObstacle.Y, shapeController.CurrentShapeCornerY);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedDiagonallyTopLeftThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xAndYOffset = 10;
            var directlyBottomRightOfObstacle = SquareFillPoint(
                x: (_shapeSetBuilder.BottomLeftBorderSquares[1].X + 1) * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomLeftBorderSquares[1].Y + 1) * TestConstants.SquareWidth);
            var insideLeftCornerShape = SquareFillPoint(
                x: _shapeSetBuilder.LeftCornerShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.LeftCornerShape.TopLeftCornerY + relativeCursorPosition.Y);
            var nearObstacleDiagonallyTopLeftOfFourSquare = SquareFillPoint(
                x: directlyBottomRightOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyBottomRightOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyTopLeftOfFourSquare = SquareFillPoint(
                x: directlyBottomRightOfObstacle.X - TestConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyBottomRightOfObstacle.Y - TestConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToTopLeftCornerByObstacle = SquareFillPoint(
                x: directlyBottomRightOfObstacle.X,
                y: directlyBottomRightOfObstacle.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideLeftCornerShape);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyTopLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyTopLeftOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, snappedToTopLeftCornerByObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, snappedToTopLeftCornerByObstacle.Y);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedDiagonallyTopRightThenShapeWillSnapToTopRightCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xAndYOffset = 10;
            var directlyBottomLeftOfObstacle = SquareFillPoint(
                x: (_shapeSetBuilder.BottomRightBorderSquares[0].X - 2) * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomRightBorderSquares[0].Y + 1) * TestConstants.SquareWidth);
            var insideLeftCornerShape = SquareFillPoint(
                x: _shapeSetBuilder.LeftCornerShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.LeftCornerShape.TopLeftCornerY + relativeCursorPosition.Y);
            var nearObstacleDiagonallyTopRightOfFourSquare = SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X + relativeCursorPosition.X - xAndYOffset,
                y: directlyBottomLeftOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyTopRightOfFourSquare = SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X + TestConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyBottomLeftOfObstacle.Y - TestConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToTopRightCornerByObstacle = SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X,
                y: directlyBottomLeftOfObstacle.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideLeftCornerShape);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyTopRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyTopRightOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, snappedToTopRightCornerByObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, snappedToTopRightCornerByObstacle.Y);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedDiagonallyBottomRightThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xAndYOffset = 10;
            var directlyTopLeftOfObstacle = SquareFillPoint(
                x: (_shapeSetBuilder.BottomRightBorderSquares[0].X * TestConstants.SquareWidth) - 2 * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomRightBorderSquares[0].Y * TestConstants.SquareWidth) - 2 * TestConstants.SquareWidth);
            var insideFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.FourSquareShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.FourSquareShape.TopLeftCornerY + relativeCursorPosition.Y);
            var nearObstacleDiagonallyBottomRightOfFourSquare = SquareFillPoint(
                x: directlyTopLeftOfObstacle.X + relativeCursorPosition.X - xAndYOffset,
                y: directlyTopLeftOfObstacle.Y + relativeCursorPosition.Y - xAndYOffset);
            var insideObstacleDiagonallyBottomRightOfFourSquare = SquareFillPoint(
                x: directlyTopLeftOfObstacle.X + TestConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopLeftOfObstacle.Y + TestConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToBottomRightCornerByObstacle = SquareFillPoint(
                x: directlyTopLeftOfObstacle.X,
                y: directlyTopLeftOfObstacle.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyBottomRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyBottomRightOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, snappedToBottomRightCornerByObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, snappedToBottomRightCornerByObstacle.Y);
        }

        [TestMethod]
        public void TestWhenObstacleIsDetectedDiagonallyBottomLeftThenShapeWillSnapToTopRightCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xAndYOffset = 10;
            var insideFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.FourSquareShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.FourSquareShape.TopLeftCornerY + relativeCursorPosition.Y);
            var directlyTopRightOfObstacle = SquareFillPoint(
                x: (_shapeSetBuilder.BottomLeftBorderSquares[1].X * TestConstants.SquareWidth) + TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomLeftBorderSquares[1].Y * TestConstants.SquareWidth) - 2 * TestConstants.SquareWidth);
            var insideObstacleDiagonallyBottomLeftOfFourSquare = SquareFillPoint(
                x: directlyTopRightOfObstacle.X - TestConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopRightOfObstacle.Y + TestConstants.SquareWidth + relativeCursorPosition.Y - xAndYOffset);
            var nearObstacleDiagonallyBottomLeftOfFourSquare = SquareFillPoint(
                x: directlyTopRightOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopRightOfObstacle.Y + relativeCursorPosition.Y - xAndYOffset);
            var snappedToBottomLeftCornerByObstacle = SquareFillPoint(
                x: directlyTopRightOfObstacle.X,
                y: directlyTopRightOfObstacle.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyBottomLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyBottomLeftOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, snappedToBottomLeftCornerByObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, snappedToBottomLeftCornerByObstacle.Y);
        }

        [TestMethod]
        public void
            TestWhenObstacleIsDetectedAndCursorLeavesShapeAndReturnsThenAllSubsequentMovementUsesNewRelativeCursorPosition
            ()
        {
            // Arrange
            var initialRelativeCursorPosition = SquareFillPoint(x: 10, y: 11);
            var laterRelativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var startingInsideShape = SquareFillPoint(
                x: _outsideContainingRectangleButAtCornerOfRightHydrantX + initialRelativeCursorPosition.X,
                y: _outsideContainingRectangleButAtCornerOfRightHydrantY + initialRelativeCursorPosition.Y);
            var collidingWithSomething = SquareFillPoint(
                x: _shapeSetBuilder.LeftWallBorderSquares[1].X * TestConstants.SquareWidth,
                y: _shapeSetBuilder.LeftWallBorderSquares[1].Y * TestConstants.SquareWidth);
            var cursorToTheRightOfObstacleWithFirstRelativeCursorPosition = SquareFillPoint(
                x: collidingWithSomething.X + TestConstants.SquareWidth + initialRelativeCursorPosition.X,
                y: collidingWithSomething.Y + initialRelativeCursorPosition.Y);
            var cursorToTheRightOfObstacleWithLaterRelativeCursorPosition = SquareFillPoint(
                x: collidingWithSomething.X + TestConstants.SquareWidth + laterRelativeCursorPosition.X,
                y: collidingWithSomething.Y + laterRelativeCursorPosition.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacleWithFirstRelativeCursorPosition);
            shapeController.ContinueMove(newLocation: collidingWithSomething);
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacleWithLaterRelativeCursorPosition);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, cursorToTheRightOfObstacleWithLaterRelativeCursorPosition.X
                                                          - laterRelativeCursorPosition.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, cursorToTheRightOfObstacleWithLaterRelativeCursorPosition.Y
                                                          - laterRelativeCursorPosition.Y);
        }

        [TestMethod]
        public void
            TestWhenObstacleIsDetectedAndCursorLeavesShapeAndReturnsInADifferentUnoccupiedPlaceThenItWillNotThinkItIsInOurShape
            ()
        {
            // Arrange
            var startingInsideShape = SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var collidingWithSomething = SquareFillPoint(
                x: _shapeSetBuilder.LeftWallBorderSquares[1].X * TestConstants.SquareWidth,
                y: _shapeSetBuilder.LeftWallBorderSquares[1].Y * TestConstants.SquareWidth);
            var cursorToTheRightOfObstacle = SquareFillPoint(
                x: _insideBorder.X + TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var topLeftCornerToTheRightOfObstacle = SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var cursorInAnUnoccupiedSpace = SquareFillPoint(
                x: collidingWithSomething.X + 3 * TestConstants.SquareWidth,
                y: collidingWithSomething.Y + 3 * TestConstants.SquareWidth);
            var cursorToTheRightOfUnoccupiedSpace = SquareFillPoint(
                x: cursorInAnUnoccupiedSpace.X + 10,
                y: cursorInAnUnoccupiedSpace.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacle);
            shapeController.ContinueMove(newLocation: _insideBorder);
            shapeController.ContinueMove(newLocation: cursorInAnUnoccupiedSpace);
            shapeController.ContinueMove(newLocation: cursorToTheRightOfUnoccupiedSpace);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, topLeftCornerToTheRightOfObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, topLeftCornerToTheRightOfObstacle.Y);
        }

        [TestMethod]
        public void
            TestWhenShapeHasStoppedDueToObstacleAndCursorHasKeptMovingIntoFreeSpaceThenEndMoveWillNotSnapToFreeSpace()
        {
            // Arrange
            var startingInsideShape = SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var cursorToTheRightOfObstacle = SquareFillPoint(
                x: _insideBorder.X + 2 * TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var topLeftCornerToTheRightOfObstacle = SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + 2 * TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var cursorInAnUnoccupiedSpace = SquareFillPoint(
                x: _insideBorder.X + 3 * TestConstants.SquareWidth,
                y: _insideBorder.Y + 3 * TestConstants.SquareWidth);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            var shapeToMove = shapeController.StartMove(cursorPositionAtStart: startingInsideShape);

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacle);
            shapeController.ContinueMove(newLocation: _insideBorder);
            shapeController.EndMove(finalLocation: cursorInAnUnoccupiedSpace);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCornerX, topLeftCornerToTheRightOfObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCornerY, topLeftCornerToTheRightOfObstacle.Y);
        }

        [TestMethod]
        public void TestWeCanDetectAShapeInTheWayWhenCursorIsNotInCentreOfShape()
        {
            // Arrange
            var cursorPositionAtStart = SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);
            var originalTopLeftX = _shapeSetBuilder.RightHydrantShape.TopLeftCornerX;

            // Act
            shapeController.ContinueMove(newLocation: _insideBorder);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, originalTopLeftX);
        }

        [TestMethod]
        public void TestWeCanDetectAShapeInTheWayWhenWeAreHorizontallyAlignedWithGrid()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var xOffset = 12;
            var directlyToRightOfObstacle = SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var insideFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.FourSquareShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.FourSquareShape.TopLeftCornerY + relativeCursorPosition.Y);
            var nearObstacleToLeftOfFourSquare = SquareFillPoint(
                x: directlyToRightOfObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y);
            var insideObstacleToLeftOfFourSquare = SquareFillPoint(
                x: directlyToRightOfObstacle.X - TestConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToLeftOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, directlyToRightOfObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, directlyToRightOfObstacle.Y);
        }

        [TestMethod]
        public void TestWeCanDetectAShapeInTheWayWhenWeAreVerticallyAlignedWithGrid()
        {
            // Arrange
            var relativeCursorPosition = SquareFillPoint(x: 1, y: 2);
            var yOffset = 11;
            var directlyBelowObstacle = SquareFillPoint(
                x: _shapeSetBuilder.BottomLeftBorderSquares[0].X * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomLeftBorderSquares[0].Y + 1) * TestConstants.SquareWidth);
            var insideFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.FourSquareShape.TopLeftCornerX + relativeCursorPosition.X,
                y: _shapeSetBuilder.FourSquareShape.TopLeftCornerY + relativeCursorPosition.Y);
            var nearObstacleAboveFourSquare = SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X,
                y: directlyBelowObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleAboveFourSquare = SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X,
                y: directlyBelowObstacle.Y - TestConstants.SquareWidth + relativeCursorPosition.Y + yOffset);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleAboveFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, directlyBelowObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, directlyBelowObstacle.Y);
        }

        [TestMethod]
        public void TestWeCanDetectAShapeInTheWayWhenWeArePerfectlyAlignedWithGrid()
        {
            // Arrange
            var topLeftCornerOfFourSquare = SquareFillPoint(
                x: _shapeSetBuilder.LeftCornerShape.TopLeftCornerX,
                y: _shapeSetBuilder.LeftCornerShape.TopLeftCornerY);
            var directlyBelowObstacle = SquareFillPoint(
                x: _shapeSetBuilder.BottomLeftBorderSquares[0].X * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomLeftBorderSquares[0].Y + 1) * TestConstants.SquareWidth);
            var insideObstacleAboveFourSquare = SquareFillPoint(
                x: directlyBelowObstacle.X,
                y: directlyBelowObstacle.Y - TestConstants.SquareWidth);
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            shapeController.StartMove(cursorPositionAtStart: topLeftCornerOfFourSquare);

            // Act
            shapeController.ContinueMove(newLocation: directlyBelowObstacle);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Asserter.AreEqual(shapeController.CurrentShapeCornerX, directlyBelowObstacle.X);
            Asserter.AreEqual(shapeController.CurrentShapeCornerY, directlyBelowObstacle.Y);
        }

        [TestMethod]
        public void TestIfShapeEndsUpInAnAlreadyOccupiedLocationThenItWillSnapToTheLastValidLocation()
        {
            // Arrange
            var shapeController = ShapeController(shapeSet: _shapeSet, occupiedGridSquares: _occupiedGridSquares);
            var lastValidLocation = SquareFillPoint(
                x: _insideBorder.X + 2 * TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var topLeftCornerLastValidLocation = SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + 2 * TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var shapeToMove = shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            shapeController.ContinueMove(newLocation: lastValidLocation);

            // Act
            shapeController.EndMove(finalLocation: _insideBorder);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCornerX, topLeftCornerLastValidLocation.X);
            Asserter.AreEqual(shapeToMove.TopLeftCornerY, topLeftCornerLastValidLocation.Y);
        }

        [TestMethod]
        public void TestCursorPositionCanBeCalculatedAccordingToTopLeftCorner()
        {
            // Arrange
            var cursorPositionAtStart = SquareFillPoint(
                x: _topLeftCornerSingleSquare.X + 15,
                y: _topLeftCornerSingleSquare.Y + 10);
            var newCursorPosition = SquareFillPoint(
                x: cursorPositionAtStart.X + 2,
                y: cursorPositionAtStart.Y + 3);
            _shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);
            _shapeController.ContinueMove(newLocation: newCursorPosition);

            // Act
            SquareFillPoint calculatedCursorPosition = _shapeController.CalculatePreviousCursorPosition();

            // Assert
            Asserter.AreEqual(calculatedCursorPosition.X, newCursorPosition.X);
            Asserter.AreEqual(calculatedCursorPosition.Y, newCursorPosition.Y);
        }
		
		[TestMethod]
		public void TestNewShapeCentreWillBeNewCursorPositionWhenCursorIsInCentreOfShape() {
			// Arrange
			var cursorPositionAtStart = SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX + TestConstants.SquareWidth/2,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY + TestConstants.SquareWidth/2);
			var newCursorPosition = SquareFillPoint(
				x: (7 * TestConstants.SquareWidth) - 10,
				y: (8 * TestConstants.SquareWidth) - 10);
            _shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeController.ContinueMove(newLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestNewTopLeftCornerWillBeNewCursorPositionAdjustedAccordingToPositionOfCursorWithinShape() {
			// Arrange
            var originalTopLeftCorner = _cornerOfDefaultSingleSquare;
			var cursorPositionAtStart = SquareFillPoint(
				x: originalTopLeftCorner.X + 10,
				y: originalTopLeftCorner.Y + 15);
			var horizontalMovement = 35;
			var verticalmovement = 30;
			var newCursorPosition = SquareFillPoint(
				x: cursorPositionAtStart.X + horizontalMovement,
				y: cursorPositionAtStart.Y + verticalmovement);
            _shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeController.ContinueMove(newLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, originalTopLeftCorner.X + horizontalMovement);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, originalTopLeftCorner.Y + verticalmovement);
		}
		
		[TestMethod]
		public void TestShapeCornerIsMovedCorrectlyWhenCursorMovementIsNegative() {
			// Arrange
			var originalCornerOfShape = _cornerOfDefaultSingleSquare;
			var cursorPositionAtStart = SquareFillPoint(
				x: originalCornerOfShape.X + 5,
				y: originalCornerOfShape.Y + 25);
			var horizontalMovement = -15;
			var verticalmovement = -25;
			var newCursorPosition = SquareFillPoint(
				x: cursorPositionAtStart.X + horizontalMovement,
				y: cursorPositionAtStart.Y + verticalmovement);
            _shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeController.ContinueMove(newLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, originalCornerOfShape.X + horizontalMovement);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, originalCornerOfShape.Y + verticalmovement);
		}
		
		[TestMethod]
		public void TestSnapToGridIfCursorDoesntMoveAndCursorIsInCentreOfShapeAndCursorInCentreOfAGridSquareThenNewShapeCentreEqualsOriginalCursorPosition(){
			// Arrange
            var cursorPositionAtStart = _centreOfDefaultSingleSquare;
            var newCursorPosition = cursorPositionAtStart;
            _shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);

            // Assert
            Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestSnapToGridIfNewCursorIsInCentreOfAGridSquareAndCursorIsInCentreOfShapeThenNewShapeCentreEqualsNewCursorPosition()    {
			// Arrange
			var cursorPositionAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth / 2,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth / 2);
            _shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewPositionIsInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenNoSnapOccurs(){
            // Arrange
            var relativeCursorPosition = SquareFillPoint(
                x: 10,
                y: -5);
            var cursorPositionAtStart = SquareFillPoint(
                x: _centreOfDefaultSingleSquare.X + relativeCursorPosition.X, 
                y: _centreOfDefaultSingleSquare.Y + relativeCursorPosition.Y);
			var newCursorPosition = SquareFillPoint(
                x: _centreOfDefaultSingleSquare.X + ShapeConstants.SquareWidth + relativeCursorPosition.X,
                y: _centreOfDefaultSingleSquare.Y + ShapeConstants.SquareWidth + relativeCursorPosition.Y);
            _shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X - relativeCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y - relativeCursorPosition.Y);
		}

        [TestMethod]
        public void TestIfNewShapeCentreIsInCentreOfAGridSquareThenItWillNotSnap() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            _shapeController.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewPositionIsNotInCentreOfAGridSquareAfterAdjustmentForShapeCentreThenSnapToGridGoesToCentreOfNearestSquare(){
            // Arrange
            var relativeCursorPosition = SquareFillPoint(
                x: 10,
                y: 10);
            var cursorPositionAtStart = SquareFillPoint(
				x: _centreOfDefaultSingleSquare.X + relativeCursorPosition.X,
				y: _centreOfDefaultSingleSquare.Y + relativeCursorPosition.Y);
			var centreOfNearestSquare = SquareFillPoint(
				x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
			var newCursorPosition = SquareFillPoint(
				x: centreOfNearestSquare.X + relativeCursorPosition.X + 1,
				y: centreOfNearestSquare.Y + relativeCursorPosition.Y + 1);
            _shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, centreOfNearestSquare.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, centreOfNearestSquare.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsNotInCentreOfAGridSquareThenItWillSnapToCentreOfNearestSquare() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
		    var offset = 2;
            var centreOfNearestSquare = SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
                y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            var newCursorPosition = SquareFillPoint(
                x: centreOfNearestSquare.X + offset, 
                y: centreOfNearestSquare.Y + offset);
            _shapeController.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, centreOfNearestSquare.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, centreOfNearestSquare.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsToLeftOfContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = SquareFillPoint(
				x:TestConstants.ContainingRectangle.X - TestConstants.SquareWidth - TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth/2);
            _shapeController.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.CentreOfShapeY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestIfNewShapeCentreIsAboveContainingRectangleThenItWillNotBeSnappedBackInsideContainer() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var newCursorPosition = SquareFillPoint(
				x:TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
				y:TestConstants.ContainingRectangle.Y - TestConstants.SquareWidth - TestConstants.SquareWidth/2);
            _shapeController.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
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
			var newCursorPosition = SquareFillPoint(x:newCursorX, y:cursorAndCentreAtStart.Y);
            _shapeController.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
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
			var newCursorPosition = SquareFillPoint(x:cursorAndCornerAtStart.X, y:newCursorY);
            _shapeController.StartMove(cursorPositionAtStart: cursorAndCornerAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, newCursorPosition.X);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, newCursorPosition.Y);
		}
		
		[TestMethod]
		public void TestWhenSnappingToGridShouldRecalculateSquareOrigins() {
			// Arrange
			var cursorAndCentreAtStart = _centreOfDefaultSingleSquare;
			var horizontalMovement = 2*TestConstants.SquareWidth;
			var verticalMovement = TestConstants.SquareWidth;
			var newCursorPosition = SquareFillPoint(
				x:cursorAndCentreAtStart.X + horizontalMovement,
				y:cursorAndCentreAtStart.Y + verticalMovement);
            var originalOrigin = SquareFillPoint(
                x: _shapeSetBuilder.SingleSquareShape.TopLeftCornerX,
                y: _shapeSetBuilder.SingleSquareShape.TopLeftCornerY);
            _shapeController.StartMove(cursorPositionAtStart: cursorAndCentreAtStart);

            // Act
            _shapeController.EndMove(finalLocation: newCursorPosition);
			
			// Assert
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerX, originalOrigin.X + horizontalMovement);
			Asserter.AreEqual(_shapeSetBuilder.SingleSquareShape.TopLeftCornerY, originalOrigin.Y + verticalMovement);
		}
	}
}
