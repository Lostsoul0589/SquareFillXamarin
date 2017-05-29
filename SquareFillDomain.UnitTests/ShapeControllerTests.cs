using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Controllers;
using SquareFillDomain.Models;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeControllerTests
    {
        private readonly SquareFillPoint _outsideContainingRectangleButInsideRightHydrant = new SquareFillPoint(
            x: ShapeConstants.SquareWidth/2,
            y: 2*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);

        private readonly SquareFillRect _containingRectangle = new SquareFillRect(
            x: ShapeConstants.ContainingRectangle.X,
            y: ShapeConstants.ContainingRectangle.Y,
            width: ShapeConstants.ContainingRectangle.Width,
            height: ShapeConstants.ContainingRectangle.Height);
        
        [Test]
        public void TestPerformanceOfStartMove()
        {
            // Arrange
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());

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
            var startingX = ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2;
            var insideContainingRectangle = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
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
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
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
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());

            // Assert
            Assert.AreEqual(shapeController.OccupiedGridSquares.Count, ShapeConstants.GridWidth);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0].Count, ShapeConstants.GridHeight);
        }

        [Test]
        public void TestAllShapesAreOccupyingGridSquaresAtStartOfGame()
        {
            // Arrange & Act
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());

            // Assert
            foreach (var shape in shapeController.ShapeSet.Shapes)
            {
                foreach (var square in shape.Squares)
                {
                    var xCoord = square.TopLeftCorner.X/ShapeConstants.SquareWidth;
                    var yCoord = square.TopLeftCorner.Y/ShapeConstants.SquareWidth;
                    Assert.AreEqual(shapeController.OccupiedGridSquares[xCoord][yCoord].Occupied, true);
                }
            }
            Assert.AreEqual(shapeController.OccupiedGridSquares[0][0].Occupied, false);
        }

        [Test]
        public void TestWhenShapeStaysOutsideGameGridThenSquaresAreStillOccupied()
        {
            // Arrange
            var outsideContainingRectangleButInsideFourSquare = new SquareFillPoint(
                x: 7*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2,
                y: 2*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var stillOutsideContainingRectangle = new SquareFillPoint(
                x: 11*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2,
                y: 18*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: outsideContainingRectangleButInsideFourSquare);

            // Act
            shapeController.EndMove(finalLocation: stillOutsideContainingRectangle);

            // Assert
            Assert.AreEqual(shapeController.OccupiedGridSquares[6][2].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[7][2].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[6][3].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[7][3].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[10][18].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[11][18].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[10][19].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[11][19].Occupied, true);
        }

        [Test]
        public void TestShapeSquaresAreVacatedAfterStartMove()
        {
            // Arrange
            var insideContainingRectangle = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            shapeController.EndMove(finalLocation: insideContainingRectangle);

            // Act
            shapeController.StartMove(cursorPositionAtStart: insideContainingRectangle);

            // Assert
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied, false);
        }

        [Test]
        public void TestSquaresAreStillVacatedIfShapeStartsOutsideGameGrid()
        {
            // Arrange
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            foreach (var occupiedGridSquareArray in shapeController.OccupiedGridSquares)
            {
                foreach (var occupiedGridSquare in occupiedGridSquareArray)
                {
                    occupiedGridSquare.Occupied = true;
                }
            }

            // Act
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Assert
            Assert.AreEqual(shapeController.OccupiedGridSquares[0][1].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0][2].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0][3].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[1][2].Occupied, false);
        }

        [Test]
        public void TestShapeSquaresAreOccupiedAfterEndMoveWhenShapeIsPerfectlyAlignedWithGrid()
        {
            // Arrange
            var insideContainingRectangle = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: insideContainingRectangle);

            // Assert
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied, true);
        }

        [Test]
        public void TestShapeSquaresAreOccupiedToSnappedLocationsAfterEndMove()
        {
            // Arrange
            var insideContainingRectangleButNotAlignedWithGrid = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2 + 5,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2 + 6);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: insideContainingRectangleButNotAlignedWithGrid);

            // Assert
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied, true);
        }

        [Test]
        public void TestShapeDoesNotMoveIfAnotherShapeIsInTheWay()
        {
            // Arrange
            var insideContainingRectangle = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2 + 1,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2 + 2);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.EndMove(finalLocation: insideContainingRectangle);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, _outsideContainingRectangleButInsideRightHydrant.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, _outsideContainingRectangleButInsideRightHydrant.Y);
        }

        [Test]
        public void TestWhenAnObstacleIsDetectedThenAnyFurtherMovesOutsideTheShapeAreIgnored()
        {
            // Arrange
            var insideContainingRectangle = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2 + 1,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2 + 2);
            var cursorOutsideLatestShapePosition = new SquareFillPoint(
                x: insideContainingRectangle.X + 3*ShapeConstants.SquareWidth,
                y: insideContainingRectangle.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: insideContainingRectangle);
            shapeController.ContinueMove(newLocation: cursorOutsideLatestShapePosition);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, _outsideContainingRectangleButInsideRightHydrant.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, _outsideContainingRectangleButInsideRightHydrant.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedAndCursorLeavesShapeItWillStartMovingAgainWhenCursorReturns()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var insideRightHydrantWithOffset = new SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X + relativeCursorPosition.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y + relativeCursorPosition.Y);
            var insideObstacle = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var immediatelyToTheRightOfObstacleWithOffset = new SquareFillPoint(
                x: insideObstacle.X + 2*ShapeConstants.SquareWidth + relativeCursorPosition.X,
                y: insideObstacle.Y + relativeCursorPosition.Y);
            var immediatelyToTheRightOfObstacleWithoutOffset = new SquareFillPoint(
                x: insideObstacle.X + 2*ShapeConstants.SquareWidth,
                y: insideObstacle.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideRightHydrantWithOffset);
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: immediatelyToTheRightOfObstacleWithOffset);
            shapeController.ContinueMove(newLocation: insideObstacle);
            shapeController.ContinueMove(newLocation: immediatelyToTheRightOfObstacleWithoutOffset);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, immediatelyToTheRightOfObstacleWithoutOffset.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, immediatelyToTheRightOfObstacleWithoutOffset.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedOnTheRightThenShapeWillSnapToRightHandBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 12;
            int yOffset = 13;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var insideObstacleToRightOfFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + 4*ShapeConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y + yOffset);
            var toLeftOfObstacle = new SquareFillPoint(
                x: insideObstacleToRightOfFourSquare.X - 2*ShapeConstants.SquareWidth,
                y: insideObstacleToRightOfFourSquare.Y);
            var directlyToLeftOfObstacle = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + 2*ShapeConstants.SquareWidth,
                y: topLeftCornerOfFourSquare.Y);
            var snappedToRightHandBorderWithObstacle = new SquareFillPoint(
                x: directlyToLeftOfObstacle.X,
                y: directlyToLeftOfObstacle.Y + yOffset);

            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: toLeftOfObstacle);
            shapeController.ContinueMove(newLocation: insideObstacleToRightOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, snappedToRightHandBorderWithObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToRightHandBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedOnTheLeftThenShapeWillSnapToLeftHandBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 12;
            int yOffset = 13;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6 * ShapeConstants.SquareWidth,
                y: 2 * ShapeConstants.SquareWidth);
            var directlyToRightOfObstacle = new SquareFillPoint(
                x: 5 * ShapeConstants.SquareWidth,
                y: 2 * ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var insideObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X - ShapeConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var nearObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var snappedToLeftHandBorderWithObstacle = new SquareFillPoint(
                x: directlyToRightOfObstacle.X,
                y: directlyToRightOfObstacle.Y + yOffset);

            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToLeftOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, snappedToLeftHandBorderWithObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToLeftHandBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedAboveThenShapeWillSnapToTopOfBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 10;
            int yOffset = 11;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var directlyBelowObstacle = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 6*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyBelowObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyBelowObstacle.Y - ShapeConstants.SquareWidth + relativeCursorPosition.Y + yOffset);
            var snappedToTopOfBorderWithObstacle = new SquareFillPoint(
                x: directlyBelowObstacle.X + xOffset,
                y: directlyBelowObstacle.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleAboveFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, snappedToTopOfBorderWithObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToTopOfBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedBelowThenShapeWillSnapToBottomOfBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 12;
            int yOffset = 13;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var directlyAboveObstacle = new SquareFillPoint(
                x: 7*ShapeConstants.SquareWidth,
                y: 12*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleBelowFourSquare = new SquareFillPoint(
                x: directlyAboveObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyAboveObstacle.Y + relativeCursorPosition.Y - yOffset);
            var insideObstacleBelowFourSquare = new SquareFillPoint(
                x: directlyAboveObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyAboveObstacle.Y + ShapeConstants.SquareWidth + relativeCursorPosition.Y - yOffset);
            var snappedToBottomOfBorderWithObstacle = new SquareFillPoint(
                x: directlyAboveObstacle.X + xOffset,
                y: directlyAboveObstacle.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleBelowFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleBelowFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, snappedToBottomOfBorderWithObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToBottomOfBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyTopLeftThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xAndYOffset = 10;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var directlyBottomRightOfObstacle = new SquareFillPoint(
                x: 4*ShapeConstants.SquareWidth,
                y: 6*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyTopLeftOfFourSquare = new SquareFillPoint(
                x: directlyBottomRightOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyBottomRightOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyTopLeftOfFourSquare = new SquareFillPoint(
                x:
                    directlyBottomRightOfObstacle.X - ShapeConstants.SquareWidth + relativeCursorPosition.X +
                    xAndYOffset,
                y:
                    directlyBottomRightOfObstacle.Y - ShapeConstants.SquareWidth + relativeCursorPosition.Y +
                    xAndYOffset);
            var snappedToTopLeftCornerByObstacle = new SquareFillPoint(
                x: directlyBottomRightOfObstacle.X,
                y: directlyBottomRightOfObstacle.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyTopLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyTopLeftOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, snappedToTopLeftCornerByObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToTopLeftCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyTopRightThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xAndYOffset = 10;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var directlyBottomLeftOfObstacle = new SquareFillPoint(
                x: 8*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyTopRightOfFourSquare = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X + relativeCursorPosition.X - xAndYOffset,
                y: directlyBottomLeftOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyTopRightOfFourSquare = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X + ShapeConstants.SquareWidth + relativeCursorPosition.X - xAndYOffset,
                y: directlyBottomLeftOfObstacle.Y - ShapeConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToTopRightCornerByObstacle = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X,
                y: directlyBottomLeftOfObstacle.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyTopRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyTopRightOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, snappedToTopRightCornerByObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToTopRightCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyBottomRightThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xAndYOffset = 10;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var directlyTopLeftOfObstacle = new SquareFillPoint(
                x: 9*ShapeConstants.SquareWidth,
                y: 11*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyBottomRightOfFourSquare = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopLeftOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyBottomRightOfFourSquare = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X + ShapeConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopLeftOfObstacle.Y + ShapeConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToBottomRightCornerByObstacle = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X,
                y: directlyTopLeftOfObstacle.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyBottomRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyBottomRightOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, snappedToBottomRightCornerByObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToBottomRightCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyBottomLeftThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xAndYOffset = 10;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var directlyTopRightOfObstacle = new SquareFillPoint(
                x: 4*ShapeConstants.SquareWidth,
                y: 11*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyBottomLeftOfFourSquare = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopRightOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyBottomLeftOfFourSquare = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X - ShapeConstants.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopRightOfObstacle.Y + ShapeConstants.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToBottomLeftCornerByObstacle = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X,
                y: directlyTopRightOfObstacle.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyBottomLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyBottomLeftOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, snappedToBottomLeftCornerByObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, snappedToBottomLeftCornerByObstacle.Y);
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
                x: _outsideContainingRectangleButInsideRightHydrant.X + initialRelativeCursorPosition.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y + initialRelativeCursorPosition.Y);
            var collidingWithSomething = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var cursorToTheRightOfObstacleWithFirstRelativeCursorPosition = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeConstants.SquareWidth + initialRelativeCursorPosition.X,
                y: collidingWithSomething.Y + initialRelativeCursorPosition.Y);
            var cursorToTheRightOfObstacleWithLaterRelativeCursorPosition = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeConstants.SquareWidth + laterRelativeCursorPosition.X,
                y: collidingWithSomething.Y + laterRelativeCursorPosition.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacleWithFirstRelativeCursorPosition);
            shapeController.ContinueMove(newLocation: collidingWithSomething);
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacleWithLaterRelativeCursorPosition);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, cursorToTheRightOfObstacleWithLaterRelativeCursorPosition.X
                                                          - laterRelativeCursorPosition.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, cursorToTheRightOfObstacleWithLaterRelativeCursorPosition.Y
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
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var cursorToTheRightOfObstacle = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeConstants.SquareWidth,
                y: collidingWithSomething.Y);
            var cursorInAnUnoccupiedSpace = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeConstants.SquareWidth,
                y: collidingWithSomething.Y + 2*ShapeConstants.SquareWidth);
            var cursorToTheRightOfUnoccupiedSpace = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeConstants.SquareWidth + 10,
                y: collidingWithSomething.Y + 2*ShapeConstants.SquareWidth);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacle);
            shapeController.ContinueMove(newLocation: collidingWithSomething);
            shapeController.ContinueMove(newLocation: cursorInAnUnoccupiedSpace);
            shapeController.ContinueMove(newLocation: cursorToTheRightOfUnoccupiedSpace);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, cursorToTheRightOfObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, cursorToTheRightOfObstacle.Y);
        }

        [Test]
        public void
            TestWhenShapeHasStoppedDueToObstacleAndCursorHasKeptMovingIntoFreeSpaceThenEndMoveWillNotSnapToFreeSpace()
        {
            // Arrange
            var startingInsideShape = new SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var collidingWithSomething = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var cursorToTheRightOfObstacle = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeConstants.SquareWidth,
                y: collidingWithSomething.Y);
            var cursorInAnUnoccupiedSpace = new SquareFillPoint(
                x: collidingWithSomething.X + 3*ShapeConstants.SquareWidth,
                y: collidingWithSomething.Y + 3*ShapeConstants.SquareWidth);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: cursorToTheRightOfObstacle);
            shapeController.ContinueMove(newLocation: collidingWithSomething);
            shapeController.EndMove(finalLocation: cursorInAnUnoccupiedSpace);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, cursorToTheRightOfObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, cursorToTheRightOfObstacle.Y);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenCursorIsNotInCentreOfShape()
        {
            // Arrange
            var cursorPositionAtStart = new SquareFillPoint(
                x: _outsideContainingRectangleButInsideRightHydrant.X + 10,
                y: _outsideContainingRectangleButInsideRightHydrant.Y);
            var bringsOurShapeIntoOccupiedSpace = new SquareFillPoint(
                x: ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2 + 10 + 1,
                y: ShapeConstants.SquareWidth + 1);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);
            shapeController.OccupiedGridSquares[2][1].Occupied = false;
            shapeController.OccupiedGridSquares[2][2].Occupied = false;
            shapeController.OccupiedGridSquares[2][3].Occupied = false;
            shapeController.OccupiedGridSquares[3][2].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: bringsOurShapeIntoOccupiedSpace);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, _outsideContainingRectangleButInsideRightHydrant.X);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeAreHorizontallyAlignedWithGrid()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int xOffset = 12;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var directlyToRightOfObstacle = new SquareFillPoint(
                x: 4*ShapeConstants.SquareWidth,
                y: 7*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y);
            var insideObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X - ShapeConstants.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToLeftOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, directlyToRightOfObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, directlyToRightOfObstacle.Y);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeAreVerticallyAlignedWithGrid()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            int yOffset = 11;
            var topLeftCornerOfFourSquare = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 2*ShapeConstants.SquareWidth);
            var directlyBelowObstacle = new SquareFillPoint(
                x: 6*ShapeConstants.SquareWidth,
                y: 6*ShapeConstants.SquareWidth);
            var insideFourSquare = new SquareFillPoint(
                x: topLeftCornerOfFourSquare.X + relativeCursorPosition.X,
                y: topLeftCornerOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X,
                y: directlyBelowObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X,
                y: directlyBelowObstacle.Y - ShapeConstants.SquareWidth + relativeCursorPosition.Y + yOffset);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleAboveFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.TopLeftCorner.X, directlyBelowObstacle.X);
            Assert.AreEqual(shapeToMove.TopLeftCorner.Y, directlyBelowObstacle.Y);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeArePerfectlyAlignedWithGrid()
        {
            // Arrange
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2,
                y: 2*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var directlyBelowObstacle = new SquareFillPoint(
                x: 7*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2,
                y: 6*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X,
                y: directlyBelowObstacle.Y - ShapeConstants.SquareWidth);
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            shapeController.StartMove(cursorPositionAtStart: centreOfFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: directlyBelowObstacle);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, directlyBelowObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, directlyBelowObstacle.Y);
        }

        [Test]
        public void TestIfShapeEndsUpInAnAlreadyOccupiedLocationThenItWillSnapToTheLastValidLocation()
        {
            // Arrange
            var shapeController = new ShapeController(squareViewFactory: new MockSquareFactory());
            var containingX = _containingRectangle.X/ShapeConstants.SquareWidth;
            var containingY = _containingRectangle.Y/ShapeConstants.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var lastValidLocation = new SquareFillPoint(
                x:
                    ShapeConstants.ContainingRectangle.X + 2*ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2 +
                    1,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2 + 1);
            var alreadyOccupiedLocation = new SquareFillPoint(
                x: ShapeConstants.ContainingRectangle.X + ShapeConstants.SquareWidth/2 + 1,
                y: ShapeConstants.ContainingRectangle.Y + ShapeConstants.SquareWidth + ShapeConstants.SquareWidth/2 + 1);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingRectangleButInsideRightHydrant);
            var shapeToMove = shapeController.ShapeToMove;
            shapeController.ContinueMove(newLocation: lastValidLocation);

            // Act
            shapeController.EndMove(finalLocation: alreadyOccupiedLocation);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, lastValidLocation.X - 1);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, lastValidLocation.Y - 1);
        }
    }
}