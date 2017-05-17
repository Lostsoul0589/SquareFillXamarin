using NUnit.Framework;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;
using SquareFillDomain.Utils;
using Linq = System.Collections.Generic; 

namespace SquareFillDomain.UnitTests
{
    [TestFixture]
    public class ShapeTests
    {
        private readonly Linq.List<Linq.List<GridSquare>> _occupiedGridSquares = ShapeSetBuilder.MakeGridSquares();
		
		[Test]
		public void TestCentreOfShapeIsDefinedAsInsideShape() 
        {
			// Arrange
			var centreOfShape = new SquareFillPoint(
				x: ShapeSetBuilder.SquareWidth/2, 
				y: ShapeSetBuilder.SquareWidth/2);
			var shape = new Shape(
				centreOfShape: centreOfShape,
                squareDefinitions: new Linq.List<Square> { new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), sprite: null) });
			
			// Act
			var isInShape = shape.IsInShape(point: centreOfShape);
			
			// Assert
			Assert.AreEqual(isInShape, true);
		}

        [Test]
        public void TestAnyLocationInCentralSquareIsDefinedAsInsideShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var shape = new Shape(
                centreOfShape: centreOfShape,
                squareDefinitions: new Linq.List<Square> { new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), sprite: null) });
            var pointInQuestion = new SquareFillPoint(x: centreOfShape.X + 10, y: centreOfShape.Y - 10);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Assert.AreEqual(isInShape, true);
        }

        [Test]
        public void TestAnyLocationOutsideShapeIsNotDefinedAsInsideShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var shape = new Shape(
                centreOfShape: centreOfShape,
                squareDefinitions: new Linq.List<Square> { new Square(positionRelativeToParent: new SquareFillPoint(x: 0, y: 0), sprite: null) });
            var pointInQuestion = new SquareFillPoint(x: centreOfShape.X + 10, y: centreOfShape.Y - 50);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Assert.AreEqual(isInShape, false);
        }

        [Test]
        public void TestAnyLocationInNonCentralSquareIsDefinedAsInsideShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var shape = new Shape(
                centreOfShape: centreOfShape,
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:1, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:1), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:-1), sprite: null)
                });
            var pointInQuestion = new SquareFillPoint(x: centreOfShape.X + ShapeSetBuilder.SquareWidth + 10, y: centreOfShape.Y - 10);

            // Act
            var isInShape = shape.IsInShape(point: pointInQuestion);

            // Assert
            Assert.AreEqual(isInShape, true);
        }

        [Test]
        public void TestWhenShapeIsMovedToNewLocationThenAllSpritesArePlacedRelativeToShapeCentre()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var squareDefinitions = new Linq.List<Square> {
                new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:-1), sprite: new MockSquareView()),
                new Square(positionRelativeToParent: new SquareFillPoint(x:-1, y:0), sprite: new MockSquareView()),
                new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), sprite: new MockSquareView()),
                new Square(positionRelativeToParent: new SquareFillPoint(x:1, y:0), sprite: new MockSquareView()),
                new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:1), sprite: new MockSquareView())
            };
            var shape = new Shape(
                centreOfShape: centreOfShape,
                squareDefinitions: squareDefinitions);
            var newCentreOfShape = new SquareFillPoint(x: 120, y: 160);

            // Act
            shape.PutShapeInNewLocation(newCentreOfShape: newCentreOfShape);

            // Assert
            foreach (var square in shape.Squares)
            {
                Assert.AreEqual(square.Sprite.Centre().X,
                               newCentreOfShape.X + (square.PositionRelativeToParent.X * ShapeSetBuilder.SquareWidth));
                Assert.AreEqual(square.Sprite.Centre().Y,
                               newCentreOfShape.Y + (square.PositionRelativeToParent.Y * ShapeSetBuilder.SquareWidth));
            }
        }

        [Test]
        public void TestNumSquaresLeftOfCentreIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresLeftOfCentre = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: ShapeSetBuilder.SquareWidth / 2, y: ShapeSetBuilder.SquareWidth / 2),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x: -numSquaresLeftOfCentre, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Assert.AreEqual(shape.NumSquaresLeftOfShapeCentre, numSquaresLeftOfCentre);
        }

        [Test]
        public void TestNumSquaresRightOfCentreIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresRightOfCentre = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: ShapeSetBuilder.SquareWidth / 2, y: ShapeSetBuilder.SquareWidth / 2),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x: numSquaresRightOfCentre, y:0), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Assert.AreEqual(shape.NumSquaresRightOfShapeCentre, numSquaresRightOfCentre);
        }

        [Test]
        public void TestNumSquaresAboveCentreIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresAboveCentre = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: ShapeSetBuilder.SquareWidth / 2, y: ShapeSetBuilder.SquareWidth / 2),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y: -numSquaresAboveCentre), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Assert.AreEqual(shape.NumSquaresAboveShapeCentre, numSquaresAboveCentre);
        }

        [Test]
        public void TestNumSquaresBelowCentreIsInitialisedAcordingToRelativePosition()
        {
            // Arrange
            int numSquaresBelowCentre = 2;

            // Act
            var shape = new Shape(
                centreOfShape: new SquareFillPoint(x: ShapeSetBuilder.SquareWidth / 2, y: ShapeSetBuilder.SquareWidth / 2),
                squareDefinitions: new Linq.List<Square> {
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y: numSquaresBelowCentre), sprite: null),
                    new Square(positionRelativeToParent: new SquareFillPoint(x:0, y:0), sprite: null)
                });

            // Assert
            Assert.AreEqual(shape.NumSquaresBelowShapeCentre, numSquaresBelowCentre);
        }

        [Test]
        public void TestWeAreWithinTheContainingRectangleIfAllEdgesAreWithinIt()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 2 * ShapeSetBuilder.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                  centreOfShape: centreOfShape,
                  relativePoints: ShapeSetBuilder.RightHydrantPoints,
                  squareFactory: new MockShapeFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Assert.AreEqual(result, true);
        }

        [Test]
        public void TestWeAreNotInTheContainingRectangleIfWeOverlapTheLeftEdge()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X - ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.RightHydrantPoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Assert.AreEqual(result, false);
        }

        [Test]
        public void TestWeAreNotInTheContainingRectangleIfWeOverlapTheTopEdge()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y - ShapeSetBuilder.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.RightHydrantPoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Assert.AreEqual(result, false);
        }

        [Test]
        public void TestWeAreNotInTheContainingRectangleIfWeOverlapTheRightEdge()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.ContainingSquare.Width - ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.RightHydrantPoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Assert.AreEqual(result, false);
        }

        [Test]
        public void TestWeAreNotInTheContainingRectangleIfWeOverlapTheBottomEdge()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.ContainingSquare.Height);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.RightHydrantPoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.WeStartedWithinTheContainingRectangle();

            // Assert
            Assert.AreEqual(result, false);
        }

        [Test]
        public void TestVacateGridSquaresWillVacateOnlyTheSquaresOccupiedByTheShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.RightHydrantPoints,
                              squareFactory: new MockShapeFactory());
            foreach (var relativePoint in ShapeSetBuilder.RightHydrantPoints)
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
                    Assert.AreEqual(_occupiedGridSquares[xCount][yCount].Occupied, false);
                }
            }
            // Check the other occupied squares are still occupied
            for (int count = 0; count <= (_occupiedGridSquares[_occupiedGridSquares.Count - 1].Count - 1); count++)
            {
                Assert.AreEqual(_occupiedGridSquares[_occupiedGridSquares.Count - 1][count].Occupied, true);
            }
        }

        [Test]
        public void TestOccupyGridSquaresWillOccupyOnlyTheSquaresOccupiedByTheShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            shape.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);

            // Assert
            foreach (var relativePoint in ShapeSetBuilder.ThreePolePoints)
            {
                Assert.AreEqual(_occupiedGridSquares[relativePoint.X][relativePoint.Y + 1].Occupied, true);
            }
            for (int yCount = 3; yCount <= (_occupiedGridSquares[0].Count - 1); yCount++)
            {
                Assert.AreEqual(_occupiedGridSquares[0][yCount].Occupied, false);
            }
            for (int xCount = 1; xCount <= (_occupiedGridSquares.Count - 1); xCount++)
            {
                for (int yCount = 0; yCount <= (_occupiedGridSquares[xCount].Count - 1); yCount++)
                {
                    Assert.AreEqual(_occupiedGridSquares[xCount][yCount].Occupied, false);
                }
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsAlongLeftEdgeOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth,
                y: 3 * ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y
                    + 2 * ShapeSetBuilder.SquareWidth);
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsAlongRightEdgeOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + ShapeSetBuilder.ScreenWidth - 2 * ShapeSetBuilder.SquareWidth,
                y: centreOfShape.Y + 2 * ShapeSetBuilder.SquareWidth);
            var xMovement = (ShapeSetBuilder.ScreenWidth / ShapeSetBuilder.SquareWidth) - 2;
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X
                    + xMovement * ShapeSetBuilder.SquareWidth);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y
                    + 2 * ShapeSetBuilder.SquareWidth);
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsAlongTopEdgeOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 2 * ShapeSetBuilder.SquareWidth,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X
                    + 2 * ShapeSetBuilder.SquareWidth);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsAlongBottomEdgeOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 2 * ShapeSetBuilder.SquareWidth,
                y: centreOfShape.Y + ShapeSetBuilder.ScreenHeight - 3 * ShapeSetBuilder.SquareWidth);
            var yMovement = (ShapeSetBuilder.ScreenHeight / ShapeSetBuilder.SquareWidth) - 3;
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X
                    + 2 * ShapeSetBuilder.SquareWidth);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y
                    + yMovement * ShapeSetBuilder.SquareWidth);
            }
        }

        [Test]
        public void TestWhenAttemptingToUpdateOriginsWillCalculateOriginsCorrectlyWhenNewShapeCentreIsInMiddleOfGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 2 * ShapeSetBuilder.SquareWidth,
                y: centreOfShape.Y + 3 * ShapeSetBuilder.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X
                    + 2 * ShapeSetBuilder.SquareWidth);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y
                    + 3 * ShapeSetBuilder.SquareWidth);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayOnTheLeft()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayOnTheRight()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][2 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayBelow()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + ShapeSetBuilder.SquareWidth + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][4 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][5 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][6 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayAbove()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 4 * ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;
            _occupiedGridSquares[0 + containingX][2 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayDiagonallyTopLeft()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 2 * ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayDiagonallyTopRight()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 2 * ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[2 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayDiagonallyBottomLeft()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 2 * ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][4 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestOriginsAreNotUpdatedIfAnotherShapeIsInTheWayDiagonallyBottomRight()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 2 * ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[2 + containingX][4 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        // !! These tests should be reinstated when we start using grid coordinates for eerything instead of pixels
        /*[Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingLeft() {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingRight() {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10)
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingUp() {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDown() {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDiagonallyUpAndLeft() {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDiagonallyUpAndRight() {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDiagonallyDownAndLeft() {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }
		
        [Test]
        public void TestOriginsAreNotUpdatedIfShapeHasNotCrossedAGridBoundaryWhenMovingDiagonallyDownAndRight() {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 10,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 10);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new List<SquareFillPoint>();
            foreach(var square in shape.Squares) {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }
			
            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);
			
            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count-1; count++) {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }*/
        // !! These tests should be reinstated when we start using grid coordinates for eerything instead of pixels

        [Test]
        public void TestWhenShapeIsStickingOutOfGridOnLeftSideThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.FourSquarePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeIsStickingOutOfGridOnRightSideThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.ContainingSquare.Width,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.FourSquarePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[5 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[5 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeIsStickingOutOfGridOnTopSideThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y - ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.FourSquarePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeIsStickingOutOfGridOnBottomSideThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.ContainingSquare.Height - ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.FourSquarePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][5 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][5 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedVerticallyThenWeCanStillUpdateOrigins()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 2 * ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - ShapeSetBuilder.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.TwoPolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y
                    - ShapeSetBuilder.SquareWidth);
            }
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedHorizontallyThenWeCanStillUpdateOrigins()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + ShapeSetBuilder.SquareWidth,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.TwoPolePoints,
                              squareFactory: new MockShapeFactory());
            var originalSquareOrigins = new Linq.List<SquareFillPoint>();
            foreach (var square in shape.Squares)
            {
                originalSquareOrigins.Add(new SquareFillPoint(x: square.Origin.X, y: square.Origin.Y));
            }

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, true);
            for (int count = 0; count <= shape.Squares.Count - 1; count++)
            {
                Assert.AreEqual(shape.Squares[count].Origin.X, originalSquareOrigins[count].X
                    + ShapeSetBuilder.SquareWidth);
                Assert.AreEqual(shape.Squares[count].Origin.Y, originalSquareOrigins[count].Y);
            }
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedVerticallyThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + 2 * ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.TwoPolePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestWhenShapeHasOnlyMovedHorizontallyThenWeCanStillDetectAnotherShapeInTheWay()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.TwoPolePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;
            _occupiedGridSquares[1 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurLeftSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurRightSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.ThreePolePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[2 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurTopSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.FourBarPoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[2 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItHasASquareStickingOutInTheMiddleOfOurBottomSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.FourBarPoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[2 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenItIsCompletelyInsideTheMovingShape()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 2 * ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth / 2,
                y: centreOfShape.Y + ShapeSetBuilder.SquareWidth / 2);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.FourSquarePoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[3 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurLeftSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + 2 * ShapeSetBuilder.SquareWidth,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.LeftHydrantPoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[0 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurRightSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth + 1);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 1,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.RightHydrantPoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[2 + containingX][1 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurTopSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y + ShapeSetBuilder.SquareWidth);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.UpsideDownTPoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[1 + containingX][0 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeHaveASquareStickingOutInTheMiddleOfOurBottomSide()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.CentreOfTopLeftGridSquare.X + ShapeSetBuilder.SquareWidth + 1,
                y: ShapeSetBuilder.CentreOfTopLeftGridSquare.Y);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.RightWayUpTPoints,
                              squareFactory: new MockShapeFactory());
            var containingX = ShapeSetBuilder.ContainingSquare.X / ShapeSetBuilder.SquareWidth;
            var containingY = ShapeSetBuilder.ContainingSquare.Y / ShapeSetBuilder.SquareWidth;
            _occupiedGridSquares[1 + containingX][2 + containingY].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheTopOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y - centreOfShape.Y - ShapeSetBuilder.SquareWidth - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheBottomOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y + _occupiedGridSquares[0].Count * ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheLeftEdgeOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - centreOfShape.X - ShapeSetBuilder.SquareWidth - 1,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveBeyondTheRightEdgeOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + _occupiedGridSquares.Count * ShapeSetBuilder.SquareWidth + ShapeSetBuilder.SquareWidth + 1,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveLessThanOneSquareBeyondTheTopOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y - centreOfShape.Y - 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveLessThanOneSquareBeyondTheBottomOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y + _occupiedGridSquares[0].Count * ShapeSetBuilder.SquareWidth + 1);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveLessThanOneSquareBeyondTheLeftEdgeOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X - centreOfShape.X - 1,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestShapeIsNotAllowedToMoveLessThanOneSquareBeyondTheRightEdgeOfTheScreen()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + _occupiedGridSquares.Count * ShapeSetBuilder.SquareWidth + 1,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeAreHorizontallyAlignedWithTheGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + 10,
                y: centreOfShape.Y);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());
            _occupiedGridSquares[1][0].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeAreVerticallyAlignedWithTheGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X,
                y: centreOfShape.Y + 10);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());
            _occupiedGridSquares[0][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }

        [Test]
        public void TestAShapeCanBeDetectedInTheWayWhenWeArePerfectlyAlignedWithTheGrid()
        {
            // Arrange
            var centreOfShape = new SquareFillPoint(
                x: ShapeSetBuilder.SquareWidth / 2,
                y: ShapeSetBuilder.SquareWidth / 2);
            var newCentreOfShape = new SquareFillPoint(
                x: centreOfShape.X + ShapeSetBuilder.SquareWidth,
                y: centreOfShape.Y + ShapeSetBuilder.SquareWidth);
            var shape = new Shape(colour: SquareFillColour.Red,
                              centreOfShape: centreOfShape,
                              relativePoints: ShapeSetBuilder.SingleSquarePoints,
                              squareFactory: new MockShapeFactory());
            _occupiedGridSquares[1][1].Occupied = true;

            // Act
            var result = shape.AttemptToUpdateOrigins(occupiedGridSquares: _occupiedGridSquares, newShapeCentre: newCentreOfShape);

            // Assert
            Assert.AreEqual(result.NoShapesAreInTheWay, false);
        }
	}
}
