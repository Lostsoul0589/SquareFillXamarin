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

        // init()
        public Square()
        {
            _topLeftCorner = SquareFillPoint(x: 0, y: 0);
            _positionRelativeToParentCorner = SquareFillPoint(x: 0, y: 0);
            _sprite = new NullSquareView();
        }

        // init (positionRelativeToParentCorner: SquareFillPoint, sprite: ISquareView)
        public Square(SquareFillPoint positionRelativeToParentCorner, ISquareView sprite)
        {
            _positionRelativeToParentCorner = positionRelativeToParentCorner;
            _sprite = sprite;
            _topLeftCorner = SquareFillPoint(x: 0, y: 0);
        }

        // public func CalculatePotentialTopLeftCorner(parentTopLeftCorner: SquareFillPoint) -> SquareFillPoint
        public SquareFillPoint CalculatePotentialTopLeftCorner(SquareFillPoint parentTopLeftCorner)
        {
            return SquareFillPoint(
                x: parentTopLeftCorner.X + (_positionRelativeToParentCorner.X * ShapeConstants.SquareWidth),
                y: parentTopLeftCorner.Y + (_positionRelativeToParentCorner.Y * ShapeConstants.SquareWidth));
        }

        // public func GetGridOrigin() -> SquareFillPoint
        public SquareFillPoint GetGridOrigin()
        {
            return SquareFillPoint(
                x: _topLeftCorner.X / ShapeConstants.SquareWidth,
                y: _topLeftCorner.Y / ShapeConstants.SquareWidth);
        }

        // public func CalculateGridOrigin(topLeftCorner: SquareFillPoint) -> SquareFillPoint
        public SquareFillPoint CalculateGridOrigin(SquareFillPoint topLeftCorner)
        {
            var gridOrigin = SquareFillPoint(
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

        // public func VacateGridSquare(occupiedGridSquares: Grid)
        public void VacateGridSquare(Grid occupiedGridSquares)
        {
            occupiedGridSquares.VacateGridSquareUsingPixels(gridReferenceInPixels: _topLeftCorner);
        }

        // public func OccupyGridSquare(occupiedGridSquares: Grid, shapeInSquare: Shape)
        public void OccupyGridSquare(Grid occupiedGridSquares, Shape shapeInSquare)
        {
            occupiedGridSquares.OccupyGridSquareUsingPixels(gridReferenceInPixels: _topLeftCorner, shapeInSquare: shapeInSquare);
        }

        // public func TopLeftCornerAsString() -> String
        public string TopLeftCornerAsString()
        {
            // var originX = String(_topLeftCorner.X);
            var originX = _topLeftCorner.X.ToString();
            var originY = _topLeftCorner.Y.ToString();

            return originX + "," + originY + " ";
        }

        // public func MoveTopLeftCorner(newTopLeftCorner: SquareFillPoint)
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

        // public func IsInSquare(point: SquareFillPoint) -> Bool
        public bool IsInSquare(SquareFillPoint point)
        {
            return PointIsBetweenLeftAndRightEdges(point: point)
                   && PointIsBetweenTopAndBottomEdges(point: point);
        }

        // private func PointIsBetweenLeftAndRightEdges(point: SquareFillPoint) -> Bool
        private bool PointIsBetweenLeftAndRightEdges(SquareFillPoint point)
        {
            return PointIsEqualOrToRightOfLeftEdge(point: point)
                   && PointIsToLeftOfRightEdge(point: point);
        }

        // private func PointIsEqualOrToRightOfLeftEdge(point: SquareFillPoint) -> Bool
        private bool PointIsEqualOrToRightOfLeftEdge(SquareFillPoint point)
        {
            return point.X >= _topLeftCorner.X;
        }

        // private func PointIsToLeftOfRightEdge(point: SquareFillPoint) -> Bool
        private bool PointIsToLeftOfRightEdge(SquareFillPoint point)
        {
            return point.X < RightEdge;
        }

        // private func PointIsBetweenTopAndBottomEdges(point: SquareFillPoint) -> Bool
        private bool PointIsBetweenTopAndBottomEdges(SquareFillPoint point)
        {
            return PointIsEqualOrBelowTopEdge(point: point)
                   && PointIsAboveBottomEdge(point: point);
        }

        // private func PointIsEqualOrBelowTopEdge(point: SquareFillPoint) -> Bool
        private bool PointIsEqualOrBelowTopEdge(SquareFillPoint point)
        {
            return point.Y >= _topLeftCorner.Y;
        }

        // private func PointIsAboveBottomEdge(point: SquareFillPoint) -> Bool
        private bool PointIsAboveBottomEdge(SquareFillPoint point)
        {
            return point.Y < BottomEdge;
        }

        private SquareFillPoint SquareFillPoint(int x, int y)
        {
            return new SquareFillPoint(x: x, y: y);
        }
    }
}