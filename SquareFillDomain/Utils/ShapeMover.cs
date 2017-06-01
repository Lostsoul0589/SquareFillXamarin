using System;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class ShapeMover
    {
        public Shape ShapeToMove { get; private set; }

        private SquareFillPoint _topLeftCornerRelativeToCursorPosition = new SquareFillPoint(x: 0, y: 0);

        public void StartMove(SquareFillPoint cursorPositionAtStart, Shape shapeToMove)
        {
            ShapeToMove = shapeToMove;
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
            if (ShapeToMove != null)
            {
                var newTopLeftCorner = CalculateTopLeftCorner(newCursorPosition: newCursorPosition);
                ShapeToMove.MoveAllShapeSquares(newTopLeftCorner: newTopLeftCorner);
            }
        }

        public void SnapToGrid(SquareFillPoint newCursorPosition)
        {
            if (ShapeToMove != null)
            {
                ShapeToMove.SnapToGrid(
                    newCursorPosition: newCursorPosition,
                    topLeftCornerRelativeToCursorPosition: _topLeftCornerRelativeToCursorPosition);
            }
        }

        private void CalculateTopLeftCornerRelativeToCursorPosition(SquareFillPoint cursorPosition)
        {
            _topLeftCornerRelativeToCursorPosition = ShapeToMove.CalculateTopLeftCornerRelativeToCursorPosition(cursorPosition: cursorPosition);
        }

        public SquareFillPoint CalculateCursorPosition()
        {
            return ShapeToMove.CalculateCursorPosition(
                topLeftCornerRelativeToCursorPosition: _topLeftCornerRelativeToCursorPosition);
        }

        public void SnapToGridInRelevantDimensionsIfPossible(MovementResult movementResult, Grid occupiedGridSquares)
        {
            ShapeToMove.SnapToGridInRelevantDimensionsIfPossible(
                movementResult: movementResult,
                occupiedGridSquares: occupiedGridSquares);
        }
    }
}