//using NUnit.Framework;

using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;
using Linq = System.Collections.Generic; 

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeTests
    {
        private Linq.List<Square> _simpleSingleSquareList;
        private Linq.List<Square> _rightHydrantSquareList;
        private Linq.List<Square> _crossShapeSquareList;
        private Linq.List<Square> _threePoleSquareList;
        private Linq.List<Square> _twoPoleSquareList;
        private Linq.List<Square> _fourBarSquareList;
        private Linq.List<Square> _nineSquareSquareList;
        private Linq.List<Square> _leftHydrantSquareList;
        private Linq.List<Square> _upsideDownTSquareList;
        private Linq.List<Square> _rightWayUpTList;

        private readonly Grid _occupiedGridSquares = TestConstants.MakeGridSquares();

        [SetUp]
        public void Setup()
        {
            _occupiedGridSquares.VacateAllSquares();
            var squareFactory = new MockSquareFactory();

            _simpleSingleSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.SingleSquarePoints,
                squareFactory: squareFactory);

            _rightHydrantSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.RightHydrantPoints,
                squareFactory: squareFactory);

            _crossShapeSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.CrossShapePoints,
                squareFactory: squareFactory);

            _threePoleSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.ThreePolePoints,
                squareFactory: squareFactory);

            _twoPoleSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.TwoPolePoints,
                squareFactory: squareFactory);

            _fourBarSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.FourBarPoints,
                squareFactory: squareFactory);

            _nineSquareSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.NineSquarePoints,
                squareFactory: squareFactory);

            _leftHydrantSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.LeftHydrantPoints,
                squareFactory: squareFactory);

            _upsideDownTSquareList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.UpsideDownTPoints,
                squareFactory: squareFactory);

            _rightWayUpTList = TestShapeSetBuilder.MakeSquares(
                colour: SquareFillColour.Red,
                relativePointsTopLeftCorner: ShapeConstants.RightWayUpTPoints,
                squareFactory: squareFactory);
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
            for (int squareCount = 0; squareCount < _crossShapeSquareList.Count; squareCount++)
            {
                Asserter.AreEqual(_crossShapeSquareList[squareCount].SpriteCornerX,
                               newTopLeftCorner.X + (ShapeConstants.CrossShapePoints[squareCount].X * TestConstants.SquareWidth));
                Asserter.AreEqual(_crossShapeSquareList[squareCount].SpriteCornerY,
                               newTopLeftCorner.Y + (ShapeConstants.CrossShapePoints[squareCount].Y * TestConstants.SquareWidth));
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);
            foreach (var relativePoint in TestConstants.RightHydrantPoints)
            {
                _occupiedGridSquares.OccupyGridSquare(x: relativePoint.X, y: relativePoint.Y);
                _occupiedGridSquares.PlaceShapeInSquare(x: relativePoint.X, y: relativePoint.Y, shapeInSquare: shape);
            }
            // Occupy some other squares too, so we can check they're still occupied afterwards
            for (int count = 0; count < _occupiedGridSquares.Height; count++)
            {
                _occupiedGridSquares.OccupyGridSquare(x: _occupiedGridSquares.Width - 1, y: count);
            }

            // Act
            shape.VacateGridSquares(occupiedGridSquares: _occupiedGridSquares);

            // Assert
            for (int xCount = 0; xCount < _occupiedGridSquares.Width - 1; xCount++)
            {
                for (int yCount = 0; yCount < _occupiedGridSquares.Height; yCount++)
                {
                    Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: xCount, y: yCount), false);
                }
            }
            // Check the other occupied squares are still occupied
            for (int count = 0; count < _occupiedGridSquares.Height; count++)
            {
                Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: _occupiedGridSquares.Width - 1, y: count), true);
            }
        }

        [Test]
        public void TestOccupyGridSquaresWillOccupyOnlyTheSquaresOccupiedByTheShape()
        {
            // Arrange
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);

            // Act
            shape.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);

            // Assert
            foreach (var relativePoint in TestConstants.ThreePolePoints)
            {
                Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: relativePoint.X, y: relativePoint.Y), true);
            }
            for (int yCount = 3; yCount < _occupiedGridSquares.Height; yCount++)
            {
                Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: 0, y: yCount), false);
            }
            for (int xCount = 1; xCount < _occupiedGridSquares.Width; xCount++)
            {
                for (int yCount = 0; yCount < _occupiedGridSquares.Height; yCount++)
                {
                    Asserter.AreEqual(_occupiedGridSquares.IsSquareOccupied(x: xCount, y: yCount), false);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(
                    x: _threePoleSquareList[squareCount].TopLeftCornerX, 
                    y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(
                    x: _threePoleSquareList[squareCount].TopLeftCornerX, 
                    y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + xMovement * TestConstants.SquareWidth);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }
            _occupiedGridSquares.OccupyGridSquare(x: TestConstants.ThreePolePoints[0].X, y: TestConstants.ThreePolePoints[0].X);
            _occupiedGridSquares.OccupyGridSquare(x: TestConstants.ThreePolePoints[1].X, y: TestConstants.ThreePolePoints[1].X);
            _occupiedGridSquares.OccupyGridSquare(x: TestConstants.ThreePolePoints[2].X, y: TestConstants.ThreePolePoints[2].X);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }
            _occupiedGridSquares.OccupyGridSquare(x: 1 + TestConstants.ThreePolePoints[0].X, y: TestConstants.ThreePolePoints[0].Y);
            _occupiedGridSquares.OccupyGridSquare(x: 1 + TestConstants.ThreePolePoints[1].X, y: TestConstants.ThreePolePoints[1].Y);
            _occupiedGridSquares.OccupyGridSquare(x: 1 + TestConstants.ThreePolePoints[2].X, y: TestConstants.ThreePolePoints[2].Y);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }
            _occupiedGridSquares.OccupyGridSquare(x: 0, y: 3);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }
            _occupiedGridSquares.OccupyGridSquare(x: 0, y: 0);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }
            _occupiedGridSquares.OccupyGridSquare(x: 0, y: 0);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }
            _occupiedGridSquares.OccupyGridSquare(x: 1, y: 0);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }
            _occupiedGridSquares.OccupyGridSquare(x: 0, y: 3);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(
                    x: _threePoleSquareList[squareCount].TopLeftCornerX, y: _threePoleSquareList[squareCount].TopLeftCornerY));
            }
            _occupiedGridSquares.OccupyGridSquare(x: 1, y: 3);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _twoPoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.TwoPolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _twoPoleSquareList[squareCount].TopLeftCornerX, y: _twoPoleSquareList[squareCount].TopLeftCornerY));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.TwoPolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_twoPoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_twoPoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y - TestConstants.SquareWidth);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _twoPoleSquareList);
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            for (int squareCount = 0; squareCount < TestConstants.TwoPolePoints.Count; squareCount++)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: _twoPoleSquareList[squareCount].TopLeftCornerX, y: _twoPoleSquareList[squareCount].TopLeftCornerY));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.TwoPolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_twoPoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + TestConstants.SquareWidth);
                Asserter.AreEqual(_twoPoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _twoPoleSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 0, y: 0);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _twoPoleSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 1, y: 0);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 0, y: 1);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _threePoleSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 1, y: 1);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _fourBarSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 2, y: 0);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _fourBarSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 2, y: 1);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _nineSquareSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 4, y: 1);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _leftHydrantSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 0, y: 1);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 2, y: 1);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _upsideDownTSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 1, y: 0);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightWayUpTList);
            _occupiedGridSquares.OccupyGridSquare(x: 1, y: 2);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);

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
                y: topLeftCorner.Y + _occupiedGridSquares.Height * TestConstants.SquareWidth);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);

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
                x: topLeftCorner.X + _occupiedGridSquares.Width * TestConstants.SquareWidth,
                y: topLeftCorner.Y);
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 1, y: 0);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 0, y: 1);

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
            var shape = new Shape(
                topLeftCorner: topLeftCorner,
                squareDefinitions: _simpleSingleSquareList);
            _occupiedGridSquares.OccupyGridSquare(x: 1, y:1);

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }
	}
}
