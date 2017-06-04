using System;
using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Controllers;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeControllerTests
    {
        private TestShapeSetBuilder _shapeSetBuilder;

        private SquareFillPoint _outsideContainingRectangleButAtCornerOfRightHydrant;
        private SquareFillPoint _outsideContainingRectangleButInsideRightHydrant;
        private SquareFillPoint _topLeftCornerInsideBorder;
        private SquareFillPoint _insideBorder;
        private readonly SquareFillPoint _relativeCursorPosition = new SquareFillPoint(x: 2, y: 3);

        [SetUp]
        public void Setup()
        {
            _shapeSetBuilder = new TestShapeSetBuilder(squareViewFactory: new MockSquareFactory());

            _outsideContainingRectangleButAtCornerOfRightHydrant = _shapeSetBuilder.RightHydrantShape.TopLeftCorner;

            _outsideContainingRectangleButInsideRightHydrant = new SquareFillPoint(
                x: _outsideContainingRectangleButAtCornerOfRightHydrant.X + _relativeCursorPosition.X,
                y: _outsideContainingRectangleButAtCornerOfRightHydrant.Y + _relativeCursorPosition.Y);

            _topLeftCornerInsideBorder = new SquareFillPoint(
                x: _shapeSetBuilder.LeftWallBorderSquares[0].X * TestConstants.SquareWidth,
                y: _shapeSetBuilder.LeftWallBorderSquares[0].Y * TestConstants.SquareWidth);

            _insideBorder = new SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + _relativeCursorPosition.X,
                y: _topLeftCornerInsideBorder.Y + _relativeCursorPosition.Y);
        }

        [Test]
        public void TestPerformanceOfStartMove()
        {
            // Arrange
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);

            // Assert
            /*self.measure()	{
            // Act
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
        }*/
        }

        [Test]
        public void TestPerformanceOfContinueMove()
        {
            // Arrange
            var startingX = TestConstants.SquareWidth + TestConstants.SquareWidth/2;
            var insideContainingRectangle = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
                y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth + TestConstants.SquareWidth/2);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Assert
            // self.measure()	{
            // for(int count = 1; count <= 100; count) {
            // shapeController.ContinueMove(newLocation: insideContainingRectangle);
            // insideContainingRectangle.X = startingX + count;
            // }
            // }
        }

        [Test]
        public void TestPerformanceOfEndMove()
        {
            // Arrange
            var insideContainingRectangle = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + TestConstants.SquareWidth/2,
                y: TestConstants.ContainingRectangle.Y + TestConstants.SquareWidth + TestConstants.SquareWidth/2);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Assert
            // self.measure()	{
            // shapeController.EndMove(finalLocation: insideContainingRectangle);
            // }
        }

        [Test]
        public void TestOccupiedGridSquareMatrixIsCorrectSizeAtStartOfGame()
        {
            // Arrange & Act
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);

            // Assert
            Asserter.AreEqual(shapeController.OccupiedGridSquares.Width(), TestConstants.GridWidth);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.Height(), TestConstants.GridHeight);
        }

        [Test]
        public void TestAllShapesAreOccupyingGridSquaresAtStartOfGame()
        {
            // Arrange & Act
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);

            // Assert
            foreach (var shape in shapeController.ShapeSet.Shapes)
            {
                foreach (var square in shape.Squares)
                {
                    var xCoord = square.TopLeftCorner.X/TestConstants.SquareWidth;
                    var yCoord = square.TopLeftCorner.Y/TestConstants.SquareWidth;
                    Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: xCoord, y: yCoord), true);
                }
            }
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0, y: 0), false);
        }

        [Test]
        public void TestWhenShapeStaysOutsideGameGridThenSquaresAreStillOccupied()
        {
            // Arrange
            var stillOutsideContainingRectangle = new SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X - 2 * TestConstants.SquareWidth,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var topLeftVacatedGridX = _shapeSetBuilder.RightHydrantShape.TopLeftCorner.X / TestConstants.SquareWidth;
            var topLeftVacatedGridY = _shapeSetBuilder.RightHydrantShape.TopLeftCorner.Y / TestConstants.SquareWidth;
            var topLeftOccupiedGridX = (_shapeSetBuilder.RightHydrantShape.TopLeftCorner.X / TestConstants.SquareWidth) - 2;
            var topLeftOccupiedGridY = topLeftVacatedGridY;
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: stillOutsideContainingRectangle);

            // Assert
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: topLeftVacatedGridX, y: topLeftVacatedGridY), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: topLeftVacatedGridX, y: topLeftVacatedGridY + 1), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: topLeftVacatedGridX, y: topLeftVacatedGridY + 2), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: topLeftVacatedGridX + 1, y: topLeftVacatedGridY + 1), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: topLeftOccupiedGridX, y: topLeftOccupiedGridY), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: topLeftOccupiedGridX, y: topLeftOccupiedGridY + 1), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: topLeftOccupiedGridX, y: topLeftOccupiedGridY + 2), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: topLeftOccupiedGridX + 1, y: topLeftOccupiedGridY + 1), true);
        }

        [Test]
        public void TestShapeSquaresAreVacatedAfterStartMove()
        {
            // Arrange
            var insideContainingRectangle = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + _relativeCursorPosition.X,
                y: TestConstants.ContainingRectangle.Y + _relativeCursorPosition.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            shapeController.EndMove(finalLocation: insideContainingRectangle);

            // Act
            shapeController.StartMove(cursorPositionAtStart: insideContainingRectangle);

            // Assert
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 0 + containingY), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 1 + containingY), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 2 + containingY), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 1 + containingX, y: 1 + containingY), false);
        }

        [Test]
        public void TestSquaresAreStillVacatedIfShapeStartsOutsideGameGrid()
        {
            // Arrange
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.OccupiedGridSquares.OccupyAllSquares();
            var topLeftGridX = _shapeSetBuilder.RightHydrantShape.TopLeftCorner.X / TestConstants.SquareWidth;
            var topLeftGridY = _shapeSetBuilder.RightHydrantShape.TopLeftCorner.Y / TestConstants.SquareWidth;

            // Act
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Assert
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + topLeftGridX, y: 0 + topLeftGridY), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + topLeftGridX, y: 0 + topLeftGridY + 1), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + topLeftGridX, y: 0 + topLeftGridY + 2), false);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + topLeftGridX + 1, y: 0 + topLeftGridY + 1), false);
        }

        [Test]
        public void TestShapeSquaresAreOccupiedAfterEndMoveWhenShapeIsPerfectlyAlignedWithGrid()
        {
            // Arrange
            var insideContainingRectangle = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + _relativeCursorPosition.X,
                y: TestConstants.ContainingRectangle.Y + _relativeCursorPosition.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: insideContainingRectangle);

            // Assert
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 0 + containingY), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 1 + containingY), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 2 + containingY), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 1 + containingX, y: 1 + containingY), true);
        }

        [Test]
        public void TestShapeSquaresAreOccupiedToSnappedLocationsAfterEndMove()
        {
            // Arrange
            var insideContainingRectangleButNotAlignedWithGrid = new SquareFillPoint(
                x: TestConstants.ContainingRectangle.X + _relativeCursorPosition.X + 5,
                y: TestConstants.ContainingRectangle.Y + _relativeCursorPosition.Y + 6);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: insideContainingRectangleButNotAlignedWithGrid);

            // Assert
            var containingX = TestConstants.ContainingRectangle.X/TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 0 + containingY), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 1 + containingY), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 0 + containingX, y: 2 + containingY), true);
            Asserter.AreEqual(shapeController.OccupiedGridSquares.IsSquareOccupied(x: 1 + containingX, y: 1 + containingY), true);
        }

        [Test]
        public void TestShapeDoesNotMoveIfAnotherShapeIsInTheWay()
        {
            // Arrange
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var shapeToMove = shapeController.ShapeToMove;
            var originalX = shapeToMove.TopLeftCorner.X;
            var originalY = shapeToMove.TopLeftCorner.Y;

            // Act
            shapeController.EndMove(finalLocation: _insideBorder);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, originalX);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, originalY);
        }

        [Test]
        public void TestWhenAnObstacleIsDetectedThenAnyFurtherMovesOutsideTheShapeAreIgnored()
        {
            // Arrange
            var cursorOutsideLatestShapePosition = new SquareFillPoint(
                x: _insideBorder.X + 3 * TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var shapeToMove = shapeController.ShapeToMove;
            var originalX = shapeToMove.TopLeftCorner.X;
            var originalY = shapeToMove.TopLeftCorner.Y;

            // Act
            shapeController.ContinueMove(newLocation: _insideBorder);
            shapeController.ContinueMove(newLocation: cursorOutsideLatestShapePosition);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, originalX);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, originalY);
        }

        [Test]
        public void TestWhenObstacleIsDetectedAndCursorLeavesShapeItWillStartMovingAgainWhenCursorReturns()
        {
            // Arrange
            int xOffset = 3;
            int yOffset = 4;
            var immediatelyToTheRightOfObstacleWithOffset = new SquareFillPoint(
                x: _insideBorder.X + TestConstants.SquareWidth + xOffset,
                y: _insideBorder.Y + yOffset);
            var immediatelyToTheRightOfObstacleWithoutOffset = new SquareFillPoint(
                x: _insideBorder.X + 2 * TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var topLeftCornerImmediatelyToTheRightOfObstacle = new SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: immediatelyToTheRightOfObstacleWithOffset);
            shapeController.ContinueMove(newLocation: _insideBorder);
            shapeController.ContinueMove(newLocation: immediatelyToTheRightOfObstacleWithoutOffset);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, topLeftCornerImmediatelyToTheRightOfObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, topLeftCornerImmediatelyToTheRightOfObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedOnTheRightThenShapeWillSnapToRightHandBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 12;
            int yOffset = 13;
            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var directlyToLeftOfObstacle = new SquareFillPoint(
                x: (_shapeSetBuilder.RightWallBorderSquares[2].X * TestConstants.SquareWidth) - 2 * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.RightWallBorderSquares[2].Y * TestConstants.SquareWidth));
            var insideObstacleToRightOfFourSquare = new SquareFillPoint(
                x: directlyToLeftOfObstacle.X + TestConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToLeftOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var toLeftOfObstacle = new SquareFillPoint(
                x: directlyToLeftOfObstacle.X + relativeCursorPosition.X - xOffset,
                y: directlyToLeftOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var snappedToRightHandBorderWithObstacle = new SquareFillPoint(
                x: directlyToLeftOfObstacle.X,
                y: directlyToLeftOfObstacle.Y + yOffset);

            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: toLeftOfObstacle);
            shapeController.ContinueMove(newLocation: insideObstacleToRightOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, snappedToRightHandBorderWithObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToRightHandBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedOnTheLeftThenShapeWillSnapToLeftHandBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 12;
            int yOffset = 13;
            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var directlyToRightOfObstacle = new SquareFillPoint(
                x: _shapeSetBuilder.RightHydrantShape.TopLeftCorner.X + 2*TestConstants.SquareWidth,
                y: _shapeSetBuilder.RightHydrantShape.TopLeftCorner.Y + TestConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var insideObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X - TestConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var nearObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var snappedToLeftHandBorderWithObstacle = new SquareFillPoint(
                x: directlyToRightOfObstacle.X,
                y: directlyToRightOfObstacle.Y + yOffset);

            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToLeftOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, snappedToLeftHandBorderWithObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToLeftHandBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedAboveThenShapeWillSnapToTopOfBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 10;
            int yOffset = 11;
            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var directlyBelowObstacle = new SquareFillPoint(
                x: _shapeSetBuilder.TopRowBorderSquares[3].X* TestConstants.SquareWidth,
                y: (_shapeSetBuilder.TopRowBorderSquares[3].Y + 1)* TestConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyBelowObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyBelowObstacle.Y - TestConstants.SquareWidth + relativeCursorPosition.Y + yOffset);
            var snappedToTopOfBorderWithObstacle = new SquareFillPoint(
                x: directlyBelowObstacle.X + xOffset,
                y: directlyBelowObstacle.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleAboveFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, snappedToTopOfBorderWithObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToTopOfBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedBelowThenShapeWillSnapToBottomOfBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 12;
            int yOffset = 13;

            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var insideObstacleBelowFourSquare = new SquareFillPoint(
                x: _shapeSetBuilder.LeftCornerShape.TopLeftCorner.X + relativeCursorPosition.X + xOffset,
                y: _shapeSetBuilder.LeftCornerShape.TopLeftCorner.Y + relativeCursorPosition.Y + yOffset);
            var directlyAboveObstacle = new SquareFillPoint(
                x: _shapeSetBuilder.LeftCornerShape.TopLeftCorner.X,
                y: _shapeSetBuilder.LeftCornerShape.TopLeftCorner.Y - 2*TestConstants.SquareWidth);
            var nearObstacleBelowFourSquare = new SquareFillPoint(
                x: directlyAboveObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyAboveObstacle.Y + relativeCursorPosition.Y - yOffset);
            var snappedToBottomOfBorderWithObstacle = new SquareFillPoint(
                x: directlyAboveObstacle.X + xOffset,
                y: directlyAboveObstacle.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleBelowFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleBelowFourSquare);

            // Assert
            Asserter.AreEqual(snappedToBottomOfBorderWithObstacle.X, shapeToMove.TopLeftCorner.X);
            Asserter.AreEqual(snappedToBottomOfBorderWithObstacle.Y, shapeToMove.TopLeftCorner.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyTopLeftThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xAndYOffset = 10;
            var topLeftCornerOfLeftCornerShape = _shapeSetBuilder.LeftCornerShape.TopLeftCorner;
            var directlyBottomRightOfObstacle = new SquareFillPoint(
                x: (_shapeSetBuilder.BottomLeftBorderSquares[1].X + 1) * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomLeftBorderSquares[1].Y + 1) * TestConstants.SquareWidth);
            var insideLeftCornerShape = new SquareFillPoint(
                x: topLeftCornerOfLeftCornerShape.X + relativeCursorPosition.X,
                y: topLeftCornerOfLeftCornerShape.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyTopLeftOfFourSquare = new SquareFillPoint(
                x: directlyBottomRightOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyBottomRightOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyTopLeftOfFourSquare = new SquareFillPoint(
                x: directlyBottomRightOfObstacle.X - TestConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyBottomRightOfObstacle.Y - TestConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToTopLeftCornerByObstacle = new SquareFillPoint(
                x: directlyBottomRightOfObstacle.X,
                y: directlyBottomRightOfObstacle.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideLeftCornerShape);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyTopLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyTopLeftOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, snappedToTopLeftCornerByObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToTopLeftCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyTopRightThenShapeWillSnapToTopRightCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xAndYOffset = 10;
            var topLeftCornerOfLeftCornerShape = _shapeSetBuilder.LeftCornerShape.TopLeftCorner;
            var directlyBottomLeftOfObstacle = new SquareFillPoint(
                x: (_shapeSetBuilder.BottomRightBorderSquares[0].X - 2) * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomRightBorderSquares[0].Y + 1) * TestConstants.SquareWidth);
            var insideLeftCornerShape = new SquareFillPoint(
                x: topLeftCornerOfLeftCornerShape.X + relativeCursorPosition.X,
                y: topLeftCornerOfLeftCornerShape.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyTopRightOfFourSquare = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X + relativeCursorPosition.X - xAndYOffset,
                y: directlyBottomLeftOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyTopRightOfFourSquare = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X + TestConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyBottomLeftOfObstacle.Y - TestConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToTopRightCornerByObstacle = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X,
                y: directlyBottomLeftOfObstacle.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideLeftCornerShape);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyTopRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyTopRightOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, snappedToTopRightCornerByObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToTopRightCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyBottomRightThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xAndYOffset = 10;
            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var directlyTopLeftOfObstacle = new SquareFillPoint(
                x: (_shapeSetBuilder.BottomRightBorderSquares[0].X * TestConstants.SquareWidth) - 2*TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomRightBorderSquares[0].Y * TestConstants.SquareWidth) - 2*TestConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyBottomRightOfFourSquare = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X + relativeCursorPosition.X - xAndYOffset,
                y: directlyTopLeftOfObstacle.Y + relativeCursorPosition.Y - xAndYOffset);
            var insideObstacleDiagonallyBottomRightOfFourSquare = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X + TestConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopLeftOfObstacle.Y + TestConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToBottomRightCornerByObstacle = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X,
                y: directlyTopLeftOfObstacle.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyBottomRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyBottomRightOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, snappedToBottomRightCornerByObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToBottomRightCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyBottomLeftThenShapeWillSnapToTopRightCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xAndYOffset = 10;
            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var directlyTopRightOfObstacle = new SquareFillPoint(
                x: (_shapeSetBuilder.BottomLeftBorderSquares[1].X * TestConstants.SquareWidth) + TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomLeftBorderSquares[1].Y * TestConstants.SquareWidth) - 2*TestConstants.SquareWidth);
            var insideObstacleDiagonallyBottomLeftOfFourSquare = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X - TestConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopRightOfObstacle.Y + TestConstants.SquareWidth + relativeCursorPosition.Y - xAndYOffset);
            var nearObstacleDiagonallyBottomLeftOfFourSquare = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopRightOfObstacle.Y + relativeCursorPosition.Y - xAndYOffset);
            var snappedToBottomLeftCornerByObstacle = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X,
                y: directlyTopRightOfObstacle.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyBottomLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyBottomLeftOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, snappedToBottomLeftCornerByObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToBottomLeftCornerByObstacle.Y);
        }

        [Test]
        public void
            TestWhenObstacleIsDetectedAndCursorLeavesShapeAndReturnsThenAllSubsequentMovementUsesNewRelativeCursorPosition
            ()
        {
            // Arrange
            var initialRelativeCursorPosition = new SquareFillPoint(x: 10, y: 11);
            var laterRelativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var startingInsideShape = new SquareFillPoint(
                x: _outsideContainingRectangleButAtCornerOfRightHydrant.X + initialRelativeCursorPosition.X,
                y: _outsideContainingRectangleButAtCornerOfRightHydrant.Y + initialRelativeCursorPosition.Y);
            var collidingWithSomething = new SquareFillPoint(
                x: _shapeSetBuilder.LeftWallBorderSquares[1].X * TestConstants.SquareWidth,
                y: _shapeSetBuilder.LeftWallBorderSquares[1].Y * TestConstants.SquareWidth);
            var cursorToTheRightOfObstacleWithFirstRelativeCursorPosition = new SquareFillPoint(
                x: collidingWithSomething.X + TestConstants.SquareWidth + initialRelativeCursorPosition.X,
                y: collidingWithSomething.Y + initialRelativeCursorPosition.Y);
            var cursorToTheRightOfObstacleWithLaterRelativeCursorPosition = new SquareFillPoint(
                x: collidingWithSomething.X + TestConstants.SquareWidth + laterRelativeCursorPosition.X,
                y: collidingWithSomething.Y + laterRelativeCursorPosition.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacleWithFirstRelativeCursorPosition);
            shapeController.ContinueMove(newLocation: collidingWithSomething);
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacleWithLaterRelativeCursorPosition);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, cursorToTheRightOfObstacleWithLaterRelativeCursorPosition.X
                                                          - laterRelativeCursorPosition.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, cursorToTheRightOfObstacleWithLaterRelativeCursorPosition.Y
                                                          - laterRelativeCursorPosition.Y);
        }

        [Test]
        public void
            TestWhenObstacleIsDetectedAndCursorLeavesShapeAndReturnsInADifferentUnoccupiedPlaceThenItWillNotThinkItIsInOurShape
            ()
        {
            // Arrange
            var startingInsideShape = new SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var collidingWithSomething = new SquareFillPoint(
                x: _shapeSetBuilder.LeftWallBorderSquares[1].X * TestConstants.SquareWidth,
                y: _shapeSetBuilder.LeftWallBorderSquares[1].Y * TestConstants.SquareWidth);
            var cursorToTheRightOfObstacle = new SquareFillPoint(
                x: _insideBorder.X + TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var topLeftCornerToTheRightOfObstacle = new SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var cursorInAnUnoccupiedSpace = new SquareFillPoint(
                x: collidingWithSomething.X + 3*TestConstants.SquareWidth,
                y: collidingWithSomething.Y + 3*TestConstants.SquareWidth);
            var cursorToTheRightOfUnoccupiedSpace = new SquareFillPoint(
                x: cursorInAnUnoccupiedSpace.X + 10,
                y: cursorInAnUnoccupiedSpace.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacle);
            shapeController.ContinueMove(newLocation: _insideBorder);
            shapeController.ContinueMove(newLocation: cursorInAnUnoccupiedSpace);
            shapeController.ContinueMove(newLocation: cursorToTheRightOfUnoccupiedSpace);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, topLeftCornerToTheRightOfObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, topLeftCornerToTheRightOfObstacle.Y);
        }

        [Test]
        public void
            TestWhenShapeHasStoppedDueToObstacleAndCursorHasKeptMovingIntoFreeSpaceThenEndMoveWillNotSnapToFreeSpace()
        {
            // Arrange
            var startingInsideShape = new SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var cursorToTheRightOfObstacle = new SquareFillPoint(
                x: _insideBorder.X + 2 * TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var topLeftCornerToTheRightOfObstacle = new SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + 2 * TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var cursorInAnUnoccupiedSpace = new SquareFillPoint(
                x: _insideBorder.X + 3 * TestConstants.SquareWidth,
                y: _insideBorder.Y + 3 * TestConstants.SquareWidth);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacle);
            shapeController.ContinueMove(newLocation: _insideBorder);
            shapeController.EndMove(finalLocation: cursorInAnUnoccupiedSpace);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, topLeftCornerToTheRightOfObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, topLeftCornerToTheRightOfObstacle.Y);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenCursorIsNotInCentreOfShape()
        {
            // Arrange
            var cursorPositionAtStart = new SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);
            var shapeToMove = shapeController.ShapeToMove;
            var originalTopLeftX = _shapeSetBuilder.RightHydrantShape.TopLeftCorner.X;

            // Act
            shapeController.ContinueMove(newLocation: _insideBorder);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, originalTopLeftX);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeAreHorizontallyAlignedWithGrid()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 12;
            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var directlyToRightOfObstacle = new SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y);
            var insideObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X - TestConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToLeftOfFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, directlyToRightOfObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, directlyToRightOfObstacle.Y);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeAreVerticallyAlignedWithGrid()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int yOffset = 11;
            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var directlyBelowObstacle = new SquareFillPoint(
                x: _shapeSetBuilder.BottomLeftBorderSquares[0].X * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomLeftBorderSquares[0].Y + 1) * TestConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X,
                y: directlyBelowObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X,
                y: directlyBelowObstacle.Y - TestConstants.SquareWidth + relativeCursorPosition.Y + yOffset);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleAboveFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, directlyBelowObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, directlyBelowObstacle.Y);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeArePerfectlyAlignedWithGrid()
        {
            // Arrange
            var topLeftCornerOfFourSquare = _shapeSetBuilder.FourSquareShape.TopLeftCorner;
            var directlyBelowObstacle = new SquareFillPoint(
                x: _shapeSetBuilder.BottomLeftBorderSquares[0].X * TestConstants.SquareWidth,
                y: (_shapeSetBuilder.BottomLeftBorderSquares[0].Y + 1) * TestConstants.SquareWidth);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X,
                y: directlyBelowObstacle.Y - TestConstants.SquareWidth);
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            shapeController.StartMove(cursorPositionAtStart: topLeftCornerOfFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: directlyBelowObstacle);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, directlyBelowObstacle.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, directlyBelowObstacle.Y);
        }

        [Test]
        public void TestIfShapeEndsUpInAnAlreadyOccupiedLocationThenItWillSnapToTheLastValidLocation()
        {
            // Arrange
            var shapeController = new ShapeController(shapeSetBuilder: _shapeSetBuilder);
            var lastValidLocation = new SquareFillPoint(
                x: _insideBorder.X + 2*TestConstants.SquareWidth,
                y: _insideBorder.Y);
            var topLeftCornerLastValidLocation = new SquareFillPoint(
                x: _topLeftCornerInsideBorder.X + 2*TestConstants.SquareWidth,
                y: _topLeftCornerInsideBorder.Y);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var shapeToMove = shapeController.ShapeToMove;
            shapeController.ContinueMove(newLocation: lastValidLocation);

            // Act
            shapeController.EndMove(finalLocation: _insideBorder);

            // Assert
            Asserter.AreEqual(shapeToMove.TopLeftCorner.X, topLeftCornerLastValidLocation.X);
            Asserter.AreEqual(shapeToMove.TopLeftCorner.Y, topLeftCornerLastValidLocation.Y);
        }
    }
}