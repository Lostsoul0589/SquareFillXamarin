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
        Linq.List<Square> _rightHydrantSquareList = new Linq.List<Square>
            {
                new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null),
                new Square(positionRelativeToParent: new SquareFillPoint(x: 1, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 1), sprite: null),
                new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 1), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 2), sprite: null),
                new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: -1), positionRelativeToParentCorner: new SquareFillPoint(x: 1, y: 1), sprite: null)
            };
        Linq.List<Square> _crossShapeSquareList = new Linq.List<Square> {
                new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:-1), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
                new Square(positionRelativeToParent: new SquareFillPoint(x:-1, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:-1, y:1), sprite: new MockSquareView()),
                new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:1), sprite: new MockSquareView()),
                new Square(positionRelativeToParent: new SquareFillPoint(x:1, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:1, y:1), sprite: new MockSquareView()),
                new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:1), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:2), sprite: new MockSquareView())
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
				centreOfShape: centreOfShape,
                topLeftCorner: topLeftCorner,
                squareDefinitions: new Linq.List<Square> { new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null) });
			
			// Act
			var isInShape = shape.IsInShape(point: centreOfShape);
			
			// Assert
			Asserter.AreEqual(isInShape, true);
		}

        [Test]
        public void TestAnyLocationInCentralSquareIsDefinedAsInsideShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                centreOfShape: centreOfShape,
                topLeftCorner: topLeftCorner,
                squareDefinitions: new Linq.List<Square> { new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null) });
            var pointInQuestion = new SquareFillPoint(x: centreOfShape.X + 10, y: centreOfShape.Y - 10);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Asserter.AreEqual(isInShape, true);
        }

        [Test]
        public void TestAnyLocationOutsideShapeIsNotDefinedAsInsideShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                centreOfShape: centreOfShape,
                topLeftCorner: topLeftCorner,
                squareDefinitions: new Linq.List<Square> { new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), positionRelativeToParentCorner: new SquareFillPoint(x: 0, y: 0), sprite: null) });
            var pointInQuestion = new SquareFillPoint(x: centreOfShape.X + 10, y: centreOfShape.Y - 50);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Asserter.AreEqual(isInShape, false);
        }

        [Test]
        public void TestAnyLocationInNonCentralSquareIsDefinedAsInsideShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(
                centreOfShape: centreOfShape,
                topLeftCorner: topLeftCorner,
                squareDefinitions: _rightHydrantSquareList);
            var pointInQuestion = new SquareFillPoint(x: centreOfShape.X + TestConstants.SquareWidth + 10, y: centreOfShape.Y - 10);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Asserter.AreEqual(isInShape, true);
        }

        [Test]
        public void TestWhenShapeIsMovedToNewLocationThenAllSpritesArePlacedRelativeToShapeCentre()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth + TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: TestConstants.SquareWidth, y: 0);
            var shape = new Shape(
                centreOfShape: centreOfShape,
                topLeftCorner: topLeftCorner,
                squareDefinitions: _crossShapeSquareList);
            var newCentreOfShape = new SquareFillPoint(x: 120, y: 160);

            // Act
            shape.PutShapeInNewLocation(newCentreOfShape: newCentreOfShape);

            // Assert
            foreach (var square in shape.Squares)
            {
                Asserter.AreEqual(square.Sprite.Centre().X,
                               newCentreOfShape.X + (square.PositionRelativeToParent.X * TestConstants.SquareWidth));
                Asserter.AreEqual(square.Sprite.Centre().Y,
                               newCentreOfShape.Y + (square.PositionRelativeToParent.Y * TestConstants.SquareWidth));
            }
        }

        [Test]
        public void TestWhenShapeIsMovedToNewLocationThenAllSpritesArePlacedRelativeToTopLeftCorner()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth + TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: TestConstants.SquareWidth, y: 0);
            var shape = new Shape(
                centreOfShape: centreOfShape,
                topLeftCorner: topLeftCorner,
                squareDefinitions: _crossShapeSquareList);
            var newTopLeftCorner = new SquareFillPoint(x: 120, y: 160);

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
        public void TestNumSquaresLeftOfCentreIsInitialisedAccordingToRelativePosition()
        {
            // Arrange
            int numSquaresLeftOfCentre = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: TestConstants.SquareWidth / 2, y: TestConstants.SquareWidth / 2),
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x: -numSquaresLeftOfCentre, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresLeftOfShapeCentre, numSquaresLeftOfCentre);
        }

        [Test]
        public void TestNumSquaresRightOfCentreIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresRightOfCentre = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: TestConstants.SquareWidth / 2, y: TestConstants.SquareWidth / 2),
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x: numSquaresRightOfCentre, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresRightOfShapeCentre, numSquaresRightOfCentre);
        }

        [Test]
        public void TestNumSquaresAboveCentreIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresAboveCentre = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: TestConstants.SquareWidth / 2, y: TestConstants.SquareWidth / 2),
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y: -numSquaresAboveCentre), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresAboveShapeCentre, numSquaresAboveCentre);
        }

        [Test]
        public void TestNumSquaresBelowCentreIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresBelowCentre = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: TestConstants.SquareWidth / 2, y: TestConstants.SquareWidth / 2),
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y: numSquaresBelowCentre), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresBelowShapeCentre, numSquaresBelowCentre);
        }

        [Test]
        public void TestNumSquaresLeftOfTopLeftCornerIsInitialisedAccordingToRelativePosition()
        {
            // Arrange
            int numSquaresLeftOfTopLeftCorner = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: TestConstants.SquareWidth / 2, y: TestConstants.SquareWidth / 2),
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x: -numSquaresLeftOfTopLeftCorner, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
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
                centreOfShape: new SquareFillPoint(x: TestConstants.SquareWidth / 2, y: TestConstants.SquareWidth / 2),
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x: numSquaresRightOfTopLeftCorner, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
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
                centreOfShape: new SquareFillPoint(x: TestConstants.SquareWidth / 2, y: TestConstants.SquareWidth / 2),
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y: -numSquaresAboveTopLeftCorner), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
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
                centreOfShape: new SquareFillPoint(x: TestConstants.SquareWidth / 2, y: TestConstants.SquareWidth / 2),
                topLeftCorner: new SquareFillPoint(x: 0, y: 0),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y: numSquaresBelowTopLeftCorner), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), positionRelativeToParentCorner: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Asserter.AreEqual(shape.NumSquaresBelowTopLeftCorner, numSquaresBelowTopLeftCorner);
        }

        [Test]
        public void TestWeAreWithinTheContainingRectangleIfAllEdgesAreWithinIt()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 2 * TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                  centreOfShape: centreOfShape,
                  topLeftCorner: topLeftCorner,
                  relativePoints: TestConstants.RightHydrantCentrePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X - TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X - TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.RightHydrantCentrePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X,
                y: TestConstants.CentreOfTopLeftGridSquare.Y - TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X,
                y: TestConstants.TopLeftGridSquare.Y - TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.RightHydrantCentrePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.ContainingRectangle.Width - TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.ContainingRectangle.Width,
                y: TestConstants.TopLeftGridSquare.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.RightHydrantCentrePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.ContainingRectangle.Height);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.ContainingRectangle.Height - TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.RightHydrantCentrePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.RightHydrantCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                              squareFactory: new MockSquareFactory());
            foreach (var relativePoint in TestConstants.RightHydrantCentrePoints)
            {
                _occupiedGridSquares[relativePoint.X][relativePoint.Y + 1].Occupied = true;
                _occupiedGridSquares[relativePoint.X][relativePoint.Y + 1].ShapeInSquare = shape;
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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            shape.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);

            // Assert
            foreach (var relativePoint in TestConstants.ThreePoleCentrePoints)
            {
                Asserter.AreEqual(_occupiedGridSquares[relativePoint.X][relativePoint.Y + 1].Occupied, true);
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
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsAlongLeftEdgeOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth,
                y: 3 * TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsAlongRightEdgeOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + TestConstants.ScreenWidth - 2 * TestConstants.SquareWidth,
                y: centreOfShape.Y + 2 * TestConstants.SquareWidth);
            var xMovement = (TestConstants.ScreenWidth / TestConstants.SquareWidth) - 2;
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsAlongTopEdgeOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 2 * TestConstants.SquareWidth,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsAlongBottomEdgeOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 2 * TestConstants.SquareWidth,
                y: centreOfShape.Y + TestConstants.ScreenHeight - 3 * TestConstants.SquareWidth);
            var yMovement = (TestConstants.ScreenHeight / TestConstants.SquareWidth) - 3;
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth + TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 2 * TestConstants.SquareWidth,
                y: centreOfShape.Y + 3 * TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][2 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + TestConstants.SquareWidth + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][4 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][5 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][6 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 4 * TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y + 3 * TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 2 * TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 2 * TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[2 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 2 * TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][4 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 2 * TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[2 + containingX][4 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10)
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
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
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var topLeftCorner = new SquareFillPoint(
                x: ShapeSetBuilder.TopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.TopLeftGridSquare.Y + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
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
        public void TestWhenShapeIsStickingOutOfGridOnLeftSideThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X - TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.FourSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.FourSquarePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeIsStickingOutOfGridOnRightSideThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.ContainingRectangle.Width - TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.ContainingRectangle.Width,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.FourSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.FourSquarePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[5 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[5 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeIsStickingOutOfGridOnTopSideThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y - TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y - TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.FourSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.FourSquarePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeIsStickingOutOfGridOnBottomSideThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.ContainingRectangle.Height - TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.ContainingRectangle.Height - TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.FourSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.FourSquarePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][5 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][5 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedVerticallyThenWeCanStillUpdateOrigins()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 2 * TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.TwoPoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.TwoPolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.X, originalSquareOrigins[count].X);
                Asserter.AreEqual(shape.Squares[count].TopLeftCorner.Y, originalSquareOrigins[count].Y
                    - TestConstants.SquareWidth);
            }
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedHorizontallyThenWeCanStillUpdateOrigins()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + TestConstants.SquareWidth,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.TwoPoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.TwoPolePoints,
                              squareFactory: new MockSquareFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.TopLeftCorner.X, y: square.TopLeftCorner.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

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
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + 2 * TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y + 2 * TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.TwoPoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.TwoPolePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedHorizontallyThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.TwoPoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.TwoPolePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurLeftSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurRightSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.ThreePoleCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.ThreePolePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[2 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurTopSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y + TestConstants.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.FourBarCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.FourBarPoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[2 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurBottomSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.FourBarCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.FourBarPoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[2 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItIsCompletelyInsideTheMovingShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X,
                y: TestConstants.TopLeftGridSquare.Y);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 2 * TestConstants.SquareWidth + TestConstants.SquareWidth / 2,
                y: centreOfShape.Y + TestConstants.SquareWidth / 2);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.FourSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.FourSquarePoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[3 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurLeftSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + 2 * TestConstants.SquareWidth,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + TestConstants.SquareWidth,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.LeftHydrantCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.LeftHydrantPoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurRightSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth + 1);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X,
                y: TestConstants.TopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.RightHydrantCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.RightHydrantPoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[2 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurTopSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y + TestConstants.SquareWidth);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.UpsideDownTCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.UpsideDownTPoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurBottomSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.CentreOfTopLeftGridSquare.X + TestConstants.SquareWidth + 1,
                y: TestConstants.CentreOfTopLeftGridSquare.Y);
            var topLeftCorner = new SquareFillPoint(
                x: TestConstants.TopLeftGridSquare.X + 1,
                y: TestConstants.TopLeftGridSquare.Y);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.RightWayUpTCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.RightWayUpTPoints,
                              squareFactory: new MockSquareFactory());
            var containingX = TestConstants.ContainingRectangle.X / TestConstants.SquareWidth;
            var containingY = TestConstants.ContainingRectangle.Y / TestConstants.SquareWidth;
            _occupiedGridSquares[1 + containingX][2 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheTopOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y - centreOfShape.Y - TestConstants.SquareWidth - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheBottomOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y + _occupiedGridSquares[0].Count * TestConstants.SquareWidth + TestConstants.SquareWidth + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheLeftEdgeOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - centreOfShape.X - TestConstants.SquareWidth - 1,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheRightEdgeOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + _occupiedGridSquares.Count * TestConstants.SquareWidth + TestConstants.SquareWidth + 1,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveLessThanOneSquareBeyondTheTopOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y - centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveLessThanOneSquareBeyondTheBottomOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y + _occupiedGridSquares[0].Count * TestConstants.SquareWidth + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveLessThanOneSquareBeyondTheLeftEdgeOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - centreOfShape.X - 1,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveLessThanOneSquareBeyondTheRightEdgeOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + _occupiedGridSquares.Count * TestConstants.SquareWidth + 1,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeAreHorizontallyAlignedWithTheGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[1][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeAreVerticallyAlignedWithTheGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[0][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeArePerfectlyAlignedWithTheGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: TestConstants.SquareWidth / 2,
                y: TestConstants.SquareWidth / 2);
            var topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + TestConstants.SquareWidth,
                y: centreOfShape.Y + TestConstants.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              topLeftCorner: topLeftCorner,
                              relativePoints: TestConstants.SingleSquareCentrePoints,
                              relativePointsTopLeftCorner: TestConstants.SingleSquarePoints,
                              squareFactory: new MockSquareFactory());
            _occupiedGridSquares[1][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins1(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Asserter.AreEqual(result.NoShapesAreInTheWay, false);
        }
	}
}
