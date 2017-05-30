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
            bool somethingIsInTheWay = false;
            var movementResult = new MovementResult();

            foreach (var square in Squares)
            {
                SquareFillPoint oldGridOrigin = square.GetGridOrigin();
                SquareFillPoint newOrigin = square.CalculatePotentialTopLeftCorner(parentTopLeftCorner: newTopLeftCorner);
                SquareFillPoint newGridOrigin = square.CalculateNewGridOrigin(newOrigin: newOrigin);

                IsDivisibleBySquareWidth isDivisibleBySquareWidth = square.HasCrossedBoundaries(
                    movementResult: movementResult, 
                    newPixels: newOrigin, 
                    oldGridVal: oldGridOrigin, 
                    newGridVal: newGridOrigin);

                if (movementResult.ShapeHasCrossedAHorizontalGridBoundary
                    || movementResult.ShapeHasCrossedAVerticalGridBoundary)
                {
                    somethingIsInTheWay = IsSomethingInTheWay(
                        isDivisibleBySquareWidth: isDivisibleBySquareWidth,
                        newGridOrigin: newGridOrigin,
                        somethingWasAlreadyIntheWay: somethingIsInTheWay,
                        occupiedGridSquares: occupiedGridSquares);
                }
            }

            if (!somethingIsInTheWay)
            {
                CalculateTopLeftCorners(newTopLeftCorner: newTopLeftCorner);
            }

            movementResult.NoShapesAreInTheWay = !somethingIsInTheWay;
            return movementResult;
        }

	    private bool IsSomethingInTheWay(
            IsDivisibleBySquareWidth isDivisibleBySquareWidth, 
            SquareFillPoint newGridOrigin, 
            bool somethingWasAlreadyIntheWay, 
            List<List<GridSquare>> occupiedGridSquares)
	    {
            bool somethingIsInTheWay = somethingWasAlreadyIntheWay;

	        List<int> newGridXCoords = GetNewGridCoordinates(isDivisibleBySquareWidth: isDivisibleBySquareWidth.X, newGridCoord: newGridOrigin.X);
	        List<int> newGridYCoords = GetNewGridCoordinates(isDivisibleBySquareWidth: isDivisibleBySquareWidth.Y, newGridCoord: newGridOrigin.Y);

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
	                    somethingIsInTheWay = true;
	                }
	                else
	                {
                        somethingIsInTheWay = somethingWasAlreadyIntheWay || occupiedGridSquares[xCoord][yCoord].Occupied;
	                }
	            }
	        }

	        return somethingIsInTheWay;
	    }

	    private List<int> GetNewGridCoordinates(bool isDivisibleBySquareWidth, int newGridCoord)
	    {
	        List<int> newGridCoords = new List<int>();

	        if (isDivisibleBySquareWidth)
	        {
	            newGridCoords.Add(newGridCoord);
	        }
	        else
	        {
	            newGridCoords.Add(newGridCoord);
	            newGridCoords.Add(newGridCoord + 1);
	        }

	        return newGridCoords;
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