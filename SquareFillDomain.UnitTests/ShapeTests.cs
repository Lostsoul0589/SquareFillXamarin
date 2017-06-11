//using NUnit.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquareFillDomain.Models;
using SquareFillDomain.UnitTests.TestUtils;
using SquareFillDomain.Utils;
using Linq = System.Collections.Generic; 

namespace SquareFillDomain.UnitTests
{
    [TestClass]
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

        [TestInitialize]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
            shape.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);

            // Assert
            for (int squareCount = 0; squareCount < _crossShapeSquareList.Count; squareCount++)
            {
                Asserter.AreEqual(_crossShapeSquareList[squareCount].SpriteCornerX,
                               newTopLeftCorner.X + (ShapeConstants.CrossShapePoints[squareCount].X * TestConstants.SquareWidth));
                Asserter.AreEqual(_crossShapeSquareList[squareCount].SpriteCornerY,
                               newTopLeftCorner.Y + (ShapeConstants.CrossShapePoints[squareCount].Y * TestConstants.SquareWidth));
            }
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestWillNotDetectCollisionWhenNewPositionIsAlongLeftEdgeOfGrid()
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
        }

        [TestMethod]
        public void TestWillNotDetectCollisionWhenNewPositionIsAlongRightEdgeOfGrid()
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
        }

        [TestMethod]
        public void TestWillNotDetectCollisionWhenNewPositionIsAlongTopEdgeOfGrid()
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
        }

        [TestMethod]
        public void TestWillNotDetectCollisionWhenNewPositionIsAlongBottomEdgeOfGrid()
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
        }

        [TestMethod]
        public void TestWillCalculateTopLeftCornersCorrectlyWhenNewPositionIsAlongLeftEdgeOfGrid()
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
            shape.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);

            // Assert
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y
                    + 2 * TestConstants.SquareWidth);
            }
        }

        [TestMethod]
        public void TestWillCalculateTopLeftCornersCorrectlyWhenNewPositionIsAlongRightEdgeOfGrid()
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
            shape.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);

            // Assert
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + xMovement * TestConstants.SquareWidth);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y
                    + 2 * TestConstants.SquareWidth);
            }
        }

        [TestMethod]
        public void TestWillCalculateTopLeftCornersCorrectlyWhenNewPositionIsAlongTopEdgeOfGrid()
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
            shape.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);

            // Assert
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
        public void TestWillCalculateTopLeftCornersCorrectlyWhenNewPositionIsAlongBottomEdgeOfGrid()
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
            shape.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);

            // Assert
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y
                    + yMovement * TestConstants.SquareWidth);
            }
        }

        [TestMethod]
        public void TestWillCalculateTopLeftCornersCorrectlyWhenNewShapeCentreIsInMiddleOfGrid()
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
            shape.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);

            // Assert
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + 2 * TestConstants.SquareWidth);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y
                    + 3 * TestConstants.SquareWidth);
            }
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
            for (int squareCount = 0; squareCount < TestConstants.ThreePolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_threePoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        // !! These tests should be reinstated when we start using grid coordinates for eerything instead of pixels
        /*[TestMethod]
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
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
            }
        }
		
        [TestMethod]
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
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
            }
        }
		
        [TestMethod]
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
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
            }
        }
		
        [TestMethod]
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
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
            }
        }
		
        [TestMethod]
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
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
            }
        }
		
        [TestMethod]
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
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
            }
        }
		
        [TestMethod]
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
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
            }
        }
		
        [TestMethod]
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
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Asserter.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[squareCount].Y);
            }
        }*/
        // !! These tests should be reinstated when we start using grid coordinates for eerything instead of pixels

        [TestMethod]
        public void TestWhenShapeHasOnlyMovedVerticallyThenTopLeftCornersAreUpdatedCorrectly()
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
            shape.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);

            // Assert
            for (int squareCount = 0; squareCount < TestConstants.TwoPolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_twoPoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X);
                Asserter.AreEqual(_twoPoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y - TestConstants.SquareWidth);
            }
        }

        [TestMethod]
        public void TestWhenShapeHasOnlyMovedHorizontallyThenTopLeftCornersAreUpdatedCorrectly()
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
            shape.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);

            // Assert
            for (int squareCount = 0; squareCount < TestConstants.TwoPolePoints.Count; squareCount++)
            {
                Asserter.AreEqual(_twoPoleSquareList[squareCount].TopLeftCornerX, originalSquareOrigins[squareCount].X
                    + TestConstants.SquareWidth);
                Asserter.AreEqual(_twoPoleSquareList[squareCount].TopLeftCornerY, originalSquareOrigins[squareCount].Y);
            }
        }

        [TestMethod]
        public void TestWhenShapeHasOnlyMovedVerticallyThenNoCollisionIsDetected()
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
        }

        [TestMethod]
        public void TestWhenShapeHasOnlyMovedHorizontallyThenNoCollisionIsDetected()
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, false);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }

        [TestMethod]
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
            var result = shape.CheckWhetherMovementIsPossible(occupiedGridSquares: _occupiedGridSquares, newTopLeftCorner: newTopLeftCorner);

            // Assert
            Asserter.AreEqual(result.ThereAreShapesInTheWay, true);
        }
	}
}
