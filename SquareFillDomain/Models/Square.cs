using System.Collections.Generic;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class Square
    {
        public int CentreX { get { return _sprite.Centre().X; } }
        public int CentreY { get { return _sprite.Centre().Y; } }
        public int TopLeftCornerX { get { return _topLeftCorner.X; } }
        public int TopLeftCornerY { get { return _topLeftCorner.Y; } }
        public int SpriteCornerX { get { return _sprite.TopLeftCorner().X; } }
        public int SpriteCornerY { get { return _sprite.TopLeftCorner().Y; } }

        private readonly SquareFillPoint _positionRelativeToParentCorner;
        private readonly ISquareView _sprite;
        private SquareFillPoint _topLeftCorner;

        public int XRelativeToParentCorner { get { return _positionRelativeToParentCorner.X; } }
        public int YRelativeToParentCorner { get { return _positionRelativeToParentCorner.Y; } }

        public Square()
        {
            _topLeftCorner = new SquareFillPoint(x: 0, y: 0);
            _positionRelativeToParentCorner = new SquareFillPoint(x: 0, y: 0);
        }

        public Square(SquareFillPoint positionRelativeToParentCorner, ISquareView sprite)
        {
            _positionRelativeToParentCorner = positionRelativeToParentCorner;
            _sprite = sprite;
            _topLeftCorner = new SquareFillPoint(x: 0, y: 0);
        }

        public SquareFillPoint CalculatePotentialTopLeftCorner(SquareFillPoint parentTopLeftCorner)
        {
            return new SquareFillPoint(
                x: parentTopLeftCorner.X + (_positionRelativeToParentCorner.X * ShapeConstants.SquareWidth),
                y: parentTopLeftCorner.Y + (_positionRelativeToParentCorner.Y * ShapeConstants.SquareWidth));
        }

        public bool IsInSquare(SquareFillPoint point)
        {
            return _topLeftCorner.X <= point.X
                   && point.X <= (_topLeftCorner.X + ShapeConstants.SquareWidth)
                   && _topLeftCorner.Y <= point.Y
                   && point.Y <= (_topLeftCorner.Y + ShapeConstants.SquareWidth);
        }

        public void CheckWhetherMovementIsPossible(
            MovementResult movementResultSoFar,
            Grid occupiedGridSquares,
            SquareFillPoint newTopLeftCorner)
        {
            SquareFillPoint oldGridOrigin = GetGridOrigin();
            SquareFillPoint newOrigin = CalculatePotentialTopLeftCorner(parentTopLeftCorner: newTopLeftCorner);
            SquareFillPoint newGridOrigin = CalculateGridOrigin(topLeftCorner: newOrigin);

            IsDivisibleBySquareWidth isDivisibleBySquareWidth = HasCrossedBoundaries(
                movementResultSoFar: movementResultSoFar,
                newPixels: newOrigin,
                oldGridVal: oldGridOrigin,
                newGridVal: newGridOrigin);

            if (movementResultSoFar.ShapeHasCrossedAHorizontalGridBoundary
                || movementResultSoFar.ShapeHasCrossedAVerticalGridBoundary)
            {
                movementResultSoFar.ThereAreShapesInTheWay = IsSomethingInTheWay(
                    isDivisibleBySquareWidth: isDivisibleBySquareWidth,
                    newGridOrigin: newGridOrigin,
                    somethingWasAlreadyInTheWay: movementResultSoFar.ThereAreShapesInTheWay,
                    occupiedGridSquares: occupiedGridSquares);
            }
        }

        public SquareFillPoint GetGridOrigin()
        {
            return new SquareFillPoint(
                x: _topLeftCorner.X / ShapeConstants.SquareWidth,
                y: _topLeftCorner.Y / ShapeConstants.SquareWidth);
        }

        public SquareFillPoint CalculateGridOrigin(SquareFillPoint topLeftCorner)
        {
            var gridOrigin = new SquareFillPoint(
                x: topLeftCorner.X / ShapeConstants.SquareWidth,
                y: topLeftCorner.Y / ShapeConstants.SquareWidth);

            if (topLeftCorner.X < 0)
            {
                gridOrigin.X = gridOrigin.X - 1;
            }

            if (topLeftCorner.Y < 0)
            {
                gridOrigin.Y = gridOrigin.Y - 1;
            }

            return gridOrigin;
        }

        public bool IsSomethingInTheWay(
            IsDivisibleBySquareWidth isDivisibleBySquareWidth,
            SquareFillPoint newGridOrigin,
            bool somethingWasAlreadyInTheWay,
            Grid occupiedGridSquares)
        {
            bool somethingIsInTheWay = somethingWasAlreadyInTheWay;

            List<int> newGridXCoords = GetNewGridCoordinates(isDivisibleBySquareWidth: isDivisibleBySquareWidth.X, newGridCoord: newGridOrigin.X);
            List<int> newGridYCoords = GetNewGridCoordinates(isDivisibleBySquareWidth: isDivisibleBySquareWidth.Y, newGridCoord: newGridOrigin.Y);

            // These nested for loops work because at the moment we are just considering one square, not the whole shape.
            foreach (var xCoord in newGridXCoords)
            {
                foreach (var yCoord in newGridYCoords)
                {
                    if (xCoord >= occupiedGridSquares.Width
                        || yCoord >= occupiedGridSquares.Height
                        || xCoord < 0
                        || yCoord < 0)
                    {
                        somethingIsInTheWay = true;
                    }
                    else
                    {
                        somethingIsInTheWay = somethingIsInTheWay || occupiedGridSquares.IsSquareOccupied(x: xCoord, y: yCoord);
                    }
                }
            }

            return somethingIsInTheWay;
        }

        public IsDivisibleBySquareWidth HasCrossedBoundaries(
            MovementResult movementResultSoFar, 
            SquareFillPoint newPixels, 
            SquareFillPoint oldGridVal, 
            SquareFillPoint newGridVal)
        {
            bool newXDivisibleBySquareWidth = newPixels.X % ShapeConstants.SquareWidth == 0;
            bool oldXDivisibleBySquareWidth = _topLeftCorner.X % ShapeConstants.SquareWidth == 0;
            if (oldXDivisibleBySquareWidth != newXDivisibleBySquareWidth
                || oldGridVal.X != newGridVal.X)
            {
                movementResultSoFar.ShapeHasCrossedAHorizontalGridBoundary = true;
            }

            bool newYDivisibleBySquareWidth = newPixels.Y % ShapeConstants.SquareWidth == 0;
            bool oldYDivisibleBySquareWidth = _topLeftCorner.Y % ShapeConstants.SquareWidth == 0;
            if (oldYDivisibleBySquareWidth != newYDivisibleBySquareWidth
                || oldGridVal.Y != newGridVal.Y)
            {
                movementResultSoFar.ShapeHasCrossedAVerticalGridBoundary = true;
            }

            return new IsDivisibleBySquareWidth
            {
                X = newXDivisibleBySquareWidth,
                Y = newYDivisibleBySquareWidth
            };
        }

        public void VacateGridSquare(Grid occupiedGridSquares)
        {
            occupiedGridSquares.VacateGridSquareUsingPixels(gridReferenceInPixels: _topLeftCorner);
        }

        public void OccupyGridSquare(Grid occupiedGridSquares, Shape shapeInSquare)
        {
            occupiedGridSquares.OccupyGridSquareUsingPixels(gridReferenceInPixels: _topLeftCorner, shapeInSquare: shapeInSquare);
        }

        public string TopLeftCornerAsString()
        {
            string originX = _topLeftCorner.X.ToString();
            string originY = _topLeftCorner.Y.ToString();

            return originX + "," + originY + " ";
        }

        public void MoveTopLeftCorner(SquareFillPoint newTopLeftCorner)
        {
            _topLeftCorner = CalculatePotentialTopLeftCorner(parentTopLeftCorner: newTopLeftCorner);
            if (_sprite != null)
            {
                _sprite.MoveTopLeftCorner(
                    newX: newTopLeftCorner.X + (_positionRelativeToParentCorner.X * ShapeConstants.SquareWidth),
                    newY: newTopLeftCorner.Y + (_positionRelativeToParentCorner.Y * ShapeConstants.SquareWidth));
            }
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
    }
}