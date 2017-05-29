using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;
using Linq = System.Collections.Generic; 

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeTests
    {
        Linq.List<Square> _simpleSingleSquareList = new Linq.List<Square>
        {
            new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null)
        };
        Linq.List<Square> _rightHydrantSquareList = new Linq.List<Square>
            {
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 1), sprite: null),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 2), sprite: null),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x: 1, y: 1), sprite: null)
            };
        Linq.List<Square> _crossShapeSquareList = new Linq.List<Square> {
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x:-1, y:1), sprite: new MockSquareView()),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:1), sprite: new MockSquareView()),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x:1, y:1), sprite: new MockSquareView()),
                new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:2), sprite: new MockSquareView())
            };
        private readonly Linq.List<Linq.List<GridSquare>> _occupiedGridSquares = TestConstants.MakeGridSquares();

        [SetUp]
        public void Setup()
        {
            foreach (var gridSquareArray in _occupiedGridSquares)
            {
                foreach(var gridSquare in gridSquareArray)
                {
                    gridSquare.Occupied = false;
                    gridSquare.ShapeInSquare = null;
                }
            }
        }

        [Test]
		public void TestCentreOfShapeIsDefinedAsInsideShape() 
        {
			// Arrange
			var centreOfShape = new SquareFillPoint(
				x: TestConstants.SquareWidth / 2, 
				y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);
			
			// Act
			var isInShape = shape.IsInShape(point: centreOfShape);
			
			// Assert
			Asserter.AreEqual(isInShape, true);
		}

        [Test]
        public void TestAnyLocationInCentralSquareIsDefinedAsInsideShape()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);
            var pointInQuestion = new SquareFillPoint(x: topLeftCorner.X + 10, y: topLeftCorner.Y + 11);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Asserter.AreEqual(isInShape, true);
        }

        [Test]
        public void TestAnyLocationOutsideShapeIsNotDefinedAsInsideShape()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList );
            var pointInQuestion = new SquareFillPoint(x: topLeftCorner.X + 10, y: topLeftCorner.Y - 10);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Asserter.AreEqual(isInShape, false);
        }

        [Test]
        public void TestAnyLocationInNonCornerSquareIsDefinedAsInsideShape()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);
            var pointInQuestion = new SquareFillPoint(
                x: topLeftCorner.X + TestConstants.SquareWidth + 10,
                y: topLeftCorner.Y + TestConstants.SquareWidth + 10);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Asserter.AreEqual(isInShape, true);
        }

        [Test]
        public void TestAnyLocationInNonCentralSquareIsDefinedAsInsideShape()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);
            var pointInQuestion = new SquareFillPoint(
                x: topLeftCorner.X + 10,
                y: topLeftCorner.Y + 10);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Asserter.AreEqual(isInShape, true);
        }

        [Test]
        public void TestWhenShapeIsMovedToNewLocationThenAllSpritesArePlacedRelativeToTopLeftCorner()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: TestConstants.SquareWidth, y: 0);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _crossShapeSquareList);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 80,
                y: topLeftCorner.Y + 90);

            // Act
            shape.MoveAllShapeSquares(newTopLeftCorner: newTopLeftCorner);

            // Assert
            foreach (var square in shape.Squares)
            {
                Asserter.AreEqual(square.Sprite.TopLeftCorner().X,
                               newTopLeftCorner.X + (square.PositionRelativeToParentCorner.X * TestConstants.SquareWidth));
                Asserter.AreEqual(square.Sprite.TopLeftCorner().Y,
                               newTopLeftCorner.Y + (square.PositionRelativeToParentCorner.Y * TestConstants.SquareWidth));
            }
        }

        [Test]
        public void TestNumSquaresLeftOfTopLeftCornerIsInitialisedAccordingToRelativePosition()
        {
            // Arrange
            int numSquaresLeftOfTopLeftCorner = 2;

            // Act
            var shape = new Shape(
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x: -numSquaresLeftOfTopLeftCorner, y:0), sprite: null),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresLeftOfTopLeftCorner, numSquaresLeftOfTopLeftCorner);
        }

        [Test]
        public void TestNumSquaresRightOfTopLeftCornerIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresRightOfTopLeftCorner = 2;

            // Act
            var shape = new Shape(
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x: numSquaresRightOfTopLeftCorner, y:0), sprite: null),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresRightOfTopLeftCorner, numSquaresRightOfTopLeftCorner);
        }

        [Test]
        public void TestNumSquaresAboveTopLeftCornerIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresAboveTopLeftCorner = 2;

            // Act
            var shape = new Shape(
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y: -numSquaresAboveTopLeftCorner), sprite: null),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresAboveTopLeftCorner, numSquaresAboveTopLeftCorner);
        }

        [Test]
        public void TestNumSquaresBelowTopLeftCornerIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresBelowTopLeftCorner = 2;

            // Act
            var shape = new Shape(
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y: numSquaresBelowTopLeftCorner), sprite: null),
                    new Square(positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresBelowTopLeftCorner, numSquaresBelowTopLeftCorner);
        }

        [Test]
        public void TestWeAreWithinTheContainingRectangleIfAllEdgesAreWithinIt()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                  topLeftCorner: topLeftCorner,
                  relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                  squareFactory: new MockSquareFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Asserter.AreEqual(result, true);
        }

        [Test]
        public void TestWeAreNotInTheContainingRectangleIfWeOverlapTheLeftEdge()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X - TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Asserter.AreEqual(result, false);
        }

        [Test]
        public void TestWeAreNotInTheContainingRectangleIfWeOverlapTheTopEdge()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X,
                y: TestConstants.TopLeftGridSquare.Y - TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Asserter.AreEqual(result, false);
        }

        [Test]
        public void TestWeAreNotInTheContainingRectangleIfWeOverlapTheRightEdge()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.ContainingRectangle.Width,
                y: TestConstants.TopLeftGridSquare.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Asserter.AreEqual(result, false);
        }

        [Test]
        public void TestWeAreNotInTheContainingRectangleIfWeOverlapTheBottomEdge()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.ContainingRectangle.Height - TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Asserter.AreEqual(result, false);
        }

        [Test]
        public void TestVacateGridSquaresWillVacateOnlyTheSquaresOccupiedByTheShape()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                              squareFactory: new MockSquareFactory());
            foreach (var relativePoint in TestConstants.RightHydrantPoints)
            {
                _occupiedGridSquares[relativePoint.X][relativePoint.Y].Occupied = true;
                _occupiedGridSquares[relativePoint.X][relativePoint.Y].ShapeInSquare = shape;
            }
            // Occupy some other squares too, so we can check they're still occupied afterwards
            for (int count = 0; count <= (_occupiedGridSquares[_occupiedGridSquares.Count - 1].Count - 1); count++)
            {
                _occupiedGridSquares[_occupiedGridSquares.Count - 1][count].Occupied = true;
            }

            // Act
            shape.VacateGridSquares(occupiedGridSquares: _occupiedGridSquares);

            // Assert
            for (int xCount = 0; xCount <= (_occupiedGridSquares.Count - 2); xCount++)
            {
                for (int yCount = 0; yCount <= (_occupiedGridSquares[xCount].Count - 1); yCount++)
                {
                    Asserter.AreEqual(_occupiedGridSquares[xCount][yCount].Occupied, false);
                }
            }
            // Check the other occupied squares are still occupied
            for (int count = 0; count <= (_occupiedGridSquares[_occupiedGridSquares.Count - 1].Count - 1); count++)
            {
                Asserter.AreEqual(_occupiedGridSquares[_occupiedGridSquares.Count - 1][count].Occupied, true);
            }
        }

        [Test]
        public void TestOccupyGridSquaresWillOccupyOnlyTheSquaresOccupiedByTheShape()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            shape.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);

            // Assert
            foreach (var relativePoint in TestConstants.ThreePolePoints)
            {
                Asserter.AreEqual(_occupiedGridSquares[relativePoint.X][relativePoint.Y].Occupied, true);
            }
            for (int yCount = 3; yCount <= (_occupiedGridSquares[0].Count - 1); yCount++)
            {
                Asserter.AreEqual(_occupiedGridSquares[0][yCount].Occupied, false);
            }
            for (int xCount = 1; xCount <= (_occupiedGridSquares.Count - 1); xCount++)
            {
                for (int yCount = 0; yCount <= (_occupiedGridSquares[xCount].Count - 1); yCount++)
                {
                    Asserter.AreEqual(_occupiedGridSquares[xCount][yCount].Occupied, false);
                }
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewPositionIsAlongLeftEdgeOfGrid()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: 0,
                y: 2 * TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y
                    + 2 * TestConstants.SquareWidth);
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewPositionIsAlongRightEdgeOfGrid()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + TestConstants.ScreenWidth - TestConstants.SquareWidth,
                y: topLeftCorner.Y + 2 * TestConstants.SquareWidth);
            var xMovement = (TestConstants.ScreenWidth / TestConstants.SquareWidth) - 1;
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X
                    + xMovement * TestConstants.SquareWidth);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y
                    + 2 * TestConstants.SquareWidth);
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewPositionIsAlongTopEdgeOfGrid()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 2 * TestConstants.SquareWidth,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewPositionIsAlongBottomEdgeOfGrid()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 2 * TestConstants.SquareWidth,
                y: topLeftCorner.Y + TestConstants.ScreenHeight - 3 * TestConstants.SquareWidth);
            var yMovement = (TestConstants.ScreenHeight / TestConstants.SquareWidth) - 3;
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y
                    + yMovement * TestConstants.SquareWidth);
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsInMiddleOfGrid()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 2 * TestConstants.SquareWidth,
                y: topLeftCorner.Y + 3 * TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y
                    + 3 * TestConstants.SquareWidth);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayOnTheLeft()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.SquareWidth,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X - 1,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            _occupiedGridSquares[TestConstants.ThreePolePoints[0].X][TestConstants.ThreePolePoints[0].X].Occupied = true;
            _occupiedGridSquares[TestConstants.ThreePolePoints[1].X][TestConstants.ThreePolePoints[1].X].Occupied = true;
            _occupiedGridSquares[TestConstants.ThreePolePoints[2].X][TestConstants.ThreePolePoints[2].X].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayOnTheRight()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 1,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            _occupiedGridSquares[1 + TestConstants.ThreePolePoints[0].X][TestConstants.ThreePolePoints[0].Y].Occupied = true;
            _occupiedGridSquares[1 + TestConstants.ThreePolePoints[1].X][TestConstants.ThreePolePoints[1].Y].Occupied = true;
            _occupiedGridSquares[1 + TestConstants.ThreePolePoints[2].X][TestConstants.ThreePolePoints[2].Y].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayBelow()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            _occupiedGridSquares[0][3].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayAbove()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: TestConstants.SquareWidth);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            _occupiedGridSquares[0][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayDiagonallyTopLeft()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.SquareWidth,
                y: TestConstants.SquareWidth);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X - 1,
                y: topLeftCorner.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            _occupiedGridSquares[0][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayDiagonallyTopRight()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: TestConstants.SquareWidth);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 1,
                y: topLeftCorner.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            _occupiedGridSquares[1][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayDiagonallyBottomLeft()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 1,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X - 1,
                y: topLeftCorner.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            _occupiedGridSquares[0][3].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayDiagonallyBottomRight()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 1,
                y: topLeftCorner.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            _occupiedGridSquares[1][3].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        // !! These tests should be reinstated when we start using grid coordinates for eerything instead of pixels
        /*[Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingLeft() {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingRight() {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10)
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingUp() {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDown() {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDiagonallyUpAndLeft() {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDiagonallyUpAndRight() {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDiagonallyDownAndLeft() {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDiagonallyDownAndRight() {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }*/
        // !! These tests should be reinstated when we start using grid coordinates for eerything instead of pixels

        [Test]
        public void TestWhenShapeHasOnlyMovedVerticallyThenWeCanStillUpdateOrigins()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0 + 1,
                y: TestConstants.SquareWidth + 1);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y - TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.TwoPolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y - TestConstants.SquareWidth);
            }
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedHorizontallyThenWeCanStillUpdateOrigins()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0 + 1,
                y: 0 + 1);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + TestConstants.SquareWidth,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.TwoPolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X
                    + TestConstants.SquareWidth);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedVerticallyThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0 + 1,
                y: TestConstants.SquareWidth);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.TwoPolePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[0][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedHorizontallyThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: 0 + 1);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 1,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.TwoPolePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[1][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurLeftSide()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.SquareWidth,
                y: 0 + 1);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X - 1,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[0][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurRightSide()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: 0 + 1);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 1,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[1][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurTopSide()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: TestConstants.SquareWidth);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.FourBarPoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[2][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurBottomSide()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0 + 1,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.FourBarPoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[2][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItIsCompletelyInsideTheMovingShape()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 3 * TestConstants.SquareWidth,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.NineSquarePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[4][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurLeftSide()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 2*TestConstants.SquareWidth,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X - 1,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.LeftHydrantPoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[0][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurRightSide()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0,
                y: 0 + 1);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 1,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[2][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurTopSide()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.SquareWidth + 1,
                y: TestConstants.SquareWidth);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.UpsideDownTPoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[1][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurBottomSide()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(
                x: 0 + 1,
                y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.RightWayUpTPoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[1][2].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheTopOfTheScreen()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheBottomOfTheScreen()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y + _occupiedGridSquares[0].Count * TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheLeftEdgeOfTheScreen()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X - 1,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheRightEdgeOfTheScreen()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + _occupiedGridSquares.Count * TestConstants.SquareWidth,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeAreHorizontallyAlignedWithTheGrid()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + 10,
                y: topLeftCorner.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[1][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeAreVerticallyAlignedWithTheGrid()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X,
                y: topLeftCorner.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[0][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeArePerfectlyAlignedWithTheGrid()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newTopLeftCorner = new SquareFillPoint(
                x: topLeftCorner.X + TestConstants.SquareWidth,
                y: topLeftCorner.Y + TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              topLeftCorner: topLeftCorner,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[1][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }
	}
}
