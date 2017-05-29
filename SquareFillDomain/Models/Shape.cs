using System;
using System.Collections.Generic;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class Shape
	{
        public SquareFillPoint TopLeftCorner { get; private set; }
        public List<Square> Squares { get; private set; }

        public int NumSquaresLeftOfTopLeftCorner { get; private set; }
        public int NumSquaresRightOfTopLeftCorner { get; private set; }
        public int NumSquaresAboveTopLeftCorner { get; private set; }
        public int NumSquaresBelowTopLeftCorner { get; private set; }

        public Shape(
            SquareFillColour colour,
            SquareFillPoint topLeftCorner,
            List<SquareFillPoint> relativePointsTopLeftCorner,
            ISquareViewFactory squareFactory,
            bool topLeftCornerIsInPixels = true)
        {
            InitialiseTopLeftCorner(topLeftCorner, topLeftCornerIsInPixels);

            List<Square> squares = new List<Square>();
            foreach(var point in relativePointsTopLeftCorner)
            {
                squares.Add(new Square(
                    positionRelativeToParentCorner: point,
                    sprite: squareFactory.MakeSquare(colour: colour)));
            }
            Squares = squares;
        
            Initialise();
		}

        public Shape(
            SquareFillPoint topLeftCorner,
            List<Square> squareDefinitions,
            bool topLeftCornerIsInPixels = true)
        {
            InitialiseTopLeftCorner(topLeftCorner, topLeftCornerIsInPixels);
            Squares = squareDefinitions;
        
            Initialise();
        }

	    private void InitialiseTopLeftCorner(SquareFillPoint topLeftCorner, bool topLeftCornerIsInPixels)
	    {
            if (topLeftCornerIsInPixels)
	        {
	            TopLeftCorner = topLeftCorner;
	        }
	        else
	        {
	            TopLeftCorner = new SquareFillPoint(
                    x: topLeftCorner.X * ShapeConstants.SquareWidth,
                    y: topLeftCorner.Y * ShapeConstants.SquareWidth);
	        }
	    }

	    public SquareFillPoint CentreOfShape
	    {
	        get
	        {
	            return new SquareFillPoint(
	                x: TopLeftCorner.X + ShapeConstants.SquareWidth / 2,
	                y: TopLeftCorner.Y + ShapeConstants.SquareWidth / 2);
	        }
	    }

        public bool IsInShape(SquareFillPoint point)
        {
            bool isInShape = false;
            
            foreach(var square in Squares) 
            {
                isInShape = isInShape || square.IsInSquare(point: point);
            }

            return isInShape;
        }

        public void MoveAllShapeSquares(SquareFillPoint newTopLeftCorner)
        {
            TopLeftCorner = newTopLeftCorner;
            foreach (var square in Squares)
            {
                if (square.Sprite != null)
                {
                    square.Sprite.MoveTopLeftCorner(
                        newX: TopLeftCorner.X + (square.PositionRelativeToParentCorner.X * ShapeConstants.SquareWidth),
                        newY: TopLeftCorner.Y + (square.PositionRelativeToParentCorner.Y * ShapeConstants.SquareWidth));
                }
            }
        }

        public void CalculateTopLeftCorners(SquareFillPoint newTopLeftCorner)
        {
            foreach (var square in Squares)
            {
                square.CalculateTopLeftCorner(parentTopLeftCorner: newTopLeftCorner);
            }
        }

        public MovementResult AttemptToUpdateOrigins(
            List<List<GridSquare>> occupiedGridSquares,
            SquareFillPoint newTopLeftCorner)
        {
            bool somethingIsintheWay = false;
            var movementResult = new MovementResult();

            foreach (var square in Squares)
            {
                var newOrigin = square.CalculatePotentialTopLeftCorner(parentTopLeftCorner: newTopLeftCorner);

                List<int> newGridXCoords = new List<int>();
                List<int> newGridYCoords = new List<int>();

                int oldGridXCoord = square.TopLeftCorner.X / ShapeConstants.SquareWidth;
                int oldGridYCoord = square.TopLeftCorner.Y / ShapeConstants.SquareWidth;
                SquareFillPoint oldGridOrigin = new SquareFillPoint(
                    x: oldGridXCoord,
                    y: oldGridYCoord);

                bool oldXDivisibleBySquareWidth =
                    square.TopLeftCorner.X % ShapeConstants.SquareWidth == 0;
                bool oldYDivisibleBySquareWidth =
                    square.TopLeftCorner.Y % ShapeConstants.SquareWidth == 0;

                int newGridXCoord = newOrigin.X / ShapeConstants.SquareWidth;
                int newGridYCoord = newOrigin.Y / ShapeConstants.SquareWidth;

                var newGridOrigin = new SquareFillPoint(
                    x: newGridXCoord,
                    y: newGridYCoord);

                if (newOrigin.X < 0)
                {
                    newGridOrigin.X = newGridOrigin.X - 1;
                }

                if (newOrigin.Y < 0)
                {
                    newGridOrigin.Y = newGridOrigin.Y - 1;
                }

                bool newXDivisibleBySquareWidth =
                    newOrigin.X % ShapeConstants.SquareWidth == 0;
                bool newYDivisibleBySquareWidth =
                    newOrigin.Y % ShapeConstants.SquareWidth == 0;

                if (oldXDivisibleBySquareWidth != newXDivisibleBySquareWidth
                    || oldGridOrigin.X != newGridOrigin.X)
                {
                    movementResult.ShapeHasCrossedAHorizontalGridBoundary = true;
                }

                if (oldYDivisibleBySquareWidth != newYDivisibleBySquareWidth
                    || oldGridOrigin.Y != newGridOrigin.Y)
                {
                    movementResult.ShapeHasCrossedAVerticalGridBoundary = true;
                }

                if (movementResult.ShapeHasCrossedAHorizontalGridBoundary
                    || movementResult.ShapeHasCrossedAVerticalGridBoundary)
                {
                    if (newXDivisibleBySquareWidth)
                    {
                        newGridXCoords.Add(newGridOrigin.X);
                    }
                    else
                    {
                        newGridXCoords.Add(newGridOrigin.X);
                        newGridXCoords.Add(newGridOrigin.X + 1);
                    }

                    if (newYDivisibleBySquareWidth)
                    {
                        newGridYCoords.Add(newGridOrigin.Y);
                    }
                    else
                    {
                        newGridYCoords.Add(newGridOrigin.Y);
                        newGridYCoords.Add(newGridOrigin.Y + 1);
                    }

                    // These nested for loops work because at the moment we are just considering one square, not the whole shape.
                    foreach (var xCoord in newGridXCoords)
                    {
                        foreach (var yCoord in newGridYCoords)
                        {
                            if (xCoord >= occupiedGridSquares.Count
                                || yCoord >= occupiedGridSquares[0].Count
                                || xCoord < 0
                                || yCoord < 0)
                            {
                                somethingIsintheWay = true;
                            }
                            else
                            {
                                somethingIsintheWay = somethingIsintheWay
                                    || occupiedGridSquares[xCoord][yCoord].Occupied;
                            }
                        }
                    }
                }
            }

            if (!somethingIsintheWay)
            {
                foreach (var square in Squares)
                {
                    square.CalculateTopLeftCorner(parentTopLeftCorner: newTopLeftCorner);
                }
            }

            movementResult.NoShapesAreInTheWay = !somethingIsintheWay;

            return movementResult;
        }

        public void VacateGridSquares(List<List<GridSquare>>occupiedGridSquares) 
        {
            foreach (var square in Squares)
            {
                int occupiedXCoordinate = square.TopLeftCorner.X/ShapeConstants.SquareWidth;
                int occupiedYCoordinate = square.TopLeftCorner.Y/ShapeConstants.SquareWidth;
            
                occupiedGridSquares[occupiedXCoordinate][occupiedYCoordinate].Occupied = false;
                occupiedGridSquares[occupiedXCoordinate][occupiedYCoordinate].ShapeInSquare = null;
            }
        }

        public void OccupyGridSquares(List<List<GridSquare>> occupiedGridSquares)
        {
            foreach (var square in Squares)
            {
                int occupiedXCoordinate = square.TopLeftCorner.X / ShapeConstants.SquareWidth;
                int occupiedYCoordinate = square.TopLeftCorner.Y / ShapeConstants.SquareWidth;

                occupiedGridSquares[occupiedXCoordinate][occupiedYCoordinate].Occupied = true;
                occupiedGridSquares[occupiedXCoordinate][occupiedYCoordinate].ShapeInSquare = this;
            }
        }

        public bool WeStartedWithinTheContainingRectangle()
        {
            var leftEdge = TopLeftCorner.X - NumSquaresLeftOfTopLeftCorner * ShapeConstants.SquareWidth;
            var topEdge = TopLeftCorner.Y - NumSquaresAboveTopLeftCorner * ShapeConstants.SquareWidth;
            var rightEdge = TopLeftCorner.X + NumSquaresRightOfTopLeftCorner * ShapeConstants.SquareWidth;
            var bottomEdge = TopLeftCorner.Y + NumSquaresBelowTopLeftCorner * ShapeConstants.SquareWidth;

            return leftEdge >= ShapeConstants.ContainingRectangle.X
                && topEdge >= ShapeConstants.ContainingRectangle.Y
                && rightEdge <= (ShapeConstants.ContainingRectangle.X + ShapeConstants.ContainingRectangle.Width)
                && bottomEdge <= (ShapeConstants.ContainingRectangle.Y + ShapeConstants.ContainingRectangle.Height);
        }

        private void Initialise()
        {
            foreach (var square in Squares)
            {
                square.CalculateTopLeftCorner(parentTopLeftCorner: TopLeftCorner);
            }
        
            CalculateNumSquaresAroundTopLeftCorner();
            MoveAllShapeSquares(newTopLeftCorner: TopLeftCorner);
        }

        private void CalculateNumSquaresAroundTopLeftCorner()
        {
            foreach (var square in Squares)
            {
                NumSquaresLeftOfTopLeftCorner = Math.Min(NumSquaresLeftOfTopLeftCorner, square.PositionRelativeToParentCorner.X);
                NumSquaresRightOfTopLeftCorner = Math.Max(NumSquaresRightOfTopLeftCorner, square.PositionRelativeToParentCorner.X);
                NumSquaresAboveTopLeftCorner = Math.Min(NumSquaresAboveTopLeftCorner, square.PositionRelativeToParentCorner.Y);
                NumSquaresBelowTopLeftCorner = Math.Max(NumSquaresBelowTopLeftCorner, square.PositionRelativeToParentCorner.Y);
            }

            DealWithNegativeNumbersOfSquares();
        }

        private void DealWithNegativeNumbersOfSquares()
        {
            NumSquaresLeftOfTopLeftCorner = Math.Abs(NumSquaresLeftOfTopLeftCorner);
            NumSquaresAboveTopLeftCorner = Math.Abs(NumSquaresAboveTopLeftCorner);
        }
	}
}