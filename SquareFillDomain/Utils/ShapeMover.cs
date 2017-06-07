using System;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class ShapeMover
    {
        public int TopLeftCornerX { get { return _shapeToMove.TopLeftCornerX; } }
        public int TopLeftCornerY { get { return _shapeToMove.TopLeftCornerY; } }

        private Shape _shapeToMove;
        private SquareFillPoint _topLeftCornerRelativeToCursorPosition = new SquareFillPoint(x: 0, y: 0);

        public void StartMove(SquareFillPoint cursorPositionAtStart, Shape shapeToMove)
        {
            _shapeToMove = shapeToMove;
            CalculateTopLeftCornerRelativeToCursorPosition(cursorPositionAtStart);
        }

        public SquareFillPoint CalculateTopLeftCorner(SquareFillPoint newCursorPosition)
        {
            return new SquareFillPoint(
                x: newCursorPosition.X + _topLeftCornerRelativeToCursorPosition.X,
                y: newCursorPosition.Y + _topLeftCornerRelativeToCursorPosition.Y);
        }

        public void MoveToNewCursorPosition(SquareFillPoint newCursorPosition)
        {
            if (_shapeToMove != null)
            {
                var newTopLeftCorner = CalculateTopLeftCorner(newCursorPosition: newCursorPosition);
                _shapeToMove.MoveAllShapeSquares(newTopLeftCorner: newTopLeftCorner);
            }
        }

        public void SnapToGrid(SquareFillPoint newCursorPosition)
        {
            if (_shapeToMove != null)
            {
                _shapeToMove.SnapToGrid(
                    newCursorPosition: newCursorPosition,
                    topLeftCornerRelativeToCursorPosition: _topLeftCornerRelativeToCursorPosition);
            }
        }

        public SquareFillPoint CalculateCursorPosition()
        {
            return _shapeToMove.CalculateCursorPosition(
                topLeftCornerRelativeToCursorPosition: _topLeftCornerRelativeToCursorPosition);
        }

        public void SnapToGridInRelevantDimensionsIfPossible(MovementResult movementResult, Grid occupiedGridSquares)
        {
            _shapeToMove.SnapToGridInRelevantDimensionsIfPossible(
                movementResult: movementResult,
                occupiedGridSquares: occupiedGridSquares);
        }

        public int SquareCentreX(int squareIndex)
        {
            return _shapeToMove.SquareCentreX(squareIndex: squareIndex);
        }

        public int SquareCentreY(int squareIndex)
        {
            return _shapeToMove.SquareCentreY(squareIndex: squareIndex);
        }

        private void CalculateTopLeftCornerRelativeToCursorPosition(SquareFillPoint cursorPosition)
        {
            _topLeftCornerRelativeToCursorPosition = _shapeToMove.CalculateTopLeftCornerRelativeToCursorPosition(cursorPosition: cursorPosition);
        }
    }
}