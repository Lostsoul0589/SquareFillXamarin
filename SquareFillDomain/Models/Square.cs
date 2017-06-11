using System.Collections.Generic;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class Square
    {
        public int TopLeftCornerX { get { return _topLeftCorner.X; } }
        public int TopLeftCornerY { get { return _topLeftCorner.Y; } }

        public int SpriteCornerX { get { return _sprite.TopLeftCorner().X; } }
        public int SpriteCornerY { get { return _sprite.TopLeftCorner().Y; } }

        public int XRelativeToParentCorner { get { return _positionRelativeToParentCorner.X; } }
        public int YRelativeToParentCorner { get { return _positionRelativeToParentCorner.Y; } }

        public int RightEdge { get { return _topLeftCorner.X + ShapeConstants.SquareWidth; } }
        public int BottomEdge { get { return _topLeftCorner.Y + ShapeConstants.SquareWidth; } }

        private readonly SquareFillPoint _positionRelativeToParentCorner;
        private readonly ISquareView _sprite;
        private SquareFillPoint _topLeftCorner;


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
            return PointIsBetweenLeftAndRightEdges(point: point)
                   && PointIsBetweenTopAndBottomEdges(point: point);
        }

        public void CheckWhetherMovementIsPossible(
            MovementResult movementResultSoFar,
            Grid occupiedGridSquares,
            SquareFillPoint newParentTopLeftCorner)
        {
            SquareFillPoint newTopLeftCorner = CalculatePotentialTopLeftCorner(parentTopLeftCorner: newParentTopLeftCorner);
            SquareFillPoint newGridOrigin = CalculateGridOrigin(topLeftCorner: newTopLeftCorner);

            if (movementResultSoFar.ShapeHasCrossedAHorizontalGridBoundary
                || movementResultSoFar.ShapeHasCrossedAVerticalGridBoundary)
            {
                movementResultSoFar.ThereAreShapesInTheWay = IsSomethingInTheWay(
                    newTopLeftCorner: newTopLeftCorner,
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
            SquareFillPoint newTopLeftCorner,
            SquareFillPoint newGridOrigin,
            bool somethingWasAlreadyInTheWay,
            Grid occupiedGridSquares)
        {
            bool somethingIsInTheWay = somethingWasAlreadyInTheWay;

            List<int> newGridXCoords = GetNewGridCoordinates(newPixelValue: newTopLeftCorner.X, newGridCoord: newGridOrigin.X);
            List<int> newGridYCoords = GetNewGridCoordinates(newPixelValue: newTopLeftCorner.Y, newGridCoord: newGridOrigin.Y);
            
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

        public void CheckWhetherBoundariesHaveBeenCrossed(
            MovementResult movementResultSoFar, 
            SquareFillPoint newParentTopLeftCorner)
        {
            SquareFillPoint oldGridOrigin = GetGridOrigin();
            SquareFillPoint newTopLeftCorner = CalculatePotentialTopLeftCorner(parentTopLeftCorner: newParentTopLeftCorner);
            SquareFillPoint newGridOrigin = CalculateGridOrigin(topLeftCorner: newTopLeftCorner);

            movementResultSoFar.ShapeHasCrossedAHorizontalGridBoundary = HasBoundaryBeenCrossed(
                oldPixelValue: _topLeftCorner.X,
                newPixelValue: newTopLeftCorner.X,
                oldGridRef: oldGridOrigin.X,
                newGridRef: newGridOrigin.X);

            movementResultSoFar.ShapeHasCrossedAVerticalGridBoundary = HasBoundaryBeenCrossed(
                oldPixelValue: _topLeftCorner.Y,
                newPixelValue: newTopLeftCorner.Y,
                oldGridRef: oldGridOrigin.Y,
                newGridRef: newGridOrigin.Y);
        }

        public bool HasBoundaryBeenCrossed(
            int oldPixelValue,
            int newPixelValue,
            int oldGridRef,
            int newGridRef)
        {
            return OnePositionIsAlignedWithGridButTheOtherIsNot(pixelValue1: oldPixelValue, pixelValue2: newPixelValue)
                   || TheyAreInDifferentGridSquares(gridRef1: oldGridRef, gridRef2: newGridRef);
        }

        private bool OnePositionIsAlignedWithGridButTheOtherIsNot(int pixelValue1, int pixelValue2)
        {
            return DivisibleBySquareWidth(value: pixelValue1) != DivisibleBySquareWidth(value: pixelValue2);
        }

        private bool TheyAreInDifferentGridSquares(int gridRef1, int gridRef2)
        {
            return gridRef1 != gridRef2;
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

        private List<int> GetNewGridCoordinates(int newPixelValue, int newGridCoord)
        {
            List<int> newGridCoords = new List<int>();

            if (DivisibleBySquareWidth(value: newPixelValue))
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

        private bool PointIsBetweenLeftAndRightEdges(SquareFillPoint point)
        {
            return PointIsToRightOfLeftEdge(point: point)
                   && PointIsToLeftOfRightEdge(point: point);
        }

        private bool PointIsToRightOfLeftEdge(SquareFillPoint point)
        {
            return point.X >= _topLeftCorner.X;
        }

        private bool PointIsToLeftOfRightEdge(SquareFillPoint point)
        {
            return point.X <= RightEdge;
        }

        private bool PointIsBetweenTopAndBottomEdges(SquareFillPoint point)
        {
            return PointIsBelowTopEdge(point: point)
                   && PointIsAboveBottomEdge(point: point);
        }

        private bool PointIsBelowTopEdge(SquareFillPoint point)
        {
            return point.Y >= _topLeftCorner.Y;
        }

        private bool PointIsAboveBottomEdge(SquareFillPoint point)
        {
            return point.Y <= BottomEdge;
        }

        private bool DivisibleBySquareWidth(int value)
        {
            return (value % ShapeConstants.SquareWidth) == 0;
        }
    }
}