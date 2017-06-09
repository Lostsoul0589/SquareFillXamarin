using System;
using SquareFillDomain.Builders;
using SquareFillDomain.Models;

namespace SquareFillDomain.Utils
{
    public class ShapeMover
    {
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

        private void CalculateTopLeftCornerRelativeToCursorPosition(SquareFillPoint cursorPosition)
        {
            _topLeftCornerRelativeToCursorPosition = _shapeToMove.CalculateTopLeftCornerRelativeToCursorPosition(cursorPosition: cursorPosition);
        }
    }
}