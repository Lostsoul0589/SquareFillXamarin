using System.Collections.Generic;
using SquareFillDomain.Builders;
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

        public void CalculateTopLeftCorner(SquareFillPoint parentTopLeftCorner)
        {
            _topLeftCorner = CalculatePotentialTopLeftCorner(parentTopLeftCorner: parentTopLeftCorner);
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

        public SquareFillPoint GetGridOrigin()
        {
            return new SquareFillPoint(
                x: _topLeftCorner.X / ShapeConstants.SquareWidth,
                y: _topLeftCorner.Y / ShapeConstants.SquareWidth);
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
            bool oldXDivisibleBySquareWidth = _topLeftCorner.X % ShapeConstants.SquareWidth == 0;
            if (oldXDivisibleBySquareWidth != newXDivisibleBySquareWidth
                || oldGridVal.X != newGridVal.X)
            {
                movementResult.ShapeHasCrossedAHorizontalGridBoundary = true;
            }

            bool newYDivisibleBySquareWidth = newPixels.Y % ShapeConstants.SquareWidth == 0;
            bool oldYDivisibleBySquareWidth = _topLeftCorner.Y % ShapeConstants.SquareWidth == 0;
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
            occupiedGridSquares.VacateGridSquare(gridReferenceInPixels: _topLeftCorner);
        }

        public void OccupyGridSquare(Grid occupiedGridSquares, Shape shapeInSquare)
        {
            occupiedGridSquares.OccupyGridSquare(gridReferenceInPixels: _topLeftCorner, shapeInSquare: shapeInSquare);
        }

        public string TopLeftCornerAsString()
        {
            string originX = _topLeftCorner.X.ToString();
            string originY = _topLeftCorner.Y.ToString();

            return originX + "," + originY + " ";
        }

        public void MoveTopLeftCorner(SquareFillPoint newTopLeftCorner)
        {
            if (_sprite != null)
            {
                _sprite.MoveTopLeftCorner(
                    newX: newTopLeftCorner.X + (_positionRelativeToParentCorner.X * ShapeConstants.SquareWidth),
                    newY: newTopLeftCorner.Y + (_positionRelativeToParentCorner.Y * ShapeConstants.SquareWidth));
            }
        }
    }
}