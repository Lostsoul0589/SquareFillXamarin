using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Controllers;
using SquareFillDomain.Models;

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeControllerTests
    {
        private int _squareWidth = ShapeSetBuilder.SquareWidth;

        private SquareFillPoint _outsideContainingSquareButInsideRightHydrant = new SquareFillPoint(
            x: ShapeSetBuilder.SquareWidth/2,
            y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);

        private SquareFillRect _containingSquare = new SquareFillRect(
            x: ShapeSetBuilder.ContainingSquare.X,
            y: ShapeSetBuilder.ContainingSquare.Y,
            width: ShapeSetBuilder.ContainingSquare.Width,
            height: ShapeSetBuilder.ContainingSquare.Height);
        
        [Test]
        public void TestPerformanceOfStartMove()
        {
            // Arrange
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);

            // Assert
            /*self.measure()	{
            // Act
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);
        }*/
        }

        [Test]
        public void TestPerformanceOfContinueMove()
        {
            // Arrange
            var startingX = ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2;
            var insideContainingSquare = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);

            // Assert
            // self.measure()	{
            // for(int count = 1; count <= 100; count) {
            // shapeController.ContinueMove(newLocation: insideContainingSquare);
            // insideContainingSquare.X = startingX + count;
            // }
            // }
        }

        [Test]
        public void TestPerformanceOfEndMove()
        {
            // Arrange
            var insideContainingSquare = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);

            // Assert
            // self.measure()	{
            // shapeController.EndMove(finalLocation: insideContainingSquare);
            // }
        }

        [Test]
        public void TestOccupiedGridSquareMatrixIsCorrectSizeAtStartOfGame()
        {
            // Arrange & Act
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);

            // Assert
            Assert.AreEqual(shapeController.OccupiedGridSquares.Count, ShapeSetBuilder.GridWidth);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0].Count, ShapeSetBuilder.GridHeight);
        }

        [Test]
        public void TestAllShapesAreOccupyingGridSquaresAtStartOfGame()
        {
            // Arrange & Act
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);

            // Assert
            foreach (var shape in shapeController.ShapeSet.Shapes)
            {
                foreach (var square in shape.Squares)
                {
                    var xCoord = square.Origin.X/ShapeSetBuilder.SquareWidth;
                    var yCoord = square.Origin.Y/ShapeSetBuilder.SquareWidth;
                    Assert.AreEqual(shapeController.OccupiedGridSquares[xCoord][yCoord].Occupied, true);
                }
            }
            Assert.AreEqual(shapeController.OccupiedGridSquares[0][0].Occupied, false);
        }

        [Test]
        public void TestWhenShapeStaysOutsideGameGridThenSquaresAreStillOccupied()
        {
            // Arrange
            var outsideContainingSquareButInsideFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var stillOutsideContainingSquare = new SquareFillPoint(
                x: 9*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 18*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: outsideContainingSquareButInsideFourSquare);

            // Act
            shapeController.EndMove(finalLocation: stillOutsideContainingSquare);

            // Assert
            Assert.AreEqual(shapeController.OccupiedGridSquares[6][2].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[7][2].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[6][3].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[7][3].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[8][18].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[9][18].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[8][19].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[9][19].Occupied, true);
        }

        [Test]
        public void TestShapeSquaresAreVacatedAfterStartMove()
        {
            // Arrange
            var insideContainingSquare = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);
            shapeController.EndMove(finalLocation: insideContainingSquare);

            // Act
            shapeController.StartMove(cursorPositionAtStart: insideContainingSquare);

            // Assert
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied, false);
            Assert.AreEqual(shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied, false);
        }

        [Test]
        public void TestSquaresAreStillVacatedIfShapeStartsOutsideGameGrid()
        {
            // Arrange
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            foreach (var occupiedGridSquareArray in shapeController.OccupiedGridSquares)
            {
                foreach (var occupiedGridSquare in occupiedGridSquareArray)
                {
                    occupiedGridSquare.Occupied = true;
                }
            }

            // Act
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);

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
            var insideContainingSquare = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: insideContainingSquare);

            // Assert
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied, true);
        }

        [Test]
        public void TestShapeSquaresAreOccupiedToSnappedLocationsAfterEndMove()
        {
            // Arrange
            var insideContainingSquareButNotAlignedWithGrid = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2 + 5,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2 + 6);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);

            // Act
            shapeController.EndMove(finalLocation: insideContainingSquareButNotAlignedWithGrid);

            // Assert
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied, true);
            Assert.AreEqual(shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied, true);
        }

        [Test]
        public void TestShapeDoesNotMoveIfAnotherShapeIsInTheWay()
        {
            // Arrange
            var insideContainingSquare = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2 + 1,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2 + 2);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.EndMove(finalLocation: insideContainingSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, _outsideContainingSquareButInsideRightHydrant.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, _outsideContainingSquareButInsideRightHydrant.Y);
        }

        [Test]
        public void TestWhenAnObstacleIsDetectedThenAnyFurtherMovesOutsideTheShapeAreIgnored()
        {
            // Arrange
            var insideContainingSquare = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2 + 1,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2 + 2);
            var cursorOutsideLatestShapePosition = new SquareFillPoint(
                x: insideContainingSquare.X + 3*ShapeSetBuilder.SquareWidth,
                y: insideContainingSquare.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: insideContainingSquare);
            shapeController.ContinueMove(newLocation: cursorOutsideLatestShapePosition);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, _outsideContainingSquareButInsideRightHydrant.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, _outsideContainingSquareButInsideRightHydrant.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedAndCursorLeavesShapeItWillStartMovingAgainWhenCursorReturns()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var insideRightHydrantWithOffset = new SquareFillPoint(
                x: _outsideContainingSquareButInsideRightHydrant.X + relativeCursorPosition.X,
                y: _outsideContainingSquareButInsideRightHydrant.Y + relativeCursorPosition.Y);
            var insideObstacle = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var immediatelyToTheRightOfObstacleWithOffset = new SquareFillPoint(
                x: insideObstacle.X + 2*ShapeSetBuilder.SquareWidth + relativeCursorPosition.X,
                y: insideObstacle.Y + relativeCursorPosition.Y);
            var immediatelyToTheRightOfObstacleWithoutOffset = new SquareFillPoint(
                x: insideObstacle.X + 2*ShapeSetBuilder.SquareWidth,
                y: insideObstacle.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideRightHydrantWithOffset);
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
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
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyToLeftOfObstacle = new SquareFillPoint(
                x: 9*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xOffset = 12;
            int yOffset = 13;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleToRightOfFourSquare = new SquareFillPoint(
                x: directlyToLeftOfObstacle.X + relativeCursorPosition.X - xOffset,
                y: directlyToLeftOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleToRightOfFourSquare = new SquareFillPoint(
                x: directlyToLeftOfObstacle.X + ShapeSetBuilder.SquareWidth + relativeCursorPosition.X - xOffset,
                y: directlyToLeftOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var snappedToRightHandBorderWithObstacle = new SquareFillPoint(
                x: directlyToLeftOfObstacle.X,
                y: directlyToLeftOfObstacle.Y + yOffset);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToRightOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, snappedToRightHandBorderWithObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, snappedToRightHandBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedOnTheLeftThenShapeWillSnapToLeftHandBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyToRightOfObstacle = new SquareFillPoint(
                x: 6*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xOffset = 12;
            int yOffset = 13;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X - ShapeSetBuilder.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y + yOffset);
            var snappedToLeftHandBorderWithObstacle = new SquareFillPoint(
                x: directlyToRightOfObstacle.X,
                y: directlyToRightOfObstacle.Y + yOffset);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToLeftOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, snappedToLeftHandBorderWithObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, snappedToLeftHandBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedAboveThenShapeWillSnapToTopOfBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyBelowObstacle = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 6*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xOffset = 10;
            int yOffset = 11;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyBelowObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyBelowObstacle.Y - ShapeSetBuilder.SquareWidth + relativeCursorPosition.Y + yOffset);
            var snappedToTopOfBorderWithObstacle = new SquareFillPoint(
                x: directlyBelowObstacle.X + xOffset,
                y: directlyBelowObstacle.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleAboveFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, snappedToTopOfBorderWithObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, snappedToTopOfBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedBelowThenShapeWillSnapToBottomOfBorderWithObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyAboveObstacle = new SquareFillPoint(
                x: 8*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 13*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xOffset = 12;
            int yOffset = 13;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleBelowFourSquare = new SquareFillPoint(
                x: directlyAboveObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyAboveObstacle.Y + relativeCursorPosition.Y - yOffset);
            var insideObstacleBelowFourSquare = new SquareFillPoint(
                x: directlyAboveObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyAboveObstacle.Y + ShapeSetBuilder.SquareWidth + relativeCursorPosition.Y - yOffset);
            var snappedToBottomOfBorderWithObstacle = new SquareFillPoint(
                x: directlyAboveObstacle.X + xOffset,
                y: directlyAboveObstacle.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleBelowFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleBelowFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, snappedToBottomOfBorderWithObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, snappedToBottomOfBorderWithObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyTopLeftThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyBottomRightOfObstacle = new SquareFillPoint(
                x: 5*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 6*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xAndYOffset = 10;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyTopLeftOfFourSquare = new SquareFillPoint(
                x: directlyBottomRightOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyBottomRightOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyTopLeftOfFourSquare = new SquareFillPoint(
                x:
                    directlyBottomRightOfObstacle.X - ShapeSetBuilder.SquareWidth + relativeCursorPosition.X +
                    xAndYOffset,
                y:
                    directlyBottomRightOfObstacle.Y - ShapeSetBuilder.SquareWidth + relativeCursorPosition.Y +
                    xAndYOffset);
            var snappedToTopLeftCornerByObstacle = new SquareFillPoint(
                x: directlyBottomRightOfObstacle.X,
                y: directlyBottomRightOfObstacle.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyTopLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyTopLeftOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, snappedToTopLeftCornerByObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, snappedToTopLeftCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyTopRightThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyBottomLeftOfObstacle = new SquareFillPoint(
                x: 10*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 6*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xAndYOffset = 10;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyTopRightOfFourSquare = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X + relativeCursorPosition.X - xAndYOffset,
                y: directlyBottomLeftOfObstacle.Y + relativeCursorPosition.Y + xAndYOffset);
            var insideObstacleDiagonallyTopRightOfFourSquare = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X + ShapeSetBuilder.SquareWidth + relativeCursorPosition.X - xAndYOffset,
                y: directlyBottomLeftOfObstacle.Y - ShapeSetBuilder.SquareWidth + relativeCursorPosition.Y + xAndYOffset);
            var snappedToTopRightCornerByObstacle = new SquareFillPoint(
                x: directlyBottomLeftOfObstacle.X,
                y: directlyBottomLeftOfObstacle.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyTopRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyTopRightOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, snappedToTopRightCornerByObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, snappedToTopRightCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyBottomRightThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyTopLeftOfObstacle = new SquareFillPoint(
                x: 10*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 11*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xAndYOffset = 10;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyBottomRightOfFourSquare = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X + relativeCursorPosition.X - xAndYOffset,
                y: directlyTopLeftOfObstacle.Y + relativeCursorPosition.Y - xAndYOffset);
            var insideObstacleDiagonallyBottomRightOfFourSquare = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X + ShapeSetBuilder.SquareWidth + relativeCursorPosition.X - xAndYOffset,
                y: directlyTopLeftOfObstacle.Y + ShapeSetBuilder.SquareWidth + relativeCursorPosition.Y - xAndYOffset);
            var snappedToBottomRightCornerByObstacle = new SquareFillPoint(
                x: directlyTopLeftOfObstacle.X,
                y: directlyTopLeftOfObstacle.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyBottomRightOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyBottomRightOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, snappedToBottomRightCornerByObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, snappedToBottomRightCornerByObstacle.Y);
        }

        [Test]
        public void TestWhenObstacleIsDetectedDiagonallyBottomLeftThenShapeWillSnapToTopLeftCornerByObstacle()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyTopRightOfObstacle = new SquareFillPoint(
                x: 5*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 11*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xAndYOffset = 10;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleDiagonallyBottomLeftOfFourSquare = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopRightOfObstacle.Y + relativeCursorPosition.Y - xAndYOffset);
            var insideObstacleDiagonallyBottomLeftOfFourSquare = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X - ShapeSetBuilder.SquareWidth + relativeCursorPosition.X + xAndYOffset,
                y: directlyTopRightOfObstacle.Y + ShapeSetBuilder.SquareWidth + relativeCursorPosition.Y - xAndYOffset);
            var snappedToBottomLeftCornerByObstacle = new SquareFillPoint(
                x: directlyTopRightOfObstacle.X,
                y: directlyTopRightOfObstacle.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleDiagonallyBottomLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleDiagonallyBottomLeftOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, snappedToBottomLeftCornerByObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, snappedToBottomLeftCornerByObstacle.Y);
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
                x: _outsideContainingSquareButInsideRightHydrant.X + initialRelativeCursorPosition.X,
                y: _outsideContainingSquareButInsideRightHydrant.Y + initialRelativeCursorPosition.Y);
            var collidingWithSomething = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var cursorToTheRightOfObstacleWithFirstRelativeCursorPosition = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeSetBuilder.SquareWidth + initialRelativeCursorPosition.X,
                y: collidingWithSomething.Y + initialRelativeCursorPosition.Y);
            var cursorToTheRightOfObstacleWithLaterRelativeCursorPosition = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeSetBuilder.SquareWidth + laterRelativeCursorPosition.X,
                y: collidingWithSomething.Y + laterRelativeCursorPosition.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
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
                x: _outsideContainingSquareButInsideRightHydrant.X,
                y: _outsideContainingSquareButInsideRightHydrant.Y);
            var collidingWithSomething = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var cursorToTheRightOfObstacle = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeSetBuilder.SquareWidth,
                y: collidingWithSomething.Y);
            var cursorInAnUnoccupiedSpace = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeSetBuilder.SquareWidth,
                y: collidingWithSomething.Y + 2*ShapeSetBuilder.SquareWidth);
            var cursorToTheRightOfUnoccupiedSpace = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeSetBuilder.SquareWidth + 10,
                y: collidingWithSomething.Y + 2*ShapeSetBuilder.SquareWidth);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
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
                x: _outsideContainingSquareButInsideRightHydrant.X,
                y: _outsideContainingSquareButInsideRightHydrant.Y);
            var collidingWithSomething = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var cursorToTheRightOfObstacle = new SquareFillPoint(
                x: collidingWithSomething.X + 2*ShapeSetBuilder.SquareWidth,
                y: collidingWithSomething.Y);
            var cursorInAnUnoccupiedSpace = new SquareFillPoint(
                x: collidingWithSomething.X + 3*ShapeSetBuilder.SquareWidth,
                y: collidingWithSomething.Y + 3*ShapeSetBuilder.SquareWidth);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: startingInsideShape);
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
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
                x: _outsideContainingSquareButInsideRightHydrant.X + 10,
                y: _outsideContainingSquareButInsideRightHydrant.Y);
            var bringsOurShapeIntoOccupiedSpace = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2 + 10 + 1,
                y: ShapeSetBuilder.SquareWidth + 1);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: cursorPositionAtStart);
            shapeController.OccupiedGridSquares[2][1].Occupied = false;
            shapeController.OccupiedGridSquares[2][2].Occupied = false;
            shapeController.OccupiedGridSquares[2][3].Occupied = false;
            shapeController.OccupiedGridSquares[3][2].Occupied = true;
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: bringsOurShapeIntoOccupiedSpace);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, _outsideContainingSquareButInsideRightHydrant.X);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeAreHorizontallyAlignedWithGrid()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyToRightOfObstacle = new SquareFillPoint(
                x: 5*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int xOffset = 12;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y);
            var insideObstacleToLeftOfFourSquare = new SquareFillPoint(
                x: directlyToRightOfObstacle.X - ShapeSetBuilder.SquareWidth + relativeCursorPosition.X + xOffset,
                y: directlyToRightOfObstacle.Y + relativeCursorPosition.Y);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleToLeftOfFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleToLeftOfFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, directlyToRightOfObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, directlyToRightOfObstacle.Y);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeAreVerticallyAlignedWithGrid()
        {
            // Arrange
            var relativeCursorPosition = new SquareFillPoint(x: 1, y: 2);
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyBelowObstacle = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 6*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            int yOffset = 11;
            var insideFourSquare = new SquareFillPoint(
                x: centreOfFourSquare.X + relativeCursorPosition.X,
                y: centreOfFourSquare.Y + relativeCursorPosition.Y);
            var nearObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X,
                y: directlyBelowObstacle.Y + relativeCursorPosition.Y + yOffset);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X + relativeCursorPosition.X,
                y: directlyBelowObstacle.Y - ShapeSetBuilder.SquareWidth + relativeCursorPosition.Y + yOffset);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            shapeController.StartMove(cursorPositionAtStart: insideFourSquare);
            var shapeToMove = shapeController.ShapeToMove;

            // Act
            shapeController.ContinueMove(newLocation: nearObstacleAboveFourSquare);
            shapeController.ContinueMove(newLocation: insideObstacleAboveFourSquare);

            // Assert
            Assert.AreEqual(shapeToMove.CentreOfShape.X, directlyBelowObstacle.X);
            Assert.AreEqual(shapeToMove.CentreOfShape.Y, directlyBelowObstacle.Y);
        }

        [Test]
        public void TestWeCanDetectAShapeInTheWayWhenWeArePerfectlyAlignedWithGrid()
        {
            // Arrange
            var centreOfFourSquare = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var directlyBelowObstacle = new SquareFillPoint(
                x: 7*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2,
                y: 6*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2);
            var insideObstacleAboveFourSquare = new SquareFillPoint(
                x: directlyBelowObstacle.X,
                y: directlyBelowObstacle.Y - ShapeSetBuilder.SquareWidth);
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
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
            var shapeController = new ShapeController(
                squareViewFactory: new MockShapeFactory(),
                screenWidth: ShapeSetBuilder.ScreenWidth,
                screenHeight: ShapeSetBuilder.ScreenHeight);
            var containingX = _containingSquare.X/ShapeSetBuilder.SquareWidth;
            var containingY = _containingSquare.Y/ShapeSetBuilder.SquareWidth;
            shapeController.OccupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;
            shapeController.OccupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            var lastValidLocation = new SquareFillPoint(
                x:
                    ShapeSetBuilder.ContainingSquare.X + 2*ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2 +
                    1,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2 + 1);
            var alreadyOccupiedLocation = new SquareFillPoint(
                x: ShapeSetBuilder.ContainingSquare.X + ShapeSetBuilder.SquareWidth/2 + 1,
                y: ShapeSetBuilder.ContainingSquare.Y + ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth/2 + 1);
            shapeController.StartMove(cursorPositionAtStart: _outsideContainingSquareButInsideRightHydrant);
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