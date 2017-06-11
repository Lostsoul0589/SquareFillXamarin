using SquareFillDomain.Models;
using SquareFillDomain.Utils;

namespace SquareFillDomain.Controllers
{
    public class ShapeController
    {
        public int CurrentShapeCornerX { get { return _shapeToMove.TopLeftCornerX; } }
        public int CurrentShapeCornerY { get { return _shapeToMove.TopLeftCornerY; } }

        private Shape _shapeToMove = null;
        private readonly Grid _occupiedGridSquares;
        private readonly ShapeSet _shapeSet = null;
        
        private SquareFillPoint _lastGoodLocation;
        private bool _colliding = false;
        private readonly Logger _logger = new Logger();
        private SquareFillPoint _topLeftCornerRelativeToCursorPosition = new SquareFillPoint(x: 0, y: 0);

        public ShapeController(ShapeSet shapeSet, Grid occupiedGridSquares)
        {
            _occupiedGridSquares = occupiedGridSquares;
            _shapeSet = shapeSet;
            _lastGoodLocation = new SquareFillPoint(x: 0, y: 0);
        }

        public Shape StartMove(SquareFillPoint cursorPositionAtStart)
        {
            _shapeToMove = _shapeSet.SelectShape(selectedPoint: cursorPositionAtStart);

            if (_shapeToMove != null)
            {
                _shapeToMove.VacateGridSquares(occupiedGridSquares: _occupiedGridSquares);
                _topLeftCornerRelativeToCursorPosition = _shapeToMove.CalculateTopLeftCornerRelativeToCursorPosition(
                    cursorPosition: cursorPositionAtStart);
                _lastGoodLocation = cursorPositionAtStart;
            }

            return _shapeToMove;
        }

        public void ContinueMove(SquareFillPoint newLocation)
        {
            if (_shapeToMove != null)
            {
                SquareFillPoint newTopLeftCorner = CalculateTopLeftCorner(newCursorPosition: newLocation);
                bool cursorIsInShape = _shapeToMove.IsInShape(point: newLocation);
                var movementResult = _shapeToMove.CheckWhetherMovementIsPossible(
                    occupiedGridSquares: _occupiedGridSquares,
                    newTopLeftCorner: newTopLeftCorner);

                SquareFillPoint positionInGrid = CalculateGridPosition(topLeftCorner: newTopLeftCorner);
                NoteLocation(cursor: newLocation, topLeftCorner: newTopLeftCorner, gridPosition: positionInGrid);

                if (_colliding == false)
                {
                    if (movementResult.ThereAreShapesInTheWay)
                    {
                        _colliding = true;
                        SnapToGridInRelevantDimensionsIfPossible(movementResult: movementResult);
                    }
                    else
                    {
                        MoveToNewCursorPosition(newCursorPosition: newLocation, newTopLeftCorner: newTopLeftCorner);
                    }
                }
                else
                {
                    if (cursorIsInShape)
                    {
                        GetMovingAgain(cursorPosition: newLocation);
                        _colliding = false;
                    }
                }
            }
        }

        public void EndMove(SquareFillPoint finalLocation)
        {
            if (_shapeToMove != null)
            {
                SquareFillPoint newTopLeftCorner = CalculateTopLeftCorner(newCursorPosition: finalLocation);
                MovementResult movementResult = _shapeToMove.CheckWhetherMovementIsPossible(
                    occupiedGridSquares: _occupiedGridSquares,
                    newTopLeftCorner: newTopLeftCorner);

                if (movementResult.ThereAreShapesInTheWay || _colliding)
                {
                    EndMoveWithObstacles();
                }
                else
                {
                    EndMoveWithNoObstacles(finalLocation: finalLocation, newTopLeftCorner: newTopLeftCorner);
                }
            }
        }

        private SquareFillPoint CalculateTopLeftCorner(SquareFillPoint newCursorPosition)
        {
            return new SquareFillPoint(
                x: newCursorPosition.X + _topLeftCornerRelativeToCursorPosition.X,
                y: newCursorPosition.Y + _topLeftCornerRelativeToCursorPosition.Y);
        }

        private void MoveToNewCursorPosition(SquareFillPoint newCursorPosition, SquareFillPoint newTopLeftCorner)
        {
            _shapeToMove.UpdateTopLeftCorner(newTopLeftCorner: newTopLeftCorner);
            _lastGoodLocation = newCursorPosition;
            LogMessagePlusOrigins(message: "Clear. ");
        }

        private void GetMovingAgain(SquareFillPoint cursorPosition)
        {
            StartMove(cursorPositionAtStart: cursorPosition);
            LogMessagePlusOrigins(message: "Moving again. ");
        }

        private void SnapToGridInRelevantDimensionsIfPossible(MovementResult movementResult)
        {
            _lastGoodLocation = CalculatePreviousCursorPosition();
            _shapeToMove.SnapToGridInRelevantDimensionsIfPossible(
                movementResult: movementResult,
                occupiedGridSquares: _occupiedGridSquares);
            LogMessagePlusOrigins(message: "Blocked. ");
        }

        public SquareFillPoint CalculatePreviousCursorPosition()
        {
            // _shapeToMove will still have its previous TopLeftCorner value,
            // so we can get it to calculate the previous cursor position based on that.
            return _shapeToMove.CalculateCursorPositionBasedOnTopLeftCorner(
                topLeftCornerRelativeToCursorPosition: _topLeftCornerRelativeToCursorPosition);
        }

        private SquareFillPoint CalculateGridPosition(SquareFillPoint topLeftCorner)
        {
            return new SquareFillPoint(
                x: topLeftCorner.X / ShapeConstants.SquareWidth,
                y: topLeftCorner.Y / ShapeConstants.SquareWidth);
        }

        private void EndMoveWithObstacles()
        {
            LogLocation(message: "End move with obstacles.", locationName: "LastGood", location: _lastGoodLocation);
            var topLeftCornerBasedOnLastGoodLocation = CalculateTopLeftCorner(newCursorPosition: _lastGoodLocation);
            _shapeToMove.SnapToGrid(newTopLeftCorner: topLeftCornerBasedOnLastGoodLocation);
            CleanUpAtEndOfMove();
        }

        private void EndMoveWithNoObstacles(SquareFillPoint finalLocation, SquareFillPoint newTopLeftCorner)
        {
            LogLocation(message: "End move with no obstacles.", locationName: "Final", location: finalLocation);
            _shapeToMove.SnapToGrid(newTopLeftCorner: newTopLeftCorner);
            CleanUpAtEndOfMove();
        }

        private void CleanUpAtEndOfMove()
        {
            _shapeToMove.OccupyGridSquares(occupiedGridSquares: _occupiedGridSquares);
            _shapeToMove = null;
            _colliding = false;
        }

        private void NoteLocation(SquareFillPoint cursor, SquareFillPoint topLeftCorner, SquareFillPoint gridPosition)
        {
            _logger
                 .Clear()
                 .WithPoint(desc: "Cursor", point: cursor)
                 .WithPoint(desc: "NewCorner", point: topLeftCorner)
                 .WithPoint(desc: "NewGrid", point: gridPosition);
        }

        private void LogMessagePlusOrigins(string message)
        {
            _logger
                .WithShape(desc: "Origins", shape: _shapeToMove)
                .WithMessage(message: message)
                .Log();
        }

        private void LogLocation(string message, string locationName, SquareFillPoint location)
        {
            _logger
                .WithMessage(message: message)
                .WithPoint(desc: locationName, point: location)
                .Log();
        }
    }
}