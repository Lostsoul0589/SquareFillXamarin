using System.Collections.Generic;
using SquareFillDomain.Builders;
using SquareFillDomain.Interfaces;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Models
{
    public class Square
    {
        public SquareFillPoint PositionRelativeToParentCorner { get; set; }
        public ISquareView Sprite { get; private set; }
        public SquareFillPoint TopLeftCorner { get; private set; }

        public Square()
        {
            TopLeftCorner = new SquareFillPoint(x: 0, y: 0);
            PositionRelativeToParentCorner = new SquareFillPoint(x: 0, y: 0);
        }

        public Square(SquareFillPoint positionRelativeToParentCorner, ISquareView sprite)
        {
            PositionRelativeToParentCorner = positionRelativeToParentCorner;
            Sprite = sprite;
            TopLeftCorner = new SquareFillPoint(x: 0, y: 0);
        }

        public void CalculateTopLeftCorner(SquareFillPoint parentTopLeftCorner)
        {
            TopLeftCorner = CalculatePotentialTopLeftCorner(parentTopLeftCorner: parentTopLeftCorner);
        }

        public SquareFillPoint CalculatePotentialTopLeftCorner(SquareFillPoint parentTopLeftCorner)
        {
            return new SquareFillPoint(
                x: parentTopLeftCorner.X + (PositionRelativeToParentCorner.X * ShapeConstants.SquareWidth),
                y: parentTopLeftCorner.Y + (PositionRelativeToParentCorner.Y * ShapeConstants.SquareWidth));
        }

        public bool IsInSquare(SquareFillPoint point)
        {
            return TopLeftCorner.X <= point.X
                   && point.X <= (TopLeftCorner.X + ShapeConstants.SquareWidth)
                   && TopLeftCorner.Y <= point.Y
                   && point.Y <= (TopLeftCorner.Y + ShapeConstants.SquareWidth);
        }

        public SquareFillPoint GetGridOrigin()
        {
            return new SquareFillPoint(
                x: TopLeftCorner.X / ShapeConstants.SquareWidth,
                y: TopLeftCorner.Y / ShapeConstants.SquareWidth);
        }

        public SquareFillPoint CalculateNewGridOrigin(SquareFillPoint newOrigin)
        {
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

            return newGridOrigin;
        }

        public IsDivisibleBySquareWidth HasCrossedBoundaries(
            MovementResult movementResult, 
            SquareFillPoint newPixels, 
            SquareFillPoint oldGridVal, 
            SquareFillPoint newGridVal)
        {
            bool newXDivisibleBySquareWidth = newPixels.X % ShapeConstants.SquareWidth == 0;
            bool oldXDivisibleBySquareWidth = TopLeftCorner.X % ShapeConstants.SquareWidth == 0;
            if (oldXDivisibleBySquareWidth != newXDivisibleBySquareWidth
                || oldGridVal.X != newGridVal.X)
            {
                movementResult.ShapeHasCrossedAHorizontalGridBoundary = true;
            }

            bool newYDivisibleBySquareWidth = newPixels.Y % ShapeConstants.SquareWidth == 0;
            bool oldYDivisibleBySquareWidth = TopLeftCorner.Y % ShapeConstants.SquareWidth == 0;
            if (oldYDivisibleBySquareWidth != newYDivisibleBySquareWidth
                || oldGridVal.Y != newGridVal.Y)
            {
                movementResult.ShapeHasCrossedAVerticalGridBoundary = true;
            }

            return new IsDivisibleBySquareWidth
            {
                X = newXDivisibleBySquareWidth,
                Y = newYDivisibleBySquareWidth
            };
        }

        public void VacateGridSquare(Grid occupiedGridSquares)
        {
            occupiedGridSquares.VacateGridSquare(gridReferenceInPixels: TopLeftCorner);
        }

        public void OccupyGridSquare(Grid occupiedGridSquares, Shape shapeInSquare)
        {
            occupiedGridSquares.OccupyGridSquare(gridReferenceInPixels: TopLeftCorner, shapeInSquare: shapeInSquare);
        }
    }
}